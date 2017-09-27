using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;
using System.Workflow.Runtime.Tracking;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using System.Configuration;
using System.Data.Common;
using System.Data;
using System.Globalization;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel.Design.Serialization;
using System.ComponentModel.Design;
using System.Xml;
using Crab.Business.Contract;
using Crab.Runtime.Contract;
using Crab.Runtime.Logic;
using Crab.Workflow;

namespace Crab.Business.Logic
{
    static public class ExportProcessLogic
    {
        static private WorkflowRuntime _runtime;

        static public WorkflowRuntime Runtime
        {
            get
            {
                if (_runtime == null)
                {
                    _runtime = new WorkflowRuntime("WorkflowRuntime");
                    System.Workflow.ComponentModel.Compiler.TypeProvider typeProvider = new System.Workflow.ComponentModel.Compiler.TypeProvider(_runtime);
                    typeProvider.AddAssembly(System.Reflection.Assembly.Load("Crab.Workflow"));
                    _runtime.AddService(typeProvider);
                    if (_runtime.GetService<ExternalDataExchangeService>() != null)
                    {
                        ExternalDataExchangeService dataExchangeService = _runtime.GetService<ExternalDataExchangeService>();
                        dataExchangeService.AddService(new ContractService());
                    }
                    _runtime.StartRuntime();
                }
                return _runtime;
            }
        }

        static private void RunWorkflowInScheduler(Guid instanceId)
        {
            ManualWorkflowSchedulerService scheduler = Runtime.GetService<ManualWorkflowSchedulerService>();
            scheduler.RunWorkflow(instanceId);
        }

        static public Guid OpenExportProcess(Guid tenantId, ShippingExportDC dc)
        {
            if (GetExportProcessbyContractId(dc.Id) != null)
                return Guid.Empty;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("TenantId", tenantId);
            parameters.Add("ObjectId", dc.Id);
            parameters.Add("Number", dc.Number);
            string textUpn = AuthenticationLogic.GetUpnByUserId((Guid)dc.Creator);
            parameters.Add("Creator", textUpn);

            WorkflowDefinition workflowDef = WorkflowLogic.GetWorkflowDefinition(tenantId, (int)ProcessTypes.ShippingExport);
            parameters.Add("InternalWorkflowType", (int)ProcessTypes.ShippingExport);
            System.Xml.XmlReader xomlReader = System.Xml.XmlReader.Create(new StringReader(workflowDef.Xoml));
            System.Xml.XmlReader rulesReader = string.IsNullOrEmpty(workflowDef.Rules)?null:System.Xml.XmlReader.Create(new StringReader(workflowDef.Rules));

            WorkflowInstance workflowInstance = Runtime.CreateWorkflow(xomlReader, rulesReader, parameters);    
            workflowInstance.Start();
            RunWorkflowInScheduler(workflowInstance.InstanceId);
            return workflowInstance.InstanceId;
        }

