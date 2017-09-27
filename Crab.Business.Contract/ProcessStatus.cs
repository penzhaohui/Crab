using System;
using System.Collections.Generic;
using System.Text;

namespace Crab.Business.Contract
{
    [Serializable]
    public enum ProcessStatus
    {
        Running =1, 
        Completed,
        Teminated
    }
}
