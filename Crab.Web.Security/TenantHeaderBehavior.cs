using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using Crab.Runtime.Contract;

namespace Crab.Web.Security
{
    public class TenantHeaderBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        public override Type BehaviorType
        {
            get
            {
                return typeof(TenantHeaderBehavior);
            }
        }

        protected override object CreateBehavior()
        {
            return new TenantHeaderBehavior();
        }

        #region IEndpointBehavior Members

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            return;
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(
                new TenantHeaderInspector());
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            return;
        }

        #endregion

        private class TenantHeaderInspector : IClientMessageInspector
        {
            #region IClientMessageInspector Members

            public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
            {
            }

            public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
            {
                MessageHeader<string> header = new MessageHeader<string>(TenantContext.TenantName);
                request.Headers.Add(header.GetUntypedHeader(Constants.Headers.TenantNameHeaderName, Constants.Headers.TenantNameHeaderNamespace));
                header = new MessageHeader<string>(TenantContext.Username);
                request.Headers.Add(header.GetUntypedHeader(Constants.Headers.UsernnameHeaderName, Constants.Headers.UsernameHeaderNamespace));
                header = new MessageHeader<string>(TenantContext.Password);
                request.Headers.Add(header.GetUntypedHeader(Constants.Headers.PasswordHeaderName, Constants.Headers.PasswordHeaderNamespace));
                return null;
            }

            #endregion

        }
    }
}
