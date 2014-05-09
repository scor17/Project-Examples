using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace healthApp.Models
{
    public class Chats
    {

        public int ID { get; set; }
        public String UserName { get; set;}
        public String Chat { get; set;}
        public DateTime DateAdded { get; set; }

        
        public static List<Chats> getShiftChats(ChatDBContext db, DateTime dateNow) 
        {
            
            string query = "SELECT * FROM Chats WHERE datepart(hh, DateAdded)";
            if (dateNow.Hour >= 0 && dateNow.Hour <= 7)
            {
                query += "BETWEEN 0 AND 7";
            }
            else if (dateNow.Hour >= 8 && dateNow.Hour <= 15)
            {
                query += "BETWEEN 8 AND 15";
            }
            else if (dateNow.Hour >= 16 && dateNow.Hour <= 24)
            {
                query += "BETWEEN 16 AND 24";
            }
            query += " AND datepart(DD, DateAdded) LIKE " + dateNow.Day;
            query += " AND datepart(MM, DateAdded) LIKE " + dateNow.Month;
            query += " AND datepart(YYYY, DateAdded) LIKE " + dateNow.Year;
            var sqlResults = db.Chats.SqlQuery(query);
            return  sqlResults.ToList(); 
        }

        public void addToDB(ChatDBContext db){
            db.Chats.Add(this);
            db.SaveChanges();
        } 
    }

    public class ChatDBContext : DbContext 
    {
        public ChatDBContext()
            : base( "DefaultConnection" ) {
        }

        public DbSet<Chats> Chats { get; set;}
    }
}