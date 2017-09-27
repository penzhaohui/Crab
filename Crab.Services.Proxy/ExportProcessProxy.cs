using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using Crab.Business.Contract;

namespace Crab.Services.Proxy
{
    public static class ExportProcessProxy
    {
        /// <summary>
        /// Channel to proxy IExportProcessProxy
        /// </summary>
        private class BasicChannel : ClientBase<IExportProcessService>, IExportProcessService
        {
            public Guid OpenExportProcess(ShippingExportDC dc)
            {
                return base.Channel.OpenExportProcess(dc);
            }


            public ExportProcessDC[] GetExportProcessListByStatus(ProcessStatus status)
            {
                return base.Channel.GetExportProcessListByStatus(status);
            }

            public ExportProcessDC[] GetExportProcessListByCreator(string creator)
            {
                return base.Channel.GetExportProcessListByCreator(creator);
            }

            public ExportProcessDC GetExportProcessbyContractId(Guid contractId)
            {
                return base.Channel.GetExportProcessbyContractId(contractId);
            }

            public WorkflowAction[] GetSubscribedActions(Guid workflowId)
            {
                return base.Channel.GetSubscribedActions(workflowId);
            }

            public void Submit(Guid workflowId, Guid objectId, string upn)
            {
                base.Channel.Submit(workflowId, objectId, upn);
            }

            public void Approve(Guid workflowId, Guid objectId, string upn, string comment)
            {
                base.Channel.Approve(workflowId, objectId, upn, comment);
            }

            public void Reject(Guid workflowId, Guid objectId, string upn, string rejectReason)
            {
                base.Channel.Reject(workflowId, objectId, upn, rejectReason);
            }

            public void Cancel(Guid workflowId, Guid objectId, string upn)
            {
                base.Channel.Cancel(workflowId, objectId, upn);
            }
        }

        static public Guid OpenExportProcess(ShippingExportDC dc)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.OpenExportProcess(dc);
            }
        }

        static public ExportProcessDC[] GetExportProcessListByStatus(ProcessStatus status)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetExportProcessListByStatus(status);
            }
        }

        static public ExportProcessDC[] GetExportProcessListByCreator(string creator)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetExportProcessListByCreator(creator);
            }
        }

        static public ExportProcessDC GetExportProcessbyContractId(Guid contractId)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetExportProcessbyContractId(contractId);
            }
        }

        static public WorkflowAction[] GetSubscribedActions(Guid workflowId)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetSubscribedActions(workflowId);
            }
        }

        static public void Submit(Guid workflowId, Guid objectId, string upn)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.Submit(workflowId, objectId, upn);
            }
        }

        static public void Approve(Guid workflowId, Guid objectId, string upn, string comment)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.Approve(workflowId, objectId, upn, comment);
            }
        }

        static public void Reject(Guid workflowId, Guid objectId, string upn, string rejectReason)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.Reject(workflowId, objectId, upn, rejectReason);
            }
        }

        static public void Cancel(Guid workflowId, Guid objectId, string upn)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.Cancel(workflowId, objectId, upn);
            }
        }
    }
}
