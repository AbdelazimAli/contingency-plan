using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            userIdentity.AddClaim(new Claim("TimeZone",(this.TimeZone== null? "Egypt Standard Time" : this.TimeZone)));
            userIdentity.AddClaim(new Claim("Culture", (this.Culture == null ? "en-GB" : this.Culture)));
            userIdentity.AddClaim(new Claim("Messages", (this.Messages == null ? "en-GB" : this.Messages)));
            userIdentity.AddClaim(new Claim("Language", (this.Messages == null ? "en-GB" : this.Language)));
            userIdentity.AddClaim(new Claim("DefaultCompany", (DefaultCompany == null ? 0 : DefaultCompany.Value).ToString()));
            userIdentity.AddClaim(new Claim("ShutdownInMin", (ShutdownInMin + "")));
            userIdentity.AddClaim(new Claim("AllowInsertCode", (this.AllowInsertCode +"")));
            userIdentity.AddClaim(new Claim("LogTooltip",(this.LogTooltip+"")));
            userIdentity.AddClaim(new Claim("IsAvailable", (this.IsAvailable + "")));
            userIdentity.AddClaim(new Claim("CanCustomize", (this.CanCustomize + "")));
            userIdentity.AddClaim(new Claim("Developer", (this.Developer + "")));
            userIdentity.AddClaim(new Claim("EmpId", (EmpId == null ? 0 : EmpId.Value).ToString()));

            return userIdentity;
        }
       
        // General Tab///////////////////////////////////////
        // Options
        public bool IsAvailable { get; set; } = true;
        public bool CanCustomize { get; set; } = true;
        public bool Developer { get; set; } = true;
        public int? DefferedEmp { get; set; }

        [MaxLength(15)]
        public string Culture { get; set; } = "en-GB";

        [MaxLength(15)]
        public string Messages { get; set; } = "en-GB";

        [MaxLength(15)]
        public string Language { get; set; } = "en-GB";

        [MaxLength(15)]
        public string NetworkDomain { get; set; }

        public DateTime? LastLogin { get; set; }

        public int? DefaultCountry { get; set; }

        public int? DefaultCompany { get; set; }
        
        // Interface
        public byte? Infolog { get; set; } // log Errors and Warnings

        public byte? ShutdownInMin { get; set; } // Automatic shutdown after specfic inactive minutes

        [MaxLength(50)]
        public string TimeZone { get; set; }

        public bool UploadDocs { get; set; } = true;

        public bool ExportExcel { get; set; } = true;

        public bool SuperUser { get; set; } = false;
        public bool AutoSave { get; set; } = true;

        public bool ResetPassword { get; set; } = false;

        public byte? ExportTo { get; set; } // Server: for read only / Client: has copy
        public bool AllowInsertCode { get; set; } = false;
        public bool LogTooltip { get; set; } = false;
        public int? EmpId { get; set; }

        // Notifications
        public bool WebNotify { get; set; } // Show notifications in browser or Mob app
        public bool EmailNotify { get; set; } // Recieve notification by email
        public bool SmsNotify { get; set; } // Allow to recieve notification via sms

        public static implicit operator ApplicationUser(string v)
        {
            throw new NotImplementedException();
        }
        ///////////// End of General Tab //////////////////////////////////////
        /// Notifications ////////////////////////
        // public short RecvNotPriod { get; set; } // Receive notifications every(minutes)
    }

    public class Company
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200), Required]
        public string Name { get; set; }

        [MaxLength(200)]
        public string SearchName { get; set; }
        
        public int? CountryId { get; set; }

        [MaxLength(15), Column(TypeName = "char")]
        public string Language { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string WebSite { get; set; }
        [MaxLength(500)]
        public string Memo { get; set; }
        public short? Purpose { get; set; }
        [MaxLength(20)]
        public string CommFileNo { get; set; }
        [MaxLength(20)]
        public string TaxCardNo { get; set; }
        [MaxLength(20)]
        public string InsuranceNo { get; set; }
        public short? TaxAuthority { get; set; }
        public bool Consolidation { get; set; } = false;
    }

    public class UserCompanyRole
    {
        public int Id { get; set; }

        [MaxLength(128)]
        [Index("IX_UserRole", IsUnique = true, Order = 1)]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Index("IX_UserRole", IsUnique = true, Order = 2)]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public string RoleId { get; set; }
        public IdentityRole Role { get; set; }
    }

    public class Menu
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public byte Version { get; set; } = 0;
        public int Order { get; set; }
        public int? ParentId { get; set; }
        public Menu Parent { get; set; }
        public string Url { get; set; }
        public bool IsVisible { get; set; } = true;
        public string Sort { get; set; }
        public string Icon { get; set; }
        public byte NodeType { get; set; } = 0;
        public string ColumnList { get; set; }
        public bool SSMenu { get; set; }
        public short Sequence { get; set; } = 0;
    }

    [Table("NamesTbl")]
    public class NameTbl
    {
        [Key, Column(Order = 1)]
        public string Culture { get; set; }
        [Key, Column(Order = 2)]
        public string Name { get; set; }
        public string Title { get; set; }
    }
    public class RoleMenu
    {
        [Key, Column(Order = 1), MaxLength(128)]
        public string RoleId { get; set; }

        public virtual IdentityRole Role { get; set; }

        [Key, Column(Order = 2)]
        public int MenuId { get; set; }

        public virtual Menu Menu { get; set; }

        public byte? DataLevel { get; set; }
        public virtual ICollection<Function> Functions { get; set; }
    }

    public class UserLog
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public byte LogEvent { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }

    public class Function
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MenuFunction
    {
        [Key, Column(Order = 1)]
        public int MenuId { get; set; }
        public Menu Menu { get; set; }

        [Key, Column(Order = 2)]
        public int FunctionId { get; set; }
        public Function Function { get; set; }

        public ICollection<RoleMenu> RoleMenus { get; set; }
    }

    public class HReport
    {
        public int Id { get; set; }

        [Index("IX_HReportMenu", Order = 1), MaxLength(50)]
        public string MenuName { get; set; }

        [Index("IX_HReportOrgRepor")]
        public int? OrgReportId { get; set; }

        [ForeignKey("OrgReportId")]
        public HReport OrgReport { get; set; }

        [MaxLength(50)]
        public string ReportName { get; set; }

        public byte[] ReportData { get; set; }

        [MaxLength(20)]
        public string Icon { get; set; }

        [MaxLength(50)]
        [Index("IX_HReport", IsUnique = true, Order = 1)]
        public string ReportTitle { get; set; }

        [MaxLength(15)]
        [Index("IX_HReport", IsUnique = true, Order = 2)]
        [Index("IX_HReportMenu", Order = 2)]
        public string Language { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class UserContext : IdentityDbContext<ApplicationUser>
    {
        public UserContext()
            : base("HrContext", throwIfV1Schema: false)
        {

        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<MenuFunction> MenuFunctions { get; set; }
        public DbSet<NameTbl> NamesTbl { get; set; }
        public DbSet<RoleMenu> RoleMenus { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<HReport> HReports { get; set; }
        public DbSet<UserCompanyRole> UserCompanyRoles { get; set; }
        public static UserContext Create()
        {
            return new UserContext();
        }
    }
}