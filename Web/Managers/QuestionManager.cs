using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.Managers
{
    public class QuestionManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //public Question GetById(Guid id)
        public IEnumerable<Answer> GetAnswers(Guid id)
        {
            return db.Answers.Where(t => t.QuestionId == id).AsEnumerable();
        }
    }
}