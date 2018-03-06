using Model.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class SendFormGridVM
    {
        public int Id { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Employees { get; set; }
        public int FormId { get; set; }
        public string Jobs { get; set; }
        public string Departments { get; set; }
        public string EmpLabel { get; set; }
        public string JobLabel { get; set; }
        public string DeptLabel { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? SentDateTime { get; set; }
        public string TargetText
        {
            get
            {
                return (string.IsNullOrEmpty(Departments) ? "" : DeptLabel + ": " +  Departments + " ") + (string.IsNullOrEmpty(Jobs) ? "" : JobLabel + ": " +  Jobs + " ") + (string.IsNullOrEmpty(Employees) ? "" : EmpLabel + ": " +  Employees);
            }
        }
    }

    public class SendFormPageVM
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int FormId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public IList<int> Employees { get; set; }
        public IList<int> Jobs { get; set; }
        public IList<int> Departments { get; set; }
        public IList<FormDropDown> FormList { get; set; }
        public IList<FormDropDown> EmpList { get; set; }
        public IList<FormDropDown> JobList { get; set; }
        public IList<FormDropDown> DeptList { get; set; }

        private IList<SendFormPeople> _sendFormPeople = new List<SendFormPeople>();
        public virtual IList<SendFormPeople> sendFormPeople
        {
            get
            {
                return this._sendFormPeople;
            }
        }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
