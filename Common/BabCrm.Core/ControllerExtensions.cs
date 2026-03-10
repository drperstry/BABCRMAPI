using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;

namespace BabCrm.Core
{
    public static class ControllerExtensions
    {
        public static string GetRoute(this ControllerBase controller)
            => controller.HttpContext.Request.Path.ToString() + controller.HttpContext.Request.QueryString;

        public static CultureInfo GetCurrentCulture(this ControllerBase controller)
        {
            try
            {
                var currentCulture = Thread.CurrentThread.CurrentCulture;
                return currentCulture;
            }
            catch (Exception ex)
            {
                return new CultureInfo(0); 
            }
        }

    }
}
