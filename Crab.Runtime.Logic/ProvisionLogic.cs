using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Crab.Runtime.Contract;
using Crab.Runtime.Contract.Exceptions;
using Crab.Runtime.Logic.Common;

namespace Crab.Runtime.Logic
{
    static public class ProvisionLogic
    {
        const string SQL_UPDATE_TENANT_PROFILE = @"UPDATE Tenant SET DisplayName=@DisplayName, Contact=@Contact,
                                                  Phone=@Phone, Fax=@Fax, Mobile=@Mobile, Email=@Email, Website=@Website,
                                                  City=@City, Address=@Address, ZipCode=@ZipCode
                                                  WHERE LOWER(Name)=LOWER(@Name)";

        const string SQL_DELETE_TENANT = @"DELETE FROM Tenant WHERE LOWER(Name)=LOWER(@Name)";

        const string SQL_APPROVE_TENANT = @"UPDATE Tenant SET Approved = 1 WHERE LOWER(Name)=LOWER(@Name)";

        const string SQL_SET_DEADLINE = @"UPDATE Tenant SET EndDate=@EndDate WHERE LOWER(Name)=LOWER(@Name)";

        const string SQL_CHECK_EXISTENCE = @"SELECT COUNT(*) FROM Tenant WHERE LOWER(Name)=LOWER(@Name)";

        const string SQL_GET_TENANT_BY_ID = @"SELECT * FROM Tenant WHERE Id=@Id";

        const string SQL_GET_TENANT_BY_NAME = @"SELECT * FROM Tenant WHERE LOWER(Name)=LOWER(@Name)";

        const string SQL_GET_ALL_TENANTS = @"SELECT * FROM Tenant ORDER BY Name";

        const string SQL_FIND_TENANTS_FORMAT = @"SELECT * FROM Tenant WHERE Name LIKE @NameToMatch {0} ORDER BY Name";

        static public Tenant CreateTenant(string tenantName, string displayName, int licenseCount,
            string contact, string phone, string fax, string mobile, string email, string website,
            string city, string address, string zipCode,
            string adminName, string adminPassword, string adminEmail)
        {
            try
            {
                if (!TenantExists(tenantName))
                {
                    Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantIdentity);
                    using (DbCommand command = db.GetStoredProcCommand("crab_CreateTenant"))
                    {
                        db.AddInParameter(command, "Id", DbType.Guid, Guid.NewGuid());
                        db.AddInParameter(command, "Name", DbType.String, tenantName);
                        db.AddInParameter(command, "DisplayName", DbType.String, displayName);
                        db.AddInParameter(command, "LicenseCount", DbType.Int32, licenseCount);
                        db.AddInParameter(command, "CreateDate", DbType.DateTime, DateTime.Now);
                        db.AddInParameter(command, "EndDate", DbType.DateTime, DateTime.Now.AddDays(Constants.Provision.TrialDays));
                        db.AddInParameter(command, "Approved", DbType.Boolean, false);
                        db.AddInParameter(command, "Contact", DbType.String, contact);
                        db.AddInParameter(command, "Phone", DbType.String, phone);
                        db.AddInParameter(command, "Fax", DbType.String, fax);
                        db.AddInParameter(command, "Mobile", DbType.String, mobile);
                        db.AddInParameter(command, "Email", DbType.String, email);
                        db.AddInParameter(command, "Website", DbType.String, website);
                        db.AddInParameter(command, "City", DbType.String, city);
                        db.AddInParameter(command, "Address", DbType.String, address);
                        db.AddInParameter(command, "ZipCode", DbType.String, zipCode);
                        db.ExecuteNonQuery(command);
                    }
                    AdamConfigurationHelper.AdamManager.CreateTenant(tenantName);
                    foreach (string roleName in AuthorizationLogic.PreDefinedRoles)
                        AuthorizationLogic.CreateRole(tenantName, roleName);

                    AuthenticationLogic.CreateUser(tenantName, adminName, adminPassword, adminEmail);
                    AuthorizationLogic.AddUsersToRoles(
                        tenantName, 
                        new string[] { adminName },
                        new string[] { AuthorizationLogic.AdminRole });

                    return GetTenantByName(tenantName);
                }
                else
                {
                    throw new ProvisioningException(string.Format("Tenant with name {0} already exists!", tenantName));
                }
            }
            catch (Exception ex)
            {
                throw new ProvisioningException(ex.Message, ex);
            }
        }

