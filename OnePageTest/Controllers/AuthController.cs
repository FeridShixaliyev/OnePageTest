using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnePageTest.Helpers;
using OnePageTest.Models;
using OnePageTest.ViewModels;
using System;
using System.Threading.Tasks;

namespace OnePageTest.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        //private readonly UserManager<AppUser> _userManager;
        //private readonly SignInManager<AppUser> _signInManager;
        //private readonly RoleManager<IdentityRole> _roleManager;

        //public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //    _roleManager = roleManager;
        //}
        //public IActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Register(RegisterVM registerVM)
        //{
        //    if (!ModelState.IsValid) return View();
        //    if (registerVM == null) return NotFound();
        //    AppUser appUser = new AppUser
        //    {
        //        FirstName = registerVM.FirstName,
        //        LastName = registerVM.LastName,
        //        UserName = registerVM.Username,
        //        Email = registerVM.Email
        //    };
        //    IdentityResult resault = await _userManager.CreateAsync(appUser, registerVM.Password);
        //    if (!resault.Succeeded)
        //    {
        //        foreach (var item in resault.Errors)
        //        {
        //            ModelState.AddModelError("", item.Description);
        //            return View();
        //        }
        //    }
        //    await _userManager.AddToRoleAsync(appUser,UserRoles.M.ToString());
        //    return RedirectToAction("Index", "Home");
        //}
        //public async Task<IActionResult> LogOut()
        //{
        //    await _signInManager.SignOutAsync();
        //    return RedirectToAction("Index", "Home");
        //}

        //public IActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginVM loginVM)
        //{
        //    AppUser user;
        //    if (loginVM.UsernameOrEmail.Contains("@"))
        //    {
        //        user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
        //    }
        //    else
        //    {
        //        user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
        //    }
        //    if (user == null)
        //    {
        //        ModelState.AddModelError("", "Email,Sifre ve ya Istifadeci adi yanlisdir!!");
        //        return View();
        //    }
        //    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, false);
        //    if (result.IsLockedOut)
        //    {
        //        ModelState.AddModelError("", "Siz max giris xetasi etdiyiniz ucun bir muddet gozlemeli olacagsiz");
        //        return View();
        //    }
        //    if (!result.Succeeded)
        //    {
        //        ModelState.AddModelError("", "Email,Sifre ve ya Istifadeci adi yanlisdir!!");
        //        return View(loginVM);
        //    }
        //    //await _signInManager.SignInAsync(user,loginVM.RememberMe);
        //    return RedirectToAction("Index", "Home");
        //}
        //public async Task CreateRoles()
        //{
        //    foreach (var item in Enum.GetValues(typeof(UserRoles)))
        //    {
        //        if (!await _roleManager.RoleExistsAsync(item.ToString()))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole(item.ToString()));
        //        }
        //    }
        //}
        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();
            if (registerVM == null) return NotFound();
            AppUser user = new AppUser
            {
                FirstName=registerVM.FirstName,
                LastName=registerVM.LastName,
                Email=registerVM.Email,
                UserName=registerVM.Username
            };
            IdentityResult result = await _userManager.CreateAsync(user,registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("",item.Description);
                    return View();
                }
            }
            await _userManager.AddToRoleAsync(user,UserRoles.Member.ToString());
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            AppUser user;
            if (loginVM.UsernameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
            }
            if (user == null)
            {
                ModelState.AddModelError("","Username,Email ve ya Sifre yanlisdir!!");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user,loginVM.Password,loginVM.RememberMe,false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username,Email ve ya Sifre yanlisdir!!");
                return View(loginVM);
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Sizin hesab 3 defe yanslis parol girildiyi ucun bloklanib bir muddet gozleyin zehmet olmasa!!");
                return View();
            }
            await _signInManager.SignInAsync(user,loginVM.RememberMe);
            return RedirectToAction("Index","Home");
        }
        public async Task CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(UserRoles)))
            {
                if (await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole(item.ToString()));
                }
            }
        }
    }
}
