namespace DemoApplication.Areas.Client.ViewModels.Account
{
    public class AdressViewModel
    {
        public AdressViewModel(string? adress, string? receiverName, string? receiverLastNameTake, string? contactNumber)
        {
            Adress = adress;
            ReceiverName = receiverName;
            ReceiverLastNameTake = receiverLastNameTake;
            ContactNumber = contactNumber;
        }

        public string? Adress { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverLastNameTake { get; set; }
        public string? ContactNumber { get; set; }
    }
}
