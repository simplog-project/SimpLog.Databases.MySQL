using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpLog.Databases.MySQL.Models.AppSettings
{
    internal class Configuration
    {
        public DatabaseConfiguration? Database_Configuration { get; set; }

        public Log? LogType { get; set; }
    }
}
