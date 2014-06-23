﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class OurServices
    {
        public int OurServicesId { get; set; }
        [DisplayName("İçerik")]
        public string Content { get; set; }
        public DateTime TimeUpdated{get;set;}
        public string Language { get; set; }
    }
}
