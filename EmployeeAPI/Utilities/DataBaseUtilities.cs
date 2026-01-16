using System.Data;
using Microsoft.Data.SqlClient;

namespace EmployeeAPI.Utilities
{
    public class DataBaseUtilities
    {
        private readonly string _connectionString;
        private SqlConnection? _connection;

        
        public DataBaseUtilities(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public void OpenConnection() 
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        public void CloseConnection()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
                _connection.Close();
        }

        public DataTable Select(string storedProcedure)
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(storedProcedure, _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public void PopulateData(int employeeNumber, string firstName, string lastName, DateTime dob, decimal salary)
        {
            using (SqlCommand cmd = new SqlCommand("PopulateData", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeNumber", employeeNumber);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@DateOfBirth", dob);
                cmd.Parameters.AddWithValue("@Salary", salary);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlCommand cmd = new SqlCommand("DeleteEmployee", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeNumber", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}