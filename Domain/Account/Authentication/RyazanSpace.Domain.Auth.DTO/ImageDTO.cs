using RyazanSpace.DAL.Entities.Resources;
using RyazanSpace.Core.DTO;
using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.Domain.Auth.DTO
{
    public class ImageDTO : EntityDTO<Image>
    {
        public ImageDTO() { }
        public ImageDTO(Image entity) : base(entity) { }

        [Required]
        public string DownloadLink { get; set; }

        public virtual UserDTO Owner { get; set; }

        public override Image MapToEntity() => new()
        {
            DownloadLink = this.DownloadLink,
            Owner = this.Owner.MapToEntity()
        };

        public override Image UpdateEntity(Image entity)
        {
            entity.DownloadLink = this.DownloadLink;
            this.Owner.UpdateEntity(entity.Owner);
            return entity;
        }

        protected override void InitByEntity(Image entity)
        {
            this.DownloadLink = entity.DownloadLink;
            this.Owner = new UserDTO(entity.Owner);
        }
    }
}
