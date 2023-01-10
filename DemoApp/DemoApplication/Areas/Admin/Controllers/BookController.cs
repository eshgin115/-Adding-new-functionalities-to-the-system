using DemoApplication.Areas.Admin.ViewModels.Book;
using DemoApplication.Areas.Admin.ViewModels.Book.Add;
using DemoApplication.Contracts.File;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/book")]
    public class BookController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<BookController> _logger;
        private readonly IFileService _fileService;

        public BookController(DataContext dataContext, ILogger<BookController> logger,IFileService fileService)
        {
            _dataContext = dataContext;
            _logger = logger;
            _fileService = fileService;
        }


        #region List

        [HttpGet("list", Name = "admin-book-list")]
        public async Task<IActionResult> ListAsync()
        {
            var model = await _dataContext.Books
                .Select(b => new ListItemViewModel(
                        b.Id,
                        b.Title,
                        b.Price,
                        $"{b.Author.FirstName} {b.Author.LastName}",
                        b.CreatedAt,
                        b.BookCategories
                            .Select(bc => bc.Category)
                                .Select(c => new ListItemViewModel.CategoryViewModeL(c.Title, c.Parent.Title)).ToList()))
                .ToListAsync();

            return View(model);
        }

        #endregion

        #region Add

        [HttpGet("add", Name = "admin-book-add")]
        public IActionResult Add()
        {
            var model = new AddViewModel
            {
                Authors = _dataContext.Authors
                    .Select(a => new AuthorListItemViewModel(a.Id, $"{a.FirstName} {a.LastName}"))
                    .ToList(),
                Categories = _dataContext.Categories
                    .Select(c => new CategoryListItemViewModel(c.Id, c.Title))
                    .ToList(),
            };

            return View(model);
        }

        [HttpPost("add", Name = "admin-book-add")]
        public async Task<IActionResult> AddAsync(AddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return GetView(model);
            }

            if (!_dataContext.Authors.Any(a => a.Id == model.AuthorId))
            {
                ModelState.AddModelError(string.Empty, "Author is not found");
                return GetView(model);
            }

            foreach (var categoryId in model.CategoryIds)
            {
                if (!_dataContext.Categories.Any(c => c.Id == categoryId))
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong");
                    _logger.LogWarning($"Category with id({categoryId}) not found in db ");
                    return GetView(model);
                }

            }
            List<string> imageNamesInSystem = await _fileService.UploadAsync(model!.Images, UploadDirectory.Book);
            var MainimageNameInSystem = await _fileService.UploadAsync(model!.MainImage, UploadDirectory.Book);
            var HoverimageNameInSystem = await _fileService.UploadAsync(model!.HoverImage, UploadDirectory.Book);
            AddBook(model.Images, imageNamesInSystem);

            return RedirectToRoute("admin-book-list");




            IActionResult GetView(AddViewModel model)
            {
                model.Authors = _dataContext.Authors
                    .Select(a => new AuthorListItemViewModel(a.Id, $"{a.FirstName} {a.LastName}"))
                    .ToList();

                model.Categories = _dataContext.Categories
                   .Select(c => new CategoryListItemViewModel(c.Id, c.Title))
                   .ToList();

                return View(model);
            }

            void AddBook(List<IFormFile> formFiles, List<string> imageNameInSystem)
            {
                var book = new Book
                {
                    Title = model.Title,
                    Price = model.Price,
                    AuthorId = model.AuthorId,
                };
                for (int i = 0; i < formFiles.Count; i++)
                {
                    var image = new Image
                    {

                        Book = book,
                        ImageName = formFiles[i].Name,
                        ImageNameInFileSystem = imageNameInSystem[i]
                    };
                    _dataContext.Images.Add(image);
                };
                var Mainimage = new Image
                {

                    Book = book,
                    Status=true,
                    ImageName = model.MainImage.FileName,
                    ImageNameInFileSystem = MainimageNameInSystem
                };
                var Hoverimage = new Image
                {

                    Book = book,
                    Status = false,
                    ImageName = model.MainImage.FileName,
                    ImageNameInFileSystem = MainimageNameInSystem
                };
                _dataContext.Images.Add(Mainimage);
                _dataContext.Images.Add(Hoverimage);
                _dataContext.Books.Add(book);

                foreach (var categoryId in model.CategoryIds)
                {
                    var bookCategory = new BookCategory
                    {
                        CategoryId = categoryId,
                        Book = book,
                    };

                    _dataContext.BookCategories.Add(bookCategory);
                }

                _dataContext.SaveChanges();
            }
        }


        #endregion

        #region Update

        [HttpGet("update/{id}", Name = "admin-book-update")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id)
        {
            var book = await _dataContext.Books.Include(b => b.BookCategories).FirstOrDefaultAsync(b => b.Id == id);
            if (book is null)
            {
                return NotFound();
            }

            var model = new AddViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Price = book.Price,
                AuthorId = book.AuthorId,
                Authors = _dataContext.Authors
                    .Select(a => new AuthorListItemViewModel(a.Id, $"{a.FirstName} {a.LastName}"))
                    .ToList(),
                Categories = _dataContext.Categories
                    .Select(c => new CategoryListItemViewModel(c.Id, c.Title))
                    .ToList(),
                CategoryIds = book.BookCategories.Select(bc => bc.CategoryId).ToList()
            };

            return View(model);
        }

        [HttpPost("update/{id}", Name = "admin-book-update")]
        public async Task<IActionResult> UpdateAsync(AddViewModel model)
        {
            var book = await _dataContext.Books.Include(b => b.BookCategories).FirstOrDefaultAsync(b => b.Id == model.Id);
            if (book is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return GetView(model);
            }

            if (!_dataContext.Authors.Any(a => a.Id == model.AuthorId))
            {
                ModelState.AddModelError(string.Empty, "Author is not found");
                return GetView(model);
            }

            foreach (var categoryId in model.CategoryIds)
            {
                if (!_dataContext.Categories.Any(c => c.Id == categoryId))
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong");
                    _logger.LogWarning($"Category with id({categoryId}) not found in db ");
                    return GetView(model);
                }

            }


            await UpdateBookAsync();

            return RedirectToRoute("admin-book-list");




            IActionResult GetView(AddViewModel model)
            {
                model.Authors = _dataContext.Authors
                    .Select(a => new AuthorListItemViewModel(a.Id, $"{a.FirstName} {a.LastName}"))
                    .ToList();

                model.Categories = _dataContext.Categories
                   .Select(c => new CategoryListItemViewModel(c.Id, c.Title))
                   .ToList();

                model.CategoryIds = book.BookCategories.Select(bc => bc.CategoryId).ToList();

                return View(model);
            }

            async Task UpdateBookAsync()
            {
                book.Title = model.Title;
                book.AuthorId = model.AuthorId;
                book.Price = model.Price;
                book.UpdatedAt = DateTime.Now;

                var categoriesInDb = book.BookCategories.Select(bc => bc.CategoryId).ToList();
                var categoriesToRemove = categoriesInDb.Except(model.CategoryIds).ToList();
                var categoriesToAdd = model.CategoryIds.Except(categoriesInDb).ToList();

                book.BookCategories.RemoveAll(bc => categoriesToRemove.Contains(bc.CategoryId));

                foreach (var categoryId in categoriesToAdd)
                {
                    var bookCategory = new BookCategory
                    {
                        CategoryId = categoryId,
                        Book = book,
                    };

                    _dataContext.BookCategories.Add(bookCategory);
                }

                _dataContext.SaveChanges();
            }
        }

        #endregion

        #region Delete

        [HttpPost("delete/{id}", Name = "admin-book-delete")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var book = await _dataContext.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book is null)
            {
                return NotFound();
            }

            _dataContext.Books.Remove(book);
            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-book-list");
        }

        #endregion
    }
}
