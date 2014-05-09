using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace healthApp
{
    [Serializable]
    //Rename to UserProfileObj
    public class UserProfileObj
    {
        public UserProfileObj(string un, string at)
        {
            Username = un;
            accType = at;
        }

        public string Username { get; set; }
        public string accType { get; set; }

    }
}
