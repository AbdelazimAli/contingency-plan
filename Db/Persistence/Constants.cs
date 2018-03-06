using Model.ViewModel;
using System;
using System.Reflection;
using Db.Persistence.Services;
using System.Collections.Generic;
using System.Linq;
using Interface.Core;
using System.Globalization;

namespace Db.Persistence
{
    public static class Constants
    {
        public static class GoogleMaps
        {


            public static void GetLat_Long(string GPSPoint, out double Lat, out double Long)
            {
                Lat = 0;
                Long = 0;
                try
                {
                    string[] PointsList = GPSPoint.Split(',');

                    Lat = Convert.ToDouble(PointsList[0]);
                    Long = Convert.ToDouble(PointsList[1]);
                }
                catch
                {
                    Lat = 0;
                    Long = 0;
                }
            }

            //public static double GetDistance(string GPSPoint_Src, string GPSPoint_Dist)
            //{
            //    double Distance = 0;
            //    try
            //    {
            //        if (!string.IsNullOrEmpty(GPSPoint_Src) && !string.IsNullOrEmpty(GPSPoint_Dist))
            //        {
            //            double SrcLat = 0, SrcLng = 0;
            //            GetLat_Long(GPSPoint_Src, out SrcLat, out SrcLng);


            //            double DistLat = 0, DistLng = 0;
            //            GetLat_Long(GPSPoint_Dist, out DistLat, out DistLng);

            //            var DistLocation = new GeoCoordinate(DistLat, DistLng);
            //            var SrcLocation = new GeoCoordinate(SrcLat, SrcLng);


            //            Distance = DistLocation.GetDistanceTo(SrcLocation);
            //        }
            //        return Math.Round(Distance, 0);
            //    }
            //    catch
            //    {
            //        return 0;
            //    }
            //}
        }
        public static class Sources
        {
            public const string People = "People";
            public const string AssignOrders = "AssignOrders";
            public const string NewAssignment = "NewAssignment";
            public const string BorrowPapers = "BorrowPapers";
            public const string Company = "Company";
            public const string CompanyLogo = "CompanyLogo";
            public const string Employee = "Employee";
            public const string EmployeePic = "EmployeePic";
            public const string LeaveRequest = "LeaveRequest";
            public const string Meeting = "Meeting";
            public const string RecieveCustody = "RecieveCustody";
            public const string RenewRequest = "RenewRequest";
            public const string Errand = "Errand";
            public const string UserProfile = "UserProfile";
            public const string RenewContract = "RenewContract";
            public const string ContractFinish = "ContractFinish";

        }

        public static class PageDivs
        {
            public static class ObjectNames
            {
                public const string CompanyDocuments = "CompanyDocuments";
                public const string RequestWfs = "RequestWfs";
            }
            public static class TableNames
            {
                public const string CompanyDocuments = "CompanyDocuments";
                public const string RequestWf = "RequestWf";
            }
        }
        public static class SystemCodes
        {
            public static class DocType
            {

                public static string CodeName = "DocType";
                public enum DocTypeEnum
                {
                    [StringValue("Pictures & Logos")]
                    Pictures_Logos = 1,
                    [StringValue("Certifications")]
                    Certifications = 2,
                    [StringValue("Employment Papers")]
                    Employment_Papers = 3,
                    [StringValue("Government Document")]
                    Government_Document = 4,
                    [StringValue("CV")]
                    CV = 5,
                    [StringValue("Other Documents")]
                    Other_Documents = 6,
                }
            }
        }

        public static class LookUpCode
        {
            public static class Status
            {
                public const string CodeName = "Status";
                public enum StatusEnum
                {
                    [StringValue("Active")]
                    Active = 1,
                    [StringValue("InActive")]
                    InActive = 2,
                    [StringValue("Deleted")]
                    Deleted = 3,
                    [StringValue("Termination")]
                    Termination = 4,

                }
            }
        }

        public static class Enumerations
        {
            public enum RequiredOptEnum
            {
                [StringValue("Not Required")]
                Not_Required = 0,
                [StringValue("Required for all jobs")]
                Required_For_All_Jobs = 1,
                [StringValue("Required for some jobs")]
                Required_For_Some_Jobs = 2,
            }
            public enum VersionsEnum
            {
                Zero = 0,
                One = 1,
                Two = 2
            }
            public enum InputTypesEnum
            {
                Text = 1,
                Number = 2,
                Select = 3,
                Date = 4,
                Time = 5,
                DateTime = 6,
                TextArea = 7,
                CheckBox = 8

            }
            public enum UploadFileTypesEnum
            {
                Image,
                Pdf,
                Word,
                Unknown
            }

