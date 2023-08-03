using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Squabble.Models.BaseModels;

namespace Squabble.Models.Entities
{
    //todo: figure out best way to check if certain properties would be unique
    //Database model for the Account entity
    public class User : BaseUser
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        //can be null, but will need to show up in the UI
        public string Avatar { get; set; }

        public virtual ICollection<Friendship> Friends { get; set; }

        public virtual ICollection<FriendRequest> FriendRequests { get; set; }

        public virtual ICollection<ChannelMember> Channels { get; set; }

        public bool IsSso { get; set; }

        public string MicrosoftSsoId { get; set; }

        public string CommunicationUserId { get; set; }

        public string CommunicationToken { get; set; }
    }
}
