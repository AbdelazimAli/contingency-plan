using System;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class UserViewModel
    {
        public bool SuperUser { get; set; }

        public string Id { get; set; }
        public int? EmpId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        [RegularExpression("[0-9()\\-+]*")]
        public string PhoneNumber { get; set; }
        public string Culture { get; set; }
        public string Language { get; set; }
        public string Messages { get; set; }

        public string Password{ get; set; }

        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]

        public string ConfirmPassword { get; set; }
        public string NetworkDomain { get; set; }
        public DateTime? LastLogin { get; set; }

        public int? DefaultCountry { get; set; }

        public int? DefaultCompany { get; set; }

        // Interface
        public byte? Infolog { get; set; } // log Errors and Warnings

        public byte? ShutdownInMin { get; set; } // Automatic shutdown after specfic inactive minutes

        public string TimeZone { get; set; }

        public bool UploadDocs { get; set; } 

        public bool ExportExcel { get; set; } 

        public byte? ExportTo { get; set; }
        public bool SSUser { get; set; } = true;
        public bool NewUser { get; set; } = false;

        public bool ResetPassword { get; set; }
        public bool Locked { get; set; }
        public bool LogTooltip { get; set; }
        public bool AllowInsertCode { get; set; }


        // Notifications
        public bool WebNotify { get; set; } // Show notifications in browser or Mob app
        public bool EmailNotify { get; set; } // Recieve notification by email
        public bool SmsNotify { get; set; } // Allow to recieve notification via sms
    }
}