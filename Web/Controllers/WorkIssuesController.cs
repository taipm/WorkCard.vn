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
using Web.Helpers;
using Web.ModelViews;
using Web.Managers;

namespace Web.Controllers
{
    
    public class WorkIssuesController : BaseController
    {
        public WorkIssuesController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {

        }
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public JsonResult AutoCompleted(string Prefix)
        {
            //Process Prefix
            string _text = Prefix;
            string _lastWord = Prefix.ToWords().LastOrDefault();
            _text = _text.DeleteEndTo(" ");
            Prefix = _lastWord;
            //Note : you can bind same list from database   
            Dictionary<string, string> _dict = new Dictionary<string, string>();
            _dict.Add("@Now", DateTime.Now.ToString());
            _dict.Add("@Today", DateTime.Today.ToString());
            _dict.Add("@Tomorrow", DateTime.Today.AddDays(1).ToString());
            _dict.Add("@Yesterday", DateTime.Today.AddDays(-1).ToString());
            _dict.Add("huynhquy9x@gmail.com", "Huỳnh Văn Quy");
            _dict.Add("taipm.vn@gmail.com", "Phan Minh Tài");
            //_dict.Add("", "Phan Minh Tài");
            //_dict.Add("", "Phan Minh Tài");
            //_dict.Add("", "Phan Minh Tài");
            //_dict.Add("", "Phan Minh Tài");

            //Searching records from list using LINQ query   
            var _keyWord = _dict.Where(t => t.Key.Contains(Prefix) || t.Value.Contains(Prefix)).Select(t => t.Value);
            return Json(_keyWord, JsonRequestBehavior.AllowGet);
        }

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
            ViewBag.LastStory = _unitOfWorkAsync.RepositoryAsync<Story>().Query().Select().FirstOrDefault();

            var _objects = _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                                .Query().Select()
                                .Where(m => !string.IsNullOrWhiteSpace(m.Title))
                                .OrderByDescending(t => t.CreatedDate).ToList();
            if (User.Identity.IsAuthenticated)
            {
                return View("Index", _objects.Where(t => t.CreatedBy == User.Identity.Name).ToList());
            }
            
            return View("Index", _objects);
        }

        //[HttpGet]
        //public ActionResult Index(int? page)
        //{
        //    var dummyItems = Enumerable.Range(1, 150).Select(x => "Item " + x);
        //    var _objects = _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
        //                        .Query().Select()
        //                        .Where(m => !string.IsNullOrWhiteSpace(m.Title))
        //                        .OrderByDescending(t => t.CreatedDate).ToList();

        //    var pager = new Pager(_objects.Count(), page);

        //    var viewModel = new IndexIssuesViewModel
        //    {
        //        Items = _objects.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
        //        Pager = pager
        //    };

        //    return View(viewModel);
        //}

        // GET: WorkIssues/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WorkIssue workIssue = await db.Issues.FindAsync(id);
            IssuesManager _manager = new IssuesManager();
            //ViewBag.Urls = db.Urls.Where(t => t.IssueId.HasValue && t.IssueId == id).Select(t => t.Address).AsEnumerable();
            ViewBag.Urls = _manager.GetLinks(workIssue);

            var _questions = new IssuesManager().GetQuestion(workIssue);

            foreach(var _question in _questions)
            {
                _question.Answers = new QuestionManager().GetAnswers(_question.Id);
            }
            ViewBag.Questions = _questions;
            string _keyWord = workIssue.Content.GetHasTags().FirstOrDefault();
            var _all = db.Issues.Where(t => t.CreatedBy.ToLower() == workIssue.CreatedBy).AsEnumerable();
            if (_all != null && _all.Count()>0)
            {
                ViewBag.ReferItems = _all.Where(t => t.HasKeywords(_keyWord)).AsEnumerable();
            }
            
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
                
                if(workIssue.HasInnerMembers())
                {
                    var _innerMembers = workIssue.GetInnerMembers();
                    foreach(string _member in _innerMembers)
                    {
                        Contact _contact = new Contact();
                        _contact.Email = _member;
                        _contact.UserName = User.Identity.Name;
                        _contact.CreatedBy = User.Identity.Name;

                        db.Contacts.Add(_contact);
                        await db.SaveChangesAsync();
                    }
                }

                if(workIssue.Links != null && workIssue.Links.Count > 0)
                {
                    foreach(string link in workIssue.Links)
                    {
                        Url _url = new Models.Url(link);
                        _url.IssueId = workIssue.Id;
                        _url.CreatedBy = User.Identity.Name;
                        _url.CreatedDate = DateTime.Now;
                        db.Urls.Add(_url);
                        await db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Index");
            }

            return View(workIssue);
        }

        // POST: WorkIssues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PushEmail(Guid id)
        {
            WorkIssue workIssue = _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                .Query().Select().Where(t => t.Id == id).FirstOrDefault();

            EmailService _email = new EmailService();
            await _email.SendAsync(workIssue);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_WorkTime", "Sent email");
            }
            return RedirectToAction("Index");
        }

        // POST: WorkIssues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
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
        public async Task<ActionResult> SetTimeTodo(Guid id, int timeTodo)
        {
            WorkIssue workIssue = _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                .Query().Select().Where(t => t.Id == id).FirstOrDefault();

            workIssue.UpdatedDate = DateTime.Now;
            workIssue.UpdatedBy = User.Identity.Name;
            workIssue.TimeToDo = timeTodo;
            db.Entry(workIssue).State = EntityState.Modified;
            await db.SaveChangesAsync();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_WorkTime", "Work on " + timeTodo.ToString() + " phút");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        //[ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> SetWorkOnRepeat(Guid id, RepeatType repeat)
        {
            WorkIssue workIssue = _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                .Query().Select().Where(t => t.Id == id).FirstOrDefault();

            workIssue.UpdatedDate = DateTime.Now;
            workIssue.UpdatedBy = User.Identity.Name;
            workIssue.Repeat = repeat;
            db.Entry(workIssue).State = EntityState.Modified;
            await db.SaveChangesAsync();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_WorkTime", repeat);
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
