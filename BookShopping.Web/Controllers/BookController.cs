using BookShopping.Web.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShopping.Web.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]

    public class BookController : Controller
    {
        private readonly IBookRepository _bookrepo;
        private readonly IGendreRepository _gendrerepo;
        private readonly IFileService _file;

        public BookController(
            IBookRepository bookrepo,
            IGendreRepository gendrerepo,
            IFileService file)

        {
            _bookrepo = bookrepo;
            _gendrerepo = gendrerepo;
            _file = file;
        }



        public async Task<IActionResult> Index()
        {
            var books = await _bookrepo.GetAllBooks();
            return View(books);
        }




        #region AddBook
        public async Task<IActionResult> AddBook()
        {
            var gendrselectlist = (await _gendrerepo.GetAllGendres())
                .Select(gen => new SelectListItem
                {
                    Text = gen.GendreName,
                    Value = gen.Id.ToString(),
                });

            BookDTOs addbook = new()
            {
                GenreList = gendrselectlist,
            };

            return View(addbook);
        }


        [HttpPost]
        public async Task<IActionResult> AddBook(BookDTOs bookDT)
        {
            var gendrselectlist = (await _gendrerepo.GetAllGendres())
             .Select(gen => new SelectListItem
             {
                 Text = gen.GendreName,
                 Value = gen.Id.ToString(),
             });

            bookDT.GenreList = gendrselectlist;
            if (!ModelState.IsValid)
                return View(bookDT);

            try
            {
                if (bookDT.ImageFile != null)
                {
                    if (bookDT.ImageFile.Length > 1 * 1024 * 1024)
                    {
                        throw new InvalidOperationException("invalid picture");
                    };

                    string[] allow = [".jpeg", ".jpg", ".png"];
                    string imagename = await _file
                        .SaveFile(bookDT.ImageFile, allow);
                    bookDT.Image = imagename;
                }

                Book books = new()
                {
                    Id = bookDT.Id,
                    Title = bookDT.BookName,
                    GendreId = bookDT.GenreId,
                    Author = bookDT.AuthorName,

                    Image = bookDT.Image,
                    Price = bookDT.Price,


                };
                await _bookrepo.AddBook(books);
                TempData["successMessage"] = "BOOK Added Successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("AddBook");

            }


        }


        #endregion



        #region  EditingBook
        public async Task<IActionResult> UpdateBook(int id)
        {
            var book = await _bookrepo.GetBookById(id);
            if (book == null)
            {
                TempData["successMassage"] = "Book Id Not Founded";
                return RedirectToAction(nameof(Index));
            }
            var GendreList = (await _gendrerepo.GetAllGendres())
                .Select(gen => new
                SelectListItem
                {
                    Text = gen.GendreName,
                    Value = gen.Id.ToString(),
                    Selected = gen.Id == book.GendreId,
                });

            BookDTOs updated = new()
            {
                Id = book.Id,
                BookName = book.Title,
                GenreId = book.GendreId,
                AuthorName = book.Author,
                Image = book.Image,
                Price = book.Price,
            };

            return View(updated);
        }







        [HttpPost]
        public async Task<IActionResult> UpdateBook(BookDTOs bookDt)
        {

            var GendreList = (await _gendrerepo.GetAllGendres())
                          .Select(gen => new
                          SelectListItem
                          {
                              Text = gen.GendreName,
                              Value = gen.Id.ToString(),
                              Selected = gen.Id == bookDt.GenreId,
                          });
            bookDt.GenreList = GendreList;
            if (!ModelState.IsValid)
            {
                return View(bookDt);
            }

            try
            {
                string oldimage = "";
                if (bookDt.ImageFile != null)
                {
                    if (bookDt.ImageFile.Length > 1 * 1024 * 1024)
                    {
                        throw new InvalidOperationException("invalid picture");

                    }


                    string[] allow = [".jpeg", ".jpg", ".png"];
                    string imagename = await _file
                        .SaveFile(bookDt.ImageFile, allow);
                    oldimage = bookDt.Image;
                    bookDt.Image = imagename;

                }

                Book books = new()
                {
                    Id = bookDt.Id,
                    Title = bookDt.BookName,
                    GendreId = bookDt.GenreId,
                    Author = bookDt.AuthorName,
                    Image = bookDt.Image,
                    Price = bookDt.Price,


                };

                await _bookrepo.EditBook(books);
                if (!string.IsNullOrEmpty(oldimage))
                {
                    _file.DeleteFile(oldimage);
                }
                TempData["successMessage"] = "BOOK Update Successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(bookDt);
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(bookDt);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Error on saving data";
                return View(bookDt);
            }
        }
        #endregion



        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await _bookrepo.GetBookById(id);
                if (book == null)
                {
                    TempData["errorMessage"] = $"Book with the id: {id} does not found";
                }
                else
                {
                    await _bookrepo.DeleteBook(book);
                    if (!string.IsNullOrWhiteSpace(book.Image))
                    {
                        _file.DeleteFile(book.Image);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Error on deleting the data";
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
