using RyazanSpace.Core.DTO;
using RyazanSpace.DAL.Entities.Resources.Base;
using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.Domain.Auth.DTO
{
    public class ResourceDTO : EntityDTO<CloudResource>
    {
        public ResourceDTO() { }
        public ResourceDTO(CloudResource entity) : base(entity) { }

        [Required]
        public string DownloadLink { get; set; }

        public virtual RegResponseDTO Owner { get; set; }

        public CloudResourceType Type { get; set; }


        public override CloudResource MapToEntity() => new()
        {
            DownloadLink = this.DownloadLink,
            Type = this.Type,
            Owner = this.Owner.MapToEntity()
        };

        public override CloudResource UpdateEntity(CloudResource entity)
        {
            entity.DownloadLink = this.DownloadLink;
            entity.Type = this.Type;
            this.Owner.UpdateEntity(entity.Owner);
            return entity;
        }

        protected override void InitByEntity(CloudResource entity)
        {
            this.DownloadLink = entity.DownloadLink;
            this.Type = entity.Type;
            this.Owner = new RegResponseDTO(entity.Owner);
        }
    }
}
