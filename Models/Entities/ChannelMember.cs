using System.ComponentModel.DataAnnotations.Schema;
using Squabble.Models.Entities;

namespace Squabble.Models
{
    public class ChannelMember
    {
        [ForeignKey("Channel")]
        public int ChannelId
        {
            get; set;
        }
        public virtual Channel Channel
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
