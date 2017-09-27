using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using Crab.Business.Contract;

namespace Crab.Services.Proxy
{
    public static class BasicInformationProxy
    {
        /// <summary>
        /// Channel to proxy IBasicInformationProxy
        /// </summary>
        private class BasicChannel : ClientBase<IBasicInformationService>, IBasicInformationService
        {
            public Customer CreateCustomer(string name, string description, string address, string phone)
            {
                return base.Channel.CreateCustomer(name, description, address, phone);
            }

            public Customer GetCustomerByName(string name)
            {
                return base.Channel.GetCustomerByName(name);
            }

            public Customer GetCustomerById(Guid id)
            {
                return base.Channel.GetCustomerById(id);
            }

            public void UpdateCustomer(Customer customer)
            {
                base.Channel.UpdateCustomer(customer);
            }

            public void DeleteCustomer(Guid id)
            {
                base.Channel.DeleteCustomer(id);
            }

            public Customer[] FindCustomersByName(string nameToMatch)
            {
                return base.Channel.FindCustomersByName(nameToMatch);
            }
        }

        public static Customer CreateCustomer(string name, string description, string address, string phone)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.CreateCustomer(name, description, address, phone);
            }
        }

        public static Customer GetCustomerByName(string name)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetCustomerByName(name);
            }
        }

        public static Customer GetCustomerById(Guid id)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetCustomerById(id);
            }
        }

        public static void UpdateCustomer(Customer customer)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.UpdateCustomer(customer);
            }
        }

        public static void DeleteCustomer(Guid id)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.DeleteCustomer(id);
            }
        }

        public static Customer[] FindCustomersByName(string nameToMatch)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.FindCustomersByName(nameToMatch);
            }
        }
    }
}
