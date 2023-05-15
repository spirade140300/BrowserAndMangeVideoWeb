using System.Data.Common;
using System.Data.SqlClient;

namespace BrowseAndManageVideos_WEB.Controllers
{
    public class DatabaseController
    {
        public DatabaseController() { }
        public DatabaseController(string databaseName) { }

        public static SqlConnection GetDataBaseConnection()
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = "KEKW\\SQLEXPRESS",         // Replace with your server name or IP address
                InitialCatalog = "Movies",   // Replace with your database name
                UserID = "username",                // Replace with your username
                Password = "password",              // Replace with your password
                IntegratedSecurity = true                                   // Additional connection options can be set here
            };

            return new SqlConnection(connectionStringBuilder.ConnectionString);
        }
    }
}
