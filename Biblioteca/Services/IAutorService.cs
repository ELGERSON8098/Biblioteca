using System.Collections.Generic;
using Biblioteca.Models;

namespace Biblioteca.Services
{
    public interface IAutorService
    {
        IEnumerable<Autor> GetAll();
        Autor GetById(int id);
        void Update(Autor autor);
        void Delete(int id);
    }
}
