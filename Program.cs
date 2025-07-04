using System;
using System.Windows.Forms;
using ParkingManagementSystem.Forms;

namespace ParkingManagementSystem
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Test database connection
            if (!DatabaseHelper.TestConnection())
            {
                MessageBox.Show("Database connection failed. Please check your connection string.", 
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new LoginForm());
        }
    }
}
