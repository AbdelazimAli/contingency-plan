using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class PersonFormPageVM
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public int EmpId { get; set; }
        public string Question { get; set; }
        public int FormColumnId { get; set; }
        public string Answer { get; set; }
        public List<string> AnswersList { get; set; }
        public string OtherText { get; set; }
        public int SendFormId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
