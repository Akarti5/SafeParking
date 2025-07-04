using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.Services
{
    public class ParkingPlaceService
    {
        public static List<ParkingPlace> GetAllParkingPlaces()
        {
            List<ParkingPlace> places = new List<ParkingPlace>();
            string query = "SELECT * FROM ParkingPlaces ORDER BY PlaceID";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            
            foreach (DataRow row in dt.Rows)
            {
                places.Add(new ParkingPlace
                {
                    PlaceID = Convert.ToInt32(row["PlaceID"]),
                    PlaceNumber = row["PlaceNumber"].ToString(),
                    IsOccupied = Convert.ToBoolean(row["IsOccupied"]),
                    CreatedDate = Convert.ToDateTime(row["CreatedDate"])
                });
            }
            
            return places;
        }

        public static List<ParkingPlace> GetAvailableParkingPlaces()
        {
            List<ParkingPlace> places = new List<ParkingPlace>();
            string query = "SELECT * FROM ParkingPlaces WHERE IsOccupied = 0 ORDER BY PlaceID";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            
            foreach (DataRow row in dt.Rows)
            {
                places.Add(new ParkingPlace
                {
                    PlaceID = Convert.ToInt32(row["PlaceID"]),
                    PlaceNumber = row["PlaceNumber"].ToString(),
                    IsOccupied = Convert.ToBoolean(row["IsOccupied"]),
                    CreatedDate = Convert.ToDateTime(row["CreatedDate"])
                });
            }
            
            return places;
        }

        public static bool UpdatePlaceStatus(int placeID, bool isOccupied)
        {
            try
            {
                string query = "UPDATE ParkingPlaces SET IsOccupied = @isOccupied WHERE PlaceID = @placeId";
                SqlParameter[] parameters = {
                    new SqlParameter("@isOccupied", isOccupied),
                    new SqlParameter("@placeId", placeID)
                };

                int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch
            {
                return false;
            }
        }

        public static int GetOccupiedPlacesCount()
        {
            string query = "SELECT COUNT(*) FROM ParkingPlaces WHERE IsOccupied = 1";
            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query));
        }

        public static int GetFreePlacesCount()
        {
            string query = "SELECT COUNT(*) FROM ParkingPlaces WHERE IsOccupied = 0";
            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query));
        }
    }
}
