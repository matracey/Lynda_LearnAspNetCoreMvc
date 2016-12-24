using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExploreCalifornia.Models;
using ImageSharp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExploreCalifornia.Controllers
{
    [Route("specials")]
    public class SpecialController : Controller
    {
        private readonly SpecialsDataContext _db;
        private readonly IHostingEnvironment _env;

        public SpecialController(SpecialsDataContext db, IHostingEnvironment env)
        {
            _db = db;
            _env = env;
        }

        #region Create
        [HttpGet, Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, Route("create")]
        //public IActionResult Create(Special special, [Bind("image")]ICollection<IFormFile> image)
        public async Task<IActionResult> Create(SpecialViewModel special)
        {
            if (_db.Specials.Any(x => x.Name == special.Name))
                ModelState.AddModelError("Name", "Special Name must be a unique value.");
            if (!ModelState.IsValid) return View();

            special.Created = DateTime.Now;

            _db.Specials.Add(special.ToSpecial());

            SaveImage(special.Image, special.ToSpecial().Key);

            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Edit
        [HttpGet, Route("edit/{key}")]
        public IActionResult Edit(string key)
        {
            var special = _db.Specials.FirstOrDefault(x => x.Key == key).ToViewModel();
            return View(special);
        }

        [HttpPost, Route("edit/{key}")]
        public async Task<IActionResult> Edit(SpecialViewModel special)
        {
            var s = _db.Specials.Where(x => x.Id != special.Id);
            if (s.Any(x => x.Name == special.Name)) ModelState.AddModelError("Name", "Special Name must be a unique value.");
            if (s.Any(x => x.Key == special.Key)) ModelState.AddModelError("Key", "Special Key must be a unique value.");

            if (!ModelState.IsValid)
            {
                special.DeleteKey = _db.Specials.FirstOrDefault(x => x.Id == special.Id).Key;
                return View(special);
            }

            var original = _db.Specials.FirstOrDefault(x => x.Id == special.Id); // Match on ID as key may have changed.

            if (special.Name != original.Name) original.Name = special.Name;
            if (special.Type != original.Type) original.Type = special.Type;
            if (special.Price != original.Price) original.Price = special.Price;
            if (special.Key != original.Key)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                try
                {
                    System.IO.File.Move(Path.Combine(uploads, original.Key + ".jpg"),
                        Path.Combine(uploads, special.Key + ".jpg"));
                }
                catch (Exception e)
                {
                    // Log exception.
                    Console.WriteLine(e.Message);
                }

                original.Key = special.Key;
            }

            SaveImage(special.Image, original.Key);

            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Delete
        [HttpGet, Route("delete/{key}")]
        public IActionResult Delete(string key)
        {
            var special = _db.Specials.FirstOrDefault(x => x.Key == key);
            return View(model: special.ToViewModel());
        }

        [HttpPost, Route("delete/{key}")]
        public async Task<IActionResult> Delete(Special s)
        {
            var special = _db.Specials.FirstOrDefault(x => x.Key == s.Key);

            var uploads = Path.Combine(_env.WebRootPath, "uploads");
            try
            {
                System.IO.File.Delete(Path.Combine(uploads, special.Key + ".jpg"));
            }
            catch (Exception e)
            {
                // Log exception.
                Console.WriteLine(e.Message);
            }

            _db.Specials.Remove(special);

            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
        #endregion

        private void SaveImage(ICollection<IFormFile> image, string key)
        {
            var uploads = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);


            if (image == null) return;
            foreach (var formFile in image)
            {
                if (formFile.Length <= 0) continue;

                var fileName = $"{key}.jpg";

                using (var stream = formFile.OpenReadStream())
                using (var output = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                {
                    var img = new Image(stream);
                    img.SaveAsJpeg(output);
                }
            }
        }
    }
}
