using System.Collections.Generic;

namespace WebApiBusiness.Interfaces
{
    internal interface ICrud<T>
    {
        bool Create(T obj);

        IEnumerable<T> Read(T obj);

        bool Update(T obj);

        bool Destroy(T obj);
    }
}
