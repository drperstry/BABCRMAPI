using Asp.Versioning;
using Bab.Attachments;
using Bab.BatchData;
using Bab.BatchData.Sql;
using Bab.BatchData.Sql.Data;
using Bab.Jobs;
using Bab.Jobs.Api;
using Bab.Jobs.Crm;
using BabCrm.ApiGateway.Filters;
using BabCrm.ApiGateway.Security;
using BabCrm.Core;
using BabCrm.Core.Caching;
using BabCrm.Core.Configurations;
using BabCrm.Crm;
using BabCrm.Crm.Configuration;
using BabCrm.Mapping;
using BabCrm.Service;
using BabCrm.Service.Crm;
using BabCrm.Service.Sql;
using BabCrm.Service.Sql.SqlConnectionFactory;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Globalization;
using System.Text.Json.Serialization;

namespace BabCrm.ApiGateway
{
    public class Program
    {
        private static bool isJobsControllerDisabled;
        private static bool isRequestCapturingEnabled;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigurationManager configuration = builder.Configuration;

            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddDbContext<BabContext>
                (options => options.UseSqlServer(builder.Configuration.GetValue<string>("Connection:DB").Decrypt()));

            builder.Services.AddDbContext<BabArchiveDataContext>
                (options => options.UseSqlServer(builder.Configuration.GetValue<string>("Connection:ArchiveDb").Decrypt()));


            builder.Services.AddSingleton<ISqlConnectionFactory>(new SqlConnectionFactory(builder.Configuration.GetValue<string>("Connection:ArchiveDb").Decrypt().Trim()));

            builder.Services.AddHttpClient("MWClient");

            // Add services to the container.
            builder.Services.AddSingleton<CrmService>();
            builder.Services.AddSingleton<IServiceStore, ServiceStore>();
            builder.Services.AddScoped<IMWExternalServiceStore, MWServiceStore>();
            builder.Services.AddScoped<IBatchDataSqlStore, BatchDataSqlStore>();
            builder.Services.AddScoped<IArchiveSqlStore, ArchiveSqlStore>();
            builder.Services.AddScoped<BatchDataManager>();
            builder.Services.AddScoped<ServiceManager>();
            builder.Services.AddScoped<MWExternalServices>();
            builder.Services.AddScoped<MWManager>();
            builder.Services.AddSingleton<IJobsStore, JobsStore>();
            builder.Services.AddScoped<JobsManager>();
            builder.Services.AddSingleton<ReportsManager>();
            builder.Services.AddSingleton<AttachmentsManager>();
            builder.Services.AddAsConfigSingleton<ICrmConfig, CrmConfig>(configuration);
            builder.Services.AddMemoryCache();
            builder.Services.AddAsConfigSingleton<ICacheConfig, CacheConfig>(configuration);
            builder.Services.AddSingleton<ICacheManager, InMemoryCacheManager>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services
                .AddControllers(options =>
                {
                    options.Filters.Add(typeof(BasicAuthenticationFilter));
                })
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            isJobsControllerDisabled = configuration.GetValue<bool>("HideJobsController");
            isRequestCapturingEnabled = configuration.GetValue<bool>("IsRequestCapturingEnabled");

            builder.Services.AddSwaggerGen(options =>
            {
                options.OperationFilter<HeaderOperationFilter>();
                options.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    Description = "Basic Authentication Header"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Basic"
                            }
                        },
                        new string[] {}
                    }
                });
                if (isJobsControllerDisabled)
                {
                    options.DocumentFilter<ExcludeControllerFilter<JobsController>>();
                }
            });

            //builder.Services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName("Bab.Jobs.Api")));
            var retentionPeriod = builder.Configuration.GetValue<int>("HangfireRetionPeriod");

            builder.Services.AddHangfire(configuration => configuration
             .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
             .UseSimpleAssemblyNameTypeSerializer()
             .UseRecommendedSerializerSettings()
             .UseSqlServerStorage(builder.Configuration.GetValue<string>("Connection:Hangfire").Decrypt(), new SqlServerStorageOptions
             {
                 CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                 SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                 QueuePollInterval = TimeSpan.Zero,
                 UseRecommendedIsolationLevel = true,
                 DisableGlobalLocks = true,
                 JobExpirationCheckInterval = TimeSpan.Zero,
             })
             .WithJobExpirationTimeout(TimeSpan.FromDays(retentionPeriod)));

            builder.Services.AddHangfireServer();

            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("En"),
                new CultureInfo("Ar")
                {
                    DateTimeFormat = new DateTimeFormatInfo
                    {
                        Calendar = new GregorianCalendar()
                    }
                }
            };

            builder.Services
                .AddLocalization()
                .Configure<RequestLocalizationOptions>(options =>
                {
                    options.DefaultRequestCulture = new RequestCulture("Ar");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                    options.RequestCultureProviders.Insert(0,
                         new CustomRequestCultureProvider(context =>
                         {
                             var userLanguage = context.Request.Headers["Accept-Language"].ToString();
                             //var userLanguagesList = userLanguages.Split(',').Select(x => x.Trim()).ToList();

                             if (userLanguage.Equals("En", StringComparison.OrdinalIgnoreCase))
                             {
                                 return Task.FromResult(new ProviderCultureResult("En"));
                             }
                             else if (userLanguage.Equals("Ar", StringComparison.OrdinalIgnoreCase))
                             {
                                 return Task.FromResult(new ProviderCultureResult("Ar"));
                             }
                             else
                             {
                                 return Task.FromResult(new ProviderCultureResult("Ar"));
                             }
                         }));
                });

            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader("X-Api-Version"));
            })
                .AddMvc()
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'V";
                    options.SubstituteApiVersionInUrl = true;
                });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            //app.UseSerilogRequestLogging();

            if (isRequestCapturingEnabled)
            {
                app.UseCatchRequest();
                app.UseResponseLogger();

            }
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {

            });

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseAuthorization();

            var options = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.MapControllers();
            app.MapHangfireDashboard();

            app.Run();
        }
    }
}