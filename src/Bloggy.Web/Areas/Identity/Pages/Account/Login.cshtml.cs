using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bloggy.Entities;
using Microsoft.Extensions.Localization;
using System.Data;
using Bloggy.Shared.Config;

namespace Bloggy.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IStringLocalizer localizer;
        private readonly IConfiguration configuration;
        public LoginModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IStringLocalizer localizer, RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
        {
            this.localizer = localizer;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            this.configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "requierd")]
            [Display(Name = "username")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "requierd")]
            [DataType(DataType.Password)]
            [Display(Name = "password")]
            public string Password { get; set; }

            [Display(Name = "remember-me")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync()
        {
            if (!await _userManager.Users.AnyAsync())
            {
                await _roleManager.CreateAsync(new ApplicationRole { Name = "Admin" });
                var AdminUser = new ApplicationUser()
                {
                    UserName = configuration["Bloggy:AdminDefaultName"],
                    EmailConfirmed = true,
                    DisplayName = "Site admin",
                    LockoutEnabled = false,
                };

                await _userManager.CreateAsync(AdminUser, configuration["Bloggy:AdminDefaultPassword"]);
                await _userManager.AddToRoleAsync(AdminUser, "Admin");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //Remove this condition for a real website
            if (BloggyConst.DoNotChange && Input.UserName == "admin" && Input.Password == "admin")
            {
                var FindedUser = await _userManager.Users.FirstOrDefaultAsync();
                await _signInManager.SignInAsync(FindedUser, Input.RememberMe);
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            if (ModelState.IsValid)
            {
                var FindedUser = await _userManager.Users.Where(m => m.NormalizedUserName.ToLower() == Input.UserName.ToLower()).FirstOrDefaultAsync();

                if (FindedUser == null)
                {
                    ModelState.AddModelError(Input.UserName, localizer.GetString("user-not-found"));
                    return Page();
                }

                var result = await _signInManager.PasswordSignInAsync(FindedUser, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, localizer.GetString("incorrect-user-or-pass"));
                    return Page();
                }
            }
            return Page();
        }
    }
}
