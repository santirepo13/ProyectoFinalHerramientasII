using System;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CodeQuest
{
    public static class DebugHelper
    {
        public static void TestDatabaseTypes()
        {
            string connectionString = @"Server=DESKTOP-FN66L1D\SQLEXPRESS;Database=CodeQuest;Integrated Security=true;TrustServerCertificate=true;";
            
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("✅ Conexión exitosa");
                    
                    // Test simple query
                    using (var command = new SqlCommand("SELECT COUNT(*) FROM Users", connection))
                    {
                        var countResult = command.ExecuteScalar();
                        Console.WriteLine($"COUNT(*) Type: {countResult.GetType().Name}, Value: {countResult}");
                    }
                    
                    // Test SCOPE_IDENTITY simulation
                    using (var command = new SqlCommand("SELECT CAST(1 AS INT) AS TestID", connection))
                    {
                        var testResult = command.ExecuteScalar();
                        Console.WriteLine($"CAST(1 AS INT) Type: {testResult.GetType().Name}, Value: {testResult}");
                    }
                    
                    // Test actual stored procedure
                    using (var command = new SqlCommand("spUser_New", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@username", "TestUser_" + DateTime.Now.Ticks);
                        
                        var spResult = command.ExecuteScalar();
                        Console.WriteLine($"spUser_New Type: {spResult.GetType().Name}, Value: {spResult}");
                        
                        // Try conversion
                        try
                        {
                            int converted = Convert.ToInt32(spResult);
                            Console.WriteLine($"✅ Conversion successful: {converted}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"❌ Conversion failed: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
    }
}