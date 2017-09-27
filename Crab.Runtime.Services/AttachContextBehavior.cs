using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;

namespace Crab.Runtime.Services
{
    public class AttachContextBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        public override Type BehaviorType
        {
            get
            {
                return typeof(AttachContextBehavior);
            }
        }

        protected override object CreateBehavior()
        {
            return new AttachContextBehavior();
        }

        #region IEndpointBehavior Members

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            return;
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new AttachContextInspector());
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            return;
        }

        #endregion

        private class AttachContextInspector : IDispatchMessageInspector
        {
            #region IDispatchMessageInspector Members

            public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
            {
                OperationContext.Current.Extensions.Add(new RequestContext());
                return null;
            }

            public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
            {
                OperationContext.Current.Extensions.Remove(RequestContext.Current);
            }

            #endregion
        }
    }
}
