using Model.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
//using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Model.Domain.Payroll;
using Model.Domain.Notifications;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Common;
using Db.Persistence;
namespace Db
{
    // internal sealed
    internal sealed class HrContext : DbContext
    {
        static HrContext()
        {
            Database.SetInitializer<HrContext>(null);
            //DbInterception.Add(new HrUnitOfWork.CommandIntercepter());
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<MsgTbl> MsgTbl { get; set; }
        public DbSet<LookUpCode> LookUpCodes { get; set; }
        public DbSet<LookUpUserCode> LookUpUserCodes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyDocsViews> CompanyDocsView { get; set; }
        public DbSet<CompanyBranch> CompanyBranches { get; set; }
        public DbSet<ColumnTitle> ColumnTitles { get; set; }
        public DbSet<RoleColumns> RoleColumns { get; set; }
        public DbSet<PageDiv> PageDiv { get; set; }
        public DbSet<GridColumn> GridColumns { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<SysColumns> SysColumns { get; set; }
        public DbSet<World> World { get; set; }
        public DbSet<CompanyPartner> CompanyPartners { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<NameTbl> NamesTbl { get; set; }
        public  DbSet<FieldSet> FieldSets { get; set; }
        public  DbSet<Section> Sections { get; set; }
        public DbSet<FormColumn> FormsColumns { get; set; }
        public DbSet<RoleMenu> RoleMenus { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<MenuFunction> MenuFunctions { get; set; }
        public DbSet<LookUpTitles> LookUpTitles { get; set; }
        public DbSet<PersonSetup> PersonSetup { get; set; }
        public DbSet<CompanyStructure> CompanyStructures { get; set; }
        public DbSet<JobClass> JobClasses { get; set; }
        public DbSet<SystemCode> SystemCode { get; set; }
        public DbSet<CareerPath> CareerPathes { get; set; }
        public DbSet<CareerPathJobs> CareerPathJobs { get; set; }
        public DbSet<DocType> DocTypes { get; set; }
        public DbSet<DocTypeAttr> DocTypeAttrs { get; set; }
        public DbSet<CompanyDocAttr> CompanyDocAttrs { get; set; }
        public DbSet<PeopleGroup> PeopleGroups { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<CompanyBudget> CompanyBudgets { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<SubPeriod> SubPeriods { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<PosBudget> PositionBudgets { get; set; }
        public DbSet<DeptBudget> DepartmentBudgets { get; set; }
        public DbSet<BudgetItem> BudgetItems { get; set; }
        public DbSet<QualGroup> QualGroups { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Kafeel> Kafeel { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<FlexColumn> FlexColumns { get; set; }
        public DbSet<FlexData> FlexData { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveRange> LeaveRanges { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Employement> Employements { get; set; }
        public DbSet<RequestWf> RequestWf { get; set; }
        public DbSet<PeopleQual> PeopleQuals { get; set; }
        public DbSet<AudiTrail> AuditTrail { get; set; }
        public DbSet<LeaveAdjust> LeaveAdjusts { get; set; }
        public DbSet<LeaveTrans> LeaveTrans { get; set; }
        public DbSet<PeopleTraining> PeopleTraining { get; set; }
        public DbSet<TrainCourse> TrainCourses { get; set; }
        public DbSet<TrainPath> TrainPath { get; set; }
        public DbSet<Custody> Custody { get; set; }
        public DbSet<EmpCustody> EmpCustodies { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<BenefitPlan> BenefitPlans { get; set; }
        public DbSet<EmpBenefit> EmpBenefits { get; set; }
        public DbSet<EmpRelative> EmpRelative { get; set; }
        public DbSet<DisplinPeriod> DisplinPeriods { get; set; }
        public DbSet<DisPeriodNo> DisPeriodNo { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<DisplinRepeat> DisplinRepeats { get; set; }
        public DbSet<EmpDiscipline> EmpDisciplines { get; set; }
        public DbSet<Termination> Terminations { get; set; }
        public DbSet<Diagram> Diagrams { get; set; }
        public DbSet<DiagramNode> DiagramNodes { get; set; }
        public DbSet<WfRole> WfRoles { get; set; }
        public DbSet<WfTrans> WfTrans { get; set; }
        public DbSet<DisplinRange> DisplinRanges { get; set; }
        public DbSet<EmpPoints> EmpPoints { get; set; }
        public DbSet<WF_TRANS> WF_TRANS { get; set; }
        public DbSet<LeavePosting> PostingLeave { get; set; }
        public DbSet<CheckList> CheckLists { get; set; }
        public DbSet<ChecklistTask> ChecklistTasks { get; set; }
        public DbSet<EmpChkList> EmpChkLists { get; set; }
        public DbSet<EmpTask> EmpTasks { get; set; }
        public DbSet<ComplainRequest> ComplainRequests { get; set; }
        public DbSet<TermDuration> TermDurations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MsgEmployee> MsgEmployees { get; set; }
        public DbSet<PeriodName> PeriodNames { get; set; }
        public DbSet<BenefitServ> BenefitServs { get; set; }
        public DbSet<BenefitRequest> BenefitRequests { get; set; }
        public DbSet<TrainEvent> TrainEvents { get; set; }
        public DbSet<BenefitServPlans> BenefitServPlans { get; set; }
        public DbSet<Investigation> Investigations { get; set; }
        public DbSet<InvestigatEmp> InvestigatEmp { get; set; }
        public DbSet<HRLetter> HRLetters { get; set; }
        public DbSet<HRLetterLog> HRLetterLog { get; set; }
        public DbSet<DeptJobLeavePlan> DeptJobLeavePlans { get; set; }
        public DbSet<FiscalYear> FiscalYears { get; set; }
        public DbSet<GroupLeave> GroupLeaves { get; set; }
        public DbSet<GroupLeaveLog> GroupLeaveLogs { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Workflow> Workflows { get; set; }

        //////////  Notifications  ///////////////////////////////
        public DbSet<Notification> Notification { get; set; }
        public DbSet<WebMobLog> WebMobLog { get; set; }
        public DbSet<EmailLog> EmailLog { get; set; }
        public DbSet<SmsLog> SmsLog { get; set; }
        public DbSet<PagePrint> PagePrint { get; set; }
        public DbSet<NotifyLetter> NotifyLetters { get; set; }
        //PagePrint

        //public DbSet<MsgTemplate> MsgTemplates { get; set; }
        //public DbSet<MsgTemplang> MsgTemplangs { get; set; }
        public DbSet<NotifyCondition> NotifyConditions { get; set; }
        public DbSet<Filter> Filters { get; set; }
        public DbSet<EmailAccount> EmailAccounts { get; set; }
        public DbSet<SchedualTask> SchedualTasks { get; set; }
        public DbSet<RenewRequest> RenewRequests { get; set; }
        public DbSet<AssignOrder> AssignOrders { get; set; }
        public DbSet<AssignOrderTasks> AssignOrderTasks { get; set; }
        public DbSet<Mailtoken> Mailtokens { get; set; }
        public DbSet<CustodyCat> CustodyCategory { get; set; }
        public DbSet<EmpDocBorrow> EmpDocBorrow { get; set; }
        public DbSet<DocBorrowList> DocBorrowList { get; set; }
        public DbSet<FlexForm> FlexForms { get; set; }
        public DbSet<FlexFormFS> FlexFormFS { get; set; }
        public DbSet<FlexFormColumn> FlexFormColumns { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MeetAttendee> MeetAttendees { get; set; }
        public DbSet<MeetSchedual> MeetScheduals { get; set; }
        /////////////////////////////////////////

        // 110 Tables in HR
        //Payroll Tables
        public DbSet<PayRequest> PayRequests { get; set; }
        public DbSet<PayRequestDet> PayRequestDets { get; set; }
        public DbSet<PayrollSetup> PayrollSetup { get; set; }
        public DbSet<Payrolls> Payrolls { get; set; }
        public DbSet<PayrollDue> PayrollDues { get; set; }
        public DbSet<PayrollGrade> PayrollGrades { get; set; }
        public DbSet<JobPayrollGrade> JobPayrollGrades { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountSetup> AccountSetup { get; set; }
        public DbSet<SalaryItem> SalaryItems { get; set; }
        public DbSet<Formula> Formulas { get; set; }
        public DbSet<InfoTable> InfoTables { get; set; }
        public DbSet<LinkTable> LinkTables { get; set; }
        public DbSet<RangeTable> RangeTables { get; set; }
        public DbSet<SalaryVar> SalaryVar { get; set; }

        [DbFunction("CodeFirstDatabaseSchema", "fn_TrlsName")]
        public static string TrlsName(string Name, string Culture)
        {
            return "";
        }

        [DbFunction("CodeFirstDatabaseSchema", "fn_TrlsMsg")]
        public static string TrlsMsg(string Name, string Culture)
        {
            return "";
        }

        [DbFunction("CodeFirstDatabaseSchema", "fn_GetAttachments")]
        public static int GetAttachments(string Source, int SourceId)
        {
            return 0;
        }

        [DbFunction("CodeFirstDatabaseSchema", "fn_GetDocsCount")]
        public static Double GetDocsCount(int? CompanyId,int? EmpId, int? JobId)
        {
            return 0;
        }

        [DbFunction("CodeFirstDatabaseSchema", "fn_GetEmpStatus")]
        public static int GetEmpStatus(int EmpId)
        {
            return 0;
        }

        [DbFunction("CodeFirstDatabaseSchema", "fn_GetLookUpCode")] 
        public static string GetLookUpCode(string codeName, short codeId, string culture)
        {
            return "";
        }

        [DbFunction("CodeFirstDatabaseSchema", "fn_GetLookUpUserCode")]
        public static string GetLookUpUserCode(string codeName, short codeId, string culture)
        {
            return "";
        }
        [DbFunction("CodeFirstDatabaseSchema", "fn_GetSysCodeId")]
        public static short GetSysCodeId(string codeName, short codeId)
        {
            return 0;
        }

        [DbFunction("CodeFirstDatabaseSchema", "fn_GetCompanyDoc")]
        public static string GetCompanyDoc(string Source, int SourceId, int TypeId)
        {
            return "";
        }
        [DbFunction("CodeFirstDatabaseSchema", "fn_GetDoc")]
        public static string GetDoc(string Source, int SourceId)
        {
            return "";
        }

        [DbFunction("CodeFirstDatabaseSchema", "[fn_GetStramDoc]")]
        public static string GetStramDoc(string Source, int SourceId)
        {
            return "";
        }

        [DbFunction("CodeFirstDatabaseSchema", "FORMAT")]
        public static string Format(DateTime value, string format, string culture)
        {
            return (value.ToString(format, System.Globalization.CultureInfo.CreateSpecificCulture(culture)));
        }

        [DbFunction("CodeFirstDatabaseSchema", "fn_GetPageId")]
        public static int GetPageId(int CompanyId, string ObjectName, byte version)
        {
            return 0;
        }

        [DbFunction("CodeFirstDatabaseSchema", "fn_GetColumnTitle")]
        public static string GetColumnTitle(int CompanyId, string Culture, string ObjectName, byte version, string ColumnName)
        {
            return "";
        }

        [DbFunction("CodeFirstDatabaseSchema", "fn_GetWorkFlowStatus")]
        public static string GetWorkFlowStatus(string Source, int SourceId, int DocumentId, string culture)
        {
            return "";
        }
        
        //[DbFunction("CodeFirstDatabaseSchema", "fn_FilterRequestTabs")]
        //public static bool FilterRequestTabs(byte Tab, byte ApprovaStatus, DateTime Start, DateTime End)
        //{
        //    return false;
        //}


        public HrContext() : base("HrContext")
        {
        }

        public HrContext(string connectionString) : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyDocsViews>().
                MapToStoredProcedures(s => s.Insert(u => u.HasName("sp_AddCompanyDoc"))
                .Update(u => u.HasName("sp_UpdateCompanyDoc"))
                .Delete(u => u.HasName("sp_DeleteCompanyDoc")));

            modelBuilder.Entity<TrainCourse>()
           .HasMany(p => p.Pathes)
           .WithMany(r => r.Courses)
           .Map(mc => mc.ToTable("TrainPathCourses"));

           // modelBuilder.Entity<RoleMenu>()
           //.HasMany(p => p.Functions)
           //.WithMany(r => r.RoleMenus)
           //.Map(mc => mc.ToTable("RoleMenuFunctions"));

            modelBuilder.Conventions.Add(new RegisterFunctionConvention());

            base.OnModelCreating(modelBuilder);
        }
    }
    public class RegisterFunctionConvention : IStoreModelConvention<EdmModel>
    {
        public void Apply(EdmModel item, DbModel model)
        {


            // fn_GetLookUpCode
            var valueParameter1 = FunctionParameter.Create("CodeName", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            var valueParameter2 = FunctionParameter.Create("Id", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int16), ParameterMode.In);
            var valueParameter3 = FunctionParameter.Create("culture", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            var returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.ReturnValue);
            var function = this.CreateAndAddFunction(item, "fn_GetLookUpCode", new[] { valueParameter1, valueParameter2, valueParameter3 }, new[] { returnValue });

            // fn_GetLookUpUserCode
            valueParameter1 = FunctionParameter.Create("CodeName", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            valueParameter2 = FunctionParameter.Create("Id", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int16), ParameterMode.In);
            valueParameter3 = FunctionParameter.Create("culture", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.ReturnValue);
            function = this.CreateAndAddFunction(item, "fn_GetLookUpUserCode", new[] { valueParameter1, valueParameter2, valueParameter3 }, new[] { returnValue });
            // fn_GetSysCodeId
            valueParameter1 = FunctionParameter.Create("CodeName", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            valueParameter2 = FunctionParameter.Create("CodeId", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int16), ParameterMode.In);
            returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int16), ParameterMode.ReturnValue);
            function = this.CreateAndAddFunction(item, "fn_GetSysCodeId", new[] { valueParameter1, valueParameter2 }, new[] { returnValue });
            // fn_GetAttachments
            valueParameter1 = FunctionParameter.Create("Source", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            valueParameter2 = FunctionParameter.Create("SourceId", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.In);
            returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.ReturnValue);
            function = this.CreateAndAddFunction(item, "fn_GetAttachments", new[] { valueParameter1, valueParameter2 }, new[] { returnValue });

            // fn_GetDocsCount
            valueParameter1 = FunctionParameter.Create("CompanyId", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.In);
            valueParameter2 = FunctionParameter.Create("EmpId", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.In);
            valueParameter3 = FunctionParameter.Create("JobId", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.In);
            returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Double), ParameterMode.ReturnValue);
            function = this.CreateAndAddFunction(item, "fn_GetDocsCount", new[] { valueParameter1, valueParameter2, valueParameter3 }, new[] { returnValue });

            // fn_TrlsName
            valueParameter1 = FunctionParameter.Create("Name", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            valueParameter2 = FunctionParameter.Create("Culture", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.ReturnValue);
            function = this.CreateAndAddFunction(item, "fn_TrlsName", new[] { valueParameter1, valueParameter2 }, new[] { returnValue });

            // fn_TrlsMsg
            valueParameter1 = FunctionParameter.Create("Name", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            valueParameter2 = FunctionParameter.Create("Culture", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.ReturnValue);
            function = this.CreateAndAddFunction(item, "fn_TrlsMsg", new[] { valueParameter1, valueParameter2 }, new[] { returnValue });

            // fn_GetEmpStatus
            valueParameter1 = FunctionParameter.Create("EmpId", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.In);
            returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.ReturnValue);
            function = this.CreateAndAddFunction(item, "fn_GetEmpStatus", new[] { valueParameter1 }, new[] { returnValue });

            // fn_GetCompanyDoc
            valueParameter1 = FunctionParameter.Create("Source", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            valueParameter2 = FunctionParameter.Create("SourceId", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.In);
            valueParameter3 = FunctionParameter.Create("TypeId", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.In);
            returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.ReturnValue);
            function = this.CreateAndAddFunction(item, "fn_GetCompanyDoc", new[] { valueParameter1, valueParameter2, valueParameter3 }, new[] { returnValue });

            // Format
            valueParameter1 = FunctionParameter.Create("value", this.GetStorePrimitiveType(model, PrimitiveTypeKind.DateTime), ParameterMode.In);
            valueParameter2 = FunctionParameter.Create("format", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            valueParameter3 = FunctionParameter.Create("culture", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.ReturnValue);              
            function = this.CreateAndAddFunction(item, "FORMAT", new[] { valueParameter1, valueParameter2, valueParameter3 }, new[] { returnValue });

            //fn_GetPageId
            valueParameter1 = FunctionParameter.Create("CompanyId", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.In);
            valueParameter2 = FunctionParameter.Create("ObjectName", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            valueParameter3 = FunctionParameter.Create("Version", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.In);
            returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.ReturnValue);
            function = this.CreateAndAddFunction(item, "fn_GetPageId", new[] { valueParameter1, valueParameter2, valueParameter3 }, new[] { returnValue });

            //fn_GetColumnTitle
            valueParameter1 = FunctionParameter.Create("CompanyId", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.In);
            valueParameter2 = FunctionParameter.Create("culture", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            valueParameter3 = FunctionParameter.Create("ObjectName", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            var valueParameter4 = FunctionParameter.Create("Version", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.In);
            var valueParameter5 = FunctionParameter.Create("ColumnName", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.ReturnValue);
            function = this.CreateAndAddFunction(item, "fn_GetColumnTitle", new[] { valueParameter1, valueParameter2, valueParameter3, valueParameter4, valueParameter5 }, new[] { returnValue });

            // fn_GetWorkFlowStatus
            valueParameter1 = FunctionParameter.Create("Source", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            valueParameter2 = FunctionParameter.Create("SourceId", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.In);
            valueParameter3 = FunctionParameter.Create("DocumentId", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.In);
            valueParameter4 = FunctionParameter.Create("culture", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.ReturnValue);
            function = this.CreateAndAddFunction(item, "fn_GetWorkFlowStatus", new[] { valueParameter1, valueParameter2, valueParameter3, valueParameter4 }, new[] { returnValue });

            //fn_GetDoc
            var param1 = FunctionParameter.Create("Source", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            var param2 = FunctionParameter.Create("SourceId", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.In);
            returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.ReturnValue);
            function = this.CreateAndAddFunction(item, "fn_GetDoc", new[] { param1, param2 }, new[] { returnValue });
            
            //fn_GetStramDoc
            var valueparam1 = FunctionParameter.Create("Source", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            var valueparam2 = FunctionParameter.Create("SourceId", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.In);
            returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.ReturnValue);
            function = this.CreateAndAddFunction(item, "fn_GetStramDoc", new[] { valueparam1, valueparam2 }, new[] { returnValue });
            
            //// fn_FilterRequestTabs
            //valueParameter1 = FunctionParameter.Create("Tab", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Byte), ParameterMode.In);
            //valueParameter2 = FunctionParameter.Create("ApprovalStatus", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Byte), ParameterMode.In);
            //valueParameter3 = FunctionParameter.Create("Start", this.GetStorePrimitiveType(model, PrimitiveTypeKind.DateTime), ParameterMode.In);
            //valueParameter4 = FunctionParameter.Create("End", this.GetStorePrimitiveType(model, PrimitiveTypeKind.DateTime), ParameterMode.In);
            //returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Boolean), ParameterMode.ReturnValue);
            //function = this.CreateAndAddFunction(item, "fn_FilterRequestTabs", new[] { valueParameter1, valueParameter2, valueParameter3, valueParameter4 }, new[] { returnValue });
        }

        protected EdmFunction CreateAndAddFunction(EdmModel item, String name, IList<FunctionParameter> parameters, IList<FunctionParameter> returnValues)
        {
            var payload = new EdmFunctionPayload { StoreFunctionName = name, Parameters = parameters, ReturnParameters = returnValues, Schema = this.GetDefaultSchema(item), IsBuiltIn = false };
            var function = EdmFunction.Create(name, this.GetDefaultNamespace(item), item.DataSpace, payload, null);

            item.AddItem(function);

            return (function);
        }

        protected EdmType GetStorePrimitiveType(DbModel model, PrimitiveTypeKind typeKind)
        {
            return (model.ProviderManifest.GetStoreType(TypeUsage.CreateDefaultTypeUsage(PrimitiveType.GetEdmPrimitiveType(typeKind))).EdmType);
        }

        protected String GetDefaultNamespace(EdmModel layerModel)
        {
            return (layerModel.GlobalItems.OfType<EdmType>().Select(t => t.NamespaceName).Distinct().Single());
        }

        protected String GetDefaultSchema(EdmModel layerModel)
        {
            return (layerModel.Container.EntitySets.Select(s => s.Schema).Distinct().SingleOrDefault());
        }
    }
}
