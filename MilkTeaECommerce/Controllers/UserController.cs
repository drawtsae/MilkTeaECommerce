﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models;
using MilkTeaECommerce.Models.Models;

namespace MilkTeaECommerce.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IHostEnvironment _hostEnvironment;
        public UserController(ApplicationDbContext db,SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IHostEnvironment hostEnvironment)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult>Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = new ProfileViewModel();
            profile.Email = user.Email;
            profile.Username = user.UserName;
            profile.Name = user.Name;
            profile.Phonenumber = user.PhoneNumber;
            profile.ImageUrl = user.ImageUrl;
            profile.IsMailComfirm = user.EmailConfirmed;
            profile.IsPhoneComfirm = user.PhoneNumberConfirmed;
            return View(profile);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel vm)
        {
            if(ModelState.IsValid)
            {
                string fileName = "";
                string folderName = "wwwroot\\Media\\";
                string webRootPath = _hostEnvironment.ContentRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (vm.file != null)
                {
                    fileName = ContentDispositionHeaderValue.Parse(vm.file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        vm.file.CopyTo(stream);

                    }
                }
                var user = await _userManager.FindByNameAsync(vm.Username);
                user.Name = vm.Name;
                if(vm.Email.ToLower() != user.Email.ToLower())
                {
                    user.UserName = vm.Email;
                    user.Email = vm.Email;
                    user.EmailConfirmed = false;
                }  
                if(vm.Phonenumber!=user.PhoneNumber)
                {
                    user.PhoneNumber = vm.Phonenumber;
                    user.PhoneNumberConfirmed = false;
                }
                if(fileName!="")
                {
                    user.ImageUrl =@"/Media/"+ fileName;
                    vm.ImageUrl = user.ImageUrl;
                }    
                try
                {
                    _db.AspNetUsers.Update(user);
                    _db.SaveChanges();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                    
                }
            }
            return View(vm);
        }
        public async Task<IActionResult> OrderDetail()
        {
            return View();
        }

    }
}
