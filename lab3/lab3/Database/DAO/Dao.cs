using System.Collections.Generic;
using lab3.Models;

namespace lab3.Database.DAO
{
    abstract class Dao<T>
    {
        protected Context Context;
        protected Dao(Context context)
        {
            Context = context;
        }

        public abstract void Create(T entity);
        public abstract T Get(long id);
        public abstract List<T> Get(int page);
        public abstract void Update(T entity);
        public abstract void Delete(long id);
        public abstract void Clear();
    }
}
