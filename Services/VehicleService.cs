using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.Services
{
    public class VehicleService
    {
        public static string GenerateTicketID()
        {
            string dateStr = DateTime.Now.ToString("yyyyMMdd");
            string query = "SELECT COUNT(*) FROM Vehicles WHERE TicketID LIKE @pattern";
            SqlParameter[] parameters = {
                new SqlParameter("@pattern", $"TK-{dateStr}-%")
            };

            int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
            return $"TK-{dateStr}-{(count + 1):D4}";
        }

        public static bool AddVehicle(Vehicle vehicle)
        {
            try
            {
                string query = @"INSERT INTO Vehicles (TicketID, OwnerName, VehicleNumber, VehicleType, 
                                ParkingPlaceID, EntryTime, AmountPerMinute, IsActive) 
                                VALUES (@ticketId, @ownerName, @vehicleNumber, @vehicleType, 
                                @parkingPlaceId, @entryTime, @amountPerMinute, 1)";

                SqlParameter[] parameters = {
                    new SqlParameter("@ticketId", vehicle.TicketID),
                    new SqlParameter("@ownerName", vehicle.OwnerName),
                    new SqlParameter("@vehicleNumber", vehicle.VehicleNumber),
                    new SqlParameter("@vehicleType", vehicle.VehicleType),
                    new SqlParameter("@parkingPlaceId", vehicle.ParkingPlaceID),
                    new SqlParameter("@entryTime", vehicle.EntryTime),
                    new SqlParameter("@amountPerMinute", vehicle.AmountPerMinute)
                };

                int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
                
                if (result > 0)
                {
                    // Update parking place status
                    ParkingPlaceService.UpdatePlaceStatus(vehicle.ParkingPlaceID, true);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static List<Vehicle> GetActiveVehicles()
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            string query = @"SELECT v.*, p.PlaceNumber FROM Vehicles v 
                            INNER JOIN ParkingPlaces p ON v.ParkingPlaceID = p.PlaceID 
                            WHERE v.IsActive = 1";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            
            foreach (DataRow row in dt.Rows)
            {
                vehicles.Add(new Vehicle
                {
                    VehicleID = Convert.ToInt32(row["VehicleID"]),
                    TicketID = row["TicketID"].ToString(),
                    OwnerName = row["OwnerName"].ToString(),
                    VehicleNumber = row["VehicleNumber"].ToString(),
                    VehicleType = row["VehicleType"].ToString(),
                    ParkingPlaceID = Convert.ToInt32(row["ParkingPlaceID"]),
                    PlaceNumber = row["PlaceNumber"].ToString(),
                    EntryTime = Convert.ToDateTime(row["EntryTime"]),
                    AmountPerMinute = Convert.ToDecimal(row["AmountPerMinute"]),
                    IsActive = Convert.ToBoolean(row["IsActive"])
                });
            }
            
            return vehicles;
        }

        public static Vehicle GetVehicleByTicketID(string ticketID)
        {
            string query = @"SELECT v.*, p.PlaceNumber FROM Vehicles v 
                            INNER JOIN ParkingPlaces p ON v.ParkingPlaceID = p.PlaceID 
                            WHERE v.TicketID = @ticketId AND v.IsActive = 1";

            SqlParameter[] parameters = {
                new SqlParameter("@ticketId", ticketID)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
            
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Vehicle
                {
                    VehicleID = Convert.ToInt32(row["VehicleID"]),
                    TicketID = row["TicketID"].ToString(),
                    OwnerName = row["OwnerName"].ToString(),
                    VehicleNumber = row["VehicleNumber"].ToString(),
                    VehicleType = row["VehicleType"].ToString(),
                    ParkingPlaceID = Convert.ToInt32(row["ParkingPlaceID"]),
                    PlaceNumber = row["PlaceNumber"].ToString(),
                    EntryTime = Convert.ToDateTime(row["EntryTime"]),
                    AmountPerMinute = Convert.ToDecimal(row["AmountPerMinute"]),
                    IsActive = Convert.ToBoolean(row["IsActive"])
                };
            }
            
            return null;
        }

        public static bool ExitVehicle(string ticketID)
        {
            try
            {
                Vehicle vehicle = GetVehicleByTicketID(ticketID);
                if (vehicle == null) return false;

                DateTime exitTime = DateTime.Now;
                TimeSpan duration = exitTime - vehicle.EntryTime;
                decimal totalAmount = (decimal)Math.Ceiling(duration.TotalMinutes) * vehicle.AmountPerMinute;

                string query = @"UPDATE Vehicles SET ExitTime = @exitTime, TotalAmount = @totalAmount, 
                                IsPaid = 1, IsActive = 0 WHERE TicketID = @ticketId";

                SqlParameter[] parameters = {
                    new SqlParameter("@exitTime", exitTime),
                    new SqlParameter("@totalAmount", totalAmount),
                    new SqlParameter("@ticketId", ticketID)
                };

                int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
                
                if (result > 0)
                {
                    // Free the parking place
                    ParkingPlaceService.UpdatePlaceStatus(vehicle.ParkingPlaceID, false);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static decimal CalculateCurrentAmount(Vehicle vehicle)
        {
            TimeSpan duration = DateTime.Now - vehicle.EntryTime;
            return (decimal)Math.Ceiling(duration.TotalMinutes) * vehicle.AmountPerMinute;
        }
    }
}
