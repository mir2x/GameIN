using GameIN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameIN.ViewModels
{
    public class AdminViewModel
    {
        public List<UserModel> Admin { get; set; }
        public List<UserModel> Users { get; set; }
    }
}