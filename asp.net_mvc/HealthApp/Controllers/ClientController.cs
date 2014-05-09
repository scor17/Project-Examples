using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using healthApp.Models;

namespace healthApp.Controllers
{
    public class ClientController : ControllerAuthentication
    {
       
        List<SelectListItem> clientTypeList = new List<SelectListItem>
        {
            new SelectListItem{Text="I", Value="I"},
            new SelectListItem{Text="E", Value="E"},
            new SelectListItem{Text="L", Value="L"}
        };

        List<SelectListItem> interpreterList = new List<SelectListItem>
        {
            new SelectListItem{Text="Yes", Value="Yes"},
            new SelectListItem{Text="No", Value="No"},   
        };


        List<SelectListItem> hearinglist = new List<SelectListItem>
        {
            new SelectListItem{Text="Good", Value="Good"},
            new SelectListItem{Text="Bad", Value="Bad"},
        };


        List<SelectListItem> visionlist = new List<SelectListItem>
        {
            new SelectListItem{Text="Good", Value="Good"},
            new SelectListItem{Text="Bad", Value="Bad"},
        };
        
        List<SelectListItem>  maritalStatusList = new List<SelectListItem>
        {
            new SelectListItem{Text="Single", Value="Single"},
            new SelectListItem{Text="Married", Value="Married"},
            new SelectListItem{Text="Divorced", Value="Divorced"},
            new SelectListItem{Text="Widow", Value="Widow"}
        };


        List<SelectListItem> genderlist = new List<SelectListItem>
        {
            new SelectListItem{Text="Male", Value="Male"},
            new SelectListItem{Text="Female", Value="Female"}
                    
        };
         
        private ClientDBContext db = new ClientDBContext();
     

        // GET: /Clients/
        public ActionResult Index(int? id)
        {
            @ViewBag.Sidebar = "Client";
            if (hasUserAccess())
            {
                if (id.HasValue)
                {
                    return View(Sort(id));
                }
                else 
                {
                    return View(db.Client.ToList());
                }
            }
            return RedirectToAction("Index", "Home");
        }

        
         [HttpGet]
        public IEnumerable<Client> Sort(int? id)
        {
            
            switch (id)
            {
                case 1:
                    return  db.Client.ToList().OrderBy(profile => profile.ClientFirstName);
                case 2:
                    return  db.Client.ToList().OrderBy(profile =>profile.ClientLastName);
                case 3:
                    return db.Client.ToList().OrderBy(profile => profile.RoomNumber);
                case 4:
                    return db.Client.ToList().OrderBy(profile => profile.ClientDOB);
                default:
                    return db.Client.ToList();
            }
        }

        public ActionResult UploadPicture(int? id)
        {
            if (hasAdminAccess())
            {
                if (id == null)
                {
                    return View("Index");
                }
                Client client = db.Client.Find(id);
                if (client == null)
                {
                    return View("Index");
                }
                return View(client);
            }
            return RedirectToAction("Index");
        }

        ActionResult x;
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ViewImage(int id)
        {

            Client client = db.Client.SingleOrDefault(profile => profile.ClientID == id);
            if (client.ClientPicture == null)
            {
                x = File("/App_Data/Images/placeholder.jpg", "image/jpg");
                return x;
            }
            byte[] buffer = client.ClientPicture;
            x = File(buffer, "image/jpg", string.Format("{0}", id));
            return x;
        }

        public ActionResult Upload(HttpPostedFileBase file, int id)
        {
            if (hasAdminAccess())
            {
                if (file == null)
                {
                    return RedirectToAction("Index");
                }
                String name = file.FileName;

                System.Drawing.Image img = System.Drawing.Image.FromStream(file.InputStream);
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(Bitmap));
                byte[] imgArray = (byte[])converter.ConvertTo(img, typeof(byte[]));

                Client client = db.Client.Find(id);
                //db.Clients.Remove(client);
                //db.SaveChanges();
                client.ClientPicture = imgArray;
                //db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index", "Client");
            }
            return RedirectToAction("Index", "Client");
            // saving to the database  call should go here 
            // return View(client);
        }

