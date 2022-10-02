using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using u21454249_HW05.Models;

namespace u21454249_HW05.Controllers
{
    public class HomeController : Controller
    {
        public static int typeFilter = 0;
        public static String authorFilter = "Reset";
        public static String BookFilter = "Reset";

        private DataService dataService = new DataService();
        public ActionResult Index()
        {

            BookRecordVM booksrecord = new BookRecordVM();
            booksrecord.Book = dataService.getBookrecords();
            booksrecord.Authors = dataService.GetAllAuthors();
            booksrecord.Genre = dataService.getTypes();
            return View(booksrecord);

            //return View(BookRecords);
        }
        public ActionResult Search(int type = 0, int author = 0, string name = null)
        {
            BookRecordVM booksrecord = new BookRecordVM();
            booksrecord.Book = dataService.Search(name, type, author);
            booksrecord.Authors = dataService.GetAllAuthors();
            booksrecord.Genre = dataService.getTypes();
            return View("Index", booksrecord);
        }

        public ActionResult BookDetails(int id)
        {
            // Create a book view model
            DetailsVM Details = new DetailsVM();
            Details.Borrowed = dataService.GetBorrows(id);
            Details.Book = dataService.getBookrecords().Where(b => b.BookID == id).FirstOrDefault();
            return View(Details);
        }
        public ActionResult Students(int bookId)
        {
            // List of student 
            SViewModel studentVM = new SViewModel();
            List<Students> students = dataService.GetStudents();
            List<Borrows> books = dataService.GetBorrows(bookId);
            foreach (var student in students)
            {
                for (int i = 0; i < books.Count(); i++)
                {
                    string name = student.Name + " " + student.Surname;
                    if (books[i].Studentname == name && (books[i].Brought == "" || books[i].Brought == null))
                    {
                        student.book = true;
                    }
                    else
                    {
                        student.book = false;

                    }
                }
            }
            studentVM.Students = students;
            studentVM.Book = dataService.getBookrecords().Where(b => b.BookID == bookId).FirstOrDefault();
            studentVM.Class = dataService.GetClases();
            return View(studentVM);
        }

        public ActionResult StudentSearch(int bookId, string name = "none", string _class = "none")
        {
            //
            SViewModel student = new SViewModel
            {
                Students = dataService.SearchStudent(name, _class),
                Book = dataService.getBookrecords().Where(b => b.BookID == bookId).FirstOrDefault(),
                Class = dataService.GetClases()

            };
            return View("Students", student);
        }
        public ActionResult ReturnBook(int bookId, int studentId)
        {
            dataService.ReturnBook(bookId, studentId);

            DetailsVM bookDetails = new DetailsVM();
            bookDetails.Borrowed = dataService.GetBorrows(bookId);
            bookDetails.Book = dataService.getBookrecords().Where(b => b.BookID == bookId).FirstOrDefault();
            return View("BookDetails", bookDetails);

        }


        public ActionResult BorrowBook(int bookId, int studentId)
        {
            dataService.BorrowBook(bookId, studentId);
            DetailsVM bookDetails = new DetailsVM();
            bookDetails.Borrowed = dataService.GetBorrows(bookId);
            bookDetails.Book = dataService.getBookrecords().Where(b => b.BookID == bookId).FirstOrDefault();
            return View("BookDetails", bookDetails);
        }

    }
}