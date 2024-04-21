using MySqlConnector;
using SimpLog.Databases.MySQL.Entities;
using SimpLog.Databases.MySQL.Models;
using SimpLog.Databases.MySQL.Models.AppSettings;
using System;

namespace SimpLog.Databases.MySQL.Services.DatabaseServices
{
    internal class DatabaseServices
    {
        public static Configuration conf = SimpLog.Databases.MySQL.Services.ConfigurationServices.ConfigService.BindConfigObject();

        /// <summary>
        /// Depending on the name of the DB, goes to the function for that stuff.
        /// </summary>
        /// <param name="storeLog"></param>
        public static void SaveIntoDatabase(StoreLog storeLog)
            => InsertIntoMySql(storeLog);
         
        /// <summary>
        /// Insert log into MySql database.
        /// </summary>
        /// <param name="storeLog"></param>
        public static void InsertIntoMySql(StoreLog storeLog)
        {
            MySqlConnection connection = new MySqlConnection(conf.Database_Configuration.Connection_String);
            MySqlCommand cmd = new MySqlCommand(null, connection);

            DatabaseMigrations.CreateMySqlIfNotExists(connection, cmd);
                        
            int EmailID = 0;

            string query = string.Empty;

            query = "INSERT INTO StoreLog(Log_Type, Log_Error, Log_Created, Log_FileName, Log_Path, Log_SendEmail, Email_ID, Saved_In_Database) " +
                "VALUES(@Log_Type, @Log_Error, @Log_Created, @Log_FileName, @Log_Path, @Log_SendEmail, @Email_ID, @Saved_In_Database)";

            cmd.Parameters.AddWithValue("@Log_Type", storeLog.Log_Type);
            cmd.Parameters.AddWithValue("@Log_Error", storeLog.Log_Error);
            cmd.Parameters.AddWithValue("@Log_Created", storeLog.Log_Created);
            cmd.Parameters.AddWithValue("@Log_FileName", storeLog.Log_FileName);
            cmd.Parameters.AddWithValue("@Log_Path", storeLog.Log_Path);
            cmd.Parameters.AddWithValue("@Log_SendEmail", storeLog.Log_SendEmail);
            cmd.Parameters.AddWithValue("@Email_ID", EmailID);
            cmd.Parameters.AddWithValue("@Saved_In_Database", DateTime.UtcNow.ToString());

            connection.Open();
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            connection.Close();

        }
    }
}