            public enum ScheduleSourcesEnum
            {
                [StringValue("Leave")]
                Leave,
                [StringValue("Meeting")]
                Meeting,
                [StringValue("Errand")]
                Errand,
                [StringValue("AssignOrder")]
                AssignOrder,
            }
        }
        public static class CompanyDocAttr
        {
            public static string getValue(CompanyDocAttrViewModel model, string Culture)
            {
                if (string.IsNullOrEmpty(model.ValueText))
                    return string.Empty;

                string value;
                switch (model.InputType)
                {

                    //case 3:  //select
                    //    value = model.ValueText;
                    //    break;
                    case (int)Enumerations.InputTypesEnum.Date:  
                        value = model.ValueText.ToMyDateTime(Culture).ToMyDateString(Culture, "yyyy/MM/dd");
                        break;
                    case (int)Enumerations.InputTypesEnum.Time:  
                        value = model.ValueText;
                        break;
                    case (int)Enumerations.InputTypesEnum.DateTime:  
                        value = model.ValueText.ToMyDateTime(Culture).ToMyDateString(Culture, "yyyy/MM/dd hh:mm");
                        break;
                    case (int)Enumerations.InputTypesEnum.CheckBox:  
                        value = model.ValueText;
                        break;
                    default:
                        value = model.Value;
                        break;
                }
                return value;
            }
        }