        static public void UpdateTenantProfile(string tenantName, string displayName,
            string contact, string phone, string fax, string mobile, string email, string website,
            string city, string address, string zipCode)
        {
            Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantIdentity);
            using (DbCommand command = db.GetSqlStringCommand(SQL_UPDATE_TENANT_PROFILE))
            {
                db.AddInParameter(command, "Name", DbType.String, tenantName);
                db.AddInParameter(command, "DisplayName", DbType.String, displayName);
                db.AddInParameter(command, "Contact", DbType.String, contact);
                db.AddInParameter(command, "Phone", DbType.String, phone);
                db.AddInParameter(command, "Fax", DbType.String, fax);
                db.AddInParameter(command, "Mobile", DbType.String, mobile);
                db.AddInParameter(command, "Email", DbType.String, email);
                db.AddInParameter(command, "Website", DbType.String, website);
                db.AddInParameter(command, "City", DbType.String, city);
                db.AddInParameter(command, "Address", DbType.String, address);
                db.AddInParameter(command, "ZipCode", DbType.String, zipCode);
                db.ExecuteNonQuery(command);
            }
        }

        static public void DeleteTenant(string tenantName, bool deleteAllDatas)
        {
            try
            {
                AdamConfigurationHelper.AdamManager.DeleteTenant(tenantName);
                if (deleteAllDatas)
                {
                    Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantIdentity);
                    using (DbCommand command = db.GetSqlStringCommand(SQL_DELETE_TENANT))
                    {
                        db.AddInParameter(command, "Name", DbType.String, tenantName);
                        db.ExecuteNonQuery(command);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProvisioningException(ex.Message, ex);
            }
        }

        static public void ApproveTenant(string tenantName)
        {
            Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantIdentity);
            using (DbCommand command = db.GetSqlStringCommand(SQL_APPROVE_TENANT))
            {
                db.AddInParameter(command, "Name", DbType.String, tenantName);
                db.ExecuteNonQuery(command);
            }
        }

        static public void SetDeadline(string tenantName, DateTime deadline)
        {
            Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantIdentity);
            using (DbCommand command = db.GetSqlStringCommand(SQL_SET_DEADLINE))
            {
                db.AddInParameter(command, "Name", DbType.String, tenantName);
                db.AddInParameter(command, "EndDate", DbType.DateTime, deadline);
                db.ExecuteNonQuery(command);
            }
        }

        static public bool TenantExists(string tenantName)
        {
            Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantIdentity);
            using (DbCommand command = db.GetSqlStringCommand(SQL_CHECK_EXISTENCE))
            {
                db.AddInParameter(command, "Name", DbType.String, tenantName);
                return (int)db.ExecuteScalar(command) > 0;
            }
        }

        static public Tenant GetTenantById(Guid id)
        {
            Tenant tenant = null;
            Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantIdentity);
            using (DbCommand command = db.GetSqlStringCommand(SQL_GET_TENANT_BY_ID))
            {
                db.AddInParameter(command, "Id", DbType.Guid, id);
                using (IDataReader reader = db.ExecuteReader(command))
                {
                    if (reader.Read())
                    {
                        tenant = new Tenant();
                        ReadTenant(reader, tenant);
                    }
                }
            }
            return tenant;
        }

        static public Tenant GetTenantByName(string tenantName)
        {
            Tenant tenant = null;
            Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantIdentity);
            using (DbCommand command = db.GetSqlStringCommand(SQL_GET_TENANT_BY_NAME))
            {
                db.AddInParameter(command, "Name", DbType.String, tenantName);
                using (IDataReader reader = db.ExecuteReader(command))
                {
                    if (reader.Read())
                    {
                        tenant = new Tenant();
                        ReadTenant(reader, tenant);
                    }
                }
            }
            return tenant;
        }

        static public Tenant[] GetAllTenants()
        {
            List<Tenant> tenants = new List<Tenant>();
            Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantIdentity);
            using (DbCommand command = db.GetSqlStringCommand(SQL_GET_ALL_TENANTS))
            {
                using (IDataReader reader = db.ExecuteReader(command))
                {
                    while(reader.Read())
                    {
                        Tenant tenant = new Tenant();
                        ReadTenant(reader, tenant);
                        tenants.Add(tenant);
                    }
                }
            }
            return tenants.ToArray();
        }

        static public Tenant[] FindTenants(string nameToMatch, bool? approved, bool? overdue)
        {
            StringBuilder sb = new StringBuilder();
            if (approved != null)
                sb.Append(" AND Approved=@Approved");
            if (overdue != null&&overdue == true)
                sb.Append(" AND EndDate<@Now");
            else if(overdue != null&&overdue == false)
                sb.Append(" AND EndDate>=@Now");
            string sqlCmd = string.Format(SQL_FIND_TENANTS_FORMAT, sb.ToString());
            List<Tenant> tenants = new List<Tenant>();
            Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantIdentity);
            using (DbCommand command = db.GetSqlStringCommand(sqlCmd))
            {
                db.AddInParameter(command, "NameToMatch", DbType.String, nameToMatch + "%");
                if (approved != null)
                    db.AddInParameter(command, "Approved", DbType.Boolean, (bool)approved);
                if(overdue!=null)
                    db.AddInParameter(command, "Now", DbType.DateTime, DateTime.Now);
                using (IDataReader reader = db.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        Tenant tenant = new Tenant();
                        ReadTenant(reader, tenant);
                        tenants.Add(tenant);
                    }
                }
            }
            return tenants.ToArray();
        }

        static private void ReadTenant(IDataRecord dataRecord, Tenant tenant)
        {
            tenant.Id = (Guid)dataRecord["Id"];
            tenant.Name = (string)dataRecord["Name"];
            tenant.DisplayName = (string)dataRecord["DisplayName"];
            tenant.Approved = (bool)dataRecord["Approved"];
            tenant.CreateDate = (DateTime)dataRecord["CreateDate"];
            tenant.EndDate = (DateTime)dataRecord["EndDate"];
            tenant.LicenseCount = (int)dataRecord["LicenseCount"];
            tenant.Contact = dataRecord["Contact"] is DBNull ? null : (string)dataRecord["Contact"];
            tenant.Phone = dataRecord["Phone"] is DBNull ? null : (string)dataRecord["Phone"];
            tenant.Fax = dataRecord["Fax"] is DBNull ? null : (string)dataRecord["Fax"];
            tenant.Mobile = dataRecord["Mobile"] is DBNull ? null : (string)dataRecord["Mobile"];
            tenant.Email = dataRecord["Email"] is DBNull ? null : (string)dataRecord["Email"];
            tenant.Website = dataRecord["Website"] is DBNull ? null : (string)dataRecord["Website"];
            tenant.City = dataRecord["City"] is DBNull ? null : (string)dataRecord["City"];
            tenant.Address = dataRecord["Address"] is DBNull ? null : (string)dataRecord["Address"];
            tenant.ZipCode = dataRecord["ZipCode"] is DBNull ? null : (string)dataRecord["ZipCode"];
        }
    }
}
