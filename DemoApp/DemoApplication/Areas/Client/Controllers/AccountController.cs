using DemoApplication.Areas.Client.ViewModels.Account;
using DemoApplication.Attributs;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoApplication.Areas.Client.Controllers
{
    [Area("client")]
    [Route("account")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserService _userService;
        private User _CurrentUser;
        
        public AccountController(DataContext dataContext, IUserService userService)
        {
            _dataContext = dataContext;
            _userService = userService;
            _CurrentUser=userService.CurrentUser;
        }
        #region Dashboard
        [HttpGet("dashboard", Name = "client-account-dashboard")]
        public IActionResult Dashboard()
        {

            return View();
        }
        #endregion





        #region orders
        [HttpGet("orders", Name = "client-account-orders")]
        public IActionResult Orders()
        {

            return View();
        }
        #endregion





        #region UpdatePassword



        [HttpGet("updatePassword", Name = "client-account-updatePassword")]
        public IActionResult UpdatePassword()
        {

            return View();
        }
        [HttpPost("updatePassword", Name = "client-account-updatePassword")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, _CurrentUser.Password))
            {
                ModelState.AddModelError("CurrentPassword", "CurrentPassword is not correct");
                return View(model);
            }
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            _CurrentUser.Password = passwordHash;

            await _dataContext.SaveChangesAsync();


            return View();
        }
        #endregion






        #region UpdateUserData
        [HttpGet("UpdateUserData", Name = "client-account-UpdateUserData")]
        public IActionResult UpdateUserData()
        {

            return View();
        }
        [HttpPost("UpdateUserData", Name = "client-account-UpdateUserData")]
        public async Task<IActionResult> UpdateUserData(UpdateUserDataViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, _CurrentUser.Password))
            {
                ModelState.AddModelError("CurrentPassword", "CurrentPassword is not correct");
                return View(model);
            }


            _CurrentUser.FirstName = model.Name;
            _CurrentUser.LastName = model.LastName;

            await _dataContext.SaveChangesAsync();


            return View();
        }
        #endregion







        #region EditAdress



        [HttpGet("Adress", Name = "client-account-Adress")]
        [ServiceFilter(typeof(IsUserAdress))]
        public IActionResult Adress()
        {


          AdressViewModel AdressView = new AdressViewModel
           (_CurrentUser.Adress!.Name, _CurrentUser.Adress.Receiver, _CurrentUser.Adress.ReceiverLastNameTake, _CurrentUser.Adress.ContactNumber);


            return View(AdressView);
        }
        [HttpGet("UpdateAdress", Name = "client-account-UpdateAdress")]
        public IActionResult UpdateAdress()
        {
            return View();
        }
        [HttpPost("UpdateAdress", Name = "client-account-UpdateAdress")]
        public async Task<IActionResult> UpdateAdress(UpdateAdressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, _CurrentUser.Password))
            {
                ModelState.AddModelError("CurrentPassword", "CurrentPassword is not correct");
                return View(model);
            }


            _CurrentUser.Adress!.Name = model.Adress;
            _CurrentUser.Adress.ContactNumber = model.ContactNumber;
            _CurrentUser.Adress.ReceiverLastNameTake = model.ReceiverLastNameTake;
            _CurrentUser.Adress.Receiver = model.ReceiverName;

            await _dataContext.SaveChangesAsync();


            return RedirectToRoute("client-account-UpdateAdress");
        }
        #endregion





        #region AddAdress

        [HttpGet("AdressAdd", Name = "client-account-AdressAdd")]
        public IActionResult AdressAdd()
        {
            return View();
        }
        [HttpPost("AdressAdd", Name = "client-account-AdressAdd")]
        public async Task<IActionResult> AdressAdd(AdressAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _dataContext.UserAdresses.AddAsync(new UserAdress
            { 
                UserId= _CurrentUser.Id,
                User= _CurrentUser,
                Name = model.Name,
                Receiver = model.Receiver,
                ReceiverLastNameTake = model.ReceiverLastNameTake,
                ContactNumber = model.ContactNumber 
            });

            await _dataContext.SaveChangesAsync();


            return RedirectToRoute("client-account-Adress");
        }

        #endregion
    }
}
