using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;using web_s10197636.Models;


namespace web_s10197636.DAL
{
    public class StaffDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public StaffDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "NPBookConnectionString");

            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        public List<Staff> GetAllStaff()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Staff ORDER BY StaffID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();


            //Read all records until the end, save data into a staff list
            List<Staff> staffList = new List<Staff>();
            while (reader.Read())
            {
                staffList.Add(
                new Staff
                {
                    StaffId = reader.GetInt32(0), //0: 1st column
                    Name = reader.GetString(1), //1: 2nd column
                                                //Get the first character of a string
                    Gender = reader.GetString(2)[0], //2: 3rd column
                    DOB = reader.GetDateTime(3), //3: 4th column
                    Salary = reader.GetDecimal(5), //5: 6th column
                    Nationality = reader.GetString(6), //6: 7th column
                    Email = reader.GetString(9), //9: 10th column
                    IsFullTime = reader.GetBoolean(11), //11: 12th column
                                                        //7 - 8th column, assign Branch Id,
                                                        //if null value in db, assign integer null value
                    BranchNo = !reader.IsDBNull(7) ?
                            reader.GetInt32(7) : (int?)null,
                }
                );
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return staffList;
        }

        public int Add(Staff staff)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Staff (Name, Gender, DOB, Salary,
                                EmailAddr, Nationality, Status)
                                OUTPUT INSERTED.StaffID
                                VALUES(@name, @gender, @dob, @salary,
                                @email, @country, @status)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", staff.Name);
            cmd.Parameters.AddWithValue("@gender", staff.Gender);
            cmd.Parameters.AddWithValue("@dob", staff.DOB);
            cmd.Parameters.AddWithValue("@salary", staff.Salary);
            cmd.Parameters.AddWithValue("@email", staff.Email);
            cmd.Parameters.AddWithValue("@country", staff.Nationality);
            cmd.Parameters.AddWithValue("@status", staff.IsFullTime);
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            staff.StaffId = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return staff.StaffId;
        }

        public bool IsEmailExist(string email, int staffId)
        {
            bool emailFound = false;
            //Create a SqlCommand object and specify the SQL statement
            //to get a staff record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT StaffID FROM Staff
                                WHERE EmailAddr=@selectedEmail";
            cmd.Parameters.AddWithValue("@selectedEmail", email);
            //Open a database connection and excute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != staffId)
                        //The email address is used by another staff
                        emailFound = true;
                }
            }
            else
            { //No record
                emailFound = false; // The email address given does not exist
            }
            reader.Close();
            conn.Close();

            return emailFound;
        }
    }
}
