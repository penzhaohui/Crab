using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using Crab.Runtime.Contract;

namespace Crab.Services.Proxy
{
    static public class ProvisionProxy
    {
        /// <summary>
        /// Channel to proxy IProvision
        /// </summary>
        private class BasicChannel : ClientBase<IProvision>, IProvision
        {
            public Tenant CreateTenant(string tenantName, string displayName, int licenseCount,
                string contact, string phone, string fax, string mobile, string email, string website,
                string city, string address, string zipCode,
                string adminName, string adminPassword, string adminEmail)
            {
                return base.Channel.CreateTenant(tenantName, displayName, licenseCount, contact, phone, fax, 
                    mobile, email, website, city, address, zipCode,
                    adminName, adminPassword, adminEmail);
            }

            public void UpdateTenantProfile(string tenantName, string displayName,
                string contact, string phone, string fax, string mobile, string email, string website,
                string city, string address, string zipcode)
            {
                base.Channel.UpdateTenantProfile(tenantName, displayName, contact, phone,fax,
                    mobile, email, website, city, address, zipcode);
            }

            public void DeleteTenant(string tenantName, bool deleteAllDatas)
            {
                base.Channel.DeleteTenant(tenantName, deleteAllDatas);
            }

            public void ApproveTenant(string tenantName)
            {
                base.Channel.ApproveTenant(tenantName);
            }

            public void SetDeadline(string tenantName, DateTime deadline)
            {
                base.Channel.SetDeadline(tenantName, deadline);
            }

            public bool TenantExists(string tenantName)
            {
                return base.Channel.TenantExists(tenantName);
            }

            public Tenant GetTenantById(Guid Id)
            {
                return base.Channel.GetTenantById(Id);
            }

            public Tenant GetTenantByName(string tenantName)
            {
                return base.Channel.GetTenantByName(tenantName);
            }

            public Tenant[] GetAllTenants()
            {
                return base.Channel.GetAllTenants();
            }

            public Tenant[] FindTenants(string nameToMatch, bool? approved, bool? overdue)
            {
                return base.Channel.FindTenants(nameToMatch, approved, overdue);
            }
        }


        static public Tenant CreateTenant(string tenantName, string displayName, int licenseCount,
            string contact, string phone, string fax, string mobile, string email, string website,
            string city, string address, string zipCode,
            string adminName, string adminPassword, string adminEmail)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.CreateTenant(tenantName, displayName, licenseCount, contact, phone, fax, mobile, email, 
                    website, city, address, zipCode,
                    adminName, adminPassword, adminEmail);
            }
        }

        static public void UpdateTenantProfile(string tenantName, string displayName,
            string contact, string phone, string fax, string mobile, string email, string website,
            string city, string address, string zipCode)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.UpdateTenantProfile(tenantName, displayName, contact, phone, fax,
                    mobile, email, website, city, address, zipCode);
            }
        }

        static public void DeleteTenant(string tenantName, bool deleteAllDatas)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.DeleteTenant(tenantName, deleteAllDatas);
            }
        }

        static public void ApproveTenant(string tenantName)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.ApproveTenant(tenantName);
            }
        }

        static public void SetDeadline(string tenantName, DateTime deadline)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.SetDeadline(tenantName, deadline);
            }
        }

        static public bool TenantExists(string tenantName)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.TenantExists(tenantName);
            }
        }

        static public Tenant GetTenantById(Guid Id)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetTenantById(Id);
            }
        }

        static public Tenant GetTenantByName(string tenantName)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetTenantByName(tenantName);
            }
        }

        static public Tenant[] GetAllTenants()
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetAllTenants();
            }
        }

        static public Tenant[] FindTenants(string nameToMatch, bool? approved, bool? overdue)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.FindTenants(nameToMatch, approved, overdue);
            }
        }
    }
}
