﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui.shared.Interfaces
{
    public interface ILoadConnections
    {
        Task<IList<IConnectionStringInfo>> Load();

        IList<IConnectionStringInfo> Find(string version, string key = "");
    }
}
