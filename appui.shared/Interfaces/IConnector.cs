﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui.shared.Interfaces
{
    public interface IConnector
    {
        Task<int> Load();
    }
}
