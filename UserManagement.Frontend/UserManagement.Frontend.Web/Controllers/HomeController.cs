using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using UserManagement.Frontend.web.Models;
using UserManagement.Frontend.Web.Helpers;
using UserManagement.Frontend.Web.Models;
using UserManagement.Frontend.Web.Models.APIModels;
using UserManagement.Frontend.Web.Models.Helper;

namespace UserManagement.Frontend.web.Controllers;

public class HomeController(HttpHelper httpHelper) : Controller
{
    public async Task<IActionResult> Index()
    {
        try
        {
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }
            else if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Error"];
            }

            var users = await httpHelper.SendGetRequestAsync<List<User>>("UserManagement/Users");

            if (users.IsError)
            {
                ViewBag.Error = users!.Errors!.First().Description;
                return View();
            }

            return View(users.Value);
        }
        catch (UnauthorizedAccessException)
        {
            return RedirectToAction("Login", "Authentication");
        }
    }

    public async Task<IActionResult> Create()
    {
        try
        {
            var groupsAndPermissions = await httpHelper.SendGetRequestAsync<GetGroupsAndPermissions>("UserManagement/GetGroupsAndPermissions");
            if (groupsAndPermissions.IsError)
            {
                ViewBag.Error = groupsAndPermissions!.Errors!.First().Description;
                return View();
            }

            var model = new CreateUserViewModel()
            {
                Permissions = groupsAndPermissions!.Value!.Permissions,
                Groups = groupsAndPermissions!.Value!.Groups.Select(x => (x, false)).ToList(),
            };

            return View(model);
        }
        catch (UnauthorizedAccessException)
        {
            return RedirectToAction("Login", "Authentication");
        }
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserModel model)
    {
        var validationResults = model.Validate();
        if (validationResults.Count > 0)
        {
            return BadRequest(validationResults);
        }
        try
        {
            var response = await httpHelper.SendPostRequestAsync<User>("UserManagement/Create", JsonSerializer.Serialize(model));
            if (response.IsError)
            {
                Dictionary<string, string> errors = [];
                errors.Add(response.Errors![0].Code ?? "ERROR", response.Errors![0].Description ?? "Internal Server Error");
                return BadRequest(errors);
            }

            return Ok();
        }
        catch (UnauthorizedAccessException)
        {
            return RedirectToAction("login", "Authentication");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int userId)
    {
        try
        {
            DeleteUserRequest model = new()
            {
                UserId = userId
            };
            var response = await httpHelper.SendPostRequestAsync<bool>("UserManagement/Delete", JsonSerializer.Serialize(model));
            if (response.IsError)
            {
                TempData["Error"] = response.Errors![0].Description;
                return RedirectToAction("Index");
            }

            TempData["Success"] = "User Deleted!";
            return RedirectToAction("Index");
        }
        catch (UnauthorizedAccessException)
        {
            return RedirectToAction("login", "Authentication");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromBody]EditUserRequest model)
    {
        var validationResults = model.Validate();
        if (validationResults.Count > 0)
        {
            return BadRequest(validationResults);
        }
        try
        {
            var response = await httpHelper.SendPostRequestAsync<bool>("UserManagement/Edit", JsonSerializer.Serialize(model));
            if (response.IsError)
            {
                Dictionary<string, string> errors = [];
                errors.Add(response.Errors![0].Code ?? "ERROR", response.Errors![0].Description ?? "Internal Server Error");
                return BadRequest(errors);
            }

            return Ok();
        }
        catch (UnauthorizedAccessException)
        {
            return RedirectToAction("login", "Authentication");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(EditUserViewModel user)
    {
        try
        {
            var groupsAndPermissions = await httpHelper.SendGetRequestAsync<GetGroupsAndPermissions>("UserManagement/GetGroupsAndPermissions");
            if (groupsAndPermissions.IsError)
            {
                ViewBag.Error = groupsAndPermissions!.Errors!.First().Description;
                return View();
            }
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            user.SetGroupsAndPermissions(groupsAndPermissions!.Value!.Groups, groupsAndPermissions.Value.Permissions);

            return View(user);

        }
        catch (UnauthorizedAccessException)
        {
            return RedirectToAction("login", "Authentication");
        }
    }

    public async Task<IActionResult> ViewStats()    
    {
        try
        {
            var stats = await httpHelper.SendGetRequestAsync<ApplicationStats>("UserManagement/GetStats");
            if (stats.IsError)
            {
                ViewBag.Error = stats!.Errors!.First().Description;
                return View();
            }
            return View(stats.Value);
        }
        catch (UnauthorizedAccessException)
        {
            return RedirectToAction("login", "Authentication");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
