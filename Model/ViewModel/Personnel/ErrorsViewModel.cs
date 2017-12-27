using Model.ViewModel.Administration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class ErrorsViewModel
    {
        public List<Error> Errors { get; set; }
        public PeopleExportViewModel Selected { get; set; }
        public IList data { get; set; }
    }
}
