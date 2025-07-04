using System;

namespace ParkingManagementSystem.Models
{
    public class ParkingPlace
    {
        public int PlaceID { get; set; }
        public string PlaceNumber { get; set; }
        public bool IsOccupied { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
