using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class ChangeVision : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            foreach (Control ctl in formPanel.Controls)
            {
                if (ctl is HtmlInputRadioButton)
                {
                    HtmlInputRadioButton radioButton = (HtmlInputRadioButton)ctl;
                    if (string.Compare(radioButton.Value, Theme, true) == 0)
                    {
                        radioButton.Checked = true;
                        break;
                    }
                }
            }
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        foreach (Control ctl in formPanel.Controls)
        {
            if (ctl is HtmlInputRadioButton)
            {
                HtmlInputRadioButton radioButton = (HtmlInputRadioButton)ctl;
                if (radioButton.Checked)
                {
                    base.ChangeTheme(radioButton.Value);
                    break;
                }
            }
        }
    }
}