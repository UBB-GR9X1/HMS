using Microsoft.AspNetCore.Mvc;
using HMS.WebClient.Models;
using HMS.WebClient.Services;
using HMS.Shared.DTOs;

namespace HMS.WebClient.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // if user is already logged in, redirect to home
            if (_authService.GetCurrentUser() != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.Login(model.Email, model.Password);

            if (result == null)
            {
                ModelState.AddModelError("", "Invalid login attempt. Please check your email and password.");
                return View(model);
            }

            // redirect based on user role
            switch (result.Role)
            {
                case Shared.Enums.UserRole.Doctor:
                    return RedirectToAction("Profile", "Doctor");
                case Shared.Enums.UserRole.Patient:
                    return RedirectToAction("Profile", "Patient");
                case Shared.Enums.UserRole.Admin:
                    return RedirectToAction("Dashboard", "Admin");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            // if user is already logged in, redirect to home
            if (_authService.GetCurrentUser() != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userDto = new UserDto
            {
                Email = model.Email,
                Password = model.Password,
                Name = model.Name,
                CNP = model.CNP,
                PhoneNumber = model.PhoneNumber,
                Role = model.Role,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _authService.Register(userDto);

            if (!result)
            {
                ModelState.AddModelError("", "Registration failed. Please try again or contact support.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Registration successful! Please login.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            _authService.Logout();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}