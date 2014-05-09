using healthApp.Models;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace healthApp.Controllers {
    public class PDFFormsController : Controller {
        private FormDBContext db = new FormDBContext();

        // GET: /StandardPDFForms/
        public ActionResult Index() {
            return View( db.PDFForms.ToList() );
        }

        public FileResult GetFile( string fileName ) {
            //Response.AppendHeader("Content-Disposition: ", "inline; filename=\"" + fileName + "\";");
            string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\";

            Response.ClearHeaders();

            return File( path + fileName, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName );
        }

        public ActionResult GetForms(int? id)
        {

            if (id.HasValue)
            {
                FormDBContext serviceDB = new FormDBContext();
                var tasks = PDFForm.listForms(serviceDB, id);
                return View(tasks);
            }
            else
            {
                return RedirectToAction("Index", "Client");
            }
            
            
        }

        // GET: /StandardPDFForms/Details/5
        public ActionResult Details( int? id ) {
            if ( id == null ) {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            PDFForm pdfform = db.PDFForms.Find( id );
            if ( pdfform == null ) {
                return HttpNotFound();
            }
            return View( pdfform );
        }

        // GET: /StandardPDFForms/Create
        public ActionResult Create() {
            return View();
        }

        // POST: /StandardPDFForms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( HttpPostedFileBase file, [Bind( Include = "ID,Title,fileName,clientName,clientID" )] PDFForm pdfform ) {

            if ( ModelState.IsValid ) {

                if ( file != null && file.ContentLength > 0 ) {
                    var fileName = pdfform.fileName + ".pdf";
                    var path = Path.Combine( Server.MapPath( "~/App_Data/" ), fileName );
                    file.SaveAs( path );
                }
                pdfform.fileName += ".pdf";
                db.PDFForms.Add( pdfform );
                db.SaveChanges();
                return RedirectToAction( "Index" );
            }


            return View( pdfform );
        }

        // GET: /StandardPDFForms/Edit/5
        public ActionResult Edit( int? id ) {
            if ( id == null ) {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            PDFForm pdfform = db.PDFForms.Find( id );
            if ( pdfform == null ) {
                return HttpNotFound();
            }
            return View( pdfform );
        }

        // POST: /StandardPDFForms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( [Bind( Include = "ID,Title,fileName" )] PDFForm pdfform ) {
            if ( ModelState.IsValid ) {
                db.Entry( pdfform ).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction( "Index" );
            }
            return View( pdfform );
        }

        // GET: /StandardPDFForms/Delete/5
        public ActionResult Delete( int? id ) {
            if ( id == null ) {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            PDFForm pdfform = db.PDFForms.Find( id );
            if ( pdfform == null ) {
                return HttpNotFound();
            }

            string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\";
            string strFileFullPath = path + pdfform.fileName;

            if ( System.IO.File.Exists( strFileFullPath ) ) {
                System.IO.File.Delete( strFileFullPath );
            }
            return View( pdfform );
        }

        // POST: /StandardPDFForms/Delete/5
        [HttpPost, ActionName( "Delete" )]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed( int id ) {
            PDFForm pdfform = db.PDFForms.Find( id );
            db.PDFForms.Remove( pdfform );
            db.SaveChanges();
            return RedirectToAction( "Index" );
        }

        protected override void Dispose( bool disposing ) {
            if ( disposing ) {
                db.Dispose();
            }
            base.Dispose( disposing );
        }
    }
}
