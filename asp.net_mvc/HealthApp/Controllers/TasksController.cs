using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using healthApp.Models;

namespace healthApp.Controllers
{
    public class TasksController : Controller
    {
        private ServicesDBContext db = new ServicesDBContext();

        // GET: /Schedule/
        public ActionResult Index()
        {
            String[] delimited = User.Identity.Name.ToString().Split('&');
            String userID =delimited[0];
            DateTime shiftStart = DateTime.Today;
            DateTime shiftEnd = DateTime.Today;
            if (userID == "morning")
            {
                shiftStart = DateTime.Today;
                shiftEnd = DateTime.Today.AddHours(12);
            }
            else if (userID.Equals("evening"))
            {
                shiftStart = DateTime.Today.AddHours(12);
                shiftEnd = DateTime.Today.AddHours(24);
            }
            var tasks = from s in db.Tasks
                        where (s.tDate >= shiftStart && s.tDate <= shiftEnd)
                        select s;
            return View(tasks);
        }

        //open TaskComplete view to add comments and actual time
        public ActionResult TaskComplete(int id = 0)
        {
            Tasks task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // save updated task record with added comments and actual time
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaskComplete(Tasks t)
        {
            if (ModelState.IsValid)
            {

                db.Entry(t).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(t);
        }

        // GET: /Schedule/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks schedule = db.Tasks.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // GET: /Schedule/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Schedule/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( [Bind( Include = "ID, ClientFirstName, ClientLastName, taskID,PatientID,RoomNo,Task,tDate,duration,actual,comments" )] Tasks schedule )
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(schedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(schedule);
        }

        // GET: /Schedule/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks schedule = db.Tasks.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: /Schedule/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( [Bind( Include = "ID,ClientFirstName, ClientLastName,taskID,PatientID,RoomNo,Task,tDate,duration,actual,comments" )] Tasks schedule )
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(schedule);
        }

        // GET: /Schedule/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks schedule = db.Tasks.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: /Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tasks schedule = db.Tasks.Find(id);
            db.Tasks.Remove(schedule);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Schedule/
        // Sends to the PrintTasks View a set of a tasks (a shift)
        public ActionResult PrintTasks()
        {
            String[] delimited = User.Identity.Name.ToString().Split('&');
            String userID = delimited[0];

            DateTime shiftStart = DateTime.Today;
            DateTime shiftEnd = DateTime.Today;
            if (userID == "morning")
            {
                shiftStart = DateTime.Today;
                shiftEnd = DateTime.Today.AddHours(12);
            }
            else if (userID.Equals("evening"))
            {
                shiftStart = DateTime.Today.AddHours(12);
                shiftEnd = DateTime.Today.AddHours(24);
            }

            ViewBag.ShiftStart = shiftStart.ToString();
            ViewBag.ShiftEnd = shiftEnd.ToString();
            ViewBag.User = userID;

            var tasks = from s in db.Tasks
                        where (s.tDate >= shiftStart && s.tDate <= shiftEnd)
                        select s;
            return View(tasks);
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