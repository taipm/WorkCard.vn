using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.Managers
{
    public class ContactManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //public Question GetById(Guid id)
        public async System.Threading.Tasks.Task<bool> AddAsync(Contact contact)
        {
            var _myContacts = db.Contacts.Where(t => t.UserName == contact.UserName).Select(t => t.Email);
            if (!_myContacts.Contains(contact.Email))
            {
                db.Contacts.Add(contact);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}