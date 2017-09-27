using System;
using System.Collections.Generic;
using System.Text;
using Crab.Business.Contract;
using Crab.Business.Logic;
using Crab.Runtime.Services;

namespace Crab.Business.Services
{
    public class BasicInformationService : IBasicInformationService
    {
        public Customer CreateCustomer(string name, string description, string address, string phone)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            return BasicInformationLogic.CreateCustomer(RequestContext.Current.TenantId, name, description, address, phone);
        }

        public Customer GetCustomerByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            return BasicInformationLogic.GetCustomerByName(RequestContext.Current.TenantId, name);
        }

        public Customer GetCustomerById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("id");
            return BasicInformationLogic.GetCustomerById(id);
        }

        public void UpdateCustomer(Customer customer)
        {
            if (customer.Id == Guid.Empty)
                throw new ArgumentNullException("customer.Id");
            BasicInformationLogic.UpdateCustomer(customer);
        }

        public void DeleteCustomer(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("id");
            BasicInformationLogic.DeleteCustomer(id);
        }

        public Customer[] FindCustomersByName(string nameToMatch)
        {
            return BasicInformationLogic.FindCustomersByName(RequestContext.Current.TenantId, nameToMatch);
        }
    }
}
