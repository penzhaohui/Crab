using System;
using System.Collections.Generic;
using System.Text;
using Crab.Runtime.Contract;
using Crab.Runtime.Logic;

namespace Crab.Runtime.Services
{
    public class WorkflowServiceSC : IWorkflow
    {
        public WorkflowDefinition GetWorkflowDefinition(int workflowType)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            if (!RequestContext.Current.IsAuthenticated)
                throw new InvalidOperationException("Insufficient privilege!");
            return WorkflowLogic.GetWorkflowDefinition(RequestContext.Current.TenantId, workflowType);
        }


        public void UpdateWorkflowDefinition(int workflowType, string xoml, string rules)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            if (string.IsNullOrEmpty(xoml))
                throw new ArgumentNullException("xoml");
            if (!RequestContext.Current.IsAuthenticated)
                throw new InvalidOperationException("Insufficient privilege!");
            WorkflowLogic.UpdateWorkflowDefinition(RequestContext.Current.TenantId, workflowType, xoml, rules);
        }

        public byte[] GetWorkflowGraphic(Guid processId)
        {
            if (!RequestContext.Current.IsAuthenticated)
                throw new InvalidOperationException("Insufficient privilege!");
            return WorkflowLogic.GetWorkflowGraphic(processId);
        }

        public byte[] GetWorkflowDefinitionGraphic(int workflowType)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            if (!RequestContext.Current.IsAuthenticated)
                throw new InvalidOperationException("Insufficient privilege!");
            return WorkflowLogic.GetWorkflowDefinitionGraphic(RequestContext.Current.TenantId, workflowType);
        }
    }
}
