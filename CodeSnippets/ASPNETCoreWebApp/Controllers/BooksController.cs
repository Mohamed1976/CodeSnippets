using ASPNETCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Controllers
{
    public class BooksController : Controller
    {

        private List<Book> books = new List<Book>()
        {
            new Book() { Id=1, Title="Title1", Genre="Genre1", PublishDate=DateTime.Now, Price=19.99m },
            new Book() { Id=2, Title="Title2", Genre="Genre1", PublishDate=DateTime.Now, Price=19.99m },
            new Book() { Id=3, Title="Title3", Genre="Genre1", PublishDate=DateTime.Now, Price=19.99m },
            new Book() { Id=4, Title="Title4", Genre="Genre1", PublishDate=DateTime.Now, Price=19.99m },
            new Book() { Id=5, Title="Title5", Genre="Genre1", PublishDate=DateTime.Now, Price=19.99m },
            new Book() { Id=6, Title="Title6", Genre="Genre1", PublishDate=DateTime.Now, Price=19.99m },
            new Book() { Id=7, Title="Title7", Genre="Genre1", PublishDate=DateTime.Now, Price=19.99m },
        };


        // GET: Books
        public async Task<IActionResult> Index()
        {
            await Task.Delay(100);
            return View(books);
        }

        public async Task<IActionResult> Details(string id)
        {
            bool isValid = int.TryParse(id, out int bookId);

            Book book = (from _book in books
                         where _book.Id == bookId
                         select _book).FirstOrDefault();

            await Task.Delay(100);
            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult>Edit(int? id)
        {
            await Task.Delay(100);

            if (id == null)
            {
                return NotFound();
            }

            Book book = (from _book in books
                         where _book.Id == id
                         select _book).FirstOrDefault();
            
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Genre,Price,PublishDate")] Book book)
        {
            Trace.WriteLine($"id: {id}, book.Id: {book.Id}, book.Title: {book.Title}");
            await Task.Delay(100);
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //try
                //{
                //    _context.Update(book);
                //    await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!BookExists(book.Id))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        //private bool BookExists(int id)
        //{
        //    return _context.Book.Any(e => e.Id == id);
        //}
    }
}
