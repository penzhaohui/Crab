using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Crab.Business.Contract
{
    [DataContract, Serializable]
    public class Customer
    {
        #region private members
        private Guid _id;
        private string _name;
        private string _description;
        private string _address;
        private string _phoneNumber;
        /*private string _city;
        private string _zipCode;
        
        private string _contact;
        
        private string _fax;
        private string _mobile;
        private string _email;
        private DateTime _createDate;*/
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
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        [DataMember]
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        [DataMember]
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; }
        }

        /*[DataMember]
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        [DataMember]
        public string ZipCode
        {
            get { return _zipCode; }
            set { _zipCode = value; }
        }

        
        [DataMember]
        public string Contact
        {
            get { return _contact; }
            set { _contact = value; }
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
        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }*/
    }
}
