using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameIN.Models
{
    public class GameModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Release { get; set; }
        public string Price { get; set; }
        public string Category { get; set; }
        public HttpPostedFileBase Poster { get; set; }
        public HttpPostedFileBase GamePlay { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public string Platform { get; set; }
        public string Processor { get; set; }   
        public string Ram { get; set; }
        public string Graphics { get; set; }
        public string Storage { get; set; }
        public string Directx { get; set; }
        public string Review { get; set; }      
    }
}