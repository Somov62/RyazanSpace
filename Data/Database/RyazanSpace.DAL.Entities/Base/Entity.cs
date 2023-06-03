using RyazanSpace.Interfaces.Entities;

namespace RyazanSpace.DAL.Entities.Base
{
    public abstract class Entity : BaseEntity, IEntity
    {
        public int Id { get; set; }
    }
}
