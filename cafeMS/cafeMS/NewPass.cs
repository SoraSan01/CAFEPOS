/*
 * Created by SharpDevelop.
 * User: Ralph
 * Date: 5/13/2023
 * Time: 12:32 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;

namespace cafeMS
{
	/// <summary>
	/// Description of NewPass.
	/// </summary>
	public partial class NewPass : Form
	{
		
		private Point mouseOffset;
    	private bool isMouseDown = false;
    	
		MySqlConnection cn;
		MySqlCommand cm;
		public NewPass()
		{
			cn = new MySqlConnection();
			cn.ConnectionString = "server=localhost; user id=root;password=; database=cafems;";
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

		void Button2Click(object sender, EventArgs e)
{
    cn.Open();
    cm = new MySqlCommand();
    cm.Connection = cn;

    // Retrieve the previous password from the user table
    cm.CommandText = "SELECT Password FROM user WHERE Email = @userEmail";
    cm.Parameters.AddWithValue("@userEmail", Fpass.to);
    object userPasswordObj = cm.ExecuteScalar();
    string previousUserPassword = userPasswordObj != null ? userPasswordObj.ToString() : null;

    // Retrieve the previous password from the admin table
    cm.CommandText = "SELECT Password FROM admin WHERE Email = @adminEmail";
    cm.Parameters.AddWithValue("@adminEmail", Fpass.to);
    object adminPasswordObj = cm.ExecuteScalar();
    string previousAdminPassword = adminPasswordObj != null ? adminPasswordObj.ToString() : null;

    // Compare the previous password with the new password
    if (previousUserPassword != textBox2.Text && previousAdminPassword != textBox2.Text)
    {
        // New password is different from both the previous user password and previous admin password
        // Update the password in the corresponding table

        if (previousUserPassword != null)
        {
            // Update the user password
            cm.Parameters.Clear();
            cm.CommandText = "UPDATE user SET Password = @pass WHERE Email = @userEmail";
            cm.Parameters.AddWithValue("@pass", textBox2.Text);
            cm.Parameters.AddWithValue("@userEmail", Fpass.to);
            cm.ExecuteNonQuery();
        }
        else if (previousAdminPassword != null)
        {
            // Update the admin password
            cm.Parameters.Clear();
            cm.CommandText = "UPDATE admin SET Password = @pass WHERE Email = @adminEmail";
            cm.Parameters.AddWithValue("@pass", textBox2.Text);
            cm.Parameters.AddWithValue("@adminEmail", Fpass.to);
            cm.ExecuteNonQuery();
        }

        MessageBox.Show("Password Changed");
        cn.Close();

        LoginForm lform = new LoginForm();
        lform.Show();
        this.Hide();
    }
    else
    {
        // New password is the same as either the previous user password or previous admin password
        MessageBox.Show("New password must be different from the previous password.");
    }
}


		void NewPassLoad(object sender, EventArgs e)
		{
	
		}
		void NewPassMouseMove(object sender, MouseEventArgs e)
		{
			if (isMouseDown)
	        {
	            Point mousePos = Control.MousePosition;
	            mousePos.Offset(mouseOffset.X, mouseOffset.Y);
	            Location = mousePos;
	        }
		}
		void NewPassMouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
	        {
	            isMouseDown = false;
	        }
		}
		void NewPassMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
	        {
	            mouseOffset = new Point(-e.X, -e.Y);
	            isMouseDown = true;
	        }
		}
		void Button1Click(object sender, EventArgs e)
		{
			LoginForm login = new LoginForm();
			login.Show();
			this.Hide();
		}
	}
}
