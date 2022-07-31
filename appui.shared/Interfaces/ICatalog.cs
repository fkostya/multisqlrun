using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui.shared.Interfaces
{
    public interface ICatalog
    {
        public string Name { get; set; }

        public string DatabaseConnectionString { get; set; }
    }
}
