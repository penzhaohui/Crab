using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CrabApp
{
    /// <summary>
    /// Summary description for FieldBehavior
    /// </summary>
    public class FieldBehavior
    {
        #region private fields
        private string _fieldName;
        private ControlTypes _controlType;
        private int _width;
        private bool _readonly;
        private string _extraItem;
        #endregion

        public FieldBehavior()
        {
        }

        public FieldBehavior(string fieldName, ControlTypes controlType, int width, bool isReadonly, string extraItem)
        {
            _fieldName = fieldName;
            _controlType = controlType;
            _width = width;
            _readonly = isReadonly;
            _extraItem = extraItem;
        }

        public FieldBehavior(string fieldName, ControlTypes controlType, int width, bool isReadonly)
            :this(fieldName, controlType, width, isReadonly, null)
        {
        }

        public FieldBehavior(string fieldName, ControlTypes controlType, int width)
            :this(fieldName, controlType, width, false)
        {
        }

        public FieldBehavior(string fieldName, ControlTypes controlType)
            :this(fieldName, controlType, 0)
        {
        }

        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        public ControlTypes ControlType
        {
            get { return _controlType; }
            set { _controlType = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public bool Readonly
        {
            get { return _readonly; }
            set { _readonly = value; }
        }

        public string ExtraItem
        {
            get { return _extraItem; }
            set { _extraItem = value; }
        }
    }

}