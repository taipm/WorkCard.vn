using System;
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
using CafeT.Text;

namespace Web.Controllers
{
    [Authorize]
    public class WorkIssuesController : BaseController
    {
        public WorkIssuesController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {

        }
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WorkIssues
        public async Task<ActionResult> SearchBy(string keyWord)
        {
            var _objects = _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                                .Query().Select()
                                .Where(m => !string.IsNullOrWhiteSpace(m.Title)
                                && (m.Title.Contains(keyWord)||(m.Description.Contains(keyWord)||m.Content.Contains(keyWord))))
                                .OrderByDescending(t => t.CreatedDate).ToList();

            
            return View("Index", _objects.ToList());
        }

        // GET: WorkIssues
        public async Task<ActionResult> SearchByCreateBy(string userName)
        {
            var _objects = _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                                .Query().Select()
                                .Where(m => !string.IsNullOrWhiteSpace(m.Title) && m.CreatedBy == userName)
                                .OrderByDescending(t => t.CreatedDate).ToList();

            return View("Index", _objects.ToList());
        }

        public async Task<ActionResult> Index()
        {
            var _objects = _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                                .Query().Select()
                                .Where(m => !string.IsNullOrWhiteSpace(m.Title))
                                .OrderByDescending(t => t.CreatedDate).ToList();
            if(User.Identity.IsAuthenticated)
            {
                return View("Index", _objects.Where(t=>t.CreatedBy == User.Identity.Name).ToList());
            }
            return View("Index", _objects);
        }
        
        // GET: WorkIssues/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WorkIssue workIssue = await db.Issues.FindAsync(id);
            
            if (workIssue == null)
            {
                return HttpNotFound();
            }

            return View(workIssue);
        }

        // GET: WorkIssues/Create
        [Authorize]
        public ActionResult Create()
        {
            var _objects = _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                                .Query().Select()
                                .Where(m => !string.IsNullOrWhiteSpace(m.Title))
                                .OrderByDescending(t => t.CreatedDate).ToList();
            ViewBag.Issues = _objects.ToList();

            return View();
        }

        // POST: WorkIssues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(WorkIssue workIssue)
        {
            if (ModelState.IsValid)
            {
                workIssue.Id = Guid.NewGuid();
                workIssue.CreatedDate = DateTime.Now;
                workIssue.CreatedBy = User.Identity.Name;
                workIssue.AutoAdjust();
                db.Issues.Add(workIssue);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(workIssue);
        }

        // POST: WorkIssues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        //[ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> SetWorkOnTime(Guid id, DateTime date)
        {
            WorkIssue workIssue = _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                .Query().Select().Where(t => t.Id == id).FirstOrDefault();

            workIssue.UpdatedDate = DateTime.Now;
            workIssue.UpdatedBy = User.Identity.Name;
            workIssue.End = date;
            workIssue.AutoAdjust();
            db.Entry(workIssue).State = EntityState.Modified;
            await db.SaveChangesAsync();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_WorkTime", "Work on " + date.ToShortDateString());
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> EmailNotify(Guid id)
        {
            EmailService _emailService = new EmailService();
            try
            {
                WorkIssue workIssue = await db.Issues.FindAsync(id);
                await _emailService.SendAsync(workIssue);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_WorkTime", "Sent email");
            }
            return RedirectToAction("Index");
        }
        //public void NotifyByEmail()
        //{
        //    EmailService _emailService = new EmailService();
        //    try
        //    {
        //        _emailService.SendAsync(this);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}
        [HttpPost]
        [Authorize]
        //[ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> MarkAsDone(Guid id)
        {
            WorkIssue workIssue = _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                .Query().Select().Where(t => t.Id == id).FirstOrDefault();

            workIssue.UpdatedDate = DateTime.Now;
            workIssue.UpdatedBy = User.Identity.Name;
            workIssue.Status = IssueStatus.Done;
            db.Entry(workIssue).State = EntityState.Modified;
            await db.SaveChangesAsync();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_WorkStatus", "Done");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        //[ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> MarkAsFinished(Guid id)
        {
            WorkIssue workIssue = _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                .Query().Select().Where(t => t.Id == id).FirstOrDefault();

            workIssue.UpdatedDate = DateTime.Now;
            workIssue.UpdatedBy = User.Identity.Name;
            workIssue.Status = IssueStatus.Finished;
            db.Entry(workIssue).State = EntityState.Modified;
            await db.SaveChangesAsync();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_WorkStatus", "Finished");
            }
            return RedirectToAction("Index");
        }

        // GET: WorkIssues/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkIssue workIssue = await db.Issues.FindAsync(id);
            if (workIssue == null)
            {
                return HttpNotFound();
            }
            return View(workIssue);
        }

        // POST: WorkIssues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(WorkIssue workIssue)
        {
            if (ModelState.IsValid)
            {
                workIssue.UpdatedDate = DateTime.Now;
                workIssue.UpdatedBy = User.Identity.Name;
                workIssue.AutoAdjust();
                db.Entry(workIssue).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(workIssue);
        }

        // GET: WorkIssues/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkIssue workIssue = await db.Issues.FindAsync(id);
            if (workIssue == null)
            {
                return HttpNotFound();
            }
            return View(workIssue);
        }

        // POST: WorkIssues/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            WorkIssue workIssue = await db.Issues.FindAsync(id);
            db.Issues.Remove(workIssue);
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
