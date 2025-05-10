using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.Advertisement;
using OrganicFood_MiniProject.Data;
using OrganicFood_MiniProject.Models;

namespace OrganicFood_MiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdvertisementController : Controller
    {
        public readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public AdvertisementController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public IActionResult Index()
        {
            Advertisement advertisement = _context.Advertisements.FirstOrDefault();
            if (advertisement is null) return View(null);
            return View(new AdvertisementVM { Id = advertisement.Id, BackgroundImage = advertisement.BackgroundImage, FirstImage = advertisement.FirstImage, SecondImage = advertisement.SecondImage, ThirdImage = advertisement.ThirdImage, FourthImage = advertisement.FourthImage });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdvertisementCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            if (!request.BackgroundImageUpload.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("BackgroundImageUpload", "Input type must be only image");
                return View(request);
            }

            string backgorungFileName = Guid.NewGuid().ToString() + "-" + request.BackgroundImageUpload.FileName;
            string backgroundFilePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", backgorungFileName);

            using (FileStream stream = new FileStream(backgroundFilePath, FileMode.Create))
            {
                await request.BackgroundImageUpload.CopyToAsync(stream);
            }


            if (!request.FirstImageUpload.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("FirstImageUpload", "Input type must be only image");
                return View(request);
            }

            string firstFileName = Guid.NewGuid().ToString() + "-" + request.FirstImageUpload.FileName;
            string firstFilePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", firstFileName);

            using (FileStream stream = new FileStream(firstFilePath, FileMode.Create))
            {
                await request.FirstImageUpload.CopyToAsync(stream);
            }


            if (!request.SecondImageUpload.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("SecondImageUpload", "Input type must be only image");
                return View(request);
            }

            string secondFileName = Guid.NewGuid().ToString() + "-" + request.SecondImageUpload.FileName;
            string secondFilePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", secondFileName);

            using (FileStream stream = new FileStream(secondFilePath, FileMode.Create))
            {
                await request.SecondImageUpload.CopyToAsync(stream);
            }


            if (!request.ThirdImageUpload.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("ThirdImageUpload", "Input type must be only image");
                return View(request);
            }

            string thirdFileName = Guid.NewGuid().ToString() + "-" + request.ThirdImageUpload.FileName;
            string thirdFilePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", thirdFileName);

            using (FileStream stream = new FileStream(thirdFilePath, FileMode.Create))
            {
                await request.ThirdImageUpload.CopyToAsync(stream);
            }


            if (!request.FourthImageUpload.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("FourthImageUpload", "Input type must be only image");
                return View(request);
            }

            string fourthFileName = Guid.NewGuid().ToString() + "-" + request.FourthImageUpload.FileName;
            string fourthFilePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", fourthFileName);

            using (FileStream stream = new FileStream(fourthFilePath, FileMode.Create))
            {
                await request.FourthImageUpload.CopyToAsync(stream);
            }


            await _context.Advertisements.AddAsync(new Advertisement { BackgroundImage = backgorungFileName, FirstImage = firstFileName, SecondImage = secondFileName, ThirdImage = thirdFileName, FourthImage = fourthFileName });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();

            Advertisement advertisement = await _context.Advertisements.FirstOrDefaultAsync(m => m.Id == id);
            if (advertisement == null) return NotFound();

            string backgroundFilePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", advertisement.BackgroundImage);

            if (!string.IsNullOrEmpty(advertisement.BackgroundImage) && System.IO.File.Exists(backgroundFilePath))
            {
                System.IO.File.Delete(backgroundFilePath);
            }


            string firstFilePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", advertisement.FirstImage);

            if (!string.IsNullOrEmpty(advertisement.FirstImage) && System.IO.File.Exists(firstFilePath))
            {
                System.IO.File.Delete(firstFilePath);
            }


            string secondFilePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", advertisement.SecondImage);

            if (!string.IsNullOrEmpty(advertisement.SecondImage) && System.IO.File.Exists(secondFilePath))
            {
                System.IO.File.Delete(secondFilePath);
            }


            string thirdFilePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", advertisement.ThirdImage);

            if (!string.IsNullOrEmpty(advertisement.ThirdImage) && System.IO.File.Exists(thirdFilePath))
            {
                System.IO.File.Delete(thirdFilePath);
            }


            string fourthFilePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", advertisement.FourthImage);

            if (!string.IsNullOrEmpty(advertisement.FourthImage) && System.IO.File.Exists(fourthFilePath))
            {
                System.IO.File.Delete(fourthFilePath);
            }

            _context.Advertisements.Remove(advertisement);
            await _context.SaveChangesAsync();

            if (!_context.Advertisements.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Advertisement advertisement = await _context.Advertisements.FirstOrDefaultAsync(m => m.Id == id);
            if (advertisement == null) return NotFound();
            return View(new AdvertisementDetailVM { BackgroundImage = advertisement.BackgroundImage, FirstImage = advertisement.FirstImage, SecondImage = advertisement.SecondImage, ThirdImage = advertisement.ThirdImage, FourthImage = advertisement.FourthImage});
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Advertisement advertisement = await _context.Advertisements.FirstOrDefaultAsync(m => m.Id == id);

            if (advertisement == null) return NotFound();

            return View(new AdvertisementEditVM { Id = advertisement.Id, ExistingBackgroundImage = advertisement.BackgroundImage, ExistingFirstImage = advertisement.FirstImage, ExistingSecondImage = advertisement.SecondImage, ExistingThirdImage = advertisement.ThirdImage, ExistingFourthImage = advertisement.FourthImage });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, AdvertisementEditVM request)
        {
            if (id == null) return BadRequest();

            Advertisement advertisement = await _context.Advertisements.FirstOrDefaultAsync(m => m.Id == id);
            if (advertisement == null) return NotFound();

            if (request.BackgroundImageUpload != null)
            {
                if (!request.BackgroundImageUpload.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("BackgroundImageUpload", "File must be an image");
                    return View(request);
                }

                string oldImagePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", advertisement.BackgroundImage);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                string backgroundFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.BackgroundImageUpload.FileName);
                string backgroundFilePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", backgroundFileName);

                using (FileStream stream = new FileStream(backgroundFilePath, FileMode.Create))
                {
                    await request.BackgroundImageUpload.CopyToAsync(stream);
                }

                advertisement.BackgroundImage = backgroundFileName;
            }


            if (request.FirstImageUpload != null)
            {
                if (!request.FirstImageUpload.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("FirstImageUpload", "File must be an image");
                    return View(request);
                }

                string oldImagePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", advertisement.FirstImage);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                string firstFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.FirstImageUpload.FileName);
                string firstFilePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", firstFileName);

                using (FileStream stream = new FileStream(firstFilePath, FileMode.Create))
                {
                    await request.FirstImageUpload.CopyToAsync(stream);
                }

                advertisement.FirstImage = firstFileName;
            }


            if (request.SecondImageUpload != null)
            {
                if (!request.SecondImageUpload.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("SecondImageUpload", "File must be an image");
                    return View(request);
                }

                string oldImagePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", advertisement.SecondImage);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                string secondFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.SecondImageUpload.FileName);
                string secondFilePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", secondFileName);

                using (FileStream stream = new FileStream(secondFilePath, FileMode.Create))
                {
                    await request.SecondImageUpload.CopyToAsync(stream);
                }

                advertisement.SecondImage = secondFileName;
            }


            if (request.ThirdImageUpload != null)
            {
                if (!request.ThirdImageUpload.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("ThirdImageUpload", "File must be an image");
                    return View(request);
                }

                string oldImagePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", advertisement.ThirdImage);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                string thirdFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ThirdImageUpload.FileName);
                string thirdFilePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", thirdFileName);

                using (FileStream stream = new FileStream(thirdFilePath, FileMode.Create))
                {
                    await request.ThirdImageUpload.CopyToAsync(stream);
                }

                advertisement.ThirdImage = thirdFileName;
            }


            if (request.FourthImageUpload != null)
            {
                if (!request.FourthImageUpload.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("FourthImageUpload", "File must be an image");
                    return View(request);
                }

                string oldImagePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", advertisement.FourthImage);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                string fourthFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.FourthImageUpload.FileName);
                string fourthFilePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", fourthFileName);

                using (FileStream stream = new FileStream(fourthFilePath, FileMode.Create))
                {
                    await request.FourthImageUpload.CopyToAsync(stream);
                }

                advertisement.FourthImage = fourthFileName;
            }


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
