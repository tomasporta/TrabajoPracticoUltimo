using ComponentesComputadoras.Abstraccioness;
using ComponentesComputadoras.Repository;

namespace ComponentesComputadoras.Application
{
    public interface IApplication<T> : IDbOperation<T> { }

    public class Application<T> : IApplication<T>
    {
        private readonly IRepository<T> _repositorio;
        public Application(IRepository<T> repositorio) { _repositorio = repositorio; }

        public void Delete(int id) => _repositorio.Delete(id);
        public IList<T> GetAll() => _repositorio.GetAll();
        public T GetById(int id) => _repositorio.GetById(id);
        public T Save(T entity) => _repositorio.Save(entity);
    }
}
