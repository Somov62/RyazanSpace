using RyazanSpace.DAL.Client.Repositories.Base;
using RyazanSpace.DAL.Entities.Groups;

namespace RyazanSpace.DAL.Client.Repositories.Groups
{
    public class WebPostRepository : WebRepository<Post>
    {
        public WebPostRepository(HttpClient client) : base(client) { }


    }
}
