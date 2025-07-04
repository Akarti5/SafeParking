using System;
using System.Drawing;
using System.Windows.Forms;
using ParkingManagementSystem.Services;

namespace ParkingManagementSystem.Forms
{
    public partial class SignUpForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private TextBox txtEmail;
        private Button btnSignUp;
        private Button btnCancel;
        private Label lblTitle;
        private Panel mainPanel;

        public SignUpForm()
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
            this.ClientSize = new Size(400, 600);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "SafeParking - Sign Up";
            this.BackColor = Color.FromArgb(135, 206, 235);
            
            this.ResumeLayout(false);
        }

        private void SetupForm()
        {
            // Main panel
            mainPanel = new Panel
            {
                Size = new Size(320, 500),
                Location = new Point(40, 50),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            
            // Title
            lblTitle = new Label
            {
                Text = "Create Account",
                Font = new Font("Poppins", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(135, 206, 235),
                Size = new Size(280, 40),
                Location = new Point(20, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Username textbox
            txtUsername = new TextBox
            {
                Font = new Font("Poppins", 12),
                Size = new Size(280, 30),
                Location = new Point(20, 100),
                PlaceholderText = "Username"
            };

            // Email textbox
            txtEmail = new TextBox
            {
                Font = new Font("Poppins", 12),
                Size = new Size(280, 30),
                Location = new Point(20, 150),
                PlaceholderText = "Email"
            };

            // Password textbox
            txtPassword = new TextBox
            {
                Font = new Font("Poppins", 12),
                Size = new Size(280, 30),
                Location = new Point(20, 200),
                UseSystemPasswordChar = true,
                PlaceholderText = "Password"
            };

            // Confirm password textbox
            txtConfirmPassword = new TextBox
            {
                Font = new Font("Poppins", 12),
                Size = new Size(280, 30),
                Location = new Point(20, 250),
                UseSystemPasswordChar = true,
                PlaceholderText = "Confirm Password"
            };

            // Sign up button
            btnSignUp = new Button
            {
                Text = "Sign Up",
                Font = new Font("Poppins", 12, FontStyle.Bold),
                Size = new Size(280, 45),
                Location = new Point(20, 310),
                BackColor = Color.FromArgb(135, 206, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSignUp.FlatAppearance.BorderSize = 0;
            btnSignUp.Click += BtnSignUp_Click;

            // Cancel button
            btnCancel = new Button
            {
                Text = "Cancel",
                Font = new Font("Poppins", 10),
                Size = new Size(280, 35),
                Location = new Point(20, 370),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(135, 206, 235),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(135, 206, 235);
            btnCancel.Click += BtnCancel_Click;

            // Add controls to panel
            mainPanel.Controls.AddRange(new Control[] { 
                lblTitle, txtUsername, txtEmail, txtPassword, txtConfirmPassword, btnSignUp, btnCancel 
            });
            
            // Add panel to form
            this.Controls.Add(mainPanel);
        }

        private void BtnSignUp_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || 
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (UserService.UserExists(txtUsername.Text.Trim()))
                {
                    MessageBox.Show("Username already exists.", "Registration Failed", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (UserService.RegisterUser(txtUsername.Text.Trim(), txtPassword.Text, txtEmail.Text.Trim()))
                {
                    MessageBox.Show("Account created successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to create account.", "Registration Failed", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
