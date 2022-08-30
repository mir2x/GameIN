using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameIN.Models
{
    public class HomeModel
    {
        public List<string> Poupular { get; set;}
        public List<string> Recents { get; set;}
        public List<string> Future { get; set; }
        public List<string> Free { get; set; }
    }
}