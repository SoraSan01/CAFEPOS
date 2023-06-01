using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace cafeMS
{
    public partial class LoginForm : Form
    {
    	private Point mouseOffset;
    	private bool isMouseDown = false;
    	
        private readonly MySqlConnection cn;
        private readonly MySqlCommand cm;
        
        public static string adminname;

        public LoginForm()
        {
            InitializeComponent();

            cn = new MySqlConnection("server=localhost; user id=root;password=; database=cafems;");
            cm = new MySqlCommand("", cn);
        }


        private void submitebtn()
        {
        	if (string.IsNullOrEmpty(usernameTB.Text) || string.IsNullOrEmpty(passwordTb.Text))
		    {
		        MessageBox.Show("Please enter a username and password.");
		        return;
		    }
		
		    try
		    {
		        using (MySqlConnection cn = new MySqlConnection("server=localhost; user id=root; password=; database=cafems;"))
		        {
		            cn.Open();
		
		            using (MySqlCommand cm = new MySqlCommand())
		            {
		                cm.Connection = cn;
		                cm.CommandText = "SELECT Name FROM user WHERE Username=@uname AND Password=@upass";
		                cm.Parameters.AddWithValue("@uname", usernameTB.Text);
		                cm.Parameters.AddWithValue("@upass", passwordTb.Text);
		
		                using (MySqlDataReader dr = cm.ExecuteReader())
		                {
		                    if (dr.HasRows)
		                    {
		                        while (dr.Read()) // Iterate over the result rows
		                        {
		                            string namee = dr["Name"].ToString();
		                            MessageBox.Show("Login Successful.");
		                            this.Hide();
		
		                            MainForm mainForm = new MainForm(namee);
		                            mainForm.Show();
		                        }
		                    }
		                    else
		                    {
		                        dr.Close();
		
		                        cm.CommandText = "SELECT * FROM admin WHERE Username=@aname AND Password=@apass";
		                        cm.Parameters.AddWithValue("@aname", usernameTB.Text);
		                        cm.Parameters.AddWithValue("@apass", passwordTb.Text);
		                        using (MySqlDataReader dr2 = cm.ExecuteReader())
		                        {
		                            if (dr2.HasRows)
		                            {
		                                while (dr2.Read())
		                                {
		                                    adminname = dr2["Name"].ToString();
		                                    MessageBox.Show("Login Successful.");
		                                    this.Hide();
		
		                                    AdminSection adminSection = new AdminSection();
		                                    adminSection.Show();
		                                }
		                            }
		                            else
		                            {
		                                MessageBox.Show("Incorrect username or password.");
		                            }
		                        }
		                    }
		                }
		            }
		        }
		    }
		    catch (MySqlException ex)
		    {
		        MessageBox.Show("An error occurred while connecting to the database: " + ex.Message);
		    }
        }

        private void Button1Click(object sender, EventArgs e)
		{
        	submitebtn();
		}


        private void Button2Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Close Application?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
			    Application.Exit();
			}
        }

        private void Label6Click(object sender, EventArgs e)
        {
            RegisterUser rUser = new RegisterUser();
            rUser.Show();
            this.Hide();
        }

        private void Panel2Paint(object sender, PaintEventArgs e)
        {

        }
		
        private void Label3Click(object sender, EventArgs e)
        {
            Fpass fform = new Fpass();
            fform.Show();
            this.Hide();
        }

        private void Label4Click(object sender, EventArgs e)
        {
	
        }
		void LoginFormMouseMove(object sender, MouseEventArgs e)
		{
			if (isMouseDown)
	        {
	            Point mousePos = Control.MousePosition;
	            mousePos.Offset(mouseOffset.X, mouseOffset.Y);
	            Location = mousePos;
	        }
		}
		void LoginFormMouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
	        {
	            isMouseDown = false;
	        }
		}
		void LoginFormMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
	        {
	            mouseOffset = new Point(-e.X, -e.Y);
	            isMouseDown = true;
	        }
		}
		void Button1KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
		    {
		        // Prevent the default button behavior
		        e.SuppressKeyPress = true;
		
		        // Trigger the form submission
		        submitebtn();
			}
		}
		void LoginFormLoad(object sender, EventArgs e)
		{
			this.KeyDown += Button1KeyDown;
		}
    }
}
