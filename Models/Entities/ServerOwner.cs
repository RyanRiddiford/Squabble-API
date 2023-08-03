using System.ComponentModel.DataAnnotations.Schema;
using Squabble.Models.Entities;

namespace Squabble.Models
{
    public class ServerOwner
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
        public int UserId
        {
            get; set;
        }

        public virtual User User
        {
            get; set;
        }
    }

}
