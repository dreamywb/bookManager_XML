using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookManager
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            if(DataManager.Books.Count > 0)
                dataGridView_book.DataSource = DataManager.Books;
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            bool existBook = false;
            foreach (var item in DataManager.Books)
            {
                if(item.Isbn == textBox_isbn.Text)
                {
                    existBook = true;
                    break;
                }
            }
            if(existBook)
                MessageBox.Show("책이 이미 있습니다.");
            else
            {
                Book book = new Book();
                book.Isbn = textBox_isbn.Text;
                book.Name = textBox_bookName.Text;
                book.Publisher = textBox_publisher.Text;
                book.Page = int.Parse(textBox_page.Text);
                DataManager.Books.Add(book);

                dataGridView_book.DataSource = null;
                dataGridView_book.DataSource = DataManager.Books;
                DataManager.Save();

            }
        }

        private void button_modify_Click(object sender, EventArgs e)
        {
            Book book = null; //book은 아무것도 안 가르킴
            for(int i = 0; i<DataManager.Books.Count; i++)
            {
                if (DataManager.Books[i].Isbn == textBox_isbn.Text)
                {
                    book = DataManager.Books[i]; //Books의 i번째 가르킴(얕은복사 == 참조복사)
                    book.Name= textBox_bookName.Text; //book의 값이 변경되면 Books의 i번째값이 변경
                    book.Publisher = textBox_publisher.Text;
                    book.Page = int.Parse(textBox_page.Text);

                    dataGridView_book.DataSource = null;
                    dataGridView_book.DataSource = DataManager.Books;
                    DataManager.Save();
                }
            }
            if(book == null)
                MessageBox.Show("없는 도서");
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            bool existBook = false;
            for(int i = 0;i<DataManager.Books.Count;i++)
            {
                if (DataManager.Books[i].Isbn == textBox_isbn.Text)
                {
                    DataManager.Books.RemoveAt(i);
                    existBook = true;
                }
            }
            if (existBook == false)
            {
                MessageBox.Show("책이 없음");
            }
            else
            {
                dataGridView_book.DataSource=null;
                if (DataManager.Books.Count > 0)
                {
                    dataGridView_book.DataSource = DataManager.Books;
                }
                DataManager.Save();
            }
        }

        private void dataGridView_book_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Book book = dataGridView_book.CurrentRow.DataBoundItem as Book;
            textBox_isbn.Text = book.Isbn;
            textBox_bookName.Text = book.Name;
            textBox_publisher.Text = book.Publisher;
            textBox_page.Text = book.Page.ToString();
        }
    }
}
