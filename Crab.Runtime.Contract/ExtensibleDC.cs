using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Crab.DataModel.Utility;
using Crab.DataModel.Common;
using Crab.Runtime.Contract;

namespace Crab.Runtime.Contract
{
    [DataContract, Serializable]
    public class ExtensibleDC
    {
        private List<StringPair> _fieldValues;

        [DataMember]
        public List<StringPair> FieldValues
        {
            get { return _fieldValues; }
            set { _fieldValues = value; }
        }

        public ExtensibleDC()
        {
        }

        public ExtensibleDC(StringPair[] values)
        {
        }

        public bool Contains(string fieldName)
        {
            return Find(fieldName) != null;
        }

        private StringPair Find(string fieldName)
        {
            if (_fieldValues == null)
                return null;
            foreach (StringPair pair in _fieldValues)
            {
                if (string.Compare(pair.Key, fieldName, true) == 0)
                    return pair;
            }
            return null;
        }

        public string GetValue(string fieldName)
        {
            StringPair pair = Find(fieldName);
            return pair != null ? pair.Value : null;
        }

        public void SetValue(string fieldName, string value)
        {
            StringPair pair = Find(fieldName);
            if (pair == null)
            {
                if (_fieldValues == null)
                    _fieldValues = new List<StringPair>();
                _fieldValues.Add(new StringPair(fieldName, value));
            }
            else
            {
                pair.Value = value;
            }
        }

        public T GetValue<T>(string fieldName)
        {
            string textValue = GetValue(fieldName);
            if (textValue == null)
                return default(T);
            else
                return (T)TypeConvert.ChangeType(textValue, typeof(T));
        }

        public void SetValue<T>(string fieldName, T value)
        {
            string textValue = value == null ? null : value.ToString();
            SetValue(fieldName, textValue);
        }

        public static string GetEntityClassName(Type entityDCType)
        {
            EntityClassAttribute[] classAttributes =
                    (EntityClassAttribute[])entityDCType.GetCustomAttributes(typeof(EntityClassAttribute), false);
            if (classAttributes == null || classAttributes.Length == 0)
                return string.Empty;
            else
                return classAttributes[0].ClassName;
        }

        public string GetEntityClassName()
        {
            return ExtensibleDC.GetEntityClassName(this.GetType());
        }
    }
}
