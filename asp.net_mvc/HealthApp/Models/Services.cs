using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace healthApp.Models
{
    public class Services
    {
        public int ID { get; set; }

        //datestamp of the creation of the event, need not be set explicitly
        public DateTime created { get; set; }

        [Display(Name = "Patient ID")]
        [Required]
        public string PatientID { get; set; }
        [Required( ErrorMessage = "This field is required" )]
        [Display( Name = "First Name" )]
        [StringLength( 12, ErrorMessage = "Name limit is 12 characters" )]
        public string ClientFirstName { get; set; }

        //Clients last name limited to length 12
        [Required( ErrorMessage = "This field is required" )]
        [Display( Name = "Last Name" )]
        [StringLength( 12, ErrorMessage = "Name limit is 12 characters" )]
        public string ClientLastName { get; set; }

        [Display(Name = "Room Number")]
        public string RoomNo { get; set; }

        public string Task { get; set; } // task description

        [Display(Name = "Duration")]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than {1} units of time")]
        public int duration { get; set; } // e.g. e hrs

        [Display(Name = "Start Date")]
        //[DataType(DataType.Date)]//to show a calendar to pick a date
        //[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)] 
        public DateTime dtStart { get; set; }

        [Display(Name = "Until")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        //[DateGreaterThan("dtStart", "Cannot be earlier than Start Date")]
        public DateTime? dtEnd { get; set; }

        [Required]
        [Display(Name = "Frequency")]
        [ValidFrequency(ErrorMessage = "is not a valid frequency")]
        public string freq { get; set; } //daily, monthly...
        public int? interval { get; set; } // every 2nd/3rd month/day
        [Display(Name = "Occurances")]
        public int? count { get; set; }  //how many times, instead of end date
        public string byDay { get; set; }  //day of weeek for weekly freq
        public int? byMonthDay { get; set; } //day of month for monthly freq

        public int dtStartWD { get; set; } // week day of the start date, needed for weekly calculations

        //ctor to set default values
        public Services()
        {
            //set the the created time to current time
            created = DateTime.Now;
            //default repeat interval of 1 
            //ie. every day, every week, every month
            interval = 1;
            //start date day of week
            dtStartWD = (int)dtStart.DayOfWeek;
        }
        public static IQueryable<Services> getTasks(ServicesDBContext db, DateTime date, String dow, int dom)
        {
            var tasks = from s in db.Services
                        where
                            // CONDITIONS: these will depend on the Calendar format that we choose
                            //every day, end date defined
                            (s.dtStart <= date && s.dtEnd >= date && s.freq.Equals("daily") && DbFunctions.DiffDays(date, s.dtStart) % s.interval == 0)
                            //s.interval == 1 && 
                            ||
                            //every day, count defined
                            (s.dtStart <= date && DbFunctions.AddDays(s.dtStart, s.count) >= date && s.freq.Equals("daily") && DbFunctions.DiffDays(date, s.dtStart) % s.interval == 0)
                            ||
                            //every week, end date defined 
                            (s.dtStart <= date && s.dtEnd >= date && s.freq.Equals("weekly") && s.byDay.Contains(dow)
                                    && (DbFunctions.DiffDays(date, DbFunctions.AddDays(s.dtStart, -s.dtStartWD)) / 7) % s.interval == 0)
                            ||
                            //every week, count defined
                            (s.dtStart <= date && DbFunctions.AddDays(s.dtStart, s.count * 7) >= date && s.freq.Equals("weekly") && s.byDay.Contains(dow)
                                    && (DbFunctions.DiffDays(date, DbFunctions.AddDays(s.dtStart, -s.dtStartWD)) / 7) % s.interval == 0)
                            ||
                            //every month, end date defined        
                            (s.dtStart <= date && s.freq.Equals("monthly") && DbFunctions.AddMonths(s.dtStart, s.count) >= date
                                     && s.byMonthDay == dom && DbFunctions.DiffMonths(date, s.dtStart) % s.interval == 0)
                            ||
                            //every month, count defined        
                            (s.dtStart <= date && s.freq.Equals("monthly") && s.dtEnd >= date
                                     && s.byMonthDay == dom && DbFunctions.DiffMonths(date, s.dtStart) % s.interval == 0)
                        select s;
            return tasks;
        }


        public static List<Services> listServices(ServicesDBContext db, int? id)
        {

            String query = "select * from Services where PatientID = " + id.ToString();

            var sqlResults = db.Services.SqlQuery(query);
            return sqlResults.ToList();

        }
    }

    public class ServicesDBContext : DbContext
    {
        public ServicesDBContext() : base("DefaultConnection") { }
        public DbSet<Services> Services { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
    }
}
