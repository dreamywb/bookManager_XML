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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label_allBookCount.Text = "";
            label_allUserCount.Text = "";
            label_allBorrowedBook.Text = "";
            label_allDelayedBook.Text = "";

            label_allBookCount.Text = DataManager.Books.Count.ToString();
            label_allUserCount.Text = DataManager.Users.Count.ToString();

            //대출중인 도서의 수
            //where안에 메소드가 매개변수로 들어감(람다)
            label_allBorrowedBook.Text = DataManager.Books.Where(x => x.isBorrowed).Count().ToString();
            //연체중인 도서의 수
            label_allDelayedBook.Text = DataManager.Books.Where(
                delegate (Book x) { return x.isBorrowed && x.BorrowedAt.AddDays(7) < DateTime.Now; }).Count().ToString();

            //데이터 그리드 뷰 설정
            if (DataManager.Users.Count >0)
            {
                dataGridView_UserManager.DataSource = DataManager.Users;
            }
            if (DataManager.Books.Count > 0)
            {
                dataGridView_BookManager.DataSource = DataManager.Books;
            }
            dataGridView_BookManager.CellClick += Book_GridView_CellClick;
        }

        private void Book_GridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Book book = dataGridView_BookManager.CurrentRow.DataBoundItem as Book;
            textBox_isbn.Text = book.Isbn;
            textBox_bookName.Text = book.Name;
        }

        private void 도서관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog(); //모달 -> 이 창을 꺼야 그 밑에 코드들이 실행

            //도서관리창에서 책을 수정
            DataManager.Load(); //저장한 값을 다시 호출해서 datamanager에 있는 List를 리셋
            dataGridView_BookManager.DataSource = null;
            if (DataManager.Books.Count > 0)
            {
                dataGridView_BookManager.DataSource = DataManager.Books;
            }
        }

        private void 사용자관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form3().ShowDialog();
            DataManager.Load();
            dataGridView_UserManager.DataSource = null;
            if(DataManager.Users.Count > 0)
                dataGridView_UserManager.DataSource = DataManager.Users;
        }

        private void timer_now_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel_now.Text = DateTime.Now.ToString("yyyy년 MM월 dd일 HH:mm:ss");
        }

        private void dataGridView_UserManager_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            User user = dataGridView_UserManager.CurrentRow.DataBoundItem as User;
            textBox_id.Text = user.Id.ToString();
        }

        private void button_Borrow_Click(object sender, EventArgs e)
        {
            if(textBox_isbn.Text.Trim() == "")
                MessageBox.Show("isbn을 입력");
            else if(textBox_id.Text.Trim() == "")
                MessageBox.Show("ID를 입력");
            else
            {
                try
                {
                    Book book = DataManager.Books.Single((x) => x.Isbn == textBox_isbn.Text);
                    if(book.isBorrowed)
                        MessageBox.Show("이미 빌린책");
                    else
                    {
                        User user = DataManager.Users.Single((x) => x.Id.ToString() == textBox_id.Text);
                        book.UserId = user.Id;
                        book.UserName = user.Name;
                        book.isBorrowed = true;
                        book.BorrowedAt = DateTime.Now;

                        dataGridView_BookManager.DataSource = null;
                        dataGridView_BookManager.DataSource = DataManager.Books;
                        DataManager.Save();
                        MessageBox.Show($"{book.Name}이/가 {user.Name}님께 대여됨");

                        label_allBookCount.Text = "";
                        label_allUserCount.Text = "";
                        label_allBorrowedBook.Text = "";
                        label_allDelayedBook.Text = "";
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("존재하지 않음");
                }
            }
        }

        private void button_Return_Click(object sender, EventArgs e)
        {
            if(textBox_isbn.Text.Trim() == "")
                MessageBox.Show("isbn 입력");
            else
            {
                try
                {
                    Book book = DataManager.Books.Single((x) => x.Isbn == textBox_isbn.Text);
                    if (book.isBorrowed)
                    {
                        DateTime oldDay = book.BorrowedAt;
                        book.UserId = 0;
                        book.UserName = "";
                        book.isBorrowed = false;
                        book.BorrowedAt = new DateTime();

                        dataGridView_BookManager.DataSource = null;
                        dataGridView_BookManager.DataSource = DataManager.Books;
                        DataManager.Save();

                        TimeSpan timeDiff = DateTime.Now - oldDay;
                        if (timeDiff.Days >7)
                        {
                            MessageBox.Show(book.Name+"은 연체되어 반납");
                        }
                        else
                            MessageBox.Show("대여 상태 아님");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("존재하지않는 도서");
                }
            }
        }
    }
}
