using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_Securite.DAL.Interfaces
{
    public interface ICrudService<TKey, TEntity>
    {
        TEntity GetById(TKey id);
        IEnumerable<TEntity> Get();

        TKey Create(TEntity entity);

        TEntity Update(TKey id, TEntity entity);

        TEntity Delete(TKey id);
    }
}
