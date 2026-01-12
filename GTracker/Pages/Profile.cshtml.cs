using BLL.Services;
using GTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;

namespace GTracker.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly UserService _userService;

        public ProfileModel(UserService userService)
        {
            _userService = userService;
        }

        public User? User { get; set; }

        [BindProperty] 
        public IFormFile? UploadedImage { get; set; }

        public IActionResult OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Login");

            User = _userService.GetUserById(userId.Value);
            if (User == null)
                return RedirectToPage("/Login");

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Login");

            if (UploadedImage == null || UploadedImage.Length == 0)
            {
                ModelState.AddModelError("UploadedImage", "Please choose an image.");
                return await ReloadUserAndReturnPage(userId.Value);
            }

            try
            {
                using var imageStream = UploadedImage.OpenReadStream();
                using var image = Image.FromStream(imageStream);

                // 400x400 max limit
                if (image.Width > 400 || image.Height > 400)
                {
                    ModelState.AddModelError("UploadedImage", "Image must be 400 × 400 pixels or smaller.");
                    return await ReloadUserAndReturnPage(userId.Value);
                }

                imageStream.Position = 0;

                using var ms = new MemoryStream();
                await imageStream.CopyToAsync(ms);

                byte[] pfpBytes = ms.ToArray();

                _userService.UpdateProfilePicture(userId.Value, pfpBytes);

                HttpContext.Session.SetString("UserPFP", Convert.ToBase64String(pfpBytes));


                return RedirectToPage(); // refresh page w updated image
            }
            catch
            {
                ModelState.AddModelError("UploadedImage", "That file isn't a valid image.");
                return await ReloadUserAndReturnPage(userId.Value);
            }
        }

        // Optional: matches a button like formaction="?handler=Remove"
        public IActionResult OnPostRemove()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Login");

            _userService.RemoveProfilePicture(userId.Value);
            HttpContext.Session.SetString("UserPFP", "");
            return RedirectToPage();
        }

        private async Task<IActionResult> ReloadUserAndReturnPage(int userId)
        {
            // Reload user so existing PFP still displays if validation fails
            User = _userService.GetUserById(userId);
            await Task.CompletedTask;
            return Page();
        }
    }
}
