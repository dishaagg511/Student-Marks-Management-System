using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    public class marksController : Controller
    {
        private DBtableEntities db = new DBtableEntities();

        // GET: marks
        public ActionResult Index()
        {
            var marks = db.marks.Include(m => m.subject).Include(m => m.student);
            return View(marks.ToList());
        }


        // GET: marks/Create
        public ActionResult Create()
        {
            ViewBag.SubjectId = new SelectList(db.subjects, "SubjectId", "SubjectName");
            ViewBag.StudentId = new SelectList(db.students, "id", "Candidate_Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubjectId,StudentId,Marks,Subject_Name")] mark mark)
        {
            if (ModelState.IsValid)
            {
                var subjectName = db.subjects.Where(s => s.SubjectId == mark.SubjectId).Select(s => s.SubjectName).FirstOrDefault();
                mark.Subject_Name = subjectName;
                db.marks.Add(mark);
                db.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("Index");
            }

            ViewBag.SubjectId = new SelectList(db.subjects, "SubjectId", "SubjectName", mark.SubjectId);
            ViewBag.StudentId = new SelectList(db.students, "id", "Candidate_Name", mark.StudentId);
            return View(mark);
        }

        // GET: marks/Edit/5
        public ActionResult Edit(int? studentId, int? subjectId)
        {
            mark mark = db.marks.Find(subjectId, studentId);

            return View(mark);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubjectId,StudentId,Marks,Subject_Name")] mark mark)
        {
            if (ModelState.IsValid)
            {
                mark.Subject_Name = db.subjects.Find(mark.SubjectId)?.SubjectName;

                db.Entry(mark).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mark);
        }

        // GET: marks1/Delete/5
        public ActionResult Delete(int? studentId, int? subjectId)
        {
            
            mark rec = db.marks.Find(subjectId, studentId);
            return View(rec);
        }

        // POST: marks1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? studentId, int? subjectId)
        {
            mark rec = db.marks.Find(subjectId,studentId);
            
            db.marks.Remove(rec);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
