using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExploreCalifornia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace ExploreCalifornia.Controllers
{
    [Route("specials")]
    public class SpecialController : Controller
    {
        private readonly SpecialsDataContext _db;

        public SpecialController(SpecialsDataContext db)
        {
            _db = db;
        }

        [HttpGet, Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, Route("create")]
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
            if (special.Key != original.Key) original.Key = special.Key;

            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
