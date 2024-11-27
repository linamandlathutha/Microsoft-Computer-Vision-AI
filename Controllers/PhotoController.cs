using Microsoft.AspNetCore.Mvc;
using WebApplication23.Models;

namespace WebApplication23.Controllers
{
    public class PhotoController : Controller
    {
        private readonly PhotoDbContext _context;
        private readonly ImageService _imageService;

        public PhotoController(PhotoDbContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        // Action for uploading an image and getting a description
        public IActionResult Upload()
        {
            return View();
        }

        public IActionResult Home() 
        
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                ViewBag.Message = "Please select an image to upload.";
                return View();
            }

            var imagePath = _imageService.SaveImage(image);
            var description = await _imageService.GetImageDescriptionAsync(imagePath);

            var photo = new Photo
            {
                ImagePath = imagePath,
                Description = description
            };

            _context.Photos.Add(photo);
            await _context.SaveChangesAsync();

            ViewBag.Description = description;
            return View();
        }

        // Action for displaying all uploaded images
        public IActionResult Gallery()
        {
            var photos = _context.Photos.ToList();
            return View(photos);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var photo = await _context.Photos.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            // Delete the image file from the server
            if (System.IO.File.Exists(photo.ImagePath))
            {
                System.IO.File.Delete(photo.ImagePath);
            }

            // Remove the record from the database
            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();

            return RedirectToAction("Gallery");
        }
    }


}
