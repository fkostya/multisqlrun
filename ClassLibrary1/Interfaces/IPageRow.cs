using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui.shared.Interfaces
{
    public interface IPageRow
    {
        public string id { get; set; }
        public string key { get; set; }
        public string database { get; set; }
        public string server { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Version { get; set; }
    }
}
