using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using Crab.DataModel;
using Crab.DataModel.Common;
using Crab.Runtime.Contract;

namespace CrabApp
{
    /// <summary>
    /// The render helper for DataModel
    /// </summary>
    public static class DataModelControlHelper
    {
        private const string _prefix = "_crab_datamodel_";
        private const int _labelWidth = 100;
        private const int _defaultWidth = 150;

        public static void RenderControls(Control container, EntityFieldDef[] fields, FieldBehavior[] behaviors, string validationGroup)
        {
            if (fields == null || fields.Length == 0)
                return;
            if (behaviors == null || behaviors.Length == 0)
            {
                behaviors = new FieldBehavior[fields.Length];
                for (int i = 0; i < fields.Length; i++)
                {
                    behaviors[i] = CreateDefaultBehavior(fields[i]);
                }
                RenderControls(container, fields, behaviors, validationGroup);
                return;
            }
            HtmlTable tbl = new HtmlTable();
            container.Controls.Add(tbl);
            foreach (FieldBehavior behavior in behaviors)
            {
                EntityFieldDef field = null;
                foreach (EntityFieldDef tempField in fields)
                {
                    if (string.Compare(tempField.Name, behavior.FieldName, true) == 0)
                    {
                        field = tempField;
                        break;
                    }
                }
                if (field == null)
                    continue;
                HtmlTableRow row = new HtmlTableRow();
                tbl.Rows.Add(row);
                HtmlTableCell cell = new HtmlTableCell();

                //label
                cell.Width = string.Format("{0}px", _labelWidth);
                cell.Align = "right";
                row.Controls.Add(cell);
                Label lblControl = new Label();
                lblControl.Text = field.Caption;
                cell.Controls.Add(lblControl);

                //seperator:
                cell = new HtmlTableCell();
                cell.InnerText = ":";
                row.Controls.Add(cell);

                //control
                cell = new HtmlTableCell();
                row.Controls.Add(cell);
                WebControl control = DataModelControlHelper.CreateControl(field, behavior);
                cell.Controls.Add(control);

                //add validators
                cell = new HtmlTableCell();
                row.Controls.Add(cell);
                BaseValidator[] validators = DataModelControlHelper.GetValidators(field, validationGroup);
                foreach (BaseValidator validator in validators)
                {
                    validator.ControlToValidate = control.ID.ToString();
                    cell.Controls.Add(validator);
                }
            }
        }

        public static void FillValues(Control container, EntityFieldDef[] fields, ExtensibleDC dc)
        {
            foreach (EntityFieldDef field in fields)
            {
                Control control = container.FindControl(_prefix + field.Name);
                if (control == null)
                    continue;
                string textValue = dc.GetValue(field.Name);
                if (control is TextBox)
                    (control as TextBox).Text = textValue;
                else if (control is DropDownList)
                    (control as DropDownList).SelectedValue = textValue;
                else if (control is Label)
                    (control as Label).Text = textValue;
            }
        }

        public static void RetrieveValues(Control container, EntityFieldDef[] fields, ExtensibleDC dc)
        {
            foreach (EntityFieldDef field in fields)
            {
                WebControl control = container.FindControl(_prefix + field.Name) as WebControl;
                if (control == null)
                    continue;
                string textValue = null;
                if (control is TextBox)
                {
                    if (!control.Enabled || !control.Visible || (control as TextBox).ReadOnly)
                        continue;
                    textValue = (control as TextBox).Text.Trim();
                }
                else if (control is DropDownList)
                {
                    if (!control.Enabled || !control.Visible)
                        continue;
                    textValue = (control as DropDownList).SelectedValue;
                }
                else
                    continue;
                dc.SetValue(field.Name, textValue);
            }
        }

        public static WebControl GetControl(Control container, string fieldName)
        {
            return container.FindControl(_prefix + fieldName) as WebControl;
        }

