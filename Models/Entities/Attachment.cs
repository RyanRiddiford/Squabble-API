using Squabble.Models.BaseModels;
using Squabble.Models.Entities;

namespace Squabble.Models
{
    public class Attachment : Post
    {
        public MediaType MediaType
        {
            get; set;
        }

        public long? FileSize
        {
            get; set;
        }
    }

}