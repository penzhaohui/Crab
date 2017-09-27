using System;
using System.Collections.Generic;
using System.Text;
using Crab.Runtime.Contract;
using Crab.DataModel;
using Crab.DataModel.Data;
using Crab.DataModel.Common;
using Crab.Business.Contract;
using Crab.Business.Contract.Exceptions;

namespace Crab.Business.Logic
{
    public class ExportLogic
    {
        public static Guid CreateExportContract(Guid tenantId, string number, Guid creator)
        {
            ShippingExportDC contractDC = null;
            using (ExtensibleObjectContext context = new ExtensibleObjectContext(Constants.Database.TenantData))
            {
                KeyValuePair<string, object> keyValueTenantId = new KeyValuePair<string, object>("TenantId", tenantId);
                KeyValuePair<string, object> keyValueNumber = new KeyValuePair<string, object>("Number", number);
                EntityQuery<ShippingExportContract> query = context.GetQuery<ShippingExportContract>(keyValueTenantId, keyValueNumber);
                if(query.GetCount()>0)
                    throw new ExportContractExcepiton(string.Format("The export contract with number {0} has already existed!"));

                contractDC = new ShippingExportDC();
                contractDC.Id = Guid.NewGuid();
                contractDC.TenantId = tenantId;
                contractDC.Number = number;
                contractDC.CreateDate = DateTime.Now;
                contractDC.Creator = creator;
                ShippingExportContract contract = new ShippingExportContract();
                EntityExchanger.WriteDCToEntity(contractDC, contract);
                context.AddObject(contract);
                context.SaveAllChanges();
            }
            ExportProcessLogic.OpenExportProcess(tenantId, contractDC);
            return contractDC.Id;
        }

        public static ShippingExportDC GetExportContractById(Guid id)
        {
            using(ExtensibleObjectContext context = new ExtensibleObjectContext(Constants.Database.TenantData))
            {
                KeyValuePair<string, object>[] keyValue = new KeyValuePair<string, object>[] { new KeyValuePair<string, object>("Id", id) };
                EntityKey key = context.CreateKey(typeof(ExtensibleObjectContext), keyValue);
                ShippingExportContract exportContract = context.GetObjectByKey<ShippingExportContract>(key);
                ShippingExportDC dc = new ShippingExportDC();
                EntityExchanger.WriteEntityToDC(exportContract, dc);
                return dc;
            }
        }

        public static ShippingExportDC GetExportContractByNumber(Guid tenantId, string number)
        {
            using (ExtensibleObjectContext context = new ExtensibleObjectContext(Constants.Database.TenantData))
            {
                KeyValuePair<string, object> keyValueTenantId = new KeyValuePair<string, object>("TenantId", tenantId);
                KeyValuePair<string, object> keyValueNumber = new KeyValuePair<string, object>("Number", number);
                EntityQuery<ShippingExportContract> query = context.GetQuery<ShippingExportContract>(keyValueTenantId, keyValueNumber);
                foreach (ShippingExportContract exportContract in query)
                {
                    ShippingExportDC dc = new ShippingExportDC();
                    EntityExchanger.WriteEntityToDC(exportContract, dc);
                    return dc;
                }
                return null;
            }
        }

        public static bool ContractExists(Guid tenantId, string number)
        {
            using (ExtensibleObjectContext context = new ExtensibleObjectContext(Constants.Database.TenantData))
            {
                KeyValuePair<string, object> keyValueTenantId = new KeyValuePair<string, object>("TenantId", tenantId);
                KeyValuePair<string, object> keyValueNumber = new KeyValuePair<string, object>("Number", number);
                EntityQuery<ShippingExportContract> query = context.GetQuery<ShippingExportContract>(keyValueTenantId, keyValueNumber);
                return query.GetCount() > 0;
            }
        }

        public static ShippingExportDC[] FindExportContractList(Guid tenantId, string numberToMatch, int? status, Guid? owner)
        {
            using (ExtensibleObjectContext context = new ExtensibleObjectContext(Constants.Database.TenantData))
            {
                List<ShippingExportDC> dcList = new List<ShippingExportDC>();
                StringBuilder queryClause =new StringBuilder();
                queryClause.Append(string.Format("TenantId='{0}'", tenantId.ToString()));
                queryClause.Append(string.Format(" AND Number LIKE '{0}%'", numberToMatch==null?string.Empty:numberToMatch.Trim().Replace("'", "''")));
                if(status!=null)
                    queryClause.Append(string.Format(" AND Status = {0}", (int)status));
                if(owner!=null)
                    queryClause.Append(string.Format(" AND Creator = '{0}'", ((Guid)owner).ToString()));
                queryClause.Append(" ORDER BY Number");
                EntityQuery<ShippingExportContract> query = context.GetQuery<ShippingExportContract>(queryClause.ToString());
                foreach (ShippingExportContract exportContract in query)
                {
                    ShippingExportDC dc = new ShippingExportDC();
                    EntityExchanger.WriteEntityToDC(exportContract, dc);
                    dcList.Add(dc);
                }
                return dcList.ToArray();
            }
        }

        public static void UpdateExportContract(ShippingExportDC dc)
        {
            using (ExtensibleObjectContext context = new ExtensibleObjectContext(Constants.Database.TenantData))
            {
                string entityDefName = ExtensibleDC.GetEntityClassName(dc.GetType());
                EntityMetadata metadata = DataModelWorkspace.Current.GetEntityMetadata(entityDefName);
                KeyValuePair<string, object>[] keyValues= new KeyValuePair<string,object>[]{new KeyValuePair<string,object>("Id", dc.Id)};
                EntityKey key = context.CreateKey(typeof(ShippingExportContract), keyValues);
                ShippingExportContract exportContract = context.GetObjectByKey<ShippingExportContract>(key);
                EntityExchanger.WriteDCToEntity(dc, exportContract);
                context.SaveAllChanges();
            }
        }

        public static void DeleteExportContract(Guid id)
        {
            using (ExtensibleObjectContext context = new ExtensibleObjectContext(Constants.Database.TenantData))
            {
                KeyValuePair<string, object>[] keyValue = new KeyValuePair<string, object>[] { new KeyValuePair<string, object>("Id", id) };
                EntityKey key = context.CreateKey(typeof(ExtensibleObjectContext), keyValue);
                ShippingExportContract exportContract = context.GetObjectByKey<ShippingExportContract>(key);
                context.DeleteObject(exportContract);
                context.SaveAllChanges();
            }
        }
    }
}
