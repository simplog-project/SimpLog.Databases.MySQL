using SimpLog.Databases.MySQL.Entities;
using SimpLog.Databases.MySQL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static SimpLog.Databases.MySQL.Models.Constants;

namespace SimpLog.Databases.MySQL.Services.FileServices
{
    internal class FileService
    {
        /// <summary>
        /// FullPath + FileName is the key and value is what should be saved into the log
        /// </summary>
        public static Dictionary<string, StringBuilder> Logs = new Dictionary<string, StringBuilder>();

        public static SimpLog.Databases.MySQL.Models.AppSettings.Configuration configuration = SimpLog.Databases.MySQL.Services.ConfigurationServices.ConfigService.BindConfigObject();

        internal readonly bool? _Trace_Db = (configuration.LogType.Trace.SaveInDatabase == null) ? true : Convert.ToBoolean(configuration.LogType.Trace.SaveInDatabase);
        internal readonly bool? _Debug_Db = (configuration.LogType.Debug.SaveInDatabase == null) ? true : Convert.ToBoolean(configuration.LogType.Debug.SaveInDatabase);
        internal readonly bool? _Info_Db = (configuration.LogType.Info.SaveInDatabase == null) ? true : Convert.ToBoolean(configuration.LogType.Info.SaveInDatabase);
        internal readonly bool? _Notice_Db = (configuration.LogType.Notice.SaveInDatabase == null) ? true : Convert.ToBoolean(configuration.LogType.Notice.SaveInDatabase);
        internal readonly bool? _Warn_Db = (configuration.LogType.Warn.SaveInDatabase == null) ? true : Convert.ToBoolean(configuration.LogType.Warn.SaveInDatabase);
        internal readonly bool? _Error_Db = (configuration.LogType.Error.SaveInDatabase == null) ? true : Convert.ToBoolean(configuration.LogType.Error.SaveInDatabase);
        internal readonly bool? _Fatal_Db = (configuration.LogType.Fatal.SaveInDatabase == null) ? true : Convert.ToBoolean(configuration.LogType.Fatal.SaveInDatabase);


        /// <summary>
        /// Converts message type from enum to string.
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        internal string MessageType(LogType logType)
        {
            switch (logType)
            {
                case LogType.Trace:
                    return LogType_Trace;
                case LogType.Debug:
                    return LogType_Debug;
                case LogType.Info:
                    return LogType_Info;
                case LogType.Notice:
                    return LogType_Notice;
                case LogType.Warn:
                    return LogType_Warn;
                case LogType.Error:
                    return LogType_Error;
                case LogType.Fatal:
                    return LogType_Fatal;
                default:
                    return LogType_NoType;
            }
        }

        /// <summary>
        /// Distributes what type of save is it configured. File, Email of Database.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        /// <param name="saveType"></param>
        /// <param name="sendEmail"></param>
        /// <param name="saveInDatabase"></param>
        /// <param name="path_to_save_log"></param>
        /// <param name="log_file_name"></param>
        /// <returns></returns>
        internal async Task Save(
            string message, 
            LogType logType, 
            bool? saveInDatabase)
        {
            try
            {
                //  Send into a database
                if (ShouldSaveInDb(saveInDatabase, logType))
                    DatabaseServices.DatabaseServices.SaveIntoDatabase(
                        configuration.Database_Configuration.Global_Database_Type,
                        storeLog(message, false, logType, saveInDatabase, FileSaveType.DontSave, "", ""),
                        false, saveInDatabase);
            }
            catch(Exception ex)
            {
//                await SaveSimpLogError(ex.Message);
                //Dispose();
            }
        }

        /// <summary>
        /// Checks the configurations for saving in Db at all.
        /// </summary>
        /// <param name="saveInDatabase"></param>
        /// <param name="logType"></param>
        /// <returns></returns>
        internal bool ShouldSaveInDb(bool? saveInDatabase, LogType logType)
        {
            //  Check if the db log is active at global level.
            if(saveInDatabase is false ||
                (configuration.Database_Configuration.Global_Enabled_Save is not null && configuration.Database_Configuration.Global_Enabled_Save is false) ||
                configuration.Database_Configuration.Connection_String is null ||
                CheckDbTypeFormat() is false)
                return false;

            switch (logType)
            {
                case LogType.Trace:
                    {
                        if (_Trace_Db is not null && _Trace_Db is false)
                            return false;
                        break;
                    }
                case LogType.Debug:
                    {
                        if (_Debug_Db is not null && _Debug_Db is false)
                            return false;
                        break;
                    }
                case LogType.Info:
                    {
                        if (_Info_Db is not null && _Info_Db is false)
                            return false;
                        break;
                    }
                case LogType.Notice:
                    {
                        if (_Notice_Db is not null && _Notice_Db is false)
                            return false;
                        break;
                    }
                case LogType.Warn:
                    {
                        if (_Warn_Db is not null && _Warn_Db is false)
                            return false;
                        break;
                    }
                case LogType.Error:
                    {
                        if (_Error_Db is not null && _Error_Db is false)
                            return false;
                        break;
                    }
                case LogType.Fatal:
                    {
                        if (_Fatal_Db is not null && _Fatal_Db is false)
                            return false;
                        break;
                    }
            }

            return true;
        }

        /// <summary>
        /// Checks if the string typed as db format is in the options.
        /// </summary>
        /// <returns></returns>
        internal bool CheckDbTypeFormat()
        {
            switch (configuration.Database_Configuration.Global_Database_Type)
            {
                case "MSSql":
                    return true;
                case "MySql":
                    return true;
                case "Postgre":
                    return true;
                case "Oracle":
                    return true;
                case "MongoDb":
                    return true;
                default :
                    return false;
            }
        }

        /// <summary>
        /// Populates the object for StoreLog in database table
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isEmailSent"></param>
        /// <param name="logType"></param>
        /// <param name="saveInDatabase"></param>
        /// <param name="saveType"></param>
        /// <param name="path_to_save_log"></param>
        /// <param name="log_file_name"></param>
        /// <returns></returns>
        internal StoreLog storeLog(
            string message, 
            bool? isEmailSent, 
            LogType? logType, 
            bool? saveInDatabase, 
            FileSaveType? saveType, 
            string? path_to_save_log,
            string? log_file_name)
        {
            StoreLog storeLog = new StoreLog()
            {
                Log_Created = DateTime.UtcNow.ToString(),
                Log_Error = message,
                Log_SendEmail = isEmailSent,
                Log_Type = logType.ToString(),
                Saved_In_Database = saveInDatabase,
                Log_File_Save_Type = saveType.Value.DisplayName(),
                Log_Path = "",
                Log_FileName = ""
            };

            return storeLog;
        }
    }
}
