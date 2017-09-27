using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.ComponentModel.Design;

public partial class Workplace_Pager : System.Web.UI.UserControl
{
    public delegate void ObtainDataDelegate(int currentIndex,int pageSize);
    public delegate int ObtainCountDelegate();

    public ObtainDataDelegate ObtainData=null;
    public ObtainCountDelegate ObtainCount = null;

    public int PageSize = int.Parse(Resources.GlobalResources.PageSize);
    public static int FirstIndex = 1;

    [Browsable(false)]
    public int TotalCount
    {
        get
        {
            int totalCount = 0;
            if (ViewState["TotalCount"] != null && int.TryParse(ViewState["TotalCount"].ToString(), out totalCount))
                return totalCount;

            return totalCount;
        }
        set
        {
            ViewState["TotalCount"] = value;

            this.lblCount.Text = GenerateTotalText(); //CurrentIndex.ToString() + " / " + MaxPages.ToString();
        }
    }

    private int MaxPages
    {
        get
        {
            int max =(int)Math.Ceiling(TotalCount * 1.0 / PageSize) ;
            return max == 0 ? FirstIndex : max;
        }
    }

    [Browsable(false)]
    public int CurrentIndex
    {
        get
        {
            int index = 1;
            if (ViewState["CurrentIndex"] != null && int.TryParse(ViewState["CurrentIndex"].ToString(), out index))
                return index;

            return index;
        }
        set
        {
            ViewState["CurrentIndex"] = value;

            this.lblCount.Text = GenerateTotalText(); //CurrentIndex.ToString() + " / " + MaxPages.ToString();
        }
    }

    [Category("Pagination"), Description("The text to be used on the first button. "), DefaultValue("First")]
    public string FirstPageText
    {
        get
        {
            return this.btnFirst.Text;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
                this.btnNext.Text = "First";
            else
                this.btnFirst.Text = value;
        }
    }

    [Category("Pagination"), Description("The text to be used on the last page button."), DefaultValue("Last")]
    public string LastPageText
    {
        get
        {
            return this.btnLast.Text;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
                this.btnNext.Text = "Last";
            else
                this.btnLast.Text = value;
        }
    }

    [Category("Pagination"), Description("The text to be used on the previous page button."), DefaultValue("Previous")]
    public string PreviousPageText
    {
        get
        {
            return this.btnPrevious.Text;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
                this.btnNext.Text = "Previous";
            else
                this.btnPrevious.Text = value;
        }
    }

    [Category("Pagination"), Description("The text to be used on the next page button."), DefaultValue("Next")]
    public string NextPageText
    {
        get
        {
            return this.btnNext.Text;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
                this.btnNext.Text = "Next";
            else
                this.btnNext.Text = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.lblCount.Text = GenerateTotalText();// "1 / 1";
        }
    }

    private string GenerateTotalText()
    {
        return CurrentIndex.ToString() + " / " + MaxPages.ToString();
    }

    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        if (this.CurrentIndex == FirstIndex)
            return;

        this.CurrentIndex--;

        RunObtainData();
        //RunObtainCount();
    }

    private void RunObtainData()
    {
        if (this.ObtainData != null)
            ObtainData(this.CurrentIndex, PageSize);
    }

    private void RunObtainCount()
    {
        if (this.ObtainCount != null)
            ObtainCount();
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (this.CurrentIndex == MaxPages)
            return;

        this.CurrentIndex++;

        RunObtainData();
    }
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        if (this.CurrentIndex == FirstIndex)
            return;

        this.CurrentIndex = FirstIndex;
        RunObtainData();
    }
    protected void btnLast_Click(object sender, EventArgs e)
    {
        if (this.CurrentIndex == this.MaxPages)
            return;

        this.CurrentIndex = this.MaxPages;
        RunObtainData();
    }

}
