using System;
using System.Collections.Generic;
using System.Text;
using Crab.Runtime.Contract;
using Crab.Runtime.Logic;

namespace Crab.Runtime.Services
{
    public class ProvisionService: IProvision
    {
        #region implements IProvision
        public Tenant CreateTenant(string tenantName, string displayName, int licenseCount,
            string contact, string phone, string fax, string mobile, string email, string website,
            string city, string address, string zipcode, 
            string adminName, string adminPassword, string adminEmail)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            if (string.IsNullOrEmpty(displayName))
                throw new ArgumentNullException("displayName");

            return ProvisionLogic.CreateTenant(tenantName, displayName, licenseCount,
                contact, phone, fax, mobile, email, website,
                city, address, zipcode,
                adminName, adminPassword, adminEmail);
        }

        public void UpdateTenantProfile(string tenantName, string displayName,
            string contact, string phone, string fax, string mobile, string email, string website,
            string city, string address, string zipcode)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            if (string.IsNullOrEmpty(displayName))
                throw new ArgumentNullException("displayName");

            ProvisionLogic.UpdateTenantProfile(tenantName, displayName,
                contact, phone, fax, mobile, email, website,
                city, address, zipcode);
        }

        public void DeleteTenant(string tenantName, bool deleteAllDatas)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            ProvisionLogic.DeleteTenant(tenantName, deleteAllDatas);
        }

        public void ApproveTenant(string tenantName)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            ProvisionLogic.ApproveTenant(tenantName);
        }

        public void SetDeadline(string tenantName, DateTime deadline)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            ProvisionLogic.SetDeadline(tenantName, deadline);
        }

        public bool TenantExists(string tenantName)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            return ProvisionLogic.TenantExists(tenantName);
        }

        public Tenant GetTenantById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("id");
            return ProvisionLogic.GetTenantById(id);
        }

        public Tenant GetTenantByName(string tenantName)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            return ProvisionLogic.GetTenantByName(tenantName);
        }

        public Tenant[] GetAllTenants()
        {
            return ProvisionLogic.GetAllTenants();
        }

        public Tenant[] FindTenants(string nameToMatch, bool? approved, bool? overdue)
        {
            return ProvisionLogic.FindTenants(nameToMatch, approved, overdue);
        }
        #endregion
    }
}
