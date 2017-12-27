namespace WebApp.Models
{
    public class UniqueViewModel
    {
        public string tablename { get; set; }
        public string id { get; set; }
        public string parentColumn { get; set; }
        public string parentId { get; set; }
        public string[] columns { get; set; }
        public string[] values { get; set; }
        public bool IsLocal { get; set; } = false;
    }

}

  