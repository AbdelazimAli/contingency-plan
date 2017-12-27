using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class DocBorrowList
    {
        [Key, Column(Order = 1)]
        public int DocBorrowId { get; set; }
        public EmpDocBorrow DocBorrow { get; set; }

        [Key, Column(Order = 2)]
        public int DocId { get; set; }
        public DocType Doc { get; set; }
    }
}
