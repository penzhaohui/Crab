using System;
using System.ServiceModel;

namespace Crab.Runtime.Contract
{
    [ServiceContract()]
    public interface IWorkflow
    {
        [OperationContract]
        WorkflowDefinition GetWorkflowDefinition(int workflowType);

        [OperationContract]
        void UpdateWorkflowDefinition(int workflowType, string xoml, string rules);

        [OperationContract]
        byte[] GetWorkflowGraphic(Guid processId);

        [OperationContract]
        byte[] GetWorkflowDefinitionGraphic(int workflowType);
    }
}
