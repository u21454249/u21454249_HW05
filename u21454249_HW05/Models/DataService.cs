using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Configuration;
using System.Data.SqlClient;

namespace u21454249_HW05.Models
{
    public class DataService
    {
        private String ConnectionString;
        

        public DataService()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            

        }
        public List<Types> getTypes()
        {
            List<Types> types = new List<Types>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("select * from types", con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Types type = new Types
                            {
                                TypeID = Convert.ToInt32(reader["typeId"]),
                                Name = reader["name"].ToString(),

                            };
                            types.Add(type);
                        }
                    }
                }
                con.Close();
            }
            return types;
        }

        public List<Books> getBookrecords()
        {
            List<Books> books = new List<Books>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open(); 
                using (SqlCommand commad = new SqlCommand("Select books.bookId AS BookID, books.name AS Name, authors.surname AS authors, types.name AS Type, books.pagecount AS PageCount, books.point AS points FROM books INNER JOIN authors ON books.authorId= authors.authorId  INNER JOIN types ON types.typeId = books.typeId", con))
                {

                    using (SqlDataReader reader = commad.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Books book = new Books
                            {
                                BookID = Convert.ToInt32(reader["BookID"]),
                                Name = reader["Name"].ToString(),
                                Authors = reader["authors"].ToString(),
                                Pagecount = Convert.ToInt32(reader["PageCount"]),
                                Point = Convert.ToInt32(reader["points"]),
                                Type = reader["Type"].ToString(),
                            };
                            books.Add(book);
                        }
                    }
                }
                con.Close();
            }
            foreach (var book in books)
            {
                // Get all borrowed books
                List<Borrows> borrowedBook = GetBorrows(book.BookID);
                // check if the book is borrowed or available
                if (borrowedBook.Where(b => b.Brought == "").Count() == 1)
                {
                    book.Status = "Book Out";
                }
                else
                {
                    book.Status = "Available";
                }
            }
            return books;

        }
        public List<Authors> GetAllAuthors()
        {
            List<Authors> authors = new List<Authors>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("select * from Authors", con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Authors author = new Authors
                            {
                                AuthorID = Convert.ToInt32(reader["authorId"]),
                                Name = reader["name"].ToString(),
                                Surname = reader["surname"].ToString()
                            };
                            authors.Add(author);
                        }
                    }
                }
                con.Close();
            }
            return authors;

        }

        
        public List<Books> Search(string name, int type, int author)
        {
            string innerJoin = " ";
            
            if (name != null && type == 0 && author == 0)
            {
                innerJoin =
                " select books.bookId as ID, books.pagecount as PageCount, books.point as Points, books.name as Name, types.name as Type, authors.surname as Author  from Books " +
                " inner join authors on books.authorId = authors.authorId " +
                " inner join types on books.typeId = types.typeId " +
                " where books.name LIKE '%" + name + "%'";
            }
            
            if (type > 0 && author == 0)
            {
                innerJoin =
                " select books.bookId as ID, books.pagecount as PageCount, books.point as Points, books.name as Name, types.name as Type, authors.surname as Author  from Books " +
                " inner join authors on books.authorId = authors.authorId " +
                " inner join types on books.typeId = types.typeId " +
                " where books.name LIKE '%" + name + "%'";
            }
            
            if (type == 0 && author > 0)
            {
                innerJoin =
                 " select books.bookId as ID, books.pagecount as PageCount, books.point as Points, books.name as Name, types.name as Type, authors.surname as Author  from Books " +
                " inner join authors on books.authorId = authors.authorId " +
                " inner join types on books.typeId = types.typeId " +
                " where  books.authorId = " + author;
            }
            
            if (type > 0 && author > 0)
            {
                innerJoin =
                " select books.bookId as ID, books.pagecount as PageCount, books.point as Points, books.name as Name, types.name as Type, authors.surname as Author  from Books " +
                " inner join authors on books.authorId = authors.authorId " +
                " inner join types on books.typeId = types.typeId " +
                " where books.typeId = " + type + " AND books.authorId = " + author;
            }

           
            if (type > 0 && name != null)
            {
                innerJoin =
                " select books.bookId as ID, books.pagecount as PageCount, books.point as Points, books.name as Name, types.name as Type, authors.surname as Author  from Books " +
                " inner join authors on books.authorId = authors.authorId " +
                " inner join types on books.typeId = types.typeId " +
                " where books.typeId = " + type + " AND books.name LIKE '%" + name + "%'";
            }

            
            if (type > 0 && author > 0 && name != null)
            {
                innerJoin =
                " select books.bookId as ID, books.pagecount as PageCount, books.point as Points, books.name as Name, types.name as Type, authors.surname as Author  from Books " +
                " inner join authors on books.authorId = authors.authorId " +
                " inner join types on books.typeId = types.typeId " +
                " where books.typeId = " + type + " AND books.name LIKE '%" + name + "%'" + " AND  books.authorId = " + author;
            }
            List<Books> books = new List<Books>();
            using (SqlConnection con = new SqlConnection(ConnectionString))

            {
                con.Open();


                using (SqlCommand cmd = new SqlCommand(innerJoin, con))

                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Books book = new Books
                            {
                                BookID = Convert.ToInt32(reader["ID"]),
                                Name = reader["Name"].ToString(),
                                Authors = reader["Author"].ToString(),
                                Pagecount = Convert.ToInt32(reader["PageCount"]),
                                Point = Convert.ToInt32(reader["Points"]),
                                Type = reader["Type"].ToString()
                            };
                            books.Add(book);
                        }
                    }
                }
                con.Close();
            }

            return books;
        }
        public List<Borrows> GetBorrows(int id = 0)
        {
           
            List<Borrows> borrowed = new List<Borrows>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                
                string innerJoin =
                    " Select CONCAT( students.name,' ',students.surname) as Student, takenDate, broughtDate, borrows.bookId ,  borrows.borrowId from students " +
                    " inner join borrows on students.studentId = borrows.studentId inner join books on books.bookId = borrows.bookId where borrows.bookId = " + id;
                if (id == 0)
                {
                    innerJoin =
                    " Select CONCAT( students.name,' ',students.surname) as Student, takenDate, broughtDate, borrows.bookId ,  borrows.borrowId from students " +
                    " inner join borrows on students.studentId = borrows.studentId inner join books on books.bookId = borrows.bookId ";
                }

                using (SqlCommand cmd = new SqlCommand(innerJoin, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Borrows book = new Borrows
                            {
                                BookID = Convert.ToInt32(reader["bookId"]),
                                Studentname = reader["student"].ToString(),
                                BorrowID = Convert.ToInt32(reader["borrowId"]),
                                Brought = reader["broughtDate"].ToString(),
                                Taken = reader["takenDate"].ToString(),
                                
                            };
                            borrowed.Add(book);
                        }
                    }
                }
                con.Close();
            }


            return borrowed;
        }
        public List<Students> GetStudents()
        {
            List<Students> students = new List<Students>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("select * from students", con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Students student = new Students
                            {
                                StudentID = Convert.ToInt32(reader["studentId"]),
                                Name = reader["name"].ToString(),
                                Surname = reader["surname"].ToString(),
                                Class = reader["class"].ToString(),
                                Point = Convert.ToInt32(reader["point"])

                            };
                            students.Add(student);
                        }
                    }
                }
                con.Close();
            }
            return students;

        }

        public List<Grade> GetClases()
        {
            List<Grade> grades = new List<Grade>();
            foreach (Students student in GetStudents())
            {
                Grade grade = new Grade
                {
                    Name = student.Class
                };
                if (grades.Where(n => n.Name == student.Class).Count() == 0)
                {
                    grades.Add(grade);
                }
            }
            return grades;
        }
        public List<Students> SearchStudent(string name, string _class)
        {
            List<Students> students = new List<Students>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                string query = " ";
                if (name != null && name != "none")
                {
                    query = "Select * from students " +
                    "Where class Like '%" + name + "%'";
                }
                if (_class != "none" || _class != "" || _class != null)
                {
                    query = "Select * from students " +
                    "Where class Like '%" + _class + "%'";
                }

                if (name != null && _class != null && name != "" && name != "none" && _class != "" && _class != "none")
                {
                    query = "Select * from students" + " Where class Like '%" + _class + "%' AND name Like '%" + name + "%'";
                }
                if (_class == "none")
                {
                    query = "Select * from students "
                    + "Where name Like '%" + name + "%'";
                }
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Students student = new Students
                            {
                                StudentID = Convert.ToInt32(reader["studentId"]),
                                Name = reader["name"].ToString(),
                                Surname = reader["surname"].ToString(),
                                Class = reader["class"].ToString(),
                                Point = Convert.ToInt32(reader["point"])

                            };
                            students.Add(student);
                        }

                    }
                }
                con.Close();
            }

            return students;
        }
        public void BorrowBook(int bookId, int studentId)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string query = "insert into borrows( studentId, bookId, takenDate) values(@studentId,@bookId,@takenDate) ";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.Add(new SqlParameter("@studentId", studentId));
                    cmd.Parameters.Add(new SqlParameter("@bookId", bookId));
                    cmd.Parameters.Add(new SqlParameter("@takenDate", DateTime.Now));
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }

            GetStudents().Where(s => s.StudentID == studentId).FirstOrDefault().book = true;

        }
        public void ReturnBook(int bookId, int studentId)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string query = "update borrows set broughtDate = @broughtDate where borrows.studentId = @studentId  AND borrows.bookId = @bookId and broughtDate IS NULL";
                ;
                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.Add(new SqlParameter("@studentId", studentId));
                    cmd.Parameters.Add(new SqlParameter("@bookId", bookId));
                    cmd.Parameters.Add(new SqlParameter("@broughtDate", DateTime.Now));
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }

        }


    }

}



    