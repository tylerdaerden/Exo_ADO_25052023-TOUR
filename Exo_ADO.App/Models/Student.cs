using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exo_ADO.App.Models
{
    internal class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int YearResult { get; set; }
        public DateTime BirthDate { get; set; }

        public int SectionID { get; set; }
    }
}
