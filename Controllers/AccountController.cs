﻿using Library_Management_Project.Models;
using Library_Management_Project.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library_Management_Project.Controllers
{

    public class AccountController : Controller
    {
        private readonly IAccountRepository _db;

        public AccountController(IAccountRepository _db)
        {
            this._db = _db;
        }
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserRegistration obj)
        {
            if (ModelState.IsValid)
            {
                var result = await _db.CreateUserAsync(obj);
                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(obj);
                }
                ModelState.Clear();
            }
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInUser obj , string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _db.LoginUser(obj);
                if (result.Succeeded)
                {
                    if(!string.IsNullOrEmpty(returnUrl)) 
                    {
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid Data Credentials");
            }

            return View(obj);
        }
        public IActionResult Logout()
        {
            _db.LogOut();
            return RedirectToAction("Index", "Home");
        }

    }
}