        // GET: /Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return RedirectToAction("Index");
            }
            return View(client);
        }

        public ActionResult PrintDetails(int id)
        {
            ServicesDBContext serviceDB = new ServicesDBContext();
            ClientDetail cd = new ClientDetail(id, db, serviceDB);
            if (cd.client == null || cd.services == null)
            {
                return RedirectToAction("Index");
            }
            return View(cd);
        }

        public class ClientDetail
        {
            public Services services;
            public Client client;
            public ClientDetail(int id, ClientDBContext cdb, ServicesDBContext sdb)
            {
                client = cdb.Client.Find(id);
                services = sdb.Services.Find(id);
            }
        }

        // GET: /Clients/Create
        public ActionResult Create()
        {
            if (hasAdminAccess())
            {
                ViewBag.clientmaritalStatus = maritalStatusList;
                ViewBag.clientgender = genderlist;
                ViewBag.clienttype = clientTypeList;
                ViewBag.clientinterpreter = interpreterList;
                ViewBag.clienthearing = hearinglist;
                ViewBag.clientvision = visionlist;
                return View();
            }
            return RedirectToAction("Index", "Client");
        }

        // POST: /Clients/Create
        // To protect from overposting attacks, please enable thDefault1e specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClientID,ClientFirstName,ClientLastName,ClientMarital,ClientDOB,ClientHealthNum,ClientGender,RoomNumber,ClientFamilyDoc,ClientPicture,ClientType,ClientSuiteNumber,ClientSuitePhone,ClientPersonalAgent,ClientSafetyConcern,ClientRiskLevel,ClientIndoorMobility,ClientOutdoorMobility,ClientPreferredLanguage,ClientInterpreterRequired,ClientHearing,ClientDoesUnderstand,ClientVision,ClientCurrentMedication,ClientFood,ClientFamilyDoctor,ClientMedication,ClientPharmacist,ClientSpecialDiet,ClientOther,ClientDiagnosis")] Client client)
        {
            if (hasAdminAccess())
            {
                if (ModelState.IsValid)
                {
                    db.Client.Add(client);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(client);
            }
            return RedirectToAction("Index", "Client");
        }

        // GET: /Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (hasAdminAccess())
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }
                Client client = db.Client.Find(id);
                if (client == null)
                {
                    return RedirectToAction("Index");
                }

                ViewBag.clientmaritalStatus = maritalStatusList;
                ViewBag.clientgender = genderlist;
                ViewBag.clienttype = clientTypeList;
                ViewBag.clientinterpreter = interpreterList;
                ViewBag.clienthearing = hearinglist;
                ViewBag.clientvision = visionlist;
                return View(client);
            }
            return RedirectToAction("Index", "Client");
        }

        // POST: /Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClientID,ClientFirstName,ClientLastName,ClientMarital,ClientDOB,ClientHealthNum,ClientGender,RoomNumber,ClientFamilyDoc,ClientPicture,ClientType,ClientSuiteNumber,ClientSuitePhone,ClientPersonalAgent,ClientSafetyConcern,ClientRiskLevel,ClientIndoorMobility,ClientOutdoorMobility,ClientPreferredLanguage,ClientInterpreterRequired,ClientHearing,ClientDoesUnderstand,ClientVision,ClientCurrentMedication,ClientFood,ClientFamilyDoctor,ClientMedication,ClientPharmacist,ClientSpecialDiet,ClientOther,ClientDiagnosis")] Client client)
        {
            if (hasAdminAccess())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(client).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(client);
            }
            return RedirectToAction("Index", "Client");
        }

        // GET: /Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (hasAdminAccess())
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }
                Client client = db.Client.Find(id);
                if (client == null)
                {
                    return RedirectToAction("Index");
                }
                return View(client);
            }
            return RedirectToAction("Index", "Client");
        }

        // POST: /Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (hasAdminAccess())
            {
                Client client = db.Client.Find(id);
                db.Client.Remove(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Client");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GetServices(int? id)
        {
            if (hasUserAccess())
            {
                if (id.HasValue)
                {
                    ServicesDBContext serviceDB = new ServicesDBContext();
                    var tasks = Services.listServices(serviceDB, id);
                    return View(tasks);
                }
                else
                {
                    return RedirectToAction("Index", "Client");
                }
            }
            return RedirectToAction("Index", "Home");
         }
	}
}