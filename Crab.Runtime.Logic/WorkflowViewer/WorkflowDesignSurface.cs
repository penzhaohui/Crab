using System;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Design;

namespace Crab.Runtime.Logic.WorkflowViewer
{
    internal sealed class WorkflowDesignSurface : DesignSurface
    {
        internal WorkflowDesignSurface()
        {
            //this.ServiceContainer.AddService(typeof(ITypeProvider), new TypeProvider(this.ServiceContainer), true);
        }

        internal WorkflowDesignSurface(IMemberCreationService memberCreationService)
        {
            //this.ServiceContainer.AddService(typeof(ITypeProvider), new TypeProvider(this.ServiceContainer), true);
            this.ServiceContainer.AddService(typeof(IMemberCreationService), memberCreationService);
            this.ServiceContainer.AddService(typeof(IMenuCommandService), new MenuCommandService(this.ServiceContainer));
        }
    }
}
