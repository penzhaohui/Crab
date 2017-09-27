using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using Crab.Business.Contract;
using Crab.Runtime.Contract;

namespace Crab.Services.Proxy
{
    public static class ExportProxy
    {
        /// <summary>
        /// Channel to proxy IExportProxy
        /// </summary>
        private class BasicChannel : ClientBase<IExportService>, IExportService
        {
            public Guid CreateExportContract(string number, Guid creator)
            {
                return base.Channel.CreateExportContract(number, creator);
            }

            public ShippingExportDC GetExportContractById(Guid id)
            {
                return base.Channel.GetExportContractById(id);
            }

            public ShippingExportDC GetExportContractByNumber(string number)
            {
                return base.Channel.GetExportContractByNumber(number);
            }

            public bool ContractExists(string number)
            {
                return base.Channel.ContractExists(number);
            }


            public ShippingExportDC[] FindExportContractList(string numberToMatch, int? status, Guid? owner)
            {
                return base.Channel.FindExportContractList(numberToMatch, status, owner);
            }

            public void UpdateExportContract(ShippingExportDC contract)
            {
                base.Channel.UpdateExportContract(contract);
            }

            public void DeleteExportContract(Guid id)
            {
                base.Channel.DeleteExportContract(id);
            }
        }

        public static Guid CreateExportContract(string number, Guid creator)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.CreateExportContract(number, creator);
            }
        }

        public static ShippingExportDC GetExportContractById(Guid id)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetExportContractById(id);
            }
        }

        public static ShippingExportDC GetExportContractByNumber(string number)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetExportContractByNumber(number);
            }
        }

        public static bool ContractExists(string number)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.ContractExists(number);
            }
        }

        public static ShippingExportDC[] FindExportContractList(string numberToMatch, int? status, Guid? owner)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.FindExportContractList(numberToMatch, status, owner);
            }
        }

        public static void UpdateExportContract(ShippingExportDC contract)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.UpdateExportContract(contract);
            }
        }

        public static void DeleteExportContract(Guid id)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.DeleteExportContract(id);
            }
        }
    }
}
