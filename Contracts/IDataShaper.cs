using Entities.Models;

namespace Contracts
{
    public interface IDataShaper<T>
    {
        public IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities,
            string fieldsString);

        ShapedEntity ShapeData(T entity, string fieldsString);
    }
}
