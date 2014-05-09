using healthApp.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace healthApp.Controllers
{
    public class ServicesController : ControllerAuthentication
    {
        private ServicesDBContext db = new ServicesDBContext();

        // GET: /Task/
        public ActionResult Index()
        {
            if (hasUserAccess())
            {
                return View(db.Services.ToList());
            }
            return RedirectToAction("Index", "Home");
        }


        //populate today's Shedule based on Tasks
        public ActionResult GenerateSched()
        {
            if (hasAdminAccess())
            {
                DateTime date = DateTime.Today; //date will be set to today
                String[] days = { "Su", "M", "Tu", "W", "Th", "F", "Sa" };
                String dow = days[(int)date.DayOfWeek]; //day of week
                int dom = date.Day;
            
                 //delete previous entries to avoid duplicates
             
                 DateTime tomorrow = DateTime.Today.AddDays(1);
                 var todayt = from s in db.Tasks
                              where (s.tDate >= DateTime.Today && s.tDate <= tomorrow)
                              select s;
                 foreach (var tTask in todayt)
                 {
                     db.Tasks.Remove(tTask);
                 }
                 db.SaveChanges();

                //call static method from tasks model to get all the tasks needed to generate schedule. 
                var tasks = Services.getTasks(db, date, dow, dom);

                Tasks sc = new Tasks();
                //populate schedule with new records
                foreach (var item in tasks.ToList())
                {
                    sc.taskID = item.ID;
                    sc.PatientID = item.PatientID;
                    sc.ClientFirstName = item.ClientFirstName;
                    sc.ClientLastName = item.ClientLastName;
                    sc.Task = item.Task;
                    sc.RoomNo = item.RoomNo;
                    sc.duration = item.duration;
                    sc.tDate = DateTime.Today + item.dtStart.TimeOfDay;
                    db.Tasks.Add(sc);
                    db.SaveChanges();
                }
                return View(db.Tasks.ToList());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // GET: /Task/Details/5
        public ActionResult Details(int? id)
        {
            if (hasUserAccess())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Services tasks = db.Services.Find(id);
                if (tasks == null)
                {
                    return HttpNotFound();
                }
                return View(tasks);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        // GET: /Task/Create
        public ActionResult Create()
        {
            if (hasAdminAccess())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: /Task/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create( [Bind( Include = "ID,ClientFirstName, ClientLastName,PatientID,RoomNo,Task,duration,dtStart,dtEnd,freq,interval,count,byDay,byMonthDay" )] Services tasks )
        {
            if (hasAdminAccess())
            {
                if (ModelState.IsValid)
                {
                    db.Services.Add(tasks);
                    db.SaveChanges();
                    return RedirectToAction("Details", "Client", tasks.PatientID);
                }

                return RedirectToAction( "Details", "Client", tasks.PatientID);
            }
            return RedirectToAction("Index");
        }
        public ActionResult AddServices( int? id, string fn, string ln, int? roomNo )
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Services tasks = new Services();
            tasks.PatientID = id.ToString();
            tasks.ClientFirstName = fn.ToString();
            tasks.ClientLastName = ln.ToString();
            tasks.RoomNo = "";
            tasks.Task = "";
            tasks.duration = 0;
            tasks.freq = "";
            tasks.count = 0;
            tasks.interval = 0;
            tasks.byDay = "";
            tasks.byMonthDay = 0;
            tasks.dtStart = DateTime.Today;
            tasks.dtEnd = null;


            return View(tasks);
        }
        [HttpPost]
        public ActionResult AddServices( [Bind( Include = "ID,ClientFirstName, ClientLastName,PatientID,RoomNo,Task,duration,dtStart,dtEnd,freq,interval,count,byDay,byMonthDay" )] Services tasks )
        {
            if (ModelState.IsValid)
            {
                db.Services.Add(tasks);
                db.SaveChanges();
                return RedirectToAction("", "Client/GetServices/" + tasks.PatientID );
            }
            return View(tasks);
        }

        // GET: /Task/Edit/5
        public ActionResult Edit(int? id)
        {
            if (hasAdminAccess())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Services tasks = db.Services.Find(id);
                if (tasks == null)
                {
                    return HttpNotFound();
                }
                return View(tasks);
            }
            else
                return RedirectToAction("Index");
        }

        // POST: /Task/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit( [Bind( Include = "ID,ClientFirstName, ClientLastName, PatientID,RoomNo,Task,duration,dtStart,dtEnd,freq,interval,count,byDay,byMonthDay" )] Services tasks )
        {
            if (hasAdminAccess())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tasks).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(tasks);
            }
            else
                return RedirectToAction("Index");
        }

        // GET: /Task/Delete/5
        public ActionResult Delete(int? id)
        {
            if (hasAdminAccess())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Services tasks = db.Services.Find(id);
                if (tasks == null)
                {
                    return HttpNotFound();
                }
                return View(tasks);
            }
            else
                return RedirectToAction("Index");
        }

        // POST: /Task/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Services tasks = db.Services.Find(id);
            db.Services.Remove(tasks);
            db.SaveChanges();
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
