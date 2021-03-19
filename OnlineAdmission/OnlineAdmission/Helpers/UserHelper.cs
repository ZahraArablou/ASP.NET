using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using OnlineAdmission.Models;

namespace OnlineAdmission.Helpers
{
    public class UserHelper
    {
        public static string GetUserId(IDbSet<ApplicationUser> Users, IIdentity identity)
        {
            var user = Users.Where(u => u.Email == identity.Name).FirstOrDefault();
            if (user != null)
                return user.Id;
            else
                return "Admin";
        }


    }
}