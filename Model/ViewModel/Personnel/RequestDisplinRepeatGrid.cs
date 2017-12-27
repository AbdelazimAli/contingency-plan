using System.Collections.Generic;

namespace Model.ViewModel.Personnel
{
    public class RequestDisplinRepeatGrid
    {
      
            public IEnumerable<DisplinRepeatViewModel> inserted { get; set; }
            public IEnumerable<DisplinRepeatViewModel> updated { get; set; }
            public IEnumerable<DisplinRepeatViewModel> deleted { get; set; }
        
    }
}
