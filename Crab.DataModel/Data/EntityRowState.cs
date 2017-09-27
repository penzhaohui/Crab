using System;
using System.Collections.Generic;
using System.Text;

namespace Crab.DataModel.Data
{
    [Flags]
    public enum EntityRowState
    {
        Detached = 1,
        Unchanged = 2,
        Added = 4,
        Deleted = 8,
        Modified = 16
    }
}
