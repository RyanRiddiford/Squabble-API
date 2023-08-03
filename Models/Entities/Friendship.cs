
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Squabble.Models.Entities;

namespace Squabble.Models
{
    public class Friendship
    {
        [ForeignKey("User")]
        public int UserID
        {
            get; set;
        }
        public virtual User User
        {
            get; set;
        }
        [ForeignKey("Friend")]
        public int FriendID
        {
            get; set;
        }
        public virtual User Friend
        {
            get; set;
        }

    }
}
