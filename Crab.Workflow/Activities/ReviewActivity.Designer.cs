using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Crab.Workflow.Activities
{
	public partial class ReviewActivity
	{
		#region Designer generated code
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
		private void InitializeComponent()
		{
            this.CanModifyActivities = true;
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition2 = new System.Workflow.Activities.CodeCondition();
            this.Terminate = new System.Workflow.ComponentModel.TerminateActivity();
            this.HandleCancelEvent = new System.Workflow.Activities.HandleExternalEventActivity();
            this.HandleRejectEvent = new System.Workflow.Activities.HandleExternalEventActivity();
            this.HandleApproveEvent = new System.Workflow.Activities.HandleExternalEventActivity();
            this.WaitingForCancel = new System.Workflow.Activities.EventDrivenActivity();
            this.WaitingForReject = new System.Workflow.Activities.EventDrivenActivity();
            this.WaitingForApprove = new System.Workflow.Activities.EventDrivenActivity();
            this.Waiting = new System.Workflow.Activities.ListenActivity();
            this.ifElseBranchActivity1 = new System.Workflow.Activities.IfElseBranchActivity();
            this.judgeAutoApprove = new System.Workflow.Activities.IfElseActivity();
            this.HandleSubmitEvent = new System.Workflow.Activities.HandleExternalEventActivity();
            this.ApproveAcitivity = new System.Workflow.Activities.SequenceActivity();
            this.While = new System.Workflow.Activities.WhileActivity();
            // 
            // Terminate
            // 
            this.Terminate.Error = "Cancelled";
            this.Terminate.Name = "Terminate";
            // 
            // HandleCancelEvent
            // 
            this.HandleCancelEvent.EventName = "Cancelled";
            this.HandleCancelEvent.InterfaceType = typeof(Crab.Workflow.IReviewService);
            this.HandleCancelEvent.Name = "HandleCancelEvent";
            this.HandleCancelEvent.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.HandleCancelEvent_Invoked);
            // 
            // HandleRejectEvent
            // 
            this.HandleRejectEvent.EventName = "Rejected";
            this.HandleRejectEvent.InterfaceType = typeof(Crab.Workflow.IReviewService);
            this.HandleRejectEvent.Name = "HandleRejectEvent";
            this.HandleRejectEvent.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.HandleRejectEvent_Invoked);
            // 
            // HandleApproveEvent
            // 
            this.HandleApproveEvent.EventName = "Approved";
            this.HandleApproveEvent.InterfaceType = typeof(Crab.Workflow.IReviewService);
            this.HandleApproveEvent.Name = "HandleApproveEvent";
            this.HandleApproveEvent.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.HandleApproveEvent_Invoked);
            // 
            // WaitingForCancel
            // 
            this.WaitingForCancel.Activities.Add(this.HandleCancelEvent);
            this.WaitingForCancel.Activities.Add(this.Terminate);
            this.WaitingForCancel.Name = "WaitingForCancel";
            // 
            // WaitingForReject
            // 
            this.WaitingForReject.Activities.Add(this.HandleRejectEvent);
            this.WaitingForReject.Name = "WaitingForReject";
            // 
            // WaitingForApprove
            // 
            this.WaitingForApprove.Activities.Add(this.HandleApproveEvent);
            this.WaitingForApprove.Name = "WaitingForApprove";
            // 
            // Waiting
            // 
            this.Waiting.Activities.Add(this.WaitingForApprove);
            this.Waiting.Activities.Add(this.WaitingForReject);
            this.Waiting.Activities.Add(this.WaitingForCancel);
            this.Waiting.Name = "Waiting";
            // 
            // ifElseBranchActivity1
            // 
            this.ifElseBranchActivity1.Activities.Add(this.Waiting);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.CheckAutoApprove);
            this.ifElseBranchActivity1.Condition = codecondition1;
            this.ifElseBranchActivity1.Name = "ifElseBranchActivity1";
            // 
            // judgeAutoApprove
            // 
            this.judgeAutoApprove.Activities.Add(this.ifElseBranchActivity1);
            this.judgeAutoApprove.Name = "judgeAutoApprove";
            // 
            // HandleSubmitEvent
            // 
            this.HandleSubmitEvent.EventName = "Submitted";
            this.HandleSubmitEvent.InterfaceType = typeof(Crab.Workflow.IReviewService);
            this.HandleSubmitEvent.Name = "HandleSubmitEvent";
            this.HandleSubmitEvent.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.HandleSubmitEvent_Invoked);
            // 
            // ApproveAcitivity
            // 
            this.ApproveAcitivity.Activities.Add(this.HandleSubmitEvent);
            this.ApproveAcitivity.Activities.Add(this.judgeAutoApprove);
            this.ApproveAcitivity.Name = "ApproveAcitivity";
            // 
            // While
            // 
            this.While.Activities.Add(this.ApproveAcitivity);
            codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.CheckCanPass);
            this.While.Condition = codecondition2;
            this.While.Name = "While";
            // 
            // ReviewActivity
            // 
            this.Activities.Add(this.While);
            this.Name = "ReviewActivity";
            this.CanModifyActivities = false;

		}

		#endregion

        private IfElseBranchActivity ifElseBranchActivity1;
        private IfElseActivity judgeAutoApprove;
        private TerminateActivity Terminate;
        private HandleExternalEventActivity HandleCancelEvent;
        private HandleExternalEventActivity HandleRejectEvent;
        private HandleExternalEventActivity HandleApproveEvent;
        private EventDrivenActivity WaitingForCancel;
        private EventDrivenActivity WaitingForReject;
        private EventDrivenActivity WaitingForApprove;
        private ListenActivity Waiting;
        private HandleExternalEventActivity HandleSubmitEvent;
        private SequenceActivity ApproveAcitivity;
        private WhileActivity While;























































































    }
}
