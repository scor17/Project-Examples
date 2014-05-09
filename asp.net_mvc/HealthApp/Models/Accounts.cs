using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DevOne.Security.Cryptography.BCrypt;

namespace healthApp.Models {
    public class Accounts {
        public int ID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string encryptedPassword { get; set; }

        [Required]
        public string salt { get; set; }

        [DisplayName( "First Name" )]
        public string fName { get; set; }

        [DisplayName( "Last Name" )]
        public string lName { get; set; }

        [Required]
        [DisplayName( "Account Type" )]
        public string acctType { get; set; }

        public static Accounts createFromCredential(Credentials credential)
        {
            Accounts account = new Accounts();
            account.acctType = credential.acctType;
            account.fName = credential.fName;
            account.lName = credential.lName;
            account.Username = credential.UserName;
            account.ID = credential.ID;

            account.salt = BCryptHelper.GenerateSalt();
            account.encryptedPassword = BCryptHelper.HashPassword(credential.Password, account.salt);
            return account;
 
        }

        public static bool IsValid( string _username, string _password, AccountsDBContext db ) {

            

            Accounts user = db.Accounts.SingleOrDefault( account => account.Username == _username);
            if (user != null)
            {
                string hashed = BCryptHelper.HashPassword(_password, user.salt);

                return (hashed == user.encryptedPassword);
               
            }

            return false;
        }

        public static string findType( string _username, string _password, AccountsDBContext db ) {

            Accounts user = db.Accounts.SingleOrDefault( account => account.Username == _username );

            return user.acctType;
        }

    }

    public class ApplicationUser : IdentityUser {
    }

    public class AccountsDBContext : IdentityDbContext<ApplicationUser> {

        public AccountsDBContext()
            : base( "DefaultConnection" ) {
        }

        public DbSet<Accounts> Accounts { get; set; }
    }

}