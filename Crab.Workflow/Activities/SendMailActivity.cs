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

namespace Crab.Workflow.Activities
{
    [ActivityDesignerThemeAttribute(typeof(SendMailActivityDesignerTheme))]
    public class SendMailActivityDesigner : ActivityDesigner
    {

    }

    internal sealed class SendMailActivityDesignerTheme : ActivityDesignerTheme
    {
        public SendMailActivityDesignerTheme(WorkflowTheme theme)
            : base(theme)
        {
            this.BorderColor = Color.GreenYellow;
            this.BorderStyle = DashStyle.Solid;
            this.BackColorStart = Color.Green;
            this.BackColorEnd = Color.Yellow;
            this.BackgroundStyle = LinearGradientMode.Vertical;
            //this.DesignerImage = 
        }
    }

    [Designer(typeof(SendMailActivityDesigner), typeof(IDesigner))]
    public partial class SendMailActivity :Activity
	{
        public SendMailActivity()
		{
			InitializeComponent();
		}

        public static DependencyProperty FromAddressProperty = DependencyProperty.Register("FromAddress", typeof(System.String), typeof(SendMailActivity));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor)), DefaultValue(""), Browsable(true)]
        public string FromAddress
        {
            get
            {
                return ((string)(base.GetValue(SendMailActivity.FromAddressProperty)));
            }
            set
            {
                base.SetValue(SendMailActivity.FromAddressProperty, value);
            }
        }

        public static DependencyProperty ToAddressProperty = DependencyProperty.Register("ToAddress", typeof(System.String), typeof(SendMailActivity));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor)), DefaultValue(""), Browsable(true)]
        public String ToAddress
        {
            get
            {
                return ((string)(base.GetValue(SendMailActivity.ToAddressProperty)));
            }
            set
            {
                base.SetValue(SendMailActivity.ToAddressProperty, value);
            }
        }

        public static DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(System.String), typeof(SendMailActivity));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor)), DefaultValue(""), Browsable(true)]
        public String Message
        {
            get
            {
                return ((string)(base.GetValue(SendMailActivity.MessageProperty)));
            }
            set
            {
                base.SetValue(SendMailActivity.MessageProperty, value);
            }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            // send mail;
            Console.WriteLine("Sending mail...");
            return ActivityExecutionStatus.Closed;
        }
	}
}
