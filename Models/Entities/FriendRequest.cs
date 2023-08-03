using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Squabble.Models.Entities;

namespace Squabble.Models
{
    public class FriendRequest
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FriendRequestID
        {
            get; set;
        }
        public string Message
        {
            get; set;
        }
        [ForeignKey("Sender")]
        [Display(Name = "Sender")]
        public int SenderID
        {
            get; set;
        }
        public virtual User Sender
        {
            get; set;
        }

        [ForeignKey("Receiver")]
        public int ReceiverID {
            get; set;
        }

        public virtual User Receiver
        {
            get; set;
        }

        public bool? Accepted
        {
            get; set;
        }
    }

}
