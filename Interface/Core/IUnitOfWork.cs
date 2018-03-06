using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Interface.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IQueryable<T> Repository<T>() where T : class;
        bool Save_CheckError_Rollback(DbContextTransaction Trans, out List<Error> errors);
        IEnumerable<T> SqlQuery<T>(string sql) where T : class;

        /// <summary>
        /// Regenerates the context.
        /// </summary>
        int RegenerateContext();

        /// <summary>
        /// Saves Context changes.
        /// </summary>
        int Save();

        List<Model.ViewModel.Error> SaveChanges();
        string HandleDbExceptions(Exception ex);
    }
}
