using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    //public class Phone:BaseObject
    //{
    //    public string PhoneNumber { set; get; }
    //    public Guid ContactId { set; get; }
    //    public Phone() : base() { }
    //    public Phone(Guid contactId, string phoneNumber):base()
    //    {
    //        ContactId = contactId;
    //        PhoneNumber = phoneNumber;
    //    }
    //}
    public struct Address
    {
        string Country { set; get; }
        string City { set; get; }
        string Town { set; get; }
        string Street { set; get; }
        string AddressNumber { set; get; }
    }

    public class Contact : BaseObject
    {
        public string FirstName { set; get; } = string.Empty;
        public string LastName { set; get; } = string.Empty;
        public string Email { set; get; }
        public string HomeAddress { set; get; }
        public string OfficeName { set; get; }
        public string About { set; get; }
        public Address Address { set; get; }
        //public string Title { set; get; }
        //public string OfficeAddress { set; get; }

        public Guid? ProjectId { set; get; }
        public Guid? IssueId { set; get; }

        //public virtual IEnumerable<Phone> Phones { set; get; }

        public string UserName { set; get; }
        public bool? IsRegistered { set; get; }

        public Contact() : base() { }
        public Contact(string email):base()
        {
            Email = email;
        }
    }
}