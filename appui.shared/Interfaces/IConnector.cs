using appui.shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui.shared.Interfaces
{
    public interface IConnector
    {
        public bool Offline { get; set; }

        Task<int> Load();
    }
}
