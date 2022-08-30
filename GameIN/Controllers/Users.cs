using GameIN.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GameIN.Controllers
{
    public class Users
    {
        public static void CreateUser(UserModel user)
        {
            string query = @"insert into [dbo].[User](Name, Email, Password, Role) Values(@Name, @Email, @Password, @Role)";
            var connStr = ConfigurationManager.ConnectionStrings["GameInDB"].ConnectionString;
            var conn = new SqlConnection(connStr);
            conn.Open();
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@Role", "User");
            command.ExecuteNonQuery();
            conn.Close();
        }

        public static Tuple<int, string> AuthenticateUser(UserModel user)
        {
            string query = @"select Name, Email, Password, Role from [dbo].[User] where Email = @Email and Password = @Password";
            var connStr = ConfigurationManager.ConnectionStrings["GameInDB"].ConnectionString;
            var conn = new SqlConnection(connStr);
            conn.Open();
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    string name;
                    string isAdmin;
                    while (reader.Read())
                    {
                        var us = LoggedUser.Instance;
                        us.user = new UserModel();
                        us.user.Name = reader.GetString(0);
                        us.user.Email = reader.GetString(1);
                        us.user.Password = reader.GetString(2);
                        name = reader.GetString(0);
                        isAdmin = reader.GetString(3);
                        if (isAdmin.Equals("Admin"))
                        {
                            return new Tuple<int, string>(1, name);
                        }
                        return new Tuple<int, string>(2, name);
                    }
                    return new Tuple<int, string>(0, null);
                }
                else
                {

                    return new Tuple<int, string>(-1, null);
                }
            }
        }
    }
}