using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.DAL.Entities.Resources.Base
{
    public abstract class CloudResource : Entity
    {
        [Required]
        public string DownloadLink { get; set; }

        public User Owner { get; set; }
    }
}
