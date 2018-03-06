using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
  public  class StreamID_DocTypeFormViewModel
    {
        public Guid? Stream_Id { set; get; }
        public DocTypeFormViewModel DocTypeFormViewModel { set; get; }
    }
}
