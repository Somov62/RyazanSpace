using RyazanSpace.Core.DTO;
using RyazanSpace.DAL.Entities.Resources.Base;

namespace RyazanSpace.Domain.Profile.DTO
{
    public class CloudResourceDTO : EntityDTO<CloudResource>
    {
        public CloudResourceDTO() { }

        public CloudResourceDTO(CloudResource entity) : base(entity) { }

        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string DownloadLink { get; set; }
        public CloudResourceType Type { get; set; }


        public override CloudResource MapToEntity()
        {
            return new CloudResource()
            {
                DownloadLink = DownloadLink,
                OwnerId = OwnerId,
                Type = Type,
                Id = Id
            };
        }

        public override CloudResource UpdateEntity(CloudResource entity)
        {
            entity.DownloadLink = DownloadLink;
            entity.OwnerId = OwnerId;
            entity.Type = Type;
            entity.Id = Id;
            return entity;
        }

        protected override void InitByEntity(CloudResource entity)
        {
            DownloadLink = entity.DownloadLink;
            OwnerId = entity.OwnerId;
            Type = entity.Type;
            Id = entity.Id;
        }
    }
}