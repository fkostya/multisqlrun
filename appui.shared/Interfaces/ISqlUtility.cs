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

    /// <summary>
    /// 
    /// </summary>
    public interface ISqlUtility
    {
        void Initialize(IList<IConnectionRecord> dbs);

        void Run(string sqlquery, Action<IProcressDbNotificaion> progress);

        Task<string> Save(string filepath);
    }
}
