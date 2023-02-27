using Bloggy.Service.AlertService;
using Bloggy.Shared.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bloggy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BaseController : Controller
    {
        private readonly IAlertService _AlertService;

        public BaseController(IAlertService alertService)
        {
            _AlertService = alertService;
        }

        public void CreateSuccessAlert() => TempData.Put("Message", _AlertService.CreateSuccessAlert());
        public void CreateNotSuccessAlert() => TempData.Put("Message", _AlertService.CreateNotSuccessAlert());
        public void CreateAlert(string Title, string Text, AlertIcons icon) => TempData.Put("Message", _AlertService.CreateAlert(Title, Text, icon));
    }
}
