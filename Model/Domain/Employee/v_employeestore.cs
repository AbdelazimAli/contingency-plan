//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace Model.Domain
{
    using System;

    public partial class v_employeestore
    {
        [Key]
        public Guid stream_id { get; set; }
        public byte[] file_stream { get; set; }
        public string name { get; set; }
        public string file_type { get; set; }
        public Nullable<long> cached_file_size { get; set; }
        public DateTimeOffset creation_time { get; set; }
        public DateTimeOffset last_write_time { get; set; }
        public Nullable<System.DateTimeOffset> last_access_time { get; set; }
        public bool is_directory { get; set; }
        public bool is_offline { get; set; }
        public bool is_hidden { get; set; }
        public bool is_readonly { get; set; }
        public bool is_archive { get; set; }
        public bool is_system { get; set; }
        public bool is_temporary { get; set; }
    }
}
