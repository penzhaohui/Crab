using System;

namespace Crab.DataModel.Common
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class EntityClassAttribute : System.Attribute
    {
        private string _className;

        public EntityClassAttribute()
        {
            _className = "";
        }

        public EntityClassAttribute(string className)
        {
            this._className = className;
        }

        public string ClassName
        {
            get { return this._className; }
            set { _className = value; }
        }
    }
}
