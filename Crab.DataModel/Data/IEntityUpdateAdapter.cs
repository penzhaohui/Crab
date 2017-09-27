using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace Crab.DataModel.Data
{
    public interface IEntityUpdateAdapter
    {
        int Update(IExtensibleEntityCache cache);

        DbConnection Connection { get; set; }
    }
}
