using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Crab.DataModel
{
    public interface IStatibleCollection<T>
    {
        void GetAllChanges(out T[] inserted, out T[] updated, out T[] deleted);

        void RefreshCollection();
    }
}
