using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.Interfaces.Entities
{
    public interface INamedEntity : IEntity
    {
        [Required]
        string Name { get; set; }
    }

}
