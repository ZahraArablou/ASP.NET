using OnlineAdmission.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineAdmission.ModelView
{
    public class ApplicationsAdminViewModel
    {
        [ForeignKey("User")]
        public string ApplicationId { get; set; }

        public string FirstName { get; set; }

        public DateTime? RegistrationDate { get; set; }

        [Column("Status")]
        public string StatusString
        {
            get { return Status.ToString(); }
            private set { Status = (Status)Enum.Parse(typeof(Status), value); }
        }
        [NotMapped]
        public Status Status { get; set; }

        public ICollection<EducationDetail> EducationDetails { get; set; }

    }
}