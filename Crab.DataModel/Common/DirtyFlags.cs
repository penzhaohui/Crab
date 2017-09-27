using System;
using System.Collections.Generic;
using System.Text;

namespace Crab.DataModel.Common
{
    public enum DirtyFlags
    {
        Clean = 0,   //none dirty
        New = 1,     //newly created
        Dirty = 2,   //updated
        Delete = 3   //deleted
    }
}
