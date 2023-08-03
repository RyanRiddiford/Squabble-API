namespace Squabble.Models.ResponseModels
{
    public class PendingFriendRequestsResponse
    {
        public int FriendRequestId { get; set; }
        public int SenderAccountId { get; set; }
        public string SenderUsername { get; set; }
        public string SenderCommunicationUserId { get; set; }
        public int ReceiverAccountId { get; set; }
        public string ReceiverUsername { get; set; }
        public string ReceiverCommunicationUserId { get; set; }
    }
}
