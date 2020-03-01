using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrologMVC.Models
{
    public class UserLogin
    {
        public int ID { get; set; }
        public string LoginUser { get; set; }
        public string PasswordUser { get; set; }
        public string FirstNameUser { get; set; }
        public string LastNameUser { get; set; }
        public string PhoneNumberUser { get; set; }
    }
}
