using System;
using System.Data.SqlClient;

public class DatabaseHelper
{
    private static readonly string connectionString = "Data Source=DESKTOP-UKCB321\\MSSQLSERVER01;Initial Catalog=ProjectManagementDB;Integrated Security=True;";

    public static SqlConnection GetConnection()
    {
        try
        {
            return new SqlConnection(connectionString);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error connecting to database: " + ex.Message);
            throw;
        }
    }
}
