using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Classes
{
    public static class GlobalCache
    {
        public static DataCache SharedCache = new DataCache();
    }
}
