﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui.shared.Interfaces
{
    public interface ILoadConnections
    {
        Task<IList<IConnectionRecord>> Load();

        IList<IConnectionRecord> Find(string version, string key = "");
    }
}
