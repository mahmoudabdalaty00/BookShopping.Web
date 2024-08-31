using BookShopping.Web.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopping.Web.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]

    public class GendreController : Controller
    {
        private readonly IGendreRepository _genrepo;

        public GendreController(IGendreRepository genrepo)
        {
            _genrepo = genrepo;
        }

        public async Task<IActionResult> Index()
        {
            var gendres = await _genrepo.GetAllGendres();
            return View(gendres);
        }

        public async Task<IActionResult> AddGendre()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddGendre(GendreDtos gendre)
        {
            if (ModelState.IsValid)
            {
                return View(gendre);
            }

            try
            {
                var addgendre = new Gendre
                {
                    GendreName = gendre.GendreName,
                    Id = gendre.Id,

                };
                await _genrepo.AddGendre(addgendre);
                TempData["successMessage"] = "Gendre Add Successful";
                return RedirectToAction("Index");
            }

            catch (Exception ex)
            {
                TempData["Failed"] = "Gendre Failed to Added ";
                return View(gendre);

            }


        }

        public async Task<IActionResult> EditGendre(int id)
        {
            var gen = await _genrepo.GetGendreById(id);
            if (gen == null)
                throw new InvalidOperationException("Gendre Not Founded");

            var updategen = new GendreDtos
            {
                GendreName = gen.GendreName,
                Id = gen.Id,
            };

            return View(updategen);

        }


        [HttpPost]
        public async Task<IActionResult> EditGendre(GendreDtos gendre)
        {
            if (!ModelState.IsValid)
            {
                return View(gendre);
            }
            try
            {
                var gen = new Gendre
                {
                    GendreName = gendre.GendreName,
                    Id = gendre.Id,
                };
                await _genrepo.UpdateGendre(gen);
                TempData["successMessage"] = "Gendre Editting Successful";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Failed"] = "Gendre Failed to Added ";
                return View(gendre);
            }

        }


        public async Task<IActionResult> DeleteGendre(Gendre gendre)
        {
            await _genrepo.DeleteGendre(gendre);
            return RedirectToAction("Index");
        }
    }
}
