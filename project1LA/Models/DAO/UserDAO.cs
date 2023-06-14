using MySql.Data.MySqlClient;
using project1LA.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project1LA.Models.DAO
{
    public class UserDAO
    {
        public string InsertUser(UserDTO user)
        {
            string response = "Failed";

            try
            {
                using (MySqlConnection connection = Config.GetConnection())
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO Users (name,email) values (@name,@email)";

                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@name", user.Name);
                        command.Parameters.AddWithValue("@email", user.Email);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            response = "Success";
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: "+ex.Message);
            }

            return response;
        }

        public List<UserDTO> ReadUsers()
        {

             List<UserDTO> users = new List<UserDTO>();

            try
            {
                using (MySqlConnection connection = Config.GetConnection())
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM Users";

                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserDTO user = new UserDTO();

                                user.Id = reader.GetInt32("id");
                                user.Name = reader[1].ToString();
                                user.Email = reader[2].ToString();
                                users.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return users;
        }


        public UserDTO UpdateUserForm(UserDTO user)
        {
            UserDTO updatedUser = null;

            try
            {
                using (MySqlConnection connection = Config.GetConnection())
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM Users WHERE id = @id";

                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", user.Id);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                updatedUser = new UserDTO
                                {
                                    Id = reader.GetInt32("id"),
                                    Name = reader.GetString("name"),
                                    Email = reader.GetString("email"),
                                    // Aquí debes asignar los demás atributos del objeto UserDTO
                                    // utilizando los valores del reader correspondientes
                                };
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return updatedUser;
        }







        public string DeleteUser(UserDTO user)
        {
            string response = "Failed";

            try
            {
                using (MySqlConnection connection = Config.GetConnection())
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM Users WHERE id = @id";

                    using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", user.Id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            response = "Success";
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return response;
        }
    }
}