using Interface.Core;
using System.Data.Entity;

namespace Db.Persistence
{
    public sealed class HrContextFactory : IContextFactory<DbContext>
    {
        /// <summary>
        /// The connection string
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="HrContextFactory"/> class.
        /// </summary>
        /// <param name="connectionStringOrName">Name of the connection string or actual connection string.</param>
        public HrContextFactory(string connectionStringOrName)
        {
            _connectionString = connectionStringOrName;
        }

        /// <summary>
        /// Creates new database context.
        /// </summary>
        /// <returns>DbContext: <see cref="Db.HrContext"/></returns>
        public DbContext Create()
        {
            return new Db.HrContext(_connectionString);
        }
    }
}
