using System;
using System.Drawing;
using System.Windows.Forms;
using ParkingManagementSystem.Services;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.UserControls
{
    public partial class ExitControl : UserControl
    {
        private TextBox txtTicketID;
        private Button btnSearch;
        private Panel vehicleInfoPanel;
        private Button btnLeaveNow;
        private Panel exitTicketPanel;

        public ExitControl()
        {
            InitializeComponent();
            SetupControls();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Size = new Size(1000, 700);
            
            this.ResumeLayout(false);
        }

        private void SetupControls()
        {
            // Title
            Label lblTitle = new Label
            {
                Text = "Exit Vehicle",
                Font = new Font("Poppins", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(135, 206, 235),
                Location = new Point(20, 20),
                Size = new Size(200, 40)
            };

            // Search Panel
            Panel searchPanel = new Panel
            {
                Location = new Point(20, 80),
                Size = new Size(450, 100),
                BackColor = Color.FromArgb(240, 248, 255),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblTicketID = new Label
            {
                Text = "Enter Ticket ID:",
                Font = new Font("Poppins", 12),
                Location = new Point(20, 20),
                Size = new Size(120, 25)
            };

            txtTicketID = new TextBox
            {
                Font = new Font("Poppins", 12),
                Location = new Point(150, 20),
                Size = new Size(200, 30),
                PlaceholderText = "TK-YYYYMMDD-XXXX"
            };

            btnSearch = new Button
            {
                Text = "Search",
                Font = new Font("Poppins", 12),
                Size = new Size(100, 35),
                Location = new Point(150, 55),
                BackColor = Color.FromArgb(135, 206, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;

            searchPanel.Controls.AddRange(new Control[] { lblTicketID, txtTicketID, btnSearch });

            // Vehicle Info Panel
            vehicleInfoPanel = new Panel
            {
                Location = new Point(20, 200),
                Size = new Size(450, 300),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            // Leave Now Button
            btnLeaveNow = new Button
            {
                Text = "Leave Now",
                Font = new Font("Poppins", 14, FontStyle.Bold),
                Size = new Size(200, 50),
                Location = new Point(20, 520),
                BackColor = Color.FromArgb(255, 99, 99),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Visible = false
            };
            btnLeaveNow.FlatAppearance.BorderSize = 0;
            btnLeaveNow.Click += BtnLeaveNow_Click;

            // Exit Ticket Panel
            exitTicketPanel = new Panel
            {
                Location = new Point(500, 80),
                Size = new Size(450, 500),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            this.Controls.AddRange(new Control[] { 
                lblTitle, searchPanel, vehicleInfoPanel, btnLeaveNow, exitTicketPanel 
            });

            // Enter key event
            txtTicketID.KeyPress += (s, e) => {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    BtnSearch_Click(s, e);
                }
            };
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTicketID.Text))
            {
                MessageBox.Show("Please enter a ticket ID.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Vehicle vehicle = VehicleService.GetVehicleByTicketID(txtTicketID.Text.Trim());
                
                if (vehicle != null)
                {
                    ShowVehicleInfo(vehicle);
                }
                else
                {
                    MessageBox.Show("Vehicle not found or already exited.", "Not Found", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HideVehicleInfo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching vehicle: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowVehicleInfo(Vehicle vehicle)
        {
            vehicleInfoPanel.Controls.Clear();
            vehicleInfoPanel.Visible = true;
            btnLeaveNow.Visible = true;
            btnLeaveNow.Tag = vehicle;

            TimeSpan duration = DateTime.Now - vehicle.EntryTime;
            decimal totalAmount = VehicleService.CalculateCurrentAmount(vehicle);

            Label lblInfoTitle = new Label
            {
                Text = "Vehicle Details",
                Font = new Font("Poppins", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(135, 206, 235),
                Location = new Point(20, 20),
                Size = new Size(400, 30)
            };

            Label lblVehicleInfo = new Label
            {
                Text = $"Ticket ID: {vehicle.TicketID}\n" +
                       $"Owner: {vehicle.OwnerName}\n" +
                       $"Vehicle: {vehicle.VehicleNumber}\n" +
                       $"Type: {vehicle.VehicleType}\n" +
                       $"Parking Place: {vehicle.PlaceNumber}\n" +
                       $"Entry Time: {vehicle.EntryTime:yyyy-MM-dd HH:mm:ss}\n" +
                       $"Duration: {duration.Hours}h {duration.Minutes}m\n" +
                       $"Rate: {vehicle.AmountPerMinute} Ariary/minute\n" +
                       $"Total Amount: {totalAmount:C}",
                Font = new Font("Poppins", 12),
                Location = new Point(20, 60),
                Size = new Size(400, 220),
                TextAlign = ContentAlignment.TopLeft
            };

            vehicleInfoPanel.Controls.AddRange(new Control[] { lblInfoTitle, lblVehicleInfo });
        }

        private void HideVehicleInfo()
        {
            vehicleInfoPanel.Visible = false;
            btnLeaveNow.Visible = false;
            exitTicketPanel.Visible = false;
        }

        private void BtnLeaveNow_Click(object sender, EventArgs e)
        {
            if (btnLeaveNow.Tag is Vehicle vehicle)
            {
                DialogResult result = MessageBox.Show(
                    $"Process exit for vehicle {vehicle.VehicleNumber}?", 
                    "Confirm Exit", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (VehicleService.ExitVehicle(vehicle.TicketID))
                        {
                            ShowExitTicket(vehicle);
                            MessageBox.Show("Vehicle exit processed successfully!", "Success", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            // Clear form
                            txtTicketID.Clear();
                            HideVehicleInfo();
                        }
                        else
                        {
                            MessageBox.Show("Failed to process vehicle exit.", "Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error processing exit: {ex.Message}", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ShowExitTicket(Vehicle vehicle)
        {
            exitTicketPanel.Controls.Clear();
            exitTicketPanel.Visible = true;

            DateTime exitTime = DateTime.Now;
            TimeSpan duration = exitTime - vehicle.EntryTime;
            decimal totalAmount = (decimal)Math.Ceiling(duration.TotalMinutes) * vehicle.AmountPerMinute;

            Label lblExitTitle = new Label
            {
                Text = "EXIT TICKET",
                Font = new Font("Poppins", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(135, 206, 235),
                Location = new Point(20, 20),
                Size = new Size(400, 35),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblExitInfo = new Label
            {
                Text = $"Ticket ID: {vehicle.TicketID}\n" +
                       $"Owner: {vehicle.OwnerName}\n" +
                       $"Vehicle: {vehicle.VehicleNumber} ({vehicle.VehicleType})\n" +
                       $"Parking Place: {vehicle.PlaceNumber}\n\n" +
                       $"Entry Time: {vehicle.EntryTime:yyyy-MM-dd HH:mm:ss}\n" +
                       $"Exit Time: {exitTime:yyyy-MM-dd HH:mm:ss}\n" +
                       $"Duration: {duration.Hours}h {duration.Minutes}m\n" +
                       $"Rate: {vehicle.AmountPerMinute} Ariary/minute\n" +
                       $"Total Amount: {totalAmount:C}\n\n" +
                       $"Status: PAID",
                Font = new Font("Poppins", 12),
                Location = new Point(20, 70),
                Size = new Size(400, 300),
                TextAlign = ContentAlignment.TopLeft
            };

            Button btnPrintExit = new Button
            {
                Text = "Print Exit Ticket",
                Font = new Font("Poppins", 12),
                Size = new Size(180, 40),
                Location = new Point(135, 400),
                BackColor = Color.FromArgb(135, 206, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnPrintExit.FlatAppearance.BorderSize = 0;
            btnPrintExit.Click += (s, e) => {
                MessageBox.Show("Print functionality would be implemented here.", "Print", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            exitTicketPanel.Controls.AddRange(new Control[] { lblExitTitle, lblExitInfo, btnPrintExit });
        }
    }
}
