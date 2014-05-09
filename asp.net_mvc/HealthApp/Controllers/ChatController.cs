using healthApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace healthApp.Controllers
{
    public class ChatController : Controller
    {
        public bool hasAccess()
        {
            return ((Session["UserProfile"] != null) );

        }
        private ChatDBContext db = new ChatDBContext();
        //
        // GET: /Chat/
        public ActionResult Index()
        {
            if (hasAccess())
            {
                return View("Chat",  Chats.getShiftChats(db, DateTime.Now));
            }
            else
            {
                return View("Index");
            }
       
        }

        public ActionResult chatPartial()
        {
            if (hasAccess())
            {
                return PartialView("ChatPartial", Chats.getShiftChats(db, DateTime.Now));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
      
        public ActionResult Chat()
        {
            return View(db.Chats.ToList());
        }
	}
}