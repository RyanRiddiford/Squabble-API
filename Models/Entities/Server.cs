using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Squabble.Models
{
    //Database model for the Group entity
    public class Server
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ServerID
        {
            get; set;
        }
        [Required(ErrorMessage = "Server must have a name")]
        public string ServerName
        {
            get; set;
        }
        public virtual ICollection<Channel> Channels
        {
            get; set;
        }

        public virtual ServerOwner ServerOwner
        {
            get; set;
        }
     //   public Avatar ServerImage
      //  {
      //      get; set;
      //  }
        public virtual ICollection<ServerAdmin> Admins
        {
            get; set;
        }

        public virtual ICollection<ServerMember> Members
        {
            get; set;
        }

    }

}
