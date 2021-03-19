using OnlineAdmission.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineAdmission.ModelView
{
    public class DocumentsViewModel
    {
        public int Id { get; set; }
        public string ApplicationId { get; set; }
        public Application Application { get; set; }
        public string DocumentType { get; set; }
        public string Name { get; set; }
        //public byte[] DocumentFile { get; set; }
        public HttpPostedFileBase DocumentFile { get; set; }
        public byte[] DocumentFileDB { get; set; }
    }
}