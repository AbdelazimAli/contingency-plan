using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Notifications
{
    public class Filter
    {
        [Key]
        public int Id { get; set; }
        public int NotifyCondId { get; set; }
        public NotifyCondition NotifyCond { get; set; }

        [Required, MaxLength(50), Column(TypeName = "varchar")]
        [Index("IX_Condition", Order = 1)]
        public string ObjectName { get; set; }

        [Index("IX_Condition", Order = 2)]
        public byte Version { get; set; } = 0;

        [Required, MaxLength(50), Column(TypeName = "varchar")]
        [Index("IX_Condition", Order = 3)]
        public string ColumnName { get; set; }

        [MaxLength(20)]
        public string ColumnType { get; set; }

        [MaxLength(10)]
        public string Operator { get; set; } // >, >=, <, <=, =, StartWith, EndWith, Contains

        [MaxLength(50)]
        public string Value { get; set; }

        [MaxLength(3)]
        public string AndOr { get; set; }
    }

    public enum Events
    {
        NumberHasChanged = 1,
        NumberIsSetTo = 2,
        HasDecreased = 3,
        HasDecreasedBelow = 4,
        HasIncreased = 5,
        HasIncreasedAbove = 6,

        StringHasChanged = 11,
        StringIsSetTo = 12,

        HasBeenPostponed = 21,
        /// <summary>
        /// has been postponed until at the earliest: value
        /// </summary>
        HasBeenPostponedUntilEarliest = 22,
        DateHasChanged = 23,
        IsDueTo = 24,
        IsDueIn = 25,
        IsSetToDateEarlierThan = 26,
        IsSetToEarlierDate = 27,
        IsSetToDate = 28,
        WasDueThisAmountofTimeAgo = 29,
        RecordHassBeenCreated = 51,
        RecordHasBeenDeleted = 52
    };

    public enum NotifyDays { _1CalendarDay = 1, _2CalendarDays, _3CalendarDays, _4CalendarDays,
        _5CalendarDays, _6CalendarDays, _7CalendarDays, _8CalendarDays, _9CalendarDays,
        _10CalendarDays, _11CalendarDays, _12CalendarDays, _13CalendarDays, _2Weeks, _3Weeks,
        _1Month, _2Months, _3Months, _4Months }
    
}
