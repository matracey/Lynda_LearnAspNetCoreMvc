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
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

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

        [HttpGet, Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, Route("create")]
        //public IActionResult Create(Special special, [Bind("image")]ICollection<IFormFile> image)
        public IActionResult Create(Special special)
        {
            if(!ModelState.IsValid) return View();

            special.Created = DateTime.Now;

            _db.Specials.Add(special);
            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet, Route("edit/{key}")]
        public IActionResult Edit(string key)
        {
            var special = _db.Specials.FirstOrDefault(x => x.Key == key);
            return View(special);
        }

        [HttpPost, Route("edit/{key}")]
        public IActionResult Edit(Special special)
        {
            if (!ModelState.IsValid) return View();

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

            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet, Route("upload-image/{key}")]
        public IActionResult UploadImage(string key)
        {
            return View();
        }

        [HttpPost, Route("upload-image/{key}")]
        public IActionResult UploadImage(ICollection<IFormFile> image, string key)
        {
            SaveImage(image, key);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost, Route("delete/{key}")]
        public IActionResult Delete(string key)
        {
            var special = _db.Specials.FirstOrDefault(x => x.Key == key);

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

            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        private void SaveImage(ICollection<IFormFile> image, string key)
        {
            var uploads = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);


            foreach (var formFile in image)
            {
                if (formFile.Length <= 0) continue;

                var fileName = $"{key}.jpg";

                using(var stream = formFile.OpenReadStream())
                using (var output = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                {
                    //await formFile.CopyToAsync(output);
                    var img = new Image(stream);
                    img.SaveAsJpeg(output);
                }
            }
        }
    }
}
