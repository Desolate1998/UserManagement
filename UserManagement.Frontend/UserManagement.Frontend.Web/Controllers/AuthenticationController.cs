using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UserManagement.Frontend.Web.Helpers;
using UserManagement.Frontend.Web.Models;
using UserManagement.Frontend.Web.Models.APIModels;

namespace UserManagement.Frontend.Web.Controllers;

public class AuthenticationController(HttpHelper httpHelper, IHttpContextAccessor httpContextAccessor) : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var response = await httpHelper.SendPostRequestAsync<LoginRequestResponse>("Authentication/Login", JsonSerializer.Serialize(model));
                if (response is null)
                {
                    ViewBag.Error = "Something went wrong, please try again later";
                    return View(model);
                }
                if (response.IsError)
                {
                    foreach (var error in response!.Errors!)
                    {
                        ModelState.AddModelError(error!.Code!, error!.Description!);
                    }
                    return View(model);
                }
                httpContextAccessor!.HttpContext!.Session.SetString("JwtToken", response!.Value!.JwtToken!);
                return RedirectToAction("Index", "Home");
            }
            catch (UnauthorizedAccessException)
            {

                ViewBag.Error = "Invalid login Details";
                return View(model);
            }
        }
        return View(model);
    }
}
