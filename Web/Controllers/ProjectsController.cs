﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using Repository.Pattern.UnitOfWork;
using Web.Managers;
using CafeT.Text;

namespace Web.Controllers
{
    public class ProjectsController : BaseController
    {
        ProjectManager _manager = new ProjectManager();
        public ProjectsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        [HttpPost]
        public JsonResult AutoCompleted(string projectId, string Prefix)
        {
            //string projectId = string.Empty;
            Guid _projectId = Guid.NewGuid();
            if (!projectId.IsNullOrEmptyOrWhiteSpace())
                _projectId = Guid.Parse(projectId);
            
            //Note : you can bind same list from database   
            Dictionary<string, string> _dict = new Dictionary<string, string>();
            var _contacts = _manager.GetContacts(_projectId);
            foreach(var _contact in _contacts)
            {
                if(!_contact.FirstName.IsNullOrEmptyOrWhiteSpace())
                    _dict.Add(_contact.FirstName, _contact.Email);
                if (!_contact.LastName.IsNullOrEmptyOrWhiteSpace())
                    _dict.Add(_contact.FirstName, _contact.Email);
            }
            char _lastChar = Prefix.ToCharArray().LastOrDefault();
            if(_lastChar == '@')
            {
                var _emails = _contacts.Select(t => t.Email).Distinct();
                foreach (var _email in _emails)
                {
                    _dict.Add(_email, _email);
                }
            }

            //Process Prefix
            string _text = Prefix;
            string _lastWord = Prefix.ToWords().LastOrDefault();
            if (_text.EndsWith(" "))
            {
                _text = _text.DeleteEndTo(" ");
            }

            if(!_lastWord.IsNullOrEmptyOrWhiteSpace())
                Prefix = _lastWord;

            var _keyWord = _dict.Where(t => t.Key.Contains(Prefix) || t.Value.Contains(Prefix)).Select(t => t.Value);
            return Json(_keyWord, JsonRequestBehavior.AllowGet);
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public async Task<ActionResult> AddContact(Guid projectId, FormCollection collection)
        {
            List<Contact> _contacts = new List<Contact>();
            var _project = _manager.GetById(projectId);
            var _emails = collection["Content"].ToString().GetEmails();

            if(_contacts != null)
            {
                foreach(string _email in _emails)
                {
                    Contact _item = new Contact(_email);
                    _item.ProjectId = projectId;
                    _item.CreatedBy = User.Identity.Name;
                    _contacts.Add(_item);
                }
                await new ProjectManager().AddContactsAsync(projectId, _contacts);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_WorkTime", "Added contacts");
            }
            return RedirectToAction("Index");
        }


        // GET: Projects
        public async Task<ActionResult> Index()
        {
            var _objects = _unitOfWorkAsync.Repository<Project>()
                                .Query().Select().Where(m => !string.IsNullOrWhiteSpace(m.Title))
                                .OrderByDescending(t => t.CreatedDate).AsEnumerable();
            if(User.Identity.IsAuthenticated)
            {
                _objects = _objects.Where(t => t.CreatedBy == User.Identity.Name).AsEnumerable();
            }
            foreach(var item in _objects)
            {
                item.Issues = _manager.GetIssues(item.Id);
                item.Questions = _manager.GetQuestions(item.Id);
            }
            return View("Index", _objects);
        }

        // GET: Projects/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            project.Issues = _manager.GetIssues(project.Id);
            project.Contacts = _manager.GetContacts(project.Id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {
                project.Id = Guid.NewGuid();
                project.CreatedBy = User.Identity.Name;
                db.Projects.Add(project);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                project.UpdatedDate = DateTime.Now;
                project.UpdatedBy = User.Identity.Name;
                db.Entry(project).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Project project = await db.Projects.FindAsync(id);
            db.Projects.Remove(project);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
