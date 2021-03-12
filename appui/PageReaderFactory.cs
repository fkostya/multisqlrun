using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui
{
    public static class PageReaderFactory
    {
        public static IPageReader CreatePageReader(bool offline)
        {
            switch (offline)
            {
                case true:
                    return new OfflineFilePageReader();
                case false:
                    return new WebPageReader();
            }
        }
    }
}
