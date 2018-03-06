using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Model.ViewModel.MissionRequest
{
    public class SiteInfoViewModel
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? dest_Latitude { get; set; }
        public double? dest_Longitude { get; set; }
    }
    public class CloseMissionCiewModel
    {
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }
        public decimal Expenses { get; set; }
        public string ValidFileExtensions { get; set; }
        public HttpPostedFileBase Image { set; get; }
        public int ErrandId { set; get; }
    }
    
}
