# What is SimpLog
Simple and very flexible tool for development with .NET Core. Saves logs to a file, or a database where you want. Also gives opportunity to 
receive logs via email with the proper configuration.

# Log Types in SimpLog
| Type | Description |
| ----- | ----- |
| Trace | This should be used during development to track bugs, but never committed to your VCS. |
| Debug | Log at this level about anything that happens in the program. This is mostly used during debugging, and I’d advocate trimming down the number of debug statement before entering the production stage, so that only the most meaningful entries are left, and can be activated during troubleshooting. |
| Info | Log at this level all actions that are user-driven, or system specific (ie regularly scheduled operations…) |
| Notice | This will certainly be the level at which the program will run when in production. Log at this level all the notable events that are not considered an error. |
| Warn | Log at this level all events that could potentially become an error. For instance if one database call took more than a predefined time, or if an in-memory cache is near capacity. This will allow proper automated alerting, and during troubleshooting will allow to better understand how the system was behaving before the failure. |
| Error | Log every error condition at this level. That can be API calls that return errors or internal error conditions. |
| Fatal | Too bad, it’s doomsday. Use this very scarcely, this shouldn’t happen a lot in a real program. Usually logging at this level signifies the end of the program. For instance, if a network daemon can’t bind a network socket, log at this level and exit is the only sensible thing to do. |

# Features of SimpLog.Databases.MSSQL

| Features | Description |
| ----- | ----- |
| &#128218; Log into database | With SimpLog you can save logs into a database. It is possible to save it in MSSql, MySql, PostgreSql and MongoDb. Have in mind, that if the tables are not created, they will be created automatically in the database from the connection string in SimpLog:Database_Configuration:Connection_String |


# &#128218; Database structure for logging into a database

You need two tables, no matter the database. The databases are as follows:

| Database | Description |
| ----- | ----- |
| StoreLog | Saves what is the error and has connection with EmailLog if the row has been sent via email. |

Scripts for creating tables:

| 🐬 MySql |
| ----- |
```
create table if not exists StoreLog(        
    ID int AUTO_INCREMENT PRIMARY KEY,
    Log_Type varchar(50),
    Log_Error varchar(50),
    Log_Created varchar(50),
    Log_FileName varchar(50),
    Log_Path varchar(50),
    Log_SendEmail bit,
    Email_ID int,
    Saved_In_Database varchar(50)     
); 

create table if not exists EmailLog(        
    ID int AUTO_INCREMENT PRIMARY KEY,
    From_Email varchar(50),
    To_Email varchar(50),
    Bcc varchar(50),
    Email_Subject varchar(50),
    Email_Body varchar(50),
    Time_Sent varchar(50)     
) 
```

# Configuration

**In Program.cs**
In Program.cs only if you use buffer type for saving into a log file. In other cases it is not needed at all.
```
Nothing needed
```

**In Controller**
```
private SimpLog.Databases.MSSQL.Services.SimpLogServices.SimpLog _simpLog = new SimpLog.Databases.MSSQL.Services.SimpLogServices.SimpLog();
```

and call the log like
```
_simpLog.Trace("place your message here");
```

options are as follows
```
_simpLog.Info({1}, {2}, {3}); 
```
and only {1} is required

| Option | Short Description | Full Description |
| ----- | ----- | ----- |
| {1} | Message | The message you want to log. |
| {2} | Save file type | There are three types of save type. See the table below |
| {3} | Save into database | Disable or enable saving into database. |

**In simplog.json**

Create simplog.json file in the root folder of your startup project. On the same level where is appsettings.json. Please have in mind that every configuration in simplog.json is optional ☺️

```
  "SimpLog": {                              -> all of the below fields are optional
    "Database_Configuration": {             -> Database configurations
      "Connection_String": string,          -> Depending on database type, use the correct connection string.
      "Global_Database_Type": string,       -> The type of a database. Please spell it correctly "MSSql" or "MySql" or "Postgre" or "MongoDb". N.B! Be carefull with spelling!
      "Use_OleDB": bool,                    -> Still not implementet, not any functionality here.
      "Global_Enabled_Save": bool           -> You can globally disable or enable saving into database. Default value is
    },
    "LogType": {
      "Trace": {                            -> TYPE OF LOG == Trace.
        "SaveInDatabase": true              -> For the TYPE OF LOG, should be enabled saving into database. Default value is true.
      },
      "Debug": {                            -> Analogically TYPE OF LOG here is Debug 
        "SaveInDatabase": true
      },
      "Info": {                             -> Analogically TYPE OF LOG here is Info
        "SaveInDatabase": true
      },
      "Notice": {                           -> Analogically TYPE OF LOG here is Notice
        "SaveInDatabase": true
      },
      "Warn": {                             -> Analogically TYPE OF LOG here is Warn
        "SaveInDatabase": true
      },
      "Error": {                            -> Analogically TYPE OF LOG here is Error
        "SaveInDatabase": true
      },
      "Fatal": {                            -> Analogically TYPE OF LOG here is Fatal
        "SaveInDatabase": true
      }
    }
  }
  ```

Hope you enjoy the NuGet Package! 😉
