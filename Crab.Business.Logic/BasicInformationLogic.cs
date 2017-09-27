using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Crab.Business.Contract;

namespace Crab.Business.Logic
{
    public class BasicInformationLogic
    {
        private const string SQL_CREATE_CUSTOMER = @"INSERT INTO Customer
                                                    (Id, TenantId, Name, Description, Address, PhoneNumber) 
                                                     VALUES
                                                     (@Id, @TenantId, @Name, @Description, @Address, @PhoneNumber)";

        private const string SQL_GET_CUSTOMER_BY_NAME = @"SELECT* FROM Customer WHERE TenantId=@TenantId AND LOWER(Name)=LOWER(@Name)";

        private const string SQL_GET_CUSTOMER_BY_ID =  @"SELECT* FROM Customer WHERE Id=@Id";

        private const string SQL_UPDATE_CUSTOMER = @"UPDATE Customer SET Description=@Description, Address=@Address, PhoneNumber=@PhoneNumber WHERE Id=@Id";

        private const string SQL_DELETE_CUSTOMER = @"DELETE FROM Customer WHERE Id=@Id";

        private const string SQL_FIND_CUSTOMER_BY_NAME = @"SELECT * FROM Customer WHERE TenantId=@TenantId AND Name LIKE @NameToMatch ORDER BY Name";

        public static Customer CreateCustomer(Guid tenantId, string name, string description, string address, string phone)
        {
            if (GetCustomerByName(tenantId, name) == null)
            {
                Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantData);
                Guid id = Guid.NewGuid();
                using (DbCommand command = db.GetSqlStringCommand(SQL_CREATE_CUSTOMER))
                {
                    db.AddInParameter(command, "Id", DbType.Guid, id);
                    db.AddInParameter(command, "TenantId", DbType.Guid, tenantId);
                    db.AddInParameter(command, "Name", DbType.String, name);
                    db.AddInParameter(command, "Description", DbType.String, description);
                    db.AddInParameter(command, "Address", DbType.String, address);
                    db.AddInParameter(command, "PhoneNumber", DbType.String, phone);
                    db.ExecuteNonQuery(command);
                }
                return GetCustomerById(id);
            }
            return null;
        }

        public static Customer GetCustomerByName(Guid tenantId, string name)
        {
            Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantData);
            using (DbCommand command = db.GetSqlStringCommand(SQL_GET_CUSTOMER_BY_NAME))
            {
                db.AddInParameter(command, "TenantId", DbType.Guid, tenantId);
                db.AddInParameter(command, "Name", DbType.String, name);
                using (IDataReader reader = db.ExecuteReader(command))
                {
                    if (reader.Read())
                    {
                        Customer customer = new Customer();
                        customer.Id = (Guid)reader["Id"];
                        customer.Name = (string)reader["Name"];
                        customer.Description = reader["Description"] is DBNull ? (string)null : (string)reader["Description"];
                        customer.PhoneNumber = reader["PhoneNumber"] is DBNull ? (string)null : (string)reader["PhoneNumber"];
                        customer.Address = reader["Address"] is DBNull ? (string)null : (string)reader["Address"];
                        return customer;
                    }
                }
            }
            return null;
        }

        public static Customer GetCustomerById(Guid id)
        {
            Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantData);
            using (DbCommand command = db.GetSqlStringCommand(SQL_GET_CUSTOMER_BY_ID))
            {
                db.AddInParameter(command, "Id", DbType.Guid, id);
                using (IDataReader reader = db.ExecuteReader(command))
                {
                    if (reader.Read())
                    {
                        Customer customer = new Customer();
                        customer.Id = (Guid)reader["Id"];
                        customer.Name = (string)reader["Name"];
                        customer.Description = reader["Description"] is DBNull ? (string)null : (string)reader["Description"];
                        customer.PhoneNumber = reader["PhoneNumber"] is DBNull ? (string)null : (string)reader["PhoneNumber"];
                        customer.Address = reader["Address"] is DBNull ? (string)null : (string)reader["Address"];
                        return customer;
                    }
                }
            }
            return null;
        }

        public static void UpdateCustomer(Customer customer)
        {
            Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantData);
            Guid id = Guid.NewGuid();
            using (DbCommand command = db.GetSqlStringCommand(SQL_UPDATE_CUSTOMER))
            {
                db.AddInParameter(command, "Id", DbType.Guid, customer.Id);
                db.AddInParameter(command, "Description", DbType.String, customer.Description);
                db.AddInParameter(command, "Address", DbType.String, customer.Address);
                db.AddInParameter(command, "PhoneNumber", DbType.String, customer.PhoneNumber);
                db.ExecuteNonQuery(command); 
            }
        }

        public static void DeleteCustomer(Guid id)
        {
            Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantData);
            using (DbCommand command = db.GetSqlStringCommand(SQL_DELETE_CUSTOMER))
            {
                db.AddInParameter(command, "Id", DbType.Guid, id);
                db.ExecuteNonQuery(command);
            }
        }

        public static Customer[] FindCustomersByName(Guid tenantId, string nameToMatch)
        {
            List<Customer> customers = new List<Customer>();
            Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantData);
            using (DbCommand command = db.GetSqlStringCommand(SQL_FIND_CUSTOMER_BY_NAME))
            {
                db.AddInParameter(command, "TenantId", DbType.Guid, tenantId);
                db.AddInParameter(command, "NameToMatch", DbType.String, nameToMatch + "%");
                using (IDataReader reader = db.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        Customer customer = new Customer();
                        customer.Id = (Guid)reader["Id"];
                        customer.Name = (string)reader["Name"];
                        customer.Description = reader["Description"] is DBNull ? (string)null : (string)reader["Description"];
                        customer.PhoneNumber = reader["PhoneNumber"] is DBNull ? (string)null : (string)reader["PhoneNumber"];
                        customer.Address = reader["Address"] is DBNull ? (string)null : (string)reader["Address"];
                        customers.Add(customer);
                    }
                }
            }
            return customers.ToArray();
        }
    }
}
