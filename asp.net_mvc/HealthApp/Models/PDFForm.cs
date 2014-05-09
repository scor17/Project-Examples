using System;
using System.Collections.Generic;
using System.Data.Entity;

using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Web;


namespace healthApp.Models {
    public class PDFForm {
        //one variable for each feild in the table
        public int ID { get; set; }
        //Title of the document for displaying on the webapp eg. Patient Discharge Form 
        public string Title { get; set; }
        //fileName of the document for storing eg. patientDischargeForm  note does not contain .pdf
        public string fileName { get; set; }
        //Name of the client the document belongs to
        public string clientName { get; set; }
        //ID of the client the document belongs to
        public string clientID { get; set; }

        public static List<PDFForm> listForms(FormDBContext db, int? id)
        {
            var ids = id.ToString();
            
            var scoreQuery =
             from f in db.PDFForms
             where f.clientID == ids
             select f;
            
            return scoreQuery.ToList();

        }
    }

    public class FormDBContext : DbContext {
        public FormDBContext()
            : base( "DefaultConnection" ) {
        }
        public DbSet<PDFForm> PDFForms { get; set; }
    }
}