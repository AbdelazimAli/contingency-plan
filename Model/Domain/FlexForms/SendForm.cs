using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class SendForm
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public FlexForm Form { get; set; }
        public int FormId { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime ExpiryDate { get; set; }

        [MaxLength(250)]
        public string Employees { get; set; }

        [MaxLength(250)]
        public string Jobs { get; set; }

        [MaxLength(250)]
        public string Departments { get; set; }
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
