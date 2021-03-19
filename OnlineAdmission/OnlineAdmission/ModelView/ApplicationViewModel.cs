using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using OnlineAdmission.Models;

namespace OnlineAdmission.ModelView
{
    public class ApplicationViewModel
    {

        [ForeignKey("User")]
        public string ApplicationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TelePhoneNumber { get; set; }

        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        public int ProgarmId { get; set; }
        [ForeignKey("ProgarmId")]
        public Program Program { get; set; }

        //public DateTime? RegistrationDate { get; set; }


        //[Column("Status")]
        //public string StatusString
        //{
        //    get { return Status.ToString(); }
        //    private set { Status = (Status)Enum.Parse(typeof(Status), value); }
        //}
        //[NotMapped]
        //public Status Status { get; set; }

        public ICollection<EducationDetail> EducationDetails { get; set; }
        public ICollection<EnclosedDocument> EnclosedDocuments { get; set; }

        //public int UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public HttpPostedFileBase Photo { get; set; }
        public byte[] PhotoDB { get; set; }
    }
}