        static public ExportProcessDC[] GetExportProcessListByStatus(Guid tenantId, ProcessStatus status)
        {
            List<ExportProcessDC> processList = new List<ExportProcessDC>();
            SqlTrackingQuery sqlTrackingQuery = new SqlTrackingQuery(ConfigurationManager.ConnectionStrings[Constants.Database.TenantWorkflowStore].ConnectionString);
            SqlTrackingQueryOptions options = new SqlTrackingQueryOptions();
            options.WorkflowStatus = ProcessHelper.ToWorkflowStatus(status);
            options.TrackingDataItems.Add(new TrackingDataItemValue("ExportProcess", "TenantId", tenantId.ToString()));
            IList<SqlTrackingWorkflowInstance> workflows = sqlTrackingQuery.GetWorkflows(options);
            foreach (SqlTrackingWorkflowInstance workflow in workflows)
            {
                ExportProcessDC exportProcessDC = new ExportProcessDC();
                exportProcessDC.ProcessId = workflow.WorkflowInstanceId;
                exportProcessDC.Status = ProcessHelper.ToProcessStatus(workflow.Status);
                int activityEventsCount = workflow.ActivityEvents.Count;
                if (activityEventsCount > 0)
                {
                    ActivityTrackingRecord record = (ActivityTrackingRecord)workflow.ActivityEvents[activityEventsCount - 1];
                    foreach (TrackingDataItem dataItem in record.Body)
                    {
                        switch (dataItem.FieldName)
                        {
                            case "ObjectId":
                                exportProcessDC.ObjectId = (Guid)dataItem.Data;
                                break;
                            case "Number":
                                exportProcessDC.Number = (string)dataItem.Data;
                                break;
                            case "Creator":
                                exportProcessDC.Creator = (string)dataItem.Data;
                                break;
                            case "Amount":
                                exportProcessDC.Amount = dataItem.Data == null ? default(decimal) : (decimal)dataItem.Data;
                                break;
                        }
                    }
                }
                processList.Add(exportProcessDC);
            }
            return processList.ToArray();
        }

        static public ExportProcessDC[] GetExportProcessListByCreator(Guid tenantId, string creator)
        {
            List<ExportProcessDC> processList = new List<ExportProcessDC>();
            SqlTrackingQuery sqlTrackingQuery = new SqlTrackingQuery(ConfigurationManager.ConnectionStrings[Constants.Database.TenantWorkflowStore].ConnectionString);
            SqlTrackingQueryOptions options = new SqlTrackingQueryOptions();
            options.TrackingDataItems.Add(new TrackingDataItemValue("ExportProcess", "Creator", creator));
            IList<SqlTrackingWorkflowInstance> workflows = sqlTrackingQuery.GetWorkflows(options);
            foreach (SqlTrackingWorkflowInstance workflow in workflows)
            {
                ExportProcessDC exportProcessDC = new ExportProcessDC();
                exportProcessDC.ProcessId = workflow.WorkflowInstanceId;
                exportProcessDC.Status = ProcessHelper.ToProcessStatus(workflow.Status);
                exportProcessDC.Creator = creator;
                int activityEventsCount = workflow.ActivityEvents.Count;
                if (activityEventsCount > 0)
                {
                    ActivityTrackingRecord record = (ActivityTrackingRecord)workflow.ActivityEvents[activityEventsCount - 1];
                    foreach (TrackingDataItem dataItem in record.Body)
                    {
                        switch (dataItem.FieldName)
                        {
                            case "ObjectId":
                                exportProcessDC.ObjectId = (Guid)dataItem.Data;
                                break;
                            case "Number":
                                exportProcessDC.Number = (string)dataItem.Data;
                                break;
                            case "Amount":
                                exportProcessDC.Amount = dataItem.Data == null ? default(decimal) : (decimal)dataItem.Data;
                                break;
                        }
                    }
                }
                processList.Add(exportProcessDC);
            }
            return processList.ToArray();
        }

        static public ExportProcessDC GetExportProcessbyContractId(Guid contractId)
        {
            SqlTrackingQuery sqlTrackingQuery = new SqlTrackingQuery(ConfigurationManager.ConnectionStrings[Constants.Database.TenantWorkflowStore].ConnectionString);
            SqlTrackingQueryOptions options = new SqlTrackingQueryOptions();
            //options.WorkflowStatus = WorkflowStatus.Running;
            options.TrackingDataItems.Add(new TrackingDataItemValue("ExportProcess", "ObjectId", contractId.ToString()));

            IList<SqlTrackingWorkflowInstance> workflows = sqlTrackingQuery.GetWorkflows(options);
            if (workflows.Count == 0)
                return null;
            SqlTrackingWorkflowInstance workflow = workflows[workflows.Count-1];
            ExportProcessDC exportProcessDC = new ExportProcessDC();
            exportProcessDC.ObjectId = contractId;
            exportProcessDC.ProcessId = workflow.WorkflowInstanceId;
            exportProcessDC.Status = ProcessHelper.ToProcessStatus(workflow.Status);
            int activityEventsCount = workflow.ActivityEvents.Count;
            if (activityEventsCount > 0)
            {
                ActivityTrackingRecord record = (ActivityTrackingRecord)workflow.ActivityEvents[activityEventsCount - 1];
                foreach (TrackingDataItem dataItem in record.Body)
                {
                    switch (dataItem.FieldName)
                    {
                        case "Number":
                            exportProcessDC.Number = (string)dataItem.Data;
                            break;
                        case "Creator":
                            exportProcessDC.Creator = (string)dataItem.Data;
                            break;
                        case "Amount":
                            exportProcessDC.Amount = dataItem.Data == null ? default(decimal) : (decimal)dataItem.Data;
                            break;
                    }
                }
            }
            return exportProcessDC;
        }

