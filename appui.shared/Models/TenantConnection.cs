﻿using appui.shared.Interfaces;

namespace appui.shared.Models
{
    public class TenantConnection : ITenantConnection
    {
        public string DbServer { get; set; }
        public string Database { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}