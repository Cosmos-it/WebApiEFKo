using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardWebApi.Models
{
    public class Token
    {
        public int UserID { get; set; }
        public DateTime Issued_On { get; set; }
        public DateTime Expires_On { get; set; }
        public string AuthToken { get; set; }
    }
}