using System;
using System.Drawing;
using System.Windows.Forms;
using ParkingManagementSystem.UserControls;

namespace ParkingManagementSystem.Forms
{
    public partial class MainForm : Form
    {
        private Panel headerPanel;
        private Panel sidebarPanel;
        private Panel contentPanel;
        private Panel footerPanel;
        private Label lblLogo;
        private Label lblDateTime;
        private Button btnDashboard;
        private Button btnAddVehicle;
        private Button btnPark;
        private Button btnExit;
        private Timer timeTimer;

        // User Controls
        private DashboardControl dashboardControl;
        private AddVehicleControl addVehicleControl;
        private ParkControl parkControl;
        private ExitControl exitControl;

        public MainForm()
        {
            InitializeComponent();
            SetupForm();
            InitializeUserControls();
            ShowDashboard();
            StartTimer();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "SafeParking - Management System";
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.White;
            
            this.ResumeLayout(false);
        }

        private void SetupForm()
        {
            // Header Panel
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(135, 206, 235)
            };

            // Logo
            lblLogo = new Label
            {
                Text = "SafeParking",
                Font = new Font("Poppins", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                Size = new Size(300, 40),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Date Time
            lblDateTime = new Label
            {
                Font = new Font("Poppins", 12),
                ForeColor = Color.White,
                Size = new Size(300, 30),
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            lblDateTime.Location = new Point(headerPanel.Width - 320, 25);

            headerPanel.Controls.AddRange(new Control[] { lblLogo, lblDateTime });

            // Sidebar Panel
            sidebarPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = Color.FromArgb(240, 248, 255)
            };

            // Sidebar Buttons
            btnDashboard = CreateSidebarButton("Dashboard", 20);
            btnAddVehicle = CreateSidebarButton("Add Vehicle", 80);
            btnPark = CreateSidebarButton("Park", 140);
            btnExit = CreateSidebarButton("Exit", 200);

            btnDashboard.Click += (s, e) => ShowDashboard();
            btnAddVehicle.Click += (s, e) => ShowAddVehicle();
            btnPark.Click += (s, e) => ShowPark();
            btnExit.Click += (s, e) => ShowExit();

            sidebarPanel.Controls.AddRange(new Control[] { btnDashboard, btnAddVehicle, btnPark, btnExit });

            // Content Panel
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            // Footer Panel
            footerPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(135, 206, 235)
            };

            Label lblFooter = new Label
            {
                Text = "SafeParking Management System v1.0",
                Font = new Font("Poppins", 10),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            footerPanel.Controls.Add(lblFooter);

            // Add panels to form
            this.Controls.AddRange(new Control[] { contentPanel, sidebarPanel, footerPanel, headerPanel });
        }

        private Button CreateSidebarButton(string text, int y)
        {
            Button btn = new Button
            {
                Text = text,
                Font = new Font("Poppins", 12),
                Size = new Size(180, 45),
                Location = new Point(10, y),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(135, 206, 235),
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = Color.FromArgb(135, 206, 235);
            btn.FlatAppearance.BorderSize = 1;
            
            return btn;
        }

        private void InitializeUserControls()
        {
            dashboardControl = new DashboardControl { Dock = DockStyle.Fill };
            addVehicleControl = new AddVehicleControl { Dock = DockStyle.Fill };
            parkControl = new ParkControl { Dock = DockStyle.Fill };
            exitControl = new ExitControl { Dock = DockStyle.Fill };
        }

        private void ShowDashboard()
        {
            ShowUserControl(dashboardControl);
            SetActiveButton(btnDashboard);
        }

        private void ShowAddVehicle()
        {
            ShowUserControl(addVehicleControl);
            SetActiveButton(btnAddVehicle);
        }

        private void ShowPark()
        {
            ShowUserControl(parkControl);
            SetActiveButton(btnPark);
        }

        private void ShowExit()
        {
            ShowUserControl(exitControl);
            SetActiveButton(btnExit);
        }

        private void ShowUserControl(UserControl control)
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(control);
            
            // Refresh the control if it has a refresh method
            if (control is DashboardControl dashboard)
                dashboard.RefreshData();
            else if (control is ParkControl park)
                park.RefreshData();
        }

        private void SetActiveButton(Button activeButton)
        {
            // Reset all buttons
            foreach (Control control in sidebarPanel.Controls)
            {
                if (control is Button btn)
                {
                    btn.BackColor = Color.White;
                    btn.ForeColor = Color.FromArgb(135, 206, 235);
                }
            }

            // Set active button
            activeButton.BackColor = Color.FromArgb(135, 206, 235);
            activeButton.ForeColor = Color.White;
        }

        private void StartTimer()
        {
            timeTimer = new Timer
            {
                Interval = 1000 // Update every second
            };
            timeTimer.Tick += (s, e) => {
                lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy HH:mm:ss");
            };
            timeTimer.Start();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            timeTimer?.Stop();
            timeTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
