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
using CafeT.Objects;

namespace Web.Controllers
{
    
    public class WorkIssuesController : BaseController
    {
        IssuesManager _manager = new IssuesManager();
        ContactManager _contactManager = new ContactManager();
        public WorkIssuesController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {

        }
        private ApplicationDbContext db = new ApplicationDbContext();

        public Dictionary<string, string> AutoCompletedStore()
        {
            Dictionary<string, string> _dict = new Dictionary<string, string>();
            return _dict;
        }

        [HttpPost]
        [Authorize]
        public JsonResult AutoCompleted(string Prefix)
        {
            //Note : you can bind same list from database   
            Dictionary<string, string> _dict = new Dictionary<string, string>();
            var _contacts = _contactManager.GetContacts(User.Identity.Name);
            foreach (var _contact in _contacts)
            {
                if (!_contact.FirstName.IsNullOrEmptyOrWhiteSpace())
                    _dict.Add(_contact.FirstName, _contact.Email);
                if (!_contact.LastName.IsNullOrEmptyOrWhiteSpace())
                    _dict.Add(_contact.LastName, _contact.Email);
            }
            char _lastChar = Prefix.ToCharArray().LastOrDefault();
            if (_lastChar == '@')
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

            if (!_lastWord.IsNullOrEmptyOrWhiteSpace())
                Prefix = _lastWord;

            var _keyWord = _dict.Where(t => t.Key.Contains(Prefix) || t.Value.Contains(Prefix)).Select(t => t.Value);
            return Json(_keyWord, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult AutoCompleted(string Prefix)
        //{
        //    //Process Prefix
        //    string _text = Prefix;
        //    string _lastWord = Prefix.ToWords().LastOrDefault();
        //    if(_text.EndsWith(" "))
        //        _text = _text.DeleteEndTo(" ");
        //    Prefix = _lastWord;
        //    //Note : you can bind same list from database   
        //    Dictionary<string, string> _dict = new Dictionary<string, string>();
        //    _dict.Add("@Now", DateTime.Now.ToString());
        //    _dict.Add("@Today", DateTime.Today.ToString());
        //    _dict.Add("@Tomorrow", DateTime.Today.AddDays(1).ToString());
        //    _dict.Add("@Yesterday", DateTime.Today.AddDays(-1).ToString());
        //    _dict.Add("huynhquy9x@gmail.com", "Huỳnh Văn Quy");
        //    _dict.Add("taipm.vn@gmail.com", "Phan Minh Tài");
        //    var _keyWord = _dict.Where(t => t.Key.Contains(Prefix) || t.Value.Contains(Prefix)).Select(t => t.Value);
        //    return Json(_keyWord, JsonRequestBehavior.AllowGet);
        //}

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

        // GET: WorkIssues/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WorkIssue workIssue = await db.Issues.FindAsync(id);
            IssuesManager _manager = new IssuesManager();
            ViewBag.Urls = _manager.GetLinks(workIssue);

            var _questions = new IssuesManager().GetQuestion(workIssue);

            foreach(var _question in _questions)
            {
                _question.Answers = new QuestionManager().GetAnswers(_question.Id);
            }
            ViewBag.Questions = _questions;
            ViewBag.Contacts = new ContactManager().GetContactsOfIssue(workIssue.Id);

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

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpGet]
        [Authorize]
        public ActionResult AddQuestion(Guid issueId)
        {
            Question _question = new Question();
            _question.IssueId = issueId;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AddQuestionAjax", _question);
            }
            return RedirectToAction("Index");
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddQuestion(Question question)
        {
            if (ModelState.IsValid)
            {
                question.CreatedBy = User.Identity.Name;
                var _result = await _manager.AddQuestionAsync(question);
                
                if (Request.IsAjaxRequest())
                {
                    if(_result)
                        return PartialView("_QuestionItem", question);
                    else
                        return PartialView("_ErrorMsg", "Không thể thêm câu hỏi: " + question.Title);
                }
                return RedirectToAction("Index");
            }

            return View(question);
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
        public async Task<ActionResult> CreateForProject(WorkIssue workIssue)
        {
            IssuesManager _manager = new IssuesManager();
            if (ModelState.IsValid)
            {
                workIssue.CreatedBy = User.Identity.Name;
                workIssue.AutoAdjust();
                bool _isAdded = await _manager.AddIssueAsync(workIssue);
                if(_isAdded)
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Error");
            }

            return View(workIssue);
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
                if(workIssue.Content.Contains("#WorkCard"))
                {
                    var _project = db.Projects.Where(t => t.Title.ToLower().Contains("workcard")).FirstOrDefault();
                    workIssue.ProjectId = _project.Id;
                }
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
                        await _contactManager.AddContactAsync(_contact);
                    }
                }

                if(workIssue.GetLinks() != null)
                {
                    UrlManager _manager = new UrlManager();
                    var _links = workIssue.GetLinks();
                    foreach (string link in _links)
                    {
                        Url _url = new Models.Url(link);
                        _url.IssueId = workIssue.Id;
                        _url.CreatedBy = User.Identity.Name;
                        _url.CreatedDate = DateTime.Now;
                        await _manager.AddAsync(_url);
                    }
                }

                if(Request.IsAjaxRequest())
                {
                    return PartialView("_IssueItem", workIssue);
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

        // POST: WorkIssues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ShareTo(Guid id, string userName)
        {
            WorkIssue workIssue = await db.Issues.FindAsync(id);
            workIssue.UpdatedDate = DateTime.Now;
            workIssue.UpdatedBy = User.Identity.Name;
            workIssue.AutoAdjust();
            db.Entry(workIssue).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            WorkIssue workIssue = await db.Issues.FindAsync(id);
            db.Issues.Remove(workIssue);
            await db.SaveChangesAsync();
            if(Request.IsAjaxRequest())
            {
                return PartialView("_DeleteMsg", "Đã xóa");
            }
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
