using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace healthApp.Models
{
    public class Credentials
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set;}

        [DisplayName( "First Name" )]
        public string fName { get; set; }

        [DisplayName( "Last Name" )]
        public string lName { get; set; }

       // [Required]
        [DisplayName( "Account Type" )]
        public string acctType { get; set; }

        public static Credentials createFromAccount(Accounts account)
        {
            Credentials cred = new Credentials();
            cred.acctType = account.acctType;
            cred.fName = account.fName;
            cred.lName = account.lName;
            cred.UserName = account.Username;
            cred.ID = account.ID;

            cred.Password = "";
           
            
            return cred;

        }
    }
}