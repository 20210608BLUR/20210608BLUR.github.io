using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Practice
{
    class Program
    {
        static string connectString = ConfigurationManager.ConnectionStrings["NorthwindContext"].ConnectionString;
       
        static void Main(string[] args)
        {
            QuerySP();
            Employee emp = new Employee
            {
                FirstName = "大衛",
                LastName = "王",
                Title = "CEO",
                Country = "美國",
            };

            int row = ExecuteInsert(emp);

            Console.WriteLine($"影響{row}筆資料!");
        }

        static void QuerySP()
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                var emp = conn.Query<Employee>("FindEmployeeByName", new { LastName = "King", FirstName = "Robert" },
                                           commandType: CommandType.StoredProcedure).SingleOrDefault();
                Console.WriteLine($"{emp.EmployeeID}, {emp.FirstName}, {emp.LastName}, {emp.City}, {emp.Country}");
            };
        }

        static int ExecuteInsert(Employee emp)
        {
            int affectedRow = 0;
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                string sql = "INSERT INTO Employees(FirstName, LastName, Title, Country) VALUES(@FirstName, @LastName, @Title, @Country)";
                affectedRow = conn.Execute(sql, new { emp.FirstName, emp.LastName, emp.Title, emp.Country });
            }

            return affectedRow;

        }
    }
}
