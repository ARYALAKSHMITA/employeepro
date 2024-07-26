using CRUDSignUp.Models;
using System.Data;
using System.Data.SqlClient;

namespace CRUDSignUp.DAL

{
    public class Employee_DAL
    {
        SqlConnection? _connection = null;
        SqlCommand ?_command = null;
        public static IConfiguration? Configuration { get; set; }

        private string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            return Configuration.GetConnectionString("DefaultConnection")?? string.Empty;
        }

        public List<Employee> GetAll()
        {
            List<Employee> employeeList = new List<Employee>();
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "SPS_Employees";
                _connection.Open();
                SqlDataReader dr = _command.ExecuteReader();

                while (dr.Read())
                {
                    Employee employee = new Employee();
                    employee.Id = Convert.ToInt32(dr["Id"]);
                    employee.FirstName = dr["FirstName"].ToString();
                    employee.LastName = dr["LastName"].ToString();
                    employee.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]).Date;
                    employee.Email = dr["Email"].ToString();
                    employee.Salary = Convert.ToDouble(dr["Salary"]);
                    employeeList.Add(employee);

                }
                _connection.Close();
            }
            return employeeList;
        }

        public bool Insert(Employee model)
        {
            int id = 0;
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "SPI_Employees";
                _command.Parameters.AddWithValue("@FirstName", model.FirstName ?? (object)DBNull.Value);
                _command.Parameters.AddWithValue("@LastName", model.LastName ?? (object)DBNull.Value);
                _command.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth ?? (object)DBNull.Value);
                _command.Parameters.AddWithValue("@Email", model.Email ?? (object)DBNull.Value);
                _command.Parameters.AddWithValue("@Salary", model.Salary ?? (object)DBNull.Value);
                _connection.Open();
                id = _command.ExecuteNonQuery();
                _connection.Close();
            }
            return id > 0 ? true : false;
        }

        public Employee GetById( int id)
        {
            Employee employee = new Employee();
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "SPS_EmployeesById";
                _command.Parameters.AddWithValue("@Id", id );
                _connection.Open();
                SqlDataReader dr = _command.ExecuteReader();

                while (dr.Read())
                {
                    
                    employee.Id = Convert.ToInt32(dr["Id"]);
                    employee.FirstName = dr["FirstName"].ToString();
                    employee.LastName = dr["LastName"].ToString();
                    employee.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]).Date;
                    employee.Email = dr["Email"].ToString();
                    employee.Salary = Convert.ToDouble(dr["Salary"]);
                   

                }
                _connection.Close();
            }
            return employee;
        }

        public bool Update(Employee model)
        {
            int id = 0;
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "SPU_Employees";
                _command.Parameters.AddWithValue("@Id", model.Id);
                _command.Parameters.AddWithValue("@FirstName", model.FirstName ?? (object)DBNull.Value);
                _command.Parameters.AddWithValue("@LastName", model.LastName ?? (object)DBNull.Value);
                _command.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth ?? (object)DBNull.Value);
                _command.Parameters.AddWithValue("@Email", model.Email ?? (object)DBNull.Value);
                _command.Parameters.AddWithValue("@Salary", model.Salary ?? (object)DBNull.Value);
                _connection.Open();
                id = _command.ExecuteNonQuery();
                _connection.Close();
            }
            return id > 0 ? true : false;
        }

        public bool Delete(int id)
        {
            int deletedRowCount = 0;
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "SPD_Employees";
                _command.Parameters.AddWithValue("@Id",id);
                _connection.Open();
                deletedRowCount = _command.ExecuteNonQuery();
                _connection.Close();
            }
            return id > 0 ? true : false;
        }
    }
}

