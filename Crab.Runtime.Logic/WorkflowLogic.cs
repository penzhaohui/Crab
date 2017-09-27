using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Data;
using System.Data.Common;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;
using System.Workflow.Runtime.Tracking;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel.Compiler;
using System.ComponentModel.Design.Serialization;
using System.ComponentModel.Design;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Crab.Runtime.Contract;
using Crab.Runtime.Logic.WorkflowViewer;

namespace Crab.Runtime.Logic
{
    static public class WorkflowLogic
    {
        public static WorkflowDefinition GetWorkflowDefinition(Guid tenantId, int workflowType)
        {
            Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantMetadata);
            using (DbCommand command = db.GetStoredProcCommand("crab_GetWorkflowDefinition"))
            {
                db.AddInParameter(command, "TenantId", DbType.Guid, tenantId);
                db.AddInParameter(command, "WorkflowType", DbType.Int32, workflowType);
                using (IDataReader reader = db.ExecuteReader(command))
                {
                    if (reader.Read())
                    {
                        WorkflowDefinition definition = new WorkflowDefinition();
                        definition.Id = (Guid)reader["Id"];
                        definition.WorkflowType = (int)reader["WorkflowType"];
                        definition.Xoml = (string)reader["Xoml"];
                        definition.Rules = (string)reader["Rules"];
                        return definition;
                    }
                }
            }
            return null;
        }

        public static void UpdateWorkflowDefinition(Guid tenantId, int workflowType, string xoml, string rules)
        {
            Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantMetadata);
            using (DbCommand command = db.GetStoredProcCommand("crab_UpdateWorkflowDefinition"))
            {
                db.AddInParameter(command, "TenantId", DbType.Guid, tenantId);
                db.AddInParameter(command, "WorkflowType", DbType.Int32, workflowType);
                db.AddInParameter(command, "Xoml", DbType.String, xoml);
                db.AddInParameter(command, "Rules", DbType.String, rules);
                db.ExecuteNonQuery(command);
            }
        }

        public static  byte[] GetWorkflowGraphic(Guid processId)
        {
            SqlTrackingWorkflowInstance sqlTrackingWorkflowInstance = null;
            SqlTrackingQuery sqlTrackingQuery = new SqlTrackingQuery(ConfigurationManager.ConnectionStrings[Constants.Database.TenantWorkflowStore].ConnectionString);
            if (sqlTrackingQuery.TryGetWorkflow(processId, out sqlTrackingWorkflowInstance))
            {
                if ((null != sqlTrackingWorkflowInstance) && (null != sqlTrackingWorkflowInstance.WorkflowDefinition))
                {
                    Dictionary<string, ActivityStatusInfo> activityStatusListValue = new Dictionary<string, ActivityStatusInfo>();
                    for (int index = sqlTrackingWorkflowInstance.ActivityEvents.Count; index >= 1; index--)
                    {

                        ActivityTrackingRecord activityTrackingRecord = sqlTrackingWorkflowInstance.ActivityEvents[index - 1];
                        if (!activityStatusListValue.ContainsKey(activityTrackingRecord.QualifiedName))
                        {
                            ActivityStatusInfo latestActivityStatus = new ActivityStatusInfo(activityTrackingRecord.QualifiedName, activityTrackingRecord.ExecutionStatus.ToString());
                            activityStatusListValue.Add(activityTrackingRecord.QualifiedName, latestActivityStatus);
                        }
                    }

                    Activity workflowDefinition = sqlTrackingWorkflowInstance.WorkflowDefinition;
                    return Viewer.GetWorkflowImageBinary(workflowDefinition, activityStatusListValue);
                }
            }
            return null;
        }

        public static  byte[] GetWorkflowDefinitionGraphic(Guid tenantId, int workflowType)
        {
            WorkflowDefinition definition = WorkflowLogic.GetWorkflowDefinition(tenantId, workflowType);
            ServiceContainer serviceContainer = new ServiceContainer();
            DesignerSerializationManager serializationManager = new DesignerSerializationManager(serviceContainer);
            WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
            TypeProvider typeProvider = new TypeProvider((IServiceProvider)serializationManager);
            typeProvider.AddAssembly(System.Reflection.Assembly.Load("Crab.Workflow"));
            serviceContainer.AddService(typeof(ITypeProvider), typeProvider);
            Activity rootActivity = null;
            using (serializationManager.CreateSession())
            {
                TextReader reader1 = new StringReader(definition.Xoml);
                using (XmlReader reader = XmlReader.Create(reader1))
                {
                    rootActivity = (Activity)serializer.Deserialize((IDesignerSerializationManager)serializationManager, reader);
                }
            }
            return Viewer.GetWorkflowImageBinary(rootActivity, null);
        }
    }
}
