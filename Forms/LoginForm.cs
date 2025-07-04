using System;
using System.Drawing;
using System.Windows.Forms;
using ParkingManagementSystem.Services;

namespace ParkingManagementSystem.Forms
{
    public partial class LoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnSignUp;
        private Label lblTitle;
        private Panel mainPanel;

        public LoginForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(400, 500);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "SafeParking - Login";
            this.BackColor = Color.FromArgb(135, 206, 235); // Sky blue
            
            this.ResumeLayout(false);
        }

        private void SetupForm()
        {
            // Main panel
            mainPanel = new Panel
            {
                Size = new Size(320, 400),
                Location = new Point(40, 50),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            
            // Title
            lblTitle = new Label
            {
                Text = "SafeParking",
                Font = new Font("Poppins", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(135, 206, 235),
                Size = new Size(280, 50),
                Location = new Point(20, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Username textbox
            txtUsername = new TextBox
            {
                Font = new Font("Poppins", 12),
                Size = new Size(280, 30),
                Location = new Point(20, 120),
                PlaceholderText = "Username"
            };

            // Password textbox
            txtPassword = new TextBox
            {
                Font = new Font("Poppins", 12),
                Size = new Size(280, 30),
                Location = new Point(20, 170),
                UseSystemPasswordChar = true,
                PlaceholderText = "Password"
            };

            // Login button
            btnLogin = new Button
            {
                Text = "Login",
                Font = new Font("Poppins", 12, FontStyle.Bold),
                Size = new Size(280, 45),
                Location = new Point(20, 220),
                BackColor = Color.FromArgb(135, 206, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            // Sign up button
            btnSignUp = new Button
            {
                Text = "Sign Up",
                Font = new Font("Poppins", 10),
                Size = new Size(280, 35),
                Location = new Point(20, 280),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(135, 206, 235),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSignUp.FlatAppearance.BorderColor = Color.FromArgb(135, 206, 235);
            btnSignUp.Click += BtnSignUp_Click;

            // Add controls to panel
            mainPanel.Controls.AddRange(new Control[] { lblTitle, txtUsername, txtPassword, btnLogin, btnSignUp });
            
            // Add panel to form
            this.Controls.Add(mainPanel);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter both username and password.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (UserService.ValidateUser(txtUsername.Text.Trim(), txtPassword.Text))
                {
                    this.Hide();
                    MainForm mainForm = new MainForm();
                    mainForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSignUp_Click(object sender, EventArgs e)
        {
            SignUpForm signUpForm = new SignUpForm();
            signUpForm.ShowDialog();
        }
    }
}
