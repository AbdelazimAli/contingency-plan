using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
   public class CompanyFormViewModel
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; }
        public string LocalName { get; set; }
        public string SearchName { get; set; }
        public int? CountryId { get; set; }
        public string Language { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string Memo { get; set; }
        public short? Purpose { get; set; }
        public string CommFileNo { get; set; }
        public string TaxCardNo { get; set; }
        public string InsuranceNo { get; set; }
        public short? TaxAuthority { get; set; }
        public bool Consolidation { get; set; } = false;
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public int Attachments { get; set; }

        // Social Insurance Information ///

        [MaxLength(50)]
        public string Region { get; set; } // Region

        [MaxLength(50)]
        public string Office { get; set; } // Office

        [MaxLength(50)]
        public string Responsible { get; set; } // Responsible Manager
        public short? LegalForm { get; set; } // Legal form  // LegalForm   
        public int? AddressId { get; set; }
        public string Address { get; set; }
        ///////////////////////////////
    }
}
