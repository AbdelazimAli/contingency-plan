using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ProfileViewModel
    {

        public string UserName { get; set; }
        public string Email { get; set; }

        [RegularExpression("[0-9()\\-+]*")]
        public string PhoneNumber { get; set; }

        public string OldPassword { get; set; }

        [StringLength(100, ErrorMessage = "passWordlenght", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "confirmNotMatch")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string Culture { get; set; }
        public DateTime? LastLogin { get; set; }

        public int? DefaultCountry { get; set; }
        public int? DefaultCompany { get; set; }
        public string TimeZone { get; set; }
        public bool SuperUser { get; set; }


        // Interface
        public byte? Infolog { get; set; } // log Errors and Warnings

        public byte? ShutdownInMin { get; set; } // Automatic shutdown after specfic inactive minutes


        public bool UploadDocs { get; set; }

        public bool ExportExcel { get; set; }

        public byte? ExportTo { get; set; }

        public bool LogTooltip { get; set; }
        public bool AllowInsertCode { get; set; }

    }
}
