﻿using CarVillaExam.Helpers;
using CarVillaExam.Models;
using CarVillaExam.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarVillaExam.Controllers
{
    public class AccountController:Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager , RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser appUser = new AppUser()
            {
                Name= registerVM.Name,
                Surname= registerVM.Surname,
                Email = registerVM.Email,
                UserName= registerVM.UserName,
            };
            var result = await _userManager.CreateAsync(appUser,registerVM.Password);
            if (!result.Succeeded) 
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }

            await _userManager.AddToRoleAsync(appUser, UserRole.Admin.ToString());
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View();
            var user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEMail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UsernameOrEMail);
                if (user == null) throw new Exception("Username/Email or Password is incorrect.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginVM.Password,false);
            if (!result.Succeeded) throw new Exception("Username/Email or Password is incorrect.");
            await _signInManager.SignInAsync(user, false);
           
            return RedirectToAction(nameof(Index),"Home");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> CreateRole()
        {
            foreach(UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if(!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole()
                    {
                        Name= role.ToString()
                    });
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
