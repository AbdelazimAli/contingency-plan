using Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class DeciplineInfoViewModel
    {
        public PeriodIDTypeViewModel periodmodel { get; set; }
        public PeriodIDTypeViewModel periodPoint { get; set; }
        public DisplinRepeat ReapetObj { get; set; }
        public List< DisplinDLLViewModel> ActDisplinDDl { get; set; }
        public List<DisplinDLLViewModel> SuggDisplinDDl { get; set; }


    }
}
