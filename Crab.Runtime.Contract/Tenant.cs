using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Crab.Runtime.Contract
{
    [DataContract, Serializable]
    public class Tenant
    {
        #region private memebers
        private Guid _id;
        private string _name;
        private string _displayName;
        private bool _approved;
        private DateTime _createDate;
        private DateTime _endDate;
        private int _licenseCount;
        private string _contact;
        private string _phone;
        private string _fax;
        private string _mobile;
        private string _email;
        private string _website;
        private string _city;
        private string _address;
        private string _zipCode;
        #endregion

        [DataMember]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [DataMember]
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        [DataMember]
        public bool Approved
        {
            get { return _approved; }
            set { _approved = value; }
        }

        [DataMember]
        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        [DataMember]
        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        [DataMember]
        public int LicenseCount
        {
            get { return _licenseCount; }
            set { _licenseCount = value; }
        }

        [DataMember]
        public string Contact
        {
            get { return _contact; }
            set { _contact = value; }
        }

        [DataMember]
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        [DataMember]
        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        [DataMember]
        public string Mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }

        [DataMember]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        [DataMember]
        public string Website
        {
            get { return _website; }
            set { _website = value; }
        }

        [DataMember]
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        [DataMember]
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        [DataMember]
        public string ZipCode
        {
            get { return _zipCode; }
            set { _zipCode = value; }
        }

        public bool IsOverdue()
        {
            return _endDate < DateTime.Now;
        }
    }
}
