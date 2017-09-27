using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using Crab.Services;
using Crab.Services.EventArgs;
using Crab.Runtime.Contract;

namespace Crab.Workflow.Activities
{
    [ActivityDesignerThemeAttribute(typeof(ReviewActivityDesignerTheme))]
    public class ReviewActivityDesigner : SequentialActivityDesigner
    {
        public override bool CanExpandCollapse
        {
            get { return true; }
        }

        public override bool Expanded
        {
            get
            {
                return false;
            }
            set
            {
                base.Expanded = false;  //disable the expand function
            }
        }
    }
   
    internal sealed class ReviewActivityDesignerTheme : CompositeDesignerTheme
    {
        public ReviewActivityDesignerTheme(WorkflowTheme theme)
            : base(theme)
        {
            this.ShowDropShadow = true;
            this.BorderColor = Color.Black;
            this.BorderStyle = DashStyle.Solid;
            this.BackColorStart = Color.SteelBlue;
            this.BackColorEnd = Color.Blue;
            this.BackgroundStyle = LinearGradientMode.Vertical;
            this.ForeColor = Color.White;
        }
    }

    [Designer(typeof(ReviewActivityDesigner), typeof(IDesigner))]
    public partial class ReviewActivity : SequenceActivity
	{
        private bool _pass;
        private bool _autoApprove;
        public static DependencyProperty ObjectIdProperty = DependencyProperty.Register("ObjectId", typeof(Guid), typeof(ReviewActivity));
        public static DependencyProperty UpnProperty = DependencyProperty.Register("Upn", typeof(string), typeof(ReviewActivity));
        public static DependencyProperty CommentProperty = DependencyProperty.Register("Comment", typeof(string), typeof(ReviewActivity));
        public static DependencyProperty FieldNameProperty = DependencyProperty.Register("FieldName", typeof(System.String), typeof(ReviewActivity));
        public static DependencyProperty FieldValueProperty = DependencyProperty.Register("FieldValue", typeof(System.String), typeof(ReviewActivity));

        public Guid ObjectId
        {
            get { return (Guid)base.GetValue(ReviewActivity.ObjectIdProperty); }
            internal set { base.SetValue(ReviewActivity.ObjectIdProperty, value); }
        }

        public string Upn
        {
            get { return (string)base.GetValue(ReviewActivity.UpnProperty); }
            internal set { base.SetValue(ReviewActivity.UpnProperty, value); }
        }

        public Guid WorkflowId
        {
            get { return WorkflowInstanceId; }
        }

        public string Comment
        {
            get { return (string)base.GetValue(ReviewActivity.CommentProperty); }
            internal set { base.SetValue(ReviewActivity.CommentProperty, value); }
        }

        [Category("Auto Approve Conditions")]
        public string FieldName
        {
            get { return (string)base.GetValue(ReviewActivity.FieldNameProperty); }
            set { base.SetValue(ReviewActivity.FieldNameProperty, value); }
        }

        [Category("Auto Approve Conditions")]
        public string FieldValue
        {
            get { return (string)base.GetValue(ReviewActivity.FieldValueProperty); }
            set { base.SetValue(ReviewActivity.FieldValueProperty, value); }
        }

		public ReviewActivity()
		{
			InitializeComponent();
		}

        private void CheckCanPass(object sender, ConditionalEventArgs e)
        {
            e.Result = !_pass;
        }

        private void HandleApproveEvent_Invoked(object sender, ExternalDataEventArgs e)
        {
            ApproveEventArgs statusArgs = e as ApproveEventArgs;
            ObjectId = statusArgs.ObjectId;
            Upn = statusArgs.Identity;
            Comment = statusArgs.Comment;
            _pass = true;
        }

        private void HandleRejectEvent_Invoked(object sender, ExternalDataEventArgs e)
        {
            RejectEventArgs statusArgs = e as RejectEventArgs;
            ObjectId = statusArgs.ObjectId;
            Upn = statusArgs.Identity;
            Comment = statusArgs.RejectReason;
        }

        private void HandleCancelEvent_Invoked(object sender, ExternalDataEventArgs e)
        {
            Crab.Services.EventArgs.CancelEventArgs statusArgs = e as Crab.Services.EventArgs.CancelEventArgs;
            ObjectId = statusArgs.ObjectId;
            Upn = statusArgs.Identity;
        }

        private void CheckAutoApprove(object sender, ConditionalEventArgs e)
        {
            e.Result = !_autoApprove;
        }

        private void HandleSubmitEvent_Invoked(object sender, ExternalDataEventArgs e)
        {
            SubmitEventArgs args = (SubmitEventArgs)e;
            ExtensibleDC submittedObject = args.SubmittedObject;
            if (submittedObject == null || string.IsNullOrEmpty(FieldName) || string.IsNullOrEmpty(FieldValue))
            {
                _autoApprove = false;
                return;
            }
            string value = submittedObject.GetValue(FieldName);
            if (string.Compare(value, FieldValue, true) == 0)
            {
                _autoApprove = true;
                _pass = true;
            }
        }
	}
}
