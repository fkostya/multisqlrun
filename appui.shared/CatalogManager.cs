using appui.shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui.shared
{
    public class CatalogManager
    {
        public IList<ICatalog> Catalogs { get; set; }
        
        public CatalogManager()
        {

        }
    }
}
