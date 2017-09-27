using System;
using System.Collections.Generic;
using System.Text;

namespace Crab.DataModel.Data
{
    internal interface IUpdateScriptGenerator
    {
        IEnumerable<string> GenerateUpdateScripts(EntityCacheEntry cacheEntry);
    }
}
