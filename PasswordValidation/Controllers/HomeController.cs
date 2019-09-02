using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PasswordValidation.Entites;
using PasswordValidation.InfraStructers;
using PasswordValidation.Models;

namespace PasswordValidation.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IUserPasswordChangeHistoryService userPasswordChangeHistoryService;

        public HomeController(IUserPasswordChangeHistoryService userPasswordChangeHistoryService, UserManager<AppUser> userManager)
        {
            this.userPasswordChangeHistoryService = userPasswordChangeHistoryService;
            this.userManager = userManager;
        }
        
        public IActionResult Index()
        {            
            return View();
        }

        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser()
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                
                var result = await userManager.CreateAsync(user, model.Password);                

                if (result.Succeeded)
                {
                    await userPasswordChangeHistoryService.AddHistory(user.Id, model.Password);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var item in result.Errors)
                        ModelState.AddModelError("", item.Description);
                }
            }

            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var user =await userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        await userPasswordChangeHistoryService.AddHistory(user.Id, model.NewPassword);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                            ModelState.AddModelError("", item.Description);
                    }
                }
                else
                    ModelState.AddModelError("", "Can Not Find User");
            }

            return View(model);
        }
    }
}