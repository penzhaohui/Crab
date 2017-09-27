using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Crab.Runtime.Logic;
using Crab.Runtime.Contract;

namespace Crab.Runtime.Services
{
    public class RequestContext : IExtension<OperationContext>
    {
        private Guid _tenantId = Guid.Empty;
        private string _tenantName;
        private string _username;
        private string _password;

        public void Attach(OperationContext owner)
        {
        }

        public void Detach(OperationContext owner)
        {
        }

        public Guid TenantId
        {
            get
            {
                if (_tenantId == Guid.Empty)
                {
                    if (TenantName != null)
                    {
                        Tenant tenant = ProvisionLogic.GetTenantByName(TenantName);
                        if (tenant != null)
                            _tenantId = tenant.Id;
                    }
                }
                return _tenantId;
            }
        }

        public string TenantName
        {
            get
            {
                if (_tenantName == null)
                {
                    int index = RequestHeaders.FindHeader(
                        Crab.Runtime.Contract.Constants.Headers.TenantNameHeaderName,
                        Crab.Runtime.Contract.Constants.Headers.TenantNameHeaderNamespace);
                    if (index > -1)
                    {
                        string tenantName = RequestHeaders.GetHeader<string>(
                            Crab.Runtime.Contract.Constants.Headers.TenantNameHeaderName,
                            Crab.Runtime.Contract.Constants.Headers.TenantNameHeaderNamespace);
                        _tenantName = tenantName;
                    }
                }
                return _tenantName;
            }
        }

        public string Username
        {
            get
            {
                if (_username == null)
                {
                    int index = RequestHeaders.FindHeader(
                        Crab.Runtime.Contract.Constants.Headers.UsernnameHeaderName,
                        Crab.Runtime.Contract.Constants.Headers.UsernameHeaderNamespace);
                    if (index > -1)
                    {
                        string username = RequestHeaders.GetHeader<string>(
                            Crab.Runtime.Contract.Constants.Headers.UsernnameHeaderName,
                            Crab.Runtime.Contract.Constants.Headers.UsernameHeaderNamespace);
                        _username = username;
                    }
                }
                return _username;
            }
        }

        public string Password
        {
            get
            {
                if (_password == null)
                {
                    int index = RequestHeaders.FindHeader(
                        Crab.Runtime.Contract.Constants.Headers.PasswordHeaderName,
                        Crab.Runtime.Contract.Constants.Headers.PasswordHeaderNamespace);
                    if (index > -1)
                    {
                        string password = RequestHeaders.GetHeader<string>(
                            Crab.Runtime.Contract.Constants.Headers.PasswordHeaderName,
                            Crab.Runtime.Contract.Constants.Headers.PasswordHeaderNamespace);
                        _password = password;
                    }
                }
                return _password;
            }
        }

        public bool IsAuthenticated
        {
            get 
            {
                return AuthenticationLogic.ValidateUser(TenantName, Username, Password);
            }
        }

        public static RequestContext Current
        {
            get { return OperationContext.Current.Extensions.Find<RequestContext>(); }
        }

        private MessageHeaders RequestHeaders
        {
            get { return OperationContext.Current.IncomingMessageHeaders; }
        }
    }
}
