using System;

namespace Model.ViewModel
{
    public class FileUploaderViewModel
    {
        public DateTimeOffset? creation_time { get; set; }
        public string Description { get; set; }
        public string DocName { get; set; }
        public int? DocType { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Keyword { get; set; }
        public DateTimeOffset? last_access_time { get; set; }
        public byte AccessLevel { get; set; }
        public Guid stream_id { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public Guid Id { get; set; }
        public int? CompanyId { get; set; }
    }
}
