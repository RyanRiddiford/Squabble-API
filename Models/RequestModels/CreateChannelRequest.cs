using System.ComponentModel.DataAnnotations;

namespace Squabble.Models.RequestModels
{
    public class CreateChannelRequest
    {
        // [Required(ErrorMessage = "The Server ID is required.")]
        public int? ServerId { get; set; }

        [Required(ErrorMessage = "The channel must is required.")]
        public string ChannelName { get; set; }

        [Required(ErrorMessage = "The Azure chat thread ID is required.")]
        public string AzureChatThreadId { get; set; }
    }
}
