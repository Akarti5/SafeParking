using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using ParkingManagementSystem.Services;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.UserControls
{
    public partial class ParkControl : UserControl
    {
        private TextBox txtSearch;
        private FlowLayoutPanel vehiclesPanel;
        private Panel mapPanel;
        private Timer refreshTimer;

        public ParkControl()
        {
            InitializeComponent();
            SetupControls();
            StartRefreshTimer();
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
                Text = "Parked Vehicles",
                Font = new Font("Poppins", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(135, 206, 235),
                Location = new Point(20, 20),
                Size = new Size(300, 40)
            };

            // Search
            Label lblSearch = new Label
            {
                Text = "Search:",
                Font = new Font("Poppins", 12),
                Location = new Point(20, 80),
                Size = new Size(80, 25)
            };

            txtSearch = new TextBox
            {
                Font = new Font("Poppins", 12),
                Location = new Point(100, 80),
                Size = new Size(300, 30),
                PlaceholderText = "Search by owner, vehicle number, or place..."
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            Button btnRefresh = new Button
            {
                Text = "Refresh",
                Font = new Font("Poppins", 10),
                Size = new Size(100, 30),
                Location = new Point(420, 80),
                BackColor = Color.FromArgb(135, 206, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => RefreshData();

            // Vehicles Panel
            vehiclesPanel = new FlowLayoutPanel
            {
                Location = new Point(20, 130),
                Size = new Size(500, 550),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            // Map Panel
            mapPanel = new Panel
            {
                Location = new Point(540, 130),
                Size = new Size(440, 550),
                BackColor = Color.FromArgb(240, 248, 255),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblMapTitle = new Label
            {
                Text = "Parking Map",
                Font = new Font("Poppins", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(135, 206, 235),
                Location = new Point(10, 10),
                Size = new Size(200, 30)
            };
            mapPanel.Controls.Add(lblMapTitle);

            this.Controls.AddRange(new Control[] { lblTitle, lblSearch, txtSearch, btnRefresh, vehiclesPanel, mapPanel });
            
            RefreshData();
        }

        public void RefreshData()
        {
            LoadVehicles();
            LoadParkingMap();
        }

        private void LoadVehicles()
        {
            vehiclesPanel.Controls.Clear();

            try
            {
                List<Vehicle> vehicles = VehicleService.GetActiveVehicles();
                
                foreach (var vehicle in vehicles)
                {
                    CreateVehicleCard(vehicle);
                }

                if (vehicles.Count == 0)
                {
                    Label lblNoVehicles = new Label
                    {
                        Text = "No vehicles currently parked",
                        Font = new Font("Poppins", 14),
                        ForeColor = Color.Gray,
                        Size = new Size(450, 50),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    vehiclesPanel.Controls.Add(lblNoVehicles);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading vehicles: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateVehicleCard(Vehicle vehicle)
        {
            Panel card = new Panel
            {
                Size = new Size(460, 120),
                BackColor = Color.FromArgb(135, 206, 235),
                Margin = new Padding(5),
                BorderStyle = BorderStyle.None
            };

            Label lblOwner = new Label
            {
                Text = $"Owner: {vehicle.OwnerName}",
                Font = new Font("Poppins", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 10),
                Size = new Size(300, 25)
            };

            Label lblVehicle = new Label
            {
                Text = $"Vehicle: {vehicle.VehicleNumber} ({vehicle.VehicleType})",
                Font = new Font("Poppins", 10),
                ForeColor = Color.White,
                Location = new Point(10, 35),
                Size = new Size(300, 20)
            };

            Label lblPlace = new Label
            {
                Text = $"Place: {vehicle.PlaceNumber}",
                Font = new Font("Poppins", 10),
                ForeColor = Color.White,
                Location = new Point(10, 55),
                Size = new Size(150, 20)
            };

            Label lblEntry = new Label
            {
                Text = $"Entry: {vehicle.EntryTime:HH:mm:ss}",
                Font = new Font("Poppins", 10),
                ForeColor = Color.White,
                Location = new Point(10, 75),
                Size = new Size(150, 20)
            };

            Button btnInfo = new Button
            {
                Text = "Info",
                Font = new Font("Poppins", 10),
                Size = new Size(80, 30),
                Location = new Point(350, 45),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(135, 206, 235),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnInfo.FlatAppearance.BorderSize = 0;
            btnInfo.Click += (s, e) => ShowVehicleInfo(vehicle);

            card.Controls.AddRange(new Control[] { lblOwner, lblVehicle, lblPlace, lblEntry, btnInfo });
            vehiclesPanel.Controls.Add(card);
        }

        private void ShowVehicleInfo(Vehicle vehicle)
        {
            TimeSpan duration = DateTime.Now - vehicle.EntryTime;
            decimal currentAmount = VehicleService.CalculateCurrentAmount(vehicle);

            string info = $"Vehicle Information\n\n" +
                         $"Ticket ID: {vehicle.TicketID}\n" +
                         $"Owner: {vehicle.OwnerName}\n" +
                         $"Vehicle: {vehicle.VehicleNumber}\n" +
                         $"Type: {vehicle.VehicleType}\n" +
                         $"Parking Place: {vehicle.PlaceNumber}\n" +
                         $"Entry Time: {vehicle.EntryTime:yyyy-MM-dd HH:mm:ss}\n" +
                         $"Duration: {duration.Hours}h {duration.Minutes}m\n" +
                         $"Rate: {vehicle.AmountPerMinute} Ariary/minute\n" +
                         $"Current Amount: {currentAmount:C}";

            MessageBox.Show(info, "Vehicle Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadParkingMap()
        {
            // Clear existing controls except title
            for (int i = mapPanel.Controls.Count - 1; i >= 0; i--)
            {
                if (mapPanel.Controls[i] is Panel)
                {
                    mapPanel.Controls.RemoveAt(i);
                }
            }

            try
            {
                List<ParkingPlace> places = ParkingPlaceService.GetAllParkingPlaces();
                
                int cols = 8;
                int rows = 7;
                int cardWidth = 45;
                int cardHeight = 35;
                int spacing = 5;
                int startX = 20;
                int startY = 50;

                for (int i = 0; i < places.Count && i < 50; i++)
                {
                    int row = i / cols;
                    int col = i % cols;
                    
                    Panel placeCard = new Panel
                    {
                        Size = new Size(cardWidth, cardHeight),
                        Location = new Point(startX + col * (cardWidth + spacing), 
                                           startY + row * (cardHeight + spacing)),
                        BackColor = places[i].IsOccupied ? Color.FromArgb(255, 99, 99) : Color.FromArgb(99, 255, 99),
                        BorderStyle = BorderStyle.FixedSingle,
                        Cursor = Cursors.Hand
                    };

                    Label lblPlace = new Label
                    {
                        Text = places[i].PlaceNumber,
                        Font = new Font("Poppins", 8, FontStyle.Bold),
                        ForeColor = Color.White,
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    placeCard.Controls.Add(lblPlace);
                    mapPanel.Controls.Add(placeCard);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading parking map: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();
            
            foreach (Control control in vehiclesPanel.Controls)
            {
                if (control is Panel card)
                {
                    bool visible = false;
                    foreach (Control cardControl in card.Controls)
                    {
                        if (cardControl is Label label && label.Text.ToLower().Contains(searchText))
                        {
                            visible = true;
                            break;
                        }
                    }
                    card.Visible = visible;
                }
            }
        }

        private void StartRefreshTimer()
        {
            refreshTimer = new Timer
            {
                Interval = 30000 // Refresh every 30 seconds
            };
            refreshTimer.Tick += (s, e) => RefreshData();
            refreshTimer.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                refreshTimer?.Stop();
                refreshTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
