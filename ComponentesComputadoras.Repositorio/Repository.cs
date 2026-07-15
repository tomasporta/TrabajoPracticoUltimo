using ComponentesComputadoras.Abstraccioness;

namespace ComponentesComputadoras.Repository
{
    public interface IRepository<T> : IDbOperation<T> { }

    public class Repository<T> : IRepository<T> where T : class, IEntidad
    {
        private readonly IDbContext<T> _db;
        public Repository(IDbContext<T> db) { _db = db; }

        public void Delete(int id) => _db.Delete(id);
        public IList<T> GetAll() => _db.GetAll();
        public T GetById(int id) => _db.GetById(id);
        public T Save(T entity) => _db.Save(entity);
    }
}
