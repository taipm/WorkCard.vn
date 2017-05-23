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
using Microsoft.Translator.API;
using System.Net.Http;

namespace Web.Controllers
{
    public class QuestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private const string SubscriptionKey = "3c45b3b12bda4d568939ab4fc7245944";   //Enter here the Key from your Microsoft Translator Text subscription on http://portal.azure.com


        /// Demonstrates getting an access token and using the token to translate.
        private static async Task<string> TranslateAsync(string text)
        {
            var translatorService = new TranslatorService.LanguageServiceClient();
            var authTokenSource = new AzureAuthToken(SubscriptionKey);
            var token = string.Empty;

            try
            {
                token = await authTokenSource.GetAccessTokenAsync();
            }
            catch (HttpRequestException)
            {
                switch (authTokenSource.RequestStatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        Console.WriteLine("Request to token service is not authorized (401). Check that the Azure subscription key is valid.");
                        break;
                    case HttpStatusCode.Forbidden:
                        Console.WriteLine("Request to token service is not authorized (403). For accounts in the free-tier, check that the account quota is not exceeded.");
                        break;
                }
                throw;
            }

            return translatorService.Translate(token, text, "en", "vi", "text/plain", "general", string.Empty);
        }

        // GET: Questions
        public async Task<ActionResult> Index()
        {
            return View(await db.Questions.OrderByDescending(t=>t.CreatedDate.Value).ToListAsync());
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpGet]
        public async Task<ActionResult> Translate(Guid questionId)
        {
            Question question = await db.Questions.FindAsync(questionId);
            question.Content = await TranslateAsync(question.Content);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_QuestionItem", question);
            }
            return RedirectToAction("Index");
        }

        // GET: Questions/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = await db.Questions.FindAsync(id);
            question.Answers = await db.Answers.Where(t => t.QuestionId == id.Value).ToListAsync();
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // GET: Questions/Create
        [Authorize]
        public ActionResult Create()
        {
            Question question = new Question();
            return View(question);
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AjaxCreate(Question question)
        {
            if (ModelState.IsValid)
            {
                question.CreatedBy = User.Identity.Name;
                db.Questions.Add(question);

                await db.SaveChangesAsync();

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_QuestionItem", question);
                }
                return RedirectToAction("Index");
            }

            return View(question);
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Question question)
        {
            if (ModelState.IsValid)
            {
                question.CreatedBy = User.Identity.Name;
                db.Questions.Add(question);

                await db.SaveChangesAsync();

                if(question.IssueId.HasValue)
                {
                    return RedirectToAction("Details", "WorkIssues",  new { id = question.IssueId.Value });
                }
                return RedirectToAction("Index");
            }

            return View(question);
        }

        // GET: Questions/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = await db.Questions.FindAsync(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(question);
        }

        // GET: Questions/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = await db.Questions.FindAsync(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Question question = await db.Questions.FindAsync(id);
            db.Questions.Remove(question);
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
