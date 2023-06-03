using RyazanSpace.DAL.Client.Repositories.Base;
using RyazanSpace.DAL.Entities.Groups;

namespace RyazanSpace.DAL.Client.Repositories.Groups
{
    public class WebGroupsRepository : WebNamedRepository<Group>
    {
        public WebGroupsRepository(HttpClient client) : base(client) { }

    }
}
