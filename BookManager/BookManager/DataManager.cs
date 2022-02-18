using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookManager
{
    public class DataManager
    {
        public static List<Book> Books = new List<Book>();
        public static List<User> Users = new List<User>();

        //DataManager에 접근하는 순간 바로 호출됨 (인스턴스 전)
        //프로그램 켜자마자 xml파일 불러오기
        static DataManager()
        {
            Load();
        }

        public static void Load()
        {
            try
            {
                string booksOutput = File.ReadAllText(@"./Books.xml");
                XElement booksXElement = XElement.Parse(booksOutput);

                //linq
                Books = (from item in booksXElement.Descendants("book") select new Book()
                         {
                             Isbn = item.Element("isbn").Value,
                             Name = item.Element("name").Value,
                             Publisher = item.Element("publisher").Value,
                             Page = int.Parse(item.Element("page").Value),
                             BorrowedAt = DateTime.Parse(item.Element("borrowedAt").Value),
                             isBorrowed = item.Element("isBorrowed").Value != "0" ? true : false,
                             UserId = int.Parse(item.Element("userId").Value),
                             UserName = item.Element("userName").Value
                         }).ToList<Book>();

                //foreach
                string usersOutput = File.ReadAllText(@"./Users.xml");
                XElement usersXElement = XElement.Parse(usersOutput);
                Users.Clear();
                foreach (var item in usersXElement.Descendants("user"))
                {
                    User temp = new User();
                    temp.Name = item.Element("name").Value;
                    temp.Id = int.Parse(item.Element("id").Value);
                    Users.Add(temp);
                }
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("파일 누락!");
                Save();
                Load();
            }
        }

        public static void Save() //기존에 있던 값들을 덮어쓰는 방식
        {
            string booksOutput = "";
            booksOutput += "<books>\n";
            foreach (var item in Books)
            {
                booksOutput += "<book>\n";

                booksOutput += $"   <isbn>{item.Isbn}</isbn>\n";
                booksOutput += $"   <name>{item.Name}</name>\n";
                booksOutput += $"   <publisher>{item.Publisher}</publisher>\n";
                booksOutput += $"   <page>{item.Page}</page>\n";
                booksOutput += $"   <borrowedAt>{item.BorrowedAt}</borrowedAt>\n";
                booksOutput += $"   <isBorrowed>" + (item.isBorrowed ? 1 : 0) + "</isBorrowed>\n";
                booksOutput += $"   <userId>{item.UserId}</userId>\n";
                booksOutput += $"   <userName>{item.UserName}</userName>\n";

                booksOutput += "</book>\n";
            }
            booksOutput += "</books>";

            File.WriteAllText(@"./Books.xml", booksOutput);

            string usersOutput = "";
            usersOutput += "<users>\n";
            foreach (var item in Users)
            {
                usersOutput += "<user>\n";
                usersOutput += $"   <id>{item.Id}</id>\n";
                usersOutput += $"   <name>{item.Name}</name>\n";
                usersOutput += "</user>\n";
            }
            usersOutput += "</users>";

            File.WriteAllText(@"./Users.xml", usersOutput);
        }
    }
}
