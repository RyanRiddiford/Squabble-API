using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Squabble.Models
{
    public class Channel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChannelId { get; set; }

        [Required(ErrorMessage = "The channel must have a name.")]
        public string ChannelName { get; set; }

        // [Required(ErrorMessage = "The Azure chat thread ID is required.")]
        public string AzureChatThreadId { get; set; }

        [ForeignKey("Server")]
        public int? ServerID { get; set; }
        public virtual Server Server { get; set; }

        public virtual ICollection<ChannelMember> Members { get; set; }
    }

}
