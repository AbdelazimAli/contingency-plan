using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Persistence.Services
{
    public static class DateTimeServices
    {
        public static DateTime ToMyDateTime(this string c, string Culture)
        {
            try
            {
                DateTimeFormatInfo usDtfi = new CultureInfo(Culture, false).DateTimeFormat;
                return Convert.ToDateTime(c, usDtfi);
            }
            catch
            {
                return Convert.ToDateTime(c);
            }
        }

        /// <summary>
        /// how to use : date.ToMyDateString(Culture);  Default Culture ="en-US"
        /// </summary>
        /// <param name="d"></param>
        /// <param name="Culture"></param>
        /// <returns></returns>
        public static string ToMyDateString(this DateTime? d, string Culture = null)
        {
            try
            {
                if (d == null) return string.Empty;
                if (string.IsNullOrEmpty(Culture)) Culture = "en-US";

                return d.Value.ToString("dddd, MMMM d, yyyy", new System.Globalization.CultureInfo(Culture));
            }
            catch
            {
                return d.Value.ToShortDateString();
            }
        }

        /// <summary>
        /// how to use : date.ToMyDateString(Culture);  Default Culture ="en-US"
        /// </summary>
        /// <param name="d"></param>
        /// <param name="Culture"></param>
        /// <returns></returns>
        public static string ToMyDateString(this DateTime d, string Culture = null)
        {
            try
            {
                if (string.IsNullOrEmpty(Culture)) Culture = "en-US";

                return d.ToString("dddd, MMMM d, yyyy", new System.Globalization.CultureInfo(Culture));
            }
            catch
            {
                return d.ToShortDateString();
            }
        }

        /// <summary>
        /// how to use : date.ToMyDateString("en-US","yyyy-MM-dd");
        /// </summary>
        /// <param name="d"></param>
        /// <param name="Culture"></param>
        /// <returns></returns>
        public static string ToMyDateString(this DateTime? d, string Culture, string DateFormat)
        {
            try
            {
                if (d == null) return string.Empty;

                return d.Value.ToString(DateFormat, new System.Globalization.CultureInfo(Culture));
            }
            catch
            {
                return d.Value.ToShortDateString();
            }
        }

        /// <summary>
        /// how to use : date.ToMyDateString("en-US","yyyy-MM-dd");
        /// </summary>
        /// <param name="d"></param>
        /// <param name="Culture"></param>
        /// <returns></returns>
        public static string ToMyDateString(this DateTime d, string Culture, string DateFormat)
        {
            try
            {

                return d.ToString(DateFormat, new System.Globalization.CultureInfo(Culture));
            }
            catch
            {
                return d.ToShortDateString();
            }
        }

        public static void GetTimeUniversal(int Hour, int Minute, string UserTimeZoneString, out int UNHour, out int UNMinute)
        {
            UNHour = 0;
            UNMinute = 0;
            try
            {
                int DummyYear = DateTime.Now.Year, DummyMonth = DateTime.Now.Month, DummyDay = DateTime.Now.Day;
                DateTime RequiredTime = new DateTime(DummyYear, DummyMonth, DummyDay, Hour, Minute, 0);

                TimeZoneInfo UserTimeZone = TimeZoneInfo.FindSystemTimeZoneById(UserTimeZoneString);
                DateTime UniversalDateTime = TimeZoneInfo.ConvertTime(RequiredTime, UserTimeZone, TimeZoneInfo.Utc);
                UNHour = UniversalDateTime.Hour;
                UNMinute = UniversalDateTime.Minute;
            }
            catch
            {
            }
        }

        public static DateTime GetUniversalDatetime(this DateTime Date, string TimeZone)
        {
            try
            {
                return TimeZoneInfo.ConvertTime(Date, TimeZoneInfo.FindSystemTimeZoneById(TimeZone));

            }
            catch
            {
                return Date;
            }
        }

        public static bool CheckConflict(List<DateTimeGroup> DateTimeGroupList)
        {

            for (int i = 0; i < DateTimeGroupList.Count(); i++)
            {
                DateTimeGroup CurrentGroup = DateTimeGroupList[i];
                List<DateTimeGroup> RemainingList = DateTimeGroupList.Where(m => !m.Equals(CurrentGroup)).ToList();

                foreach (DateTimeGroup RemainGroup in RemainingList)
                {
                    bool IsValid =
                        (CurrentGroup.StartDate < RemainGroup.StartDate && CurrentGroup.EndDate > RemainGroup.StartDate) ||
                        (CurrentGroup.StartDate < RemainGroup.EndDate && CurrentGroup.EndDate > RemainGroup.EndDate) ||

                        (RemainGroup.StartDate < CurrentGroup.StartDate && RemainGroup.EndDate > CurrentGroup.StartDate) ||
                        (RemainGroup.StartDate < CurrentGroup.EndDate && RemainGroup.EndDate > CurrentGroup.EndDate);

                    if (IsValid) return false;

                }

            }

            return true;
        }
    }

    public class DateTimeGroup
    {
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
    }
}
