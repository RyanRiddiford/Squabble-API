using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Squabble.Models.Entities
{
    public class Post
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        public string Content { get; set; }

        [Required]
        [ForeignKey("Channel")]
        public int ChannelId { get; set; }
        public virtual Channel Channel { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public DateTime TimePosted { get; set; }

        public DateTime? ExpiresOn { get; set; }
    }
}