        public static WebControl CreateControl(EntityFieldDef field, FieldBehavior behavior)
        {
            WebControl control = null;
            switch (behavior.ControlType)
            {
                case ControlTypes.List:
                    {
                        control = new DropDownList();
                        if (!string.IsNullOrEmpty(behavior.ExtraItem))
                        {
                            if (field.Nullable)
                                (control as DropDownList).Items.Add(new ListItem(string.Empty, string.Empty));
                            string[] items = behavior.ExtraItem.Split('|');
                            foreach (string item in items)
                            {
                                string[] textValue = item.Split(',');
                                if (textValue.Length == 1)
                                    (control as DropDownList).Items.Add(new ListItem(item, item));
                                else
                                    (control as DropDownList).Items.Add(new ListItem(textValue[0], textValue[1]));
                            }
                        }
                        else if (field.DataType == DataTypes.Boolean)
                        {
                            if (field.Nullable)
                                (control as DropDownList).Items.Add(new ListItem(string.Empty, string.Empty));
                            (control as DropDownList).Items.Add(new ListItem("Yes", true.ToString()));
                            (control as DropDownList).Items.Add(new ListItem("No", false.ToString()));
                        }
                    } break;
                case ControlTypes.Label:
                    {
                        control = new Label();
                    } break;
                case ControlTypes.Text:
                case ControlTypes.MultiText:
                default:
                    {
                        TextBox textBox = new TextBox();
                        control = textBox;
                        if (behavior.ControlType == ControlTypes.MultiText)
                        {
                            textBox.TextMode = TextBoxMode.MultiLine;
                            textBox.Height = new Unit(60);

                        }
                        if (field.DataType == DataTypes.String)
                        {
                            int maxLength = field.Length;
                            if (maxLength <= 0 || maxLength > 256)
                                textBox.MaxLength = 256;
                            else
                                textBox.MaxLength = maxLength;
                        }
                        else if (field.DataType == DataTypes.Int)
                            textBox.MaxLength = 10;
                        else if (field.DataType == DataTypes.Decimal)
                            textBox.MaxLength = 18;
                    } break;
            }
            control.ID = _prefix + field.Name;
            control.Width = new Unit(behavior.Width == 0 ? _defaultWidth : behavior.Width);
            control.Enabled = !behavior.Readonly;
            return control;
        }

        public static BaseValidator[] GetValidators(EntityFieldDef field, string validationGroup)
        {
            List<BaseValidator> validators = new List<BaseValidator>();
            switch (field.DataType)
            {
                case DataTypes.DateTime:
                    {
                        CompareValidator validator = new System.Web.UI.WebControls.CompareValidator();
                        validator.Operator = ValidationCompareOperator.DataTypeCheck;
                        validator.Display = ValidatorDisplay.Dynamic;
                        validator.Type = ValidationDataType.Date;
                        validator.ErrorMessage = Resources.GlobalResources.InvalidFormat;
                        validator.ValidationGroup = validationGroup;
                        validators.Add(validator);
                    }
                    break;
                case DataTypes.Int:
                    {
                        CompareValidator validator = new System.Web.UI.WebControls.CompareValidator();
                        validator.Operator = ValidationCompareOperator.DataTypeCheck;
                        validator.Display = ValidatorDisplay.Dynamic;
                        validator.Type = ValidationDataType.Integer;
                        validator.ErrorMessage = Resources.GlobalResources.InvalidFormat;
                        validator.ValidationGroup = validationGroup;
                        validators.Add(validator);
                    } break;
                case DataTypes.Decimal:
                    {
                        CompareValidator validator = new System.Web.UI.WebControls.CompareValidator();
                        validator.Operator = ValidationCompareOperator.DataTypeCheck;
                        validator.Display = ValidatorDisplay.Dynamic;
                        validator.Type = ValidationDataType.Double;
                        validator.ErrorMessage = Resources.GlobalResources.InvalidFormat;
                        validator.ValidationGroup = validationGroup;
                        validators.Add(validator);
                    } break;
            }
            return validators.ToArray();
        }

        public static FieldBehavior CreateDefaultBehavior(EntityFieldDef field)
        {
            FieldBehavior behavior = new FieldBehavior();
            behavior.FieldName = field.Name;
            behavior.Width = _defaultWidth;
            behavior.Readonly = false;
            switch (field.DataType)
            {
                case DataTypes.Boolean:
                    {
                        behavior.ControlType = ControlTypes.List;
                        List<string> values = new List<string>();
                        values.Add("Yes," + true.ToString());
                        values.Add("No," + false.ToString());
                        behavior.ExtraItem = string.Join("|", values.ToArray());
                    } break;
                default:
                    {
                        behavior.ControlType = ControlTypes.Text;
                    }
                    break;
            }
            return behavior;
        }

    }
}