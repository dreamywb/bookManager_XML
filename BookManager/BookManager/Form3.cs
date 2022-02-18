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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            if(DataManager.Users.Count > 0)
                dataGridView_users.DataSource = DataManager.Users;

            dataGridView_users.CellClick += user_cellclick;

            //sender 와 e 가 정해져있지 않음
            button_add.Click += (sender, e) =>
            {
                if(DataManager.Users.Exists((x)=>x.Id == int.Parse(textBox_id.Text)))
                    MessageBox.Show("ID가 이미있음");
                else //새 아이디일 경우
                {
                    User user = new User() { Id=int.Parse(textBox_id.Text),Name=textBox_name.Text};
                    DataManager.Users.Add(user);

                    dataGridView_users.DataSource = null;
                    dataGridView_users.DataSource = DataManager.Users;
                    DataManager.Save();
                }
            };
            button_modify.Click += delegate (object sender, EventArgs e)
            {
                //single
                try
                {
                    User user = DataManager.Users.Single((x)=> x.Id== int.Parse(textBox_id.Text));
                    user.Name =textBox_name.Text;

                    try
                    {
                        Book book = DataManager.Books.Single((x)=> x.UserId==int.Parse(textBox_id.Text));
                        book.UserName =textBox_name.Text;
                    }
                    catch (Exception)
                    {

                    }
                }
                catch (Exception) //single로 검색했는데 없는 경우
                {
                    MessageBox.Show(textBox_id.Text+"는 없는 아이디");
                }
                dataGridView_users.DataSource = null;
                dataGridView_users.DataSource = DataManager.Users;
                DataManager.Save();
            };
            button_delete.Click += btn_delete;
        }

        private void btn_delete(object sender, EventArgs e)
        {
            try
            {
                //single로 내가 지우고자 하는 유저 찾기
                //주소값 이용하여 지우기
                User user = DataManager.Users.Single((x)=> x.Id==int.Parse((textBox_id.Text)));
                DataManager.Users.Remove(user);

                dataGridView_users.DataSource=null;
                if(DataManager.Users.Count>0)
                    dataGridView_users.DataSource=DataManager.Users;
                DataManager.Save();
            }
            catch (Exception)
            {
                MessageBox.Show("해당 아이디 사용자는 없음");
            }
        }

        private void user_cellclick(object sender, DataGridViewCellEventArgs e)
        {
            User user = dataGridView_users.CurrentRow.DataBoundItem as User;
            textBox_id.Text = user.Id.ToString();
            textBox_name.Text = user.Name;
        }
    }
}