        static public WorkflowAction[] GetSubscribedActions(Guid workflowId)
        {
            List<WorkflowAction> actions = new List<WorkflowAction>();
            SqlTrackingQuery sqlTrackingQuery = new SqlTrackingQuery(ConfigurationManager.ConnectionStrings[Constants.Database.TenantWorkflowStore].ConnectionString);
            SqlTrackingWorkflowInstance trackingWorkflow;
            if (!sqlTrackingQuery.TryGetWorkflow(workflowId, out trackingWorkflow)
                || trackingWorkflow.Status == WorkflowStatus.Completed
                || trackingWorkflow.Status == WorkflowStatus.Terminated)
                return actions.ToArray();
                
            WorkflowInstance instance = Runtime.GetWorkflow(workflowId);
            Activity workflow = instance.GetWorkflowDefinition();
            ReadOnlyCollection<WorkflowQueueInfo> queues = instance.GetWorkflowQueueData();
            foreach (WorkflowQueueInfo s in queues)
            {
                if (s.SubscribedActivityNames.Count == 0)
                    continue;
                EventQueueName eventQueueName = s.QueueName as EventQueueName;
                if (eventQueueName != null)
                {
                    WorkflowAction action = new WorkflowAction();
                    action.ActionName = eventQueueName.MethodName;
                    string activityName = s.SubscribedActivityNames[0];
                    string[] splittedNames = activityName.Split('.');
                    action.StepName = splittedNames[0];
                    HandleExternalEventActivity activity = workflow.GetActivityByName(activityName) as HandleExternalEventActivity;
                    if (activity != null && activity.Roles != null && activity.Roles.Count > 0)
                    {
                        List<string> roleNames = new List<string>();
                        foreach (WorkflowRole role in activity.Roles)
                        {
                            roleNames.Add(role.Name);
                        }
                        action.QualifiedRoles = roleNames.ToArray();
                    }
                    actions.Add(action);
                }
            }
            return actions.ToArray();
        }

        static public void Submit(Guid workflowId, Guid objectId, string upn)
        {
            ContractService service = Runtime.GetService<ContractService>();
            service.Submit(workflowId, objectId, upn, ExportLogic.GetExportContractById(objectId));
            RunWorkflowInScheduler(workflowId);
        }

        static public void Approve(Guid workflowId, Guid objectId, string upn, string comment)
        {
            ContractService service = Runtime.GetService<ContractService>();
            service.Approve(workflowId, objectId, upn, comment);
            RunWorkflowInScheduler(workflowId);
        }

        static public void Reject(Guid workflowId, Guid objectId, string upn, string rejectReason)
        {
            ContractService service = Runtime.GetService<ContractService>();
            service.Reject(workflowId, objectId, upn, rejectReason);
            RunWorkflowInScheduler(workflowId);
        }

        static public void Cancel(Guid workflowId, Guid objectId, string upn)
        {
            ContractService service = Runtime.GetService<ContractService>();
            service.Cancel(workflowId, objectId, upn);
            RunWorkflowInScheduler(workflowId);
        }
    }
}
