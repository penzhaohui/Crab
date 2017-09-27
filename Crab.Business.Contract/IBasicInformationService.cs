using System;
using System.ServiceModel;
using Crab.Business.Contract;

namespace Crab.Business.Contract
{
    [ServiceContract()]
    public interface IBasicInformationService
    {
        [OperationContract]
        Customer CreateCustomer(string name, string description, string address, string phone);

        [OperationContract]
        Customer GetCustomerByName(string name);

        [OperationContract]
        Customer GetCustomerById(Guid id);

        [OperationContract]
        void UpdateCustomer(Customer customer);

        [OperationContract]
        void DeleteCustomer(Guid id);

        [OperationContract]
        Customer[] FindCustomersByName(string nameToMatch);
    }
}
