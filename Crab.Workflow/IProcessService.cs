using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.Activities;
using Crab.Services.EventArgs;

namespace Crab.Workflow
{
    [ExternalDataExchange]
    public interface IProcessService
    {
        event EventHandler<StatusEventArgs> StatusProceeded;
    }
}
