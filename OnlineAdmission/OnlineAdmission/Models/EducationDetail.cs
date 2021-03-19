using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineAdmission.Models
{
    public class EducationDetail
    {
        public int Id { get; set; }
        public string ApplicationId { get; set; }
        public Application Application { get; set; }
        public string Qualification { get; set; }
        public int Year { get; set; }
        public double Duration { get; set; }
        public string BoardUniversity { get; set; }
        public string Subjects { get; set; }
        public int Percentage { get; set; }

    }
}