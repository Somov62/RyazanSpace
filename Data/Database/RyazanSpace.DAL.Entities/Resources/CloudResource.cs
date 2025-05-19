using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.DAL.Entities.Resources.Base
{
    public class CloudResource : Entity
    {
        [Required]
        public string DownloadLink { get; set; }

        public int OwnerId { get; set; }
        public virtual User Owner { get; set; }

        [Required]
        public CloudResourceType Type { get; set; } = CloudResourceType.Document;
    }

    public enum CloudResourceType
    {
        Document,
        Image
    }

}
