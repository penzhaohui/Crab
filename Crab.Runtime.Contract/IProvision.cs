using System;
using System.ServiceModel;

namespace Crab.Runtime.Contract
{
    [ServiceContract()]
    public interface IProvision
    {
        [OperationContract]
        Tenant CreateTenant(string tenantName, string displayName, int licenseCount,
            string contact, string phone, string fax, string mobile, string email, string website,
            string city, string address, string zipcode,
            string adminName, string adminPassword, string adminEmail);

        [OperationContract]
        void UpdateTenantProfile(string tenantName, string displayName,
            string contact, string phone, string fax, string mobile, string email, string website, 
            string city, string address, string zipcode);

        [OperationContract]
        void DeleteTenant(string tenantName, bool deleteAllDatas);

        [OperationContract]
        void ApproveTenant(string tenantName);

        [OperationContract]
        void SetDeadline(string tenantName, DateTime deadline);

        [OperationContract]
        bool TenantExists(string tenantName);

        [OperationContract]
        Tenant GetTenantById(Guid id);

        [OperationContract]
        Tenant GetTenantByName(string tenantName);

        [OperationContract]
        Tenant[] GetAllTenants();

        [OperationContract]
        Tenant[] FindTenants(string nameToMatch, bool? approved, bool? overdue);
    }
}
