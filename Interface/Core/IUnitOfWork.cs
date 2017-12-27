using System;
using System.Collections.Generic;

namespace Interface.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IEnumerable<T> Repository<T>() where T : class;

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
