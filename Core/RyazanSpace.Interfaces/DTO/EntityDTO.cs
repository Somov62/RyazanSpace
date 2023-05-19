using RyazanSpace.Interfaces.Entities;

namespace RyazanSpace.Interfaces.DTO
{
    public abstract class EntityDTO<T> where T : class, IEntity
    {
        public EntityDTO() { }
        public EntityDTO(T entity)
        {
            if (entity != null) 
            {
                InitByEntity(entity);
            }
        }

        protected abstract void InitByEntity(T entity);
        public abstract T MapToEntity();
        public abstract T UpdateEntity(T entity);
    }
}
