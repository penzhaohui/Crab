set netpath=%windir%\Microsoft.NET\Framework\v3.0
sqlcmd -S localhost -E -i WorkflowStoreDatabase.sql
sqlcmd -S localhost -E -d CrabWorkflowStore -i "%netpath%\Windows Workflow Foundation\SQL\EN\SqlPersistenceService_Schema.sql"
sqlcmd -S localhost -E -d CrabWorkflowStore -i "%netpath%\Windows Workflow Foundation\SQL\EN\SqlPersistenceService_Logic.sql"
sqlcmd -S localhost -E -d CrabWorkflowStore -i "%netpath%\Windows Workflow Foundation\SQL\EN\Tracking_Schema.sql"
sqlcmd -S localhost -E -d CrabWorkflowStore -i "%netpath%\Windows Workflow Foundation\SQL\EN\Tracking_Logic.sql"
sqlcmd -S localhost -E -d CrabWorkflowStore -i "WorkflowStore_InsertProfiles.sql"
