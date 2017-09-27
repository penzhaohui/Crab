using System;
using System.Collections.Generic;
using System.Text;

namespace Crab.DataModel.Data
{
    public interface IExtDataRecord
    {
        object GetValue(string fieldName);

        void SetValue(string fieldName, object value);

        bool TryGetValue(string fieldName, out object value);

        T GetValue<T>(string fieldName);

        void SetValue<T>(string fieldName, T value);
    }
}
