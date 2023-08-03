using System.ComponentModel.DataAnnotations.Schema;
using Squabble.Models.Entities;

namespace Squabble.Models
{
    public class ServerMember
    {
        [ForeignKey("Server")]
        public int ServerID
        {
            get; set;
        }
        public virtual Server Server
        {
            get; set;
        }

        [ForeignKey("User")]
        public int UserID
        {
            get; set;
        }
        public virtual User User
        {
            get; set;
        }
    }

}
