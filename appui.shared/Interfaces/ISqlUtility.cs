using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui.shared.Interfaces
{
    public class IProcressDbNotificaion
    {
        public string Database { get; set; }
    }

    public interface ISqlUtility
    {
        void Open(IList<IConnectionRecord> dbs);

        void Run(string sqlquery, Action<IProcressDbNotificaion> progress);

        Task<string> Save(string filepath);
    }
}
