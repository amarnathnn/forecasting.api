using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PS.CTS.Common.Entities
{

    [Table("TblFBUsers")]
    public class User
    {
        [Key]
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Region { get; set; }
        public string OfficeLocation { get; set; }
        public string Geography { get; set; }
        public bool isDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Password { get; set; }
        public string EmailId { get; set; }             
        
    }
}
