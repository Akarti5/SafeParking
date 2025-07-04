using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using ParkingManagementSystem.Services;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.UserControls
{
    public partial class AddVehicleControl : UserControl
    {
        private TextBox txtOwnerName;
        private TextBox txtVehicleNumber;
        private ComboBox cmbVehicleType;
        private ComboBox cmbParkingPlace;
        private Button btnAdd;
        private Panel ticketPanel;
        private Label lblSelectedPlace;

        public AddVehicleControl()
        {
            InitializeComponent();
            SetupControls();
            LoadParkingPlaces();
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
                Text = "Add Vehicle",
                Font = new Font("Poppins", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(135, 206, 235),
                Location = new Point(20, 20),
                Size = new Size(200, 40)
            };

            // Form Panel
            Panel formPanel = new Panel
            {
                Location = new Point(20, 80),
                Size = new Size(450, 400),
                BackColor = Color.FromArgb(240, 248, 255),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Owner Name
            Label lblOwnerName = new Label
            {
                Text = "Owner Name:",
                Font = new Font("Poppins", 12),
                Location = new Point(20, 30),
                Size = new Size(120, 25)
            };

            txtOwnerName = new TextBox
            {
                Font = new Font("Poppins", 12),
                Location = new Point(150, 30),
                Size = new Size(250, 30)
            };

            // Vehicle Number
            Label lblVehicleNumber = new Label
            {
                Text = "Vehicle Number:",
                Font = new Font("Poppins", 12),
                Location = new Point(20, 80),
                Size = new Size(120, 25)
            };

            txtVehicleNumber = new TextBox
            {
                Font = new Font("Poppins", 12),
                Location = new Point(150, 80),
                Size = new Size(250, 30)
            };

            // Vehicle Type
            Label lblVehicleType = new Label
            {
                Text = "Vehicle Type:",
                Font = new Font("Poppins", 12),
                Location = new Point(20, 130),
                Size = new Size(120, 25)
            };

            cmbVehicleType = new ComboBox
            {
                Font = new Font("Poppins", 12),
                Location = new Point(150, 130),
                Size = new Size(250, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbVehicleType.Items.AddRange(new string[] { "Motor", "Vehicle" });

            // Parking Place
            Label lblParkingPlace = new Label
            {
                Text = "Parking Place:",
                Font = new Font("Poppins", 12),
                Location = new Point(20, 180),
                Size = new Size(120, 25)
            };

            cmbParkingPlace = new ComboBox
            {
                Font = new Font("Poppins", 12),
                Location = new Point(150, 180),
                Size = new Size(250, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Selected Place Display
            lblSelectedPlace = new Label
            {
                Text = "No place selected",
                Font = new Font("Poppins", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(135, 206, 235),
                Location = new Point(150, 220),
                Size = new Size(250, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Add Button
            btnAdd = new Button
            {
                Text = "Add Vehicle",
                Font = new Font("Poppins", 14, FontStyle.Bold),
                Size = new Size(200, 50),
                Location = new Point(125, 270),
                BackColor = Color.FromArgb(135, 206, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;

            // Refresh Button
            Button btnRefresh = new Button
            {
                Text = "Refresh Places",
                Font = new Font("Poppins", 10),
                Size = new Size(120, 30),
                Location = new Point(280, 330),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(135, 206, 235),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(135, 206, 235);
            btnRefresh.Click += (s, e) => LoadParkingPlaces();

            formPanel.Controls.AddRange(new Control[] {
                lblOwnerName, txtOwnerName, lblVehicleNumber, txtVehicleNumber,
                lblVehicleType, cmbVehicleType, lblParkingPlace, cmbParkingPlace,
                lblSelectedPlace, btnAdd, btnRefresh
            });

            // Ticket Panel
            ticketPanel = new Panel
            {
                Location = new Point(500, 80),
                Size = new Size(450, 400),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            this.Controls.AddRange(new Control[] { lblTitle, formPanel, ticketPanel });

            // Event handlers
            cmbParkingPlace.SelectedIndexChanged += CmbParkingPlace_SelectedIndexChanged;
        }

        private void LoadParkingPlaces()
        {
            try
            {
                cmbParkingPlace.Items.Clear();
                List<ParkingPlace> availablePlaces = ParkingPlaceService.GetAvailableParkingPlaces();
                
                foreach (var place in availablePlaces)
                {
                    cmbParkingPlace.Items.Add(new ComboBoxItem { Text = place.PlaceNumber, Value = place.PlaceID });
                }

                if (cmbParkingPlace.Items.Count == 0)
                {
                    MessageBox.Show("No parking places available!", "Warning", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading parking places: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbParkingPlace_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbParkingPlace.SelectedItem is ComboBoxItem item)
            {
                lblSelectedPlace.Text = $"Selected: {item.Text}";
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtOwnerName.Text) ||
                string.IsNullOrWhiteSpace(txtVehicleNumber.Text) ||
                cmbVehicleType.SelectedItem == null ||
                cmbParkingPlace.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ComboBoxItem selectedPlace = (ComboBoxItem)cmbParkingPlace.SelectedItem;
                
                Vehicle vehicle = new Vehicle
                {
                    TicketID = VehicleService.GenerateTicketID(),
                    OwnerName = txtOwnerName.Text.Trim(),
                    VehicleNumber = txtVehicleNumber.Text.Trim(),
                    VehicleType = cmbVehicleType.SelectedItem.ToString(),
                    ParkingPlaceID = selectedPlace.Value,
                    EntryTime = DateTime.Now,
                    AmountPerMinute = cmbVehicleType.SelectedItem.ToString() == "Motor" ? 20 : 30,
                    IsActive = true
                };

                if (VehicleService.AddVehicle(vehicle))
                {
                    ShowTicket(vehicle, selectedPlace.Text);
                    ClearForm();
                    LoadParkingPlaces();
                    
                    MessageBox.Show("Vehicle added successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to add vehicle.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding vehicle: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowTicket(Vehicle vehicle, string placeNumber)
        {
            ticketPanel.Controls.Clear();
            ticketPanel.Visible = true;

            Label lblTicketTitle = new Label
            {
                Text = "PARKING TICKET",
                Font = new Font("Poppins", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(135, 206, 235),
                Location = new Point(20, 20),
                Size = new Size(400, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblTicketInfo = new Label
            {
                Text = $"Ticket ID: {vehicle.TicketID}\n" +
                       $"Owner: {vehicle.OwnerName}\n" +
                       $"Vehicle: {vehicle.VehicleNumber}\n" +
                       $"Type: {vehicle.VehicleType}\n" +
                       $"Place: {placeNumber}\n" +
                       $"Entry: {vehicle.EntryTime:yyyy-MM-dd HH:mm:ss}\n" +
                       $"Rate: {vehicle.AmountPerMinute} Ariary/minute",
                Font = new Font("Poppins", 12),
                Location = new Point(20, 70),
                Size = new Size(400, 200),
                TextAlign = ContentAlignment.TopLeft
            };

            Button btnPrint = new Button
            {
                Text = "Print Ticket",
                Font = new Font("Poppins", 12),
                Size = new Size(150, 40),
                Location = new Point(150, 300),
                BackColor = Color.FromArgb(135, 206, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.Click += (s, e) => {
                MessageBox.Show("Print functionality would be implemented here.", "Print", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            ticketPanel.Controls.AddRange(new Control[] { lblTicketTitle, lblTicketInfo, btnPrint });
        }

        private void ClearForm()
        {
            txtOwnerName.Clear();
            txtVehicleNumber.Clear();
            cmbVehicleType.SelectedIndex = -1;
            cmbParkingPlace.SelectedIndex = -1;
            lblSelectedPlace.Text = "No place selected";
            ticketPanel.Visible = false;
        }

        private class ComboBoxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }
            
            public override string ToString()
            {
                return Text;
            }
        }
    }
}
