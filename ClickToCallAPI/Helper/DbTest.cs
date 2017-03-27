using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ClickToCallAPI.Helper
{
    public class DbTest
    {
        public DbTest()
        {
            var program = new Program();
        }
    }

    public class Employee
    {
        public string LastName { get; set; }
        public int Salary { get; set; }
        public string Address { get; set; }
    }

    class Program
    {
        public Program()
        {
            var emp = new Employee
            {
                LastName = "YMC",
                Salary = 78,
                Address = "Boston"
            };

            var emptwo = new Employee
            {
                LastName = "AYM",
                Salary = 87,
                Address = "Mumbai"
            };

            IEnumerable<Employee> list = new List<Employee> { emp, emptwo };

            AddEmployee(FillDataTableFromCollection(list));
        }

        public static DataTable FillDataTableFromCollection(params object[] parameters)
        {
            var employees = (IEnumerable<Employee>)parameters[0];

            var properties = typeof(Employee).GetProperties();

            var dataTable = new DataTable();
            foreach (var info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name,
                    Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (var entity in employees)
            {
                var values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static void AddEmployee(DataTable dataTable)
        {
            using (var conn = new SqlConnection(
                            @"Data Source = (local); Initial Catalog = MyCompany; Integrated Security = True;"))
            {
                conn.Open();
                using (var cmd = new SqlCommand("AddEmployee"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@EmpList", dataTable);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}