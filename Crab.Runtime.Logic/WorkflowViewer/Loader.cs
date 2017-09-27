using System;
using System.IO;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Xml;
using System.Web;
using System.Text;

namespace Crab.Runtime.Logic.WorkflowViewer
{
    //This type is used to load the workflow definition
    internal sealed class Loader : WorkflowDesignerLoader
    {
        private Activity workflowDefinitionValue = null;

        internal Loader()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            IDesignerLoaderHost host = LoaderHost;
            if (host != null)
            {
                TypeProvider typeProvider = new TypeProvider(host);
                typeProvider.AddAssemblyReference(typeof(string).Assembly.Location);
                host.AddService(typeof(ITypeProvider), typeProvider, true);
            }
        }

        public override TextReader GetFileReader(string filePath)
        {
            return new StreamReader(filePath);
        }

        public override TextWriter GetFileWriter(string filePath)
        {
            return new StreamWriter(filePath);
        }

        public override string FileName
        {
            get
            {
                return string.Empty;
            }
        }

        internal Activity WorkflowDefinition
        {
            set
            {
                this.workflowDefinitionValue = value;
            }
        }

        //Load the workflow definition from WorkflowMarkup
        protected override void PerformLoad(IDesignerSerializationManager serializationManager)
        {
            IDesignerHost designerHost = (IDesignerHost)GetService(typeof(IDesignerHost));
            Activity activity = this.workflowDefinitionValue;

            //Add the rootactivity the designer
            if (activity != null && designerHost != null)
            {
                Helpers.AddObjectGraphToDesignerHost(designerHost, activity);
                SetBaseComponentClassName(activity.Name);
            }
        }

        protected override void PerformFlush(IDesignerSerializationManager manager)
        {
        }
    }
}
