using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


public partial class supportteam_task_allocate : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void AssignTaskNow()
    {
        // Database connection string
        string connectionString = c.OpenConnection();

        // Fetch employees from the database
        List<Employee> employees = GetEmployees(connectionString);

        // Fetch tasks from the database
        List<Task> tasks = GetTasks(connectionString);

        // Distribute tasks among employees
        DistributeTasks(employees, tasks);

        // Display the task distribution
        foreach (var employee in employees)
        {
            Response.Write("Employee: " + employee.Name);
            Response.Write("Tasks:");
            foreach (var task in employee.Tasks)
            {
                Response.Write(task.Description);
            }
            Console.WriteLine();
        }
    }

    static List<Employee> GetEmployees(string connectionString)
    {
        List<Employee> employees = new List<Employee>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT EmployeeID, Name FROM Employee";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int employeeID = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        employees.Add(new Employee { EmployeeID = employeeID, Name = name });
                    }
                }
            }
        }

        return employees;
    }

    static List<Task> GetTasks(string connectionString)
    {
        List<Task> tasks = new List<Task>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT TaskID, Description FROM TaskData";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int taskID = reader.GetInt32(0);
                        string description = reader.GetString(1);
                        tasks.Add(new Task { TaskID = taskID, Description = description });
                    }
                }
            }
        }

        return tasks;
    }

    static void DistributeTasks(List<Employee> employees, List<Task> tasks)
    {
        int employeeIndex = 0;

        foreach (var task in tasks)
        {
            employees[employeeIndex].Tasks.Add(task);
            // Inser query
            employeeIndex = (employeeIndex + 1) % employees.Count;
        }
    }

    class Employee
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public List<Task> Tasks { get; set; }

        public Employee()
        {
            Tasks = new List<Task>();
        }
    }

    class Task
    {
        public int TaskID { get; set; }
        public string Description { get; set; }
    }

    class StaffData
    {
        public int StaffID { get; set; }
    }
}