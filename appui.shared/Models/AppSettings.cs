using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui.shared.Models
{
    public class AppSettings
    {
        public int TimeputOpenConnection { get; set; }

        public int StopAfterMilliseconds { get; set; }

        public string DefaultSqlUserName { get; set; }

        public string DefaultSqlUserPwd { get; set; }

        public bool Offline { get; set; }
    }
}
