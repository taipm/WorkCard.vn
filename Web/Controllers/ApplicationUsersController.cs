﻿using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;

using System.Linq;
using Web.Models;
using Microsoft.AspNet.Identity;
using Repository.Pattern.UnitOfWork;

namespace Web.Controllers
{
    public class ApplicationUsersController : BaseController
    {
        public ApplicationUsersController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }
       
        // GET: ApplicationUsers/Details/5
        public async Task<ActionResult> Details(string userName)
        {
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = UserManager.FindByNameAsync(userName).Result;
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            
            ViewBag.User = applicationUser;
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(string userName)
        {
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = UserManager.FindByNameAsync(userName).Result;
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                UserManager.Update(applicationUser);
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }
    }
}
