using System.Collections.Generic;

namespace Model.ViewModel
{
    public class DataSource<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity> Data { get; set; }
        public int Total { get; set; } = 1;
        public string AggregateResults { get; set; }
        public IList<Error> Errors { get; set; }
    }

    public class ErrorMessage
    {
        public string message { get; set; }
        public string field { get; set; }
    }

    public class Error
    {
        public int id { get; set; }
        public short row { get; set; }
        public short page { get; set; }
        public List<ErrorMessage> errors { get; set; }
    }
}
