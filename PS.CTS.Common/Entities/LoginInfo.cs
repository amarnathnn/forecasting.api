using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PS.CTS.Common.Entities
{
    public class LoginInfo
    {
        public string Token { get; set; }
        public string Role { get; set; }       
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
