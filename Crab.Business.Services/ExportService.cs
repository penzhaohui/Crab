using System;
using System.Collections.Generic;
using System.Text;
using Crab.Business.Contract;
using Crab.Business.Contract.Exceptions;
using Crab.Business.Logic;
using Crab.Runtime.Contract;
using Crab.Runtime.Services;
using Crab.DataModel;

namespace Crab.Business.Services
{
    public class ExportService: IExportService
    {
        public Guid CreateExportContract(string number, Guid creator)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            if (number==null||string.IsNullOrEmpty(number.Trim()))
                throw new ArgumentNullException("number");
            if(creator == Guid.Empty)
                throw new ArgumentNullException("creator");
            try
            {
                DataModelContext.Initialize(RequestContext.Current.TenantId);
                return ExportLogic.CreateExportContract(RequestContext.Current.TenantId, number.Trim(), creator);
            }
            catch (Exception ex)
            {
                throw new ExportContractExcepiton(ex.Message, ex);
            }
            finally
            {
                DataModelContext.Clear();
            }
        }

        public ShippingExportDC GetExportContractById(Guid id)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            if (id == Guid.Empty)
                throw new ArgumentNullException("id");
            try
            {
                DataModelContext.Initialize(RequestContext.Current.TenantId);
                return ExportLogic.GetExportContractById(id);
            }
            catch (Exception ex)
            {
                throw new ExportContractExcepiton(ex.Message, ex);
            }
            finally
            {
                DataModelContext.Clear();
            }
        }

        public ShippingExportDC GetExportContractByNumber(string number)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            if (string.IsNullOrEmpty(number))
                throw new ArgumentNullException("number");
            try
            {
                DataModelContext.Initialize(RequestContext.Current.TenantId);
                return ExportLogic.GetExportContractByNumber(RequestContext.Current.TenantId, number.Trim());
            }
            catch (Exception ex)
            {
                throw new ExportContractExcepiton(ex.Message, ex);
            }
            finally
            {
                DataModelContext.Clear();
            }
        }

        public bool ContractExists(string number)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            if (string.IsNullOrEmpty(number))
                throw new ArgumentNullException("number");
            try
            {
                DataModelContext.Initialize(RequestContext.Current.TenantId);
                return ExportLogic.ContractExists(RequestContext.Current.TenantId, number.Trim());
            }
            catch (Exception ex)
            {
                throw new ExportContractExcepiton(ex.Message, ex);
            }
            finally
            {
                DataModelContext.Clear();
            }
        }

        public ShippingExportDC[] FindExportContractList(string numberToMatch, int? status, Guid? owner)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            try
            {
                DataModelContext.Initialize(RequestContext.Current.TenantId);
                return ExportLogic.FindExportContractList(
                    RequestContext.Current.TenantId,
                    numberToMatch!=null?numberToMatch.Trim():string.Empty,
                    status, owner);
            }
            catch (Exception ex)
            {
                throw new ExportContractExcepiton(ex.Message, ex);
            }
            finally
            {
                DataModelContext.Clear();
            }
        }

        public void UpdateExportContract(ShippingExportDC contract)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            if (contract == null || contract.Id == Guid.Empty)
                throw new ArgumentNullException("contract");
            try
            {
                DataModelContext.Initialize(RequestContext.Current.TenantId);
                ExportLogic.UpdateExportContract(contract);
            }
            catch (Exception ex)
            {
                throw new ExportContractExcepiton(ex.Message, ex);
            }
            finally
            {
                DataModelContext.Clear();
            }
        }

        public void DeleteExportContract(Guid id)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            try
            {
                DataModelContext.Initialize(RequestContext.Current.TenantId);
                ExportLogic.DeleteExportContract(id);
            }
            catch (Exception ex)
            {
                throw new ExportContractExcepiton(ex.Message, ex);
            }
            finally
            {
                DataModelContext.Clear();
            }
        }
    }
}
