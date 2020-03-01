using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrologMVC.Models
{
    public class Rezerwacje
    {
        public string DataVisitStart { get; set; }
        public string TimeStartVisit { get; set; }
        public string DataVisitEnd { get; set; }
        public string TimeVisitEnd { get; set; }
        public string FirstNameUser { get; set; }
        public string LastNameUser { get; set; }
        public string FirstNameTherapist { get; set; }
        public string LastNameTherapist { get; set; }
        public string IDTherapist { get; set; }
        public string RoomVisit { get; set; }

    }
}
