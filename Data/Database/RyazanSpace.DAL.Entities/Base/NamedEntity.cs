using RyazanSpace.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.DAL.Entities.Base
{
    public abstract class NamedEntity : Entity, INamedEntity
    {
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
    }
}
