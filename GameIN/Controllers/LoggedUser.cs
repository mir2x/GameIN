using GameIN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameIN.Controllers
{
    public sealed class LoggedUser
    {
        
        private static LoggedUser instance;

        private LoggedUser() { }

        public static LoggedUser Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoggedUser();
                }
                return instance;
            }
        }

        public UserModel user { get; set; }
    }
}