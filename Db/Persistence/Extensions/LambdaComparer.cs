using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Persistence.Extensions
{
    public class LambdaComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparer;
        private readonly Func<T, int> _hash;

        public LambdaComparer(Func<T, T, bool> comparer)
            : this(comparer, o => 0)
        {
        }

        public LambdaComparer(Func<T, T, bool> comparer, Func<T, int> hash)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            if (hash == null)
                throw new ArgumentNullException("hash");
            _comparer = comparer;
            _hash = hash;
        }

        public bool Equals(T x, T y)
        {
            return _comparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _hash(obj);
        }
    }

}
