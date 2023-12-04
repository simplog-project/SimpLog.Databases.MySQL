using MySqlConnector;
using SimpLog.Databases.MySQL.Models.AppSettings;
using System.Text;

namespace SimpLog.Databases.MySQL.Services.DatabaseServices
{
    internal class DatabaseMigrations
    {
        public static Configuration conf = SimpLog.Databases.MySQL.Services.ConfigurationServices.ConfigService.BindConfigObject();

        /// <summary>
        /// Create MySql table if not exists.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="cmd"></param>
        public static void CreateMySqlIfNotExists(MySqlConnection connection, MySqlCommand cmd)
        {
            StringBuilder query = new StringBuilder();

            query.Append($"create table if not exists StoreLog ");
            query.Append($"    ( ");
            query.Append($"       ID int AUTO_INCREMENT PRIMARY KEY ");
            query.Append($"      ,Log_Type varchar(50) ");
            query.Append($"      ,Log_Error varchar(50) ");
            query.Append($"      ,Log_Created varchar(50) ");
            query.Append($"      ,Log_FileName varchar(50) ");
            query.Append($"      ,Log_Path varchar(50) ");
            query.Append($"      ,Log_SendEmail bit ");
            query.Append($"      ,Email_ID int ");
            query.Append($"      ,Saved_In_Database varchar(50) ");
            query.Append($"    ); ");

            query.Append($"create table if not exists EmailLog ");
            query.Append($"    ( ");
            query.Append($"       ID int AUTO_INCREMENT PRIMARY KEY ");
            query.Append($"      ,From_Email varchar(50) ");
            query.Append($"      ,To_Email varchar(50) ");
            query.Append($"      ,Bcc varchar(50) ");
            query.Append($"      ,Email_Subject varchar(50) ");
            query.Append($"      ,Email_Body varchar(50) ");
            query.Append($"      ,Time_Sent varchar(50) ");
            query.Append($"    ) ");

            connection.Open();

            cmd.CommandText = query.ToString();
            cmd.ExecuteNonQuery();

            connection.Close();
        }

    }
}
