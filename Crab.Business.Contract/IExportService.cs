using System;
using System.ServiceModel;
using Crab.Runtime.Contract;
using Crab.Business.Contract;

namespace Crab.Business.Contract
{
    [ServiceContract()]
    public interface IExportService
    {
        [OperationContract]
        Guid CreateExportContract(string number, Guid creator);

        [OperationContract]
        ShippingExportDC GetExportContractById(Guid id);

        [OperationContract]
        ShippingExportDC GetExportContractByNumber(string number);

        [OperationContract]
        bool ContractExists(string number);

        [OperationContract]
        ShippingExportDC[] FindExportContractList(string numberToMatch, int? status, Guid? owner);

        [OperationContract]
        void UpdateExportContract(ShippingExportDC contract);

        [OperationContract]
        void DeleteExportContract(Guid id);
    }
}
