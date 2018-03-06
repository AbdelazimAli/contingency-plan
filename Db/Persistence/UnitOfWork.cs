using Interface.Core;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;

namespace Db.Persistence
{
    /// <summary>
    /// Unit of work implementation for having single instance of context and doing DB operation as transaction
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    public abstract class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        /// <summary>
        /// DB context
        /// </summary>
        private TContext _context;

        /// <summary>
        /// The DB context factory
        /// </summary>
        private readonly IContextFactory<TContext> _contextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork{TContext}"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The db context factory.</param>
        protected UnitOfWork(IContextFactory<TContext> dbContextFactory)
            : this(dbContextFactory.Create())
        {
            _contextFactory = dbContextFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork{TContext}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        protected UnitOfWork(TContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        public virtual TContext Context
        {
            get
            {
                return _context;
            }
        }
        /// <summary>
        /// Regenerates the context.
        /// </summary>
        /// <remarks>WARNING: Calling with dirty context will save changes automatically</remarks>
        public int RegenerateContext()
        {
            int changes = 0;
            if (_context != null)
            {
                changes = Save();
            }
            _context = _contextFactory.Create();
            return changes;
        }

        public DbContextTransaction GetTransaction()
        {
            return Context.Database.BeginTransaction();
        }

        public bool Save_CheckError_Rollback(DbContextTransaction Trans,out List<Error> errors)
        {
             errors = new List<Error>();
            try
            {
                errors = SaveChanges();
                if (errors.Count > 0)
                {
                    Trans.Rollback();
                    Trans.Dispose();
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Saves Context changes.
        /// </summary>
        public int Save()
        {
            return _context.SaveChanges();
        }

        public virtual string HandleDbExceptions(Exception ex)
        {
            var message = "";
            if (ex is DbEntityValidationException)
            {
                var e = (DbEntityValidationException)ex;
                foreach (var errors in e.EntityValidationErrors)
                {
                    foreach (var error in errors.ValidationErrors)
                    {
                        message += error.ErrorMessage;
                    }
                }
            }
            else if (ex.InnerException.InnerException is SqlException)
            {
                var e = (SqlException)ex.InnerException.InnerException;
                if (e.Number == 2601)
                    message = "Date Already Exists";
                else if (e.Number == 547)
                {
                    message = "Can't delete related data found";
                }
                else
                    message += e.Message;
            }
            else
            {
                message += ex.InnerException.Message;
            }

            return message;
        }
       
        public List<Error> SaveChanges()
        {
            string message = "";
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                message = HandleDbExceptions(ex);
            }

            if (message.Length > 0)
            {
                return new List<Error>() { new Error { errors = new List<ErrorMessage>() { new ErrorMessage() {
                message = message
                } } } };
            }
            else
            {
                return new List<Error>();
            }
        }

        public IQueryable<T> Repository<T>() where T : class
        {
            return _context.Set<T>();
        }

        public IEnumerable<T> SqlQuery<T>(string sql) where T : class
        {
            return _context.Database.SqlQuery<T>(sql);
        }

        #region " Dispose "

        /// <summary>
        /// To detect redundant calls
        /// </summary>
        private bool _disposedValue;

        /// <summary>
        /// Dispose the object
        /// </summary>
        /// <param name="disposing">IsDisposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_context != null)
                    {
                        _context.Dispose();
                    }
                }
            }
            _disposedValue = true;
        }

        #region " IDisposable Support "

        /// <summary>
        /// Dispose the object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion " IDisposable Support "

        #endregion " Dispose "
    }
}
