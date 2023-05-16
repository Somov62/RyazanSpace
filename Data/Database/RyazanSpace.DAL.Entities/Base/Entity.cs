using RyazanSpace.Interfaces.Entities;

namespace RyazanSpace.DAL.Entities.Base
{
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }
    }
}
