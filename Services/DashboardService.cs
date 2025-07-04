using System;
using System.Data.SqlClient;

namespace ParkingManagementSystem.Services
{
    public class DashboardService
    {
        public static int GetTotalVehiclesToday()
        {
            string query = "SELECT COUNT(*) FROM Vehicles WHERE CAST(EntryTime AS DATE) = CAST(GETDATE() AS DATE)";
            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query));
        }

        public static decimal GetMoneyEarnedToday()
        {
            string query = @"SELECT ISNULL(SUM(TotalAmount), 0) FROM Vehicles 
                            WHERE CAST(EntryTime AS DATE) = CAST(GETDATE() AS DATE) AND IsPaid = 1";
            return Convert.ToDecimal(DatabaseHelper.ExecuteScalar(query));
        }

        public static int GetPaidVehiclesToday()
        {
            string query = @"SELECT COUNT(*) FROM Vehicles 
                            WHERE CAST(EntryTime AS DATE) = CAST(GETDATE() AS DATE) AND IsPaid = 1";
            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query));
        }

        public static int GetActiveVehiclesCount()
        {
            string query = "SELECT COUNT(*) FROM Vehicles WHERE IsActive = 1";
            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query));
        }
    }
}
