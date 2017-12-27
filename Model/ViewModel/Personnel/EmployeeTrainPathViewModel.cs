using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class EmployeeTrainPathViewModel
    {
        public int Id { get; set; }
        public string Employee { get; set; }
        public string TrainPathName { get; set; }
        public int CourseId { get; set; }
        public double Percent { get; set; }
        public int EmpId { get; set; }
        public IEnumerable<string> CourseName { get; set; }
        public IEnumerable<int> CourseIds { get; set; }
        public int sub { get; set; }


    }
}
