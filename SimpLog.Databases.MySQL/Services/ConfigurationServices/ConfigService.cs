using SimpLog.Databases.MySQL.Models.AppSettings;
using System;
using System.IO;
using System.Text.Json;

namespace SimpLog.Databases.MySQL.Services.ConfigurationServices
{
    internal static class ConfigService
    {
        #region Database Configuration Variable

        static readonly string? _Connection_String = null;
        static readonly string? _Global_Database_Type = null;
        static readonly bool? _Use_OleDB = null;
        static readonly bool? _Global_Enabled_Save = null;

        #endregion Database Configuration Variable

        #region Log Type Configuration Variable
        
        static readonly bool? _TraceSaveInDatabase   = false;

        static readonly bool? _DebugSaveInDatabase   = false;
        
        static readonly bool? _InfoSaveInDatabase    = false;
        
        static readonly bool? _NoticeSaveInDatabase  = false;
        
        static readonly bool? _WarnSaveInDatabase    = false;
        
        static readonly bool? _ErrorSaveInDatabase   = false;
        
        static readonly bool? _FatalSaveInDatabase   = false;

        #endregion Log Type Configuration Variable

        static ConfigService()
        {
            Configuration? simpLogConfig;

            //  If there is not found a configuration file
            if (!File.Exists(Environment.CurrentDirectory + "\\simplog.json"))
            {
                simpLogConfig = new Configuration()
                {
                    Database_Configuration = new DatabaseConfiguration()
                    {
                        Connection_String = null,
                        Global_Database_Type = null,
                        Global_Enabled_Save = null,
                        Use_OleDB = null
                    },
                    LogType = new Log()
                    {
                        Debug = new LogTypeObject()
                        {
                            SaveInDatabase = null,
                        },
                        Error = new LogTypeObject()
                        {
                            SaveInDatabase = null,
                        },
                        Fatal = new LogTypeObject()
                        {
                            SaveInDatabase = null,
                        },
                        Info = new LogTypeObject()
                        {
                            SaveInDatabase = null,
                        },
                        Notice = new LogTypeObject()
                        {
                            SaveInDatabase = null,
                        },
                        Trace = new LogTypeObject()
                        {
                            SaveInDatabase = null,
                        },
                        Warn = new LogTypeObject()
                        {
                            SaveInDatabase = null,
                        }
                    }
                };
            }
            else
                simpLogConfig = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(Environment.CurrentDirectory + "\\simplog.json"));

            #region Database Configuration Get From Json

            _Connection_String = simpLogConfig.Database_Configuration.Connection_String;
            _Global_Database_Type = simpLogConfig.Database_Configuration.Global_Database_Type;
            _Use_OleDB = simpLogConfig.Database_Configuration.Use_OleDB;
            _Global_Enabled_Save = simpLogConfig.Database_Configuration.Global_Enabled_Save;

            #endregion Database Configuration Get From Json

            #region Log Type Configuration Get From Json

            //  Checks if the configuration exists at all
            _TraceSaveInDatabase = simpLogConfig.LogType.Trace.SaveInDatabase;

            _DebugSaveInDatabase = simpLogConfig.LogType.Debug.SaveInDatabase;

            _InfoSaveInDatabase = simpLogConfig.LogType.Info.SaveInDatabase;

            _NoticeSaveInDatabase = simpLogConfig.LogType.Notice.SaveInDatabase;

            _WarnSaveInDatabase = simpLogConfig.LogType.Warn.SaveInDatabase;

            _ErrorSaveInDatabase = simpLogConfig.LogType.Error.SaveInDatabase;

            _FatalSaveInDatabase = simpLogConfig.LogType.Fatal.SaveInDatabase;

            #endregion Log Type Configuration Get From Json
        }

        /// <summary>
        /// Put into an object the configuration from appsettings.json file
        /// </summary>
        /// <returns></returns>
        public static Configuration BindConfigObject()
        {
            return new Configuration()
            {
                Database_Configuration  = new DatabaseConfiguration()
                {
                    Connection_String       = _Connection_String,
                    Global_Database_Type    = _Global_Database_Type,
                    Use_OleDB               = _Use_OleDB,
                    Global_Enabled_Save     = _Global_Enabled_Save
                },
                LogType                 = new Log()
                {
                    Trace   = new LogTypeObject()
                    {
                        SaveInDatabase  = _TraceSaveInDatabase
                    },
                    Debug   = new LogTypeObject()
                    {
                        SaveInDatabase  = _DebugSaveInDatabase
                    },
                    Info    = new LogTypeObject()
                    {
                        SaveInDatabase  = _InfoSaveInDatabase
                    },
                    Notice  = new LogTypeObject()
                    {
                        SaveInDatabase  = _NoticeSaveInDatabase
                    },
                    Warn    = new LogTypeObject()
                    {
                        SaveInDatabase  = _WarnSaveInDatabase
                    },
                    Error   = new LogTypeObject()
                    {
                        SaveInDatabase  = _ErrorSaveInDatabase
                    },
                    Fatal   = new LogTypeObject()
                    {
                        SaveInDatabase  = _FatalSaveInDatabase
                    },
                }
            };
        }
    }
}
