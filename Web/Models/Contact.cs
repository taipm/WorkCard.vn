using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class Contact : BaseObject
    {
        public string FirstName { set; get; } = string.Empty;
        public string LastName { set; get; } = string.Empty;
        public string Email { set; get; }

        public string UserName { set; get; }
        public bool? IsRegistered { set; get; }

        public Contact() : base() { }
        public Contact(string email)
        {
        }
    }
}