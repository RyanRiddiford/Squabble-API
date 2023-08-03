namespace Squabble.Models.RequestModels
{
    public class UpdateServerUserRoleRequest
    {
        public int ServerId { get; set; }
        public int AccountId { get; set; }
    }
}
