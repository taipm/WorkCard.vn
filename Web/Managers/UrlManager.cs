using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.Managers
{
    public class UrlManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async System.Threading.Tasks.Task<bool> AddAsync(Url url)
        {
            var _myUrls = db.Urls.Where(t => t.CreatedBy == url.CreatedBy).Select(t => t.Address);
            if (!_myUrls.Contains(url.Address))
            {
                db.Urls.Add(url);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}