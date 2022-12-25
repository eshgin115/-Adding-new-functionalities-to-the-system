namespace DemoApplication.Areas.Client.ViewModels.Account
{
    public class UpdateAdressViewModel
    {
        public UpdateAdressViewModel(string? currentPassword, string? adress, string? receiverName, string? receiverLastNameTake, string? contactNumber)
        {
            CurrentPassword = currentPassword;
            Adress = adress;
            ReceiverName = receiverName;
            ReceiverLastNameTake = receiverLastNameTake;
            ContactNumber = contactNumber;
        }
        public UpdateAdressViewModel()
        {

        }
        public string? CurrentPassword { get; set; }
        public string? Adress { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverLastNameTake { get; set; }
        public string? ContactNumber { get; set; }
      
    }
}
