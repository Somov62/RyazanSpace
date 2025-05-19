using RyazanSpace.DAL.Entities.Resources.Base;
using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.Domain.Cloud.DTO
{
    public record UploadRequestDTO([Required] byte[] File, [Required] CloudResourceType Type);
}
