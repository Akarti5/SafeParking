using System;

namespace ParkingManagementSystem.Models
{
    public class Vehicle
    {
        public int VehicleID { get; set; }
        public string TicketID { get; set; }
        public string OwnerName { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleType { get; set; }
        public int ParkingPlaceID { get; set; }
        public string PlaceNumber { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public decimal AmountPerMinute { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool IsPaid { get; set; }
        public bool IsActive { get; set; }
    }
}
