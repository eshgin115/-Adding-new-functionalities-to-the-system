using DemoApplication.Database.Models.Common;

namespace DemoApplication.Database.Models
{
    public class UserAdress : BaseEntity<int>
    {
      
      
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public string? Name { get; set; }

        public string? Receiver { get; set; }
        public string? ReceiverLastNameTake { get; set; }
        public string? ContactNumber { get; set; }
    }
}
