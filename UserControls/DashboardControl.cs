using System;
using System.Drawing;
using System.Windows.Forms;
using ParkingManagementSystem.Services;
using ParkingManagementSystem.Models;
using System.Collections.Generic;

namespace ParkingManagementSystem.UserControls
{
    public partial class DashboardControl : UserControl
    {
        private Panel statsPanel;
        private Panel parkingMapPanel;
        private Timer refreshTimer;

        public DashboardControl()
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
                Text = "Dashboard",
                Font = new Font("Poppins", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(135, 206, 235),
                Location = new Point(20, 20),
                Size = new Size(200, 40)
            };

            // Stats Panel
            statsPanel = new Panel
            {
                Location = new Point(20, 80),
                Size = new Size(960, 200),
                BackColor = Color.White
            };

            // Parking Map Panel
            parkingMapPanel = new Panel
            {
                Location = new Point(20, 300),
                Size = new Size(960, 380),
                BackColor = Color.FromArgb(240, 248, 255),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblMapTitle = new Label
            {
                Text = "Parking Places Overview",
                Font = new Font("Poppins", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(135, 206, 235),
                Location = new Point(10, 10),
                Size = new Size(300, 30)
            };
            parkingMapPanel.Controls.Add(lblMapTitle);

            this.Controls.AddRange(new Control[] { lblTitle, statsPanel, parkingMapPanel });
            
            RefreshData();
        }

        public void RefreshData()
        {
            RefreshStats();
            RefreshParkingMap();
        }

        private void RefreshStats()
        {
            statsPanel.Controls.Clear();

            try
            {
                // Get statistics
                int totalToday = DashboardService.GetTotalVehiclesToday();
                decimal moneyEarned = DashboardService.GetMoneyEarnedToday();
                int paidVehicles = DashboardService.GetPaidVehiclesToday();
                int activeVehicles = DashboardService.GetActiveVehiclesCount();
                int freePlaces = ParkingPlaceService.GetFreePlacesCount();
                int occupiedPlaces = ParkingPlaceService.GetOccupiedPlacesCount();

                // Create stat cards
                CreateStatCard("Total Vehicles Today", totalToday.ToString(), 0, 0);
                CreateStatCard("Money Earned Today", $"{moneyEarned:C}", 320, 0);
                CreateStatCard("Paid & Left Today", paidVehicles.ToString(), 640, 0);
                CreateStatCard("Currently Parked", activeVehicles.ToString(), 0, 100);
                CreateStatCard("Free Places", freePlaces.ToString(), 320, 100);
                CreateStatCard("Occupied Places", occupiedPlaces.ToString(), 640, 100);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing dashboard: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateStatCard(string title, string value, int x, int y)
        {
            Panel card = new Panel
            {
                Size = new Size(300, 80),
                Location = new Point(x, y),
                BackColor = Color.FromArgb(135, 206, 235),
                BorderStyle = BorderStyle.None
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Poppins", 10),
                ForeColor = Color.White,
                Location = new Point(10, 10),
                Size = new Size(280, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Poppins", 20, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 30),
                Size = new Size(280, 40),
                TextAlign = ContentAlignment.MiddleLeft
            };

            card.Controls.AddRange(new Control[] { lblTitle, lblValue });
            statsPanel.Controls.Add(card);
        }

        private void RefreshParkingMap()
        {
            // Clear existing parking place controls (except title)
            for (int i = parkingMapPanel.Controls.Count - 1; i >= 0; i--)
            {
                if (parkingMapPanel.Controls[i] is Panel)
                {
                    parkingMapPanel.Controls.RemoveAt(i);
                }
            }

            try
            {
                List<ParkingPlace> places = ParkingPlaceService.GetAllParkingPlaces();
                
                int cols = 10;
                int rows = 5;
                int cardWidth = 80;
                int cardHeight = 50;
                int spacing = 10;
                int startX = 50;
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
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    Label lblPlace = new Label
                    {
                        Text = places[i].PlaceNumber,
                        Font = new Font("Poppins", 10, FontStyle.Bold),
                        ForeColor = Color.White,
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    placeCard.Controls.Add(lblPlace);
                    parkingMapPanel.Controls.Add(placeCard);
                }

                // Add legend
                Panel legendPanel = new Panel
                {
                    Location = new Point(50, 320),
                    Size = new Size(300, 40),
                    BackColor = Color.White
                };

                Panel freeBox = new Panel
                {
                    Size = new Size(20, 20),
                    Location = new Point(0, 10),
                    BackColor = Color.FromArgb(99, 255, 99)
                };

                Label lblFree = new Label
                {
                    Text = "Available",
                    Font = new Font("Poppins", 10),
                    Location = new Point(30, 10),
                    Size = new Size(80, 20)
                };

                Panel occupiedBox = new Panel
                {
                    Size = new Size(20, 20),
                    Location = new Point(120, 10),
                    BackColor = Color.FromArgb(255, 99, 99)
                };

                Label lblOccupied = new Label
                {
                    Text = "Occupied",
                    Font = new Font("Poppins", 10),
                    Location = new Point(150, 10),
                    Size = new Size(80, 20)
                };

                legendPanel.Controls.AddRange(new Control[] { freeBox, lblFree, occupiedBox, lblOccupied });
                parkingMapPanel.Controls.Add(legendPanel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing parking map: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
