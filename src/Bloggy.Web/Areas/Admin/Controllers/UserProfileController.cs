using Bloggy.Entities;
using Bloggy.Service.AlertService;
using Bloggy.Shared.Config;
using Bloggy.Shared.Extension;
using Bloggy.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggy.Web.Areas.Admin.Controllers
{
    public class UserProfileController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserProfileController(IAlertService alertService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : base(alertService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            if (BloggyConst.DoNotChange)
            {
                CreateSuccessAlert();
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            var res = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.Password);

            if (res.Succeeded)
                CreateSuccessAlert();
            else
                CreateNotSuccessAlert();

            return View();
        }

        public async Task<IActionResult> ChangeDisplayName()
        {
            var userId = User.Identity.GetUserIdInt();
            var res = await _userManager.FindByIdAsync(userId.ToString());
            UserDisplayNameDto dto = new()
            {
                DisplayName = res.DisplayName
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDisplayName(UserDisplayNameDto dto)
        {
            if (BloggyConst.DoNotChange)
            {
                CreateSuccessAlert();
                return View();
            }

            var userId = User.Identity.GetUserIdInt();
            var res = await _userManager.FindByIdAsync(userId.ToString());
            res.DisplayName = dto.DisplayName;
            await _userManager.UpdateAsync(res);
            return View(dto);
        }

        public async Task<IActionResult> LogOutUser()
        {
            await _signInManager.SignOutAsync();           
            return RedirectPermanent("/");
        }
    }
}
