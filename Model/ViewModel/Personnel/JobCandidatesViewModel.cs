using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class JobCandidatesViewModel
    {
        public int Id { get; set; }
        public string EmpName { get; set; }
        public string Department { get; set; }
        public string JobName { get; set; }
        public string PosName { get; set; }
        public string Identical { get; set; }
    }

    public class EmpIdenticalViewModel
    {
        public int EmpId { get; set; }
        public string ColumnName { get; set; }
        public string JobValue { get; set; }
        public string EmpValue { get; set; }
        public byte Found { get; set; }
    }
}
