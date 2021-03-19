using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineAdmission.Models
{
    public class Program
    {
        [Key]
        public int ProgarmId { get; set; }
        public string Name { get; set; }
       
    }
}