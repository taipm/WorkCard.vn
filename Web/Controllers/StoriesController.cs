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

namespace Web.Controllers
{
    public class StoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Stories
        public async Task<ActionResult> Index()
        {
            return View(await db.Stories.ToListAsync());
        }

        // GET: Stories/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = await db.Stories.FindAsync(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            return View(story);
        }

        //public ActionResult GetTopArticlesFromNextSection(int lastRowId, bool isHistoryBack)
        //{
        //    var sectionArticles = BLL.SectionArticle.GetNextSectionTopArticles(lastRowId, isHistoryBack);
        //    return Json(sectionArticles, JsonRequestBehavior.AllowGet);
        //}


        // GET: Stories/Create
        //[Authorize]
        [HttpGet]
        public ActionResult AddQuestion(Guid storyId)
        {
            if(Request.IsAjaxRequest())
            {
                return PartialView("_AddQuestion", new Question() { StoryId = storyId});
            }
            return View();
        }

        // POST: Stories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> AddQuestion(Story story)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        story.Id = Guid.NewGuid();
        //        db.Stories.Add(story);
        //        await db.SaveChangesAsync();
        //        if(Request.IsAjaxRequest())
        //        {
        //            return PartialView("_")
        //        }
        //        return RedirectToAction("Index");
        //    }

        //    return View(story);
        //}

        // GET: Stories/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,ProjectId,CreatedDate,UpdatedDate,UpdatedBy,CreatedBy")] Story story)
        {
            if (ModelState.IsValid)
            {
                story.Id = Guid.NewGuid();
                db.Stories.Add(story);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(story);
        }

        // GET: Stories/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = await db.Stories.FindAsync(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            return View(story);
        }

        // POST: Stories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,ProjectId,CreatedDate,UpdatedDate,UpdatedBy,CreatedBy")] Story story)
        {
            if (ModelState.IsValid)
            {
                db.Entry(story).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(story);
        }

        // GET: Stories/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = await db.Stories.FindAsync(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            return View(story);
        }

        // POST: Stories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Story story = await db.Stories.FindAsync(id);
            db.Stories.Remove(story);
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
