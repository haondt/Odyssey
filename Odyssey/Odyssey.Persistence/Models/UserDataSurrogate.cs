using Microsoft.AspNetCore.Identity;

namespace Odyssey.Persistence.Models
{
    public class UserDataSurrogate : IdentityUser
    {
        public ICollection<BoardMetadataDataModel> BoardMetadatas { get; set; } = [];
    }
}