        public static class KendoGridValidation
        {
            private static bool CheckRequiredValidation(CompanyDocAttrViewModel row, string Culture, ref List<string> ErrorMessages)
            {
                try
                {
                    if (row.IsRequired && string.IsNullOrEmpty(row.ValueText))
                    {
                        ErrorMessages.Add(string.Format(MsgUtils.Instance.Trls(Culture, "this field is required : ") + " {0}", row.Attribute));
                        return false;
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public static void AddErrorMessage(CompanyDocAttrViewModel row, string Culture, ref List<string> ErrorMessages)
            {
                try
                {
                    ErrorMessages.Add(string.Format(MsgUtils.Instance.Trls(Culture, "Invalid data in field") + " {0}", row.Attribute));
                }
                catch
                {

                }
            }

            public static bool IsValid(IEnumerable<CompanyDocAttrViewModel> DataList, string Culture, out string ErrorMessage)
            {
                bool IsValid = true;
                List<string> ErrorMessages = new List<string>();
                ErrorMessage = string.Empty;
                foreach (var row in DataList)
                {

                    bool IsRequiredValid_Text = CheckRequiredValidation(row, Culture, ref ErrorMessages);
                    if (!IsRequiredValid_Text)
                    {
                        IsValid = false;
                        continue;
                    }

                    switch (row.InputType)
                    {

                        case (int)Enumerations.InputTypesEnum.Number:  //date


                            double ValueNumber;
                            if (!string.IsNullOrEmpty(row.ValueText) && !double.TryParse(row.ValueText, out ValueNumber))
                            {
                                AddErrorMessage(row, Culture, ref ErrorMessages);
                                IsValid = false;
                            }
                            break;
                        case (int)Enumerations.InputTypesEnum.Date:  //date
                        case (int)Enumerations.InputTypesEnum.DateTime:

                            DateTime ValueDate;
                            if (!string.IsNullOrEmpty(row.ValueText) && !DateTime.TryParse(row.ValueText, out ValueDate))
                            {

                                AddErrorMessage(row, Culture, ref ErrorMessages);
                                IsValid = false;
                            }
                            break;
                        case (int)Enumerations.InputTypesEnum.Time:  //time

                            string TextToValidate = ConvertTime12To24(row.ValueText);
                            DateTime dt;
                            if (!string.IsNullOrEmpty(row.ValueText) && !DateTime.TryParseExact(TextToValidate, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                            {
                                AddErrorMessage(row, Culture, ref ErrorMessages);
                                IsValid = false;
                            }

                            break;
                        default:
                            break;
                    }
                }

                ErrorMessage = (ErrorMessages.Count() > 0) ? ErrorMessages.Aggregate((e1, e2) => e1 + " , " + e2) : string.Empty;
                return IsValid;
            }

            private static string ConvertTime12To24(string CurrentTime)
            {
                try
                {
                    CurrentTime = CurrentTime.Replace("ص", "AM").Replace("م", "PM");
                    var timeString = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + CurrentTime).ToString("HH:mm");
                    return timeString;
                }
                catch
                {
                    return CurrentTime;
                }
            }
        }


        public static class MessagesKeys
        {
            public const string InvalidData = "InvalidData";
        }

        public static class CheckDateRangAndTasksConflict
        {
            public static bool IsValid(IHrUnitOfWork _hrUnitOfWork, string EmpIds, string Source, int SourceId, DateTime StartTime, DateTime EndTime, DateTime DateToCheck, string Language,out string ErrorMessage)
            {
                ErrorMessage = string.Empty;
                bool IsValid = true;
                var Tasks = _hrUnitOfWork.MeetingRepository.GetEmployeeSchedual(EmpIds, StartTime, EndTime, Source, SourceId, Language).Select(a => a.Tasks).ToList();
                foreach (var item in Tasks)
                {
                    if (item.Select(s => s.EndTime).FirstOrDefault() != null)
                    { 

                        if (item.FirstOrDefault().StartTime != null)
                        {

                            if (item.FirstOrDefault().Source.Equals(StringEnum.GetStringValue(Constants.Enumerations.ScheduleSourcesEnum.Leave)))
                                IsValid = false;

                            if (item.FirstOrDefault().Source.Equals(StringEnum.GetStringValue(Constants.Enumerations.ScheduleSourcesEnum.AssignOrder)))
                                IsValid = false;

                            if (IsValid)
                            {
                                var Task_StartDateTime = item.Select(s => s.StartTime).FirstOrDefault();
                                var Task_EndDateTime = item.Select(s => s.EndTime).FirstOrDefault();
                                var StartDateTime = StartTime;
                                var EndDateTime = EndTime;
                                List<DateTimeGroup> DateTimeGroupList = new List<DateTimeGroup>();
                                DateTimeGroupList.Add(new DateTimeGroup() { StartDate = StartDateTime, EndDate = EndDateTime });
                                DateTimeGroupList.Add(new DateTimeGroup() { StartDate = Convert.ToDateTime(Task_StartDateTime), EndDate = Convert.ToDateTime(Task_EndDateTime) });
                                IsValid = DateTimeServices.CheckConflict(DateTimeGroupList);
                            }

                            if (!IsValid)
                            {
                                 ErrorMessage = GetErrorMessage(item.FirstOrDefault(), Language);
                                 return IsValid;
                            }
                        }
                }

                }

                return true;

            }
            private static string GetErrorMessage(Model.ViewModel.Personnel.EmpSchedual item,string culture)
            {
                var message = string.Empty;
                if (item.Source == "Meeting")
                {
                    message = MsgUtils.Instance.Trls(culture, "Errand Confilct") + " " + MsgUtils.Instance.Trls(culture, "Meeting") + " "
                  + MsgUtils.Instance.Trls(culture,"fromHours") +" " +item.StartTime.Value.ToString("hh:mm tt", new System.Globalization.CultureInfo(culture)) + " " 
                  + MsgUtils.Instance.Trls( culture, "ToHours") + " " + item.EndTime.Value.ToString("hh:mm tt", new System.Globalization.CultureInfo(culture)) + " " 
                  + MsgUtils.Instance.Trls( culture, "Meeting Date") + item.StartTime.Value.ToString("dddd dd/MM/yyyy") + " " +
                  " " + MsgUtils.Instance.Trls(culture, "organiser") + item.Organizer;
                }
                else if(item.Source == "Errand")
                {
                    if (item.MultiDays == 1)
                    {
                        message = MsgUtils.Instance.Trls(culture, "Errand Confilct") + " " + MsgUtils.Instance.Trls(culture, "Mission") + " "
                     + MsgUtils.Instance.Trls(culture, "from") + " " + item.StartTime.Value.ToString("dddd dd/MM/yyyy") + " "
                     + MsgUtils.Instance.Trls(culture, "To") + " " + item.EndTime.Value.ToString("dddd dd/MM/yyyy");
                    }
                    else
                    {
                        message = MsgUtils.Instance.Trls(culture, "Errand Confilct") + " " + MsgUtils.Instance.Trls(culture, "Mission") + " "
                      + MsgUtils.Instance.Trls(culture, "fromHours") + " " + item.StartTime.Value.ToString("hh:mm tt", new System.Globalization.CultureInfo(culture)) + " "
                      + MsgUtils.Instance.Trls(culture, "ToHours") + " " + item.EndTime.Value.ToString("hh:mm tt", new System.Globalization.CultureInfo(culture)) + " "
                      + MsgUtils.Instance.Trls(culture, "Meeting Date") + item.StartTime.Value.ToString("dddd dd/MM/yyyy");
                    }

                }
                else if(item.Source == "Leave")
                {
                    message = MsgUtils.Instance.Trls(culture, "Errand Confilct") + " " + MsgUtils.Instance.Trls(culture, "Leave") + " "
                + MsgUtils.Instance.Trls(culture, "from") + " " + item.StartTime.Value.ToString("dddd dd/MM/yyyy") + " "
                + MsgUtils.Instance.Trls(culture, "To") + " " + item.EndTime.Value.ToString("dddd dd/MM/yyyy");
                }
                else if(item.Source == "AssignOrder")
                {
                    message = MsgUtils.Instance.Trls(culture, "Errand Confilct") + " " + MsgUtils.Instance.Trls(culture, "Assign order") + " "
               + MsgUtils.Instance.Trls(culture, "froة") + " " + item.StartTime.Value.ToString("dddd dd/MM/yyyy") + " "
               + MsgUtils.Instance.Trls(culture, "To") + " " + item.EndTime.Value.ToString("dddd dd/MM/yyyy");
                }

                return message;
            }

        }

    }

    public class StringValue : System.Attribute
    {
        private readonly string _value;

        public StringValue(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }

    public static class StringEnum
    {
        public static string GetStringValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();

            //Check first in our cached results...

            //Look for our 'StringValueAttribute' 

            //in the field's custom attributes

            FieldInfo fi = type.GetField(value.ToString());
            StringValue[] attrs =
               fi.GetCustomAttributes(typeof(StringValue),
                                       false) as StringValue[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }
    }



}
