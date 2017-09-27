using System;
using System.IO;
using System.CodeDom;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Windows.Forms;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Threading;


namespace Crab.Runtime.Logic.WorkflowViewer
{
    internal class Viewer
    {
        //Expand or collapse all composite activities

        static public byte[] GetWorkflowImageBinary(Activity workflowDefinition, Dictionary<string, ActivityStatusInfo> activityStatusList)
        {
            AutoResetEvent waitHandle = new AutoResetEvent(false);
            byte[] results = null;
            Thread thread = new Thread(delegate()
            {
                Loader loader = new Loader();
                WorkflowDesignSurface surface = new WorkflowDesignSurface(new MemberCreationService());
                IDesignerHost host = (IDesignerHost)surface.GetService(typeof(IDesignerHost));
                if (host == null)
                {
                    waitHandle.Set();
                    return;
                }
                loader.WorkflowDefinition = workflowDefinition;
                surface.BeginLoad(loader);
                if (activityStatusList != null)
                {
                    IDesignerGlyphProviderService glyphService = surface.GetService(typeof(IDesignerGlyphProviderService)) as IDesignerGlyphProviderService;
                    WorkflowMonitorDesignerGlyphProvider glyphProvider = new WorkflowMonitorDesignerGlyphProvider(activityStatusList);
                    glyphService.AddGlyphProvider(glyphProvider);
                }
                ((IDesignerLoaderHost)host).EndLoad(host.RootComponent.Site.Name, true, null);
                IDesignerHost designerHost = surface.GetService(typeof(IDesignerHost)) as IDesignerHost;
                if (designerHost != null && designerHost.RootComponent != null)
                {
                    IRootDesigner rootDesigner = designerHost.GetDesigner(designerHost.RootComponent) as IRootDesigner;
                    if (rootDesigner != null)
                    {
                        MemoryStream ms = new MemoryStream();
                        WorkflowView workflowView = rootDesigner.GetView(ViewTechnology.Default) as WorkflowView;
                        workflowView.SaveWorkflowImage(ms, ImageFormat.Png);
                        results = ms.GetBuffer();      
                    }
                }
                waitHandle.Set();
            });
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            waitHandle.WaitOne();
            return results;
        }

        private class MemberCreationService : IMemberCreationService
        {
            #region IMemberCreationService Members
            // Designer host requires an IMemberCreationService - we don't need this functionality
            // so all of the methods are blank

            void IMemberCreationService.CreateField(string className, string fieldName, Type fieldType, Type[] genericParameterTypes, MemberAttributes attributes, CodeSnippetExpression initializationExpression, bool overwriteExisting) { }
            void IMemberCreationService.CreateProperty(string className, string propertyName, Type propertyType, AttributeInfo[] attributes, bool emitDependencyProperty, bool isMetaProperty, bool isAttached, Type ownerType, bool isReadOnly) { }
            void IMemberCreationService.CreateEvent(string className, string eventName, Type eventType, AttributeInfo[] attributes, bool emitDependencyProperty) { }

            void IMemberCreationService.UpdateTypeName(string oldClassName, string newClassName) { }
            void IMemberCreationService.UpdateProperty(string className, string oldPropertyName, Type oldPropertyType, string newPropertyName, Type newPropertyType, AttributeInfo[] attributes, bool emitDependencyProperty, bool isMetaProperty) { }
            void IMemberCreationService.UpdateEvent(string className, string oldEventName, Type oldEventType, string newEventName, Type newEventType, AttributeInfo[] attributes, bool emitDependencyProperty, bool isMetaProperty) { }
            void IMemberCreationService.UpdateBaseType(string className, Type baseType) { }

            void IMemberCreationService.RemoveProperty(string className, string propertyName, Type propertyType) { }
            void IMemberCreationService.RemoveEvent(string className, string eventName, Type eventType) { }

            void IMemberCreationService.ShowCode(Activity activity, string methodName, Type delegateType) { }
            void IMemberCreationService.ShowCode() { }

            #endregion
        }

        internal class ZoomChangedEventArgs : EventArgs
        {
            private Int32 zoomValue;

            public Int32 Zoom
            {
                get { return zoomValue; }
            }
            public ZoomChangedEventArgs(Int32 zoom)
            {
                zoomValue = zoom;
            }
        }
    }
}
