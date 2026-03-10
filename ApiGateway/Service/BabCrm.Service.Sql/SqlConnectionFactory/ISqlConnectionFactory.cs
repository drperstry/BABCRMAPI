using System.Data.SqlClient;

namespace BabCrm.Service.Sql.SqlConnectionFactory
{
    public interface ISqlConnectionFactory
    {
        SqlConnection CreateConnection();
    }

}
