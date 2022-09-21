using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Cryptography;

namespace UserRegistrationAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public RegisterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static User user = new User();

        [HttpPost("register")]
        public async Task<ActionResult<User>> registerUser(UserDTO request)
        {
            string query = @"INSERT INTO users (FIRSTNAME, LASTNAME, EMAIL_ADDRESS, PASSWORD) VALUES(@FirstName, @LastName, @EmailAddress, @Password);";
            DataTable table = new DataTable();
            string dataSource = _configuration.GetConnectionString("DefaultConnection");
            createPasswordHash(request.password, out byte[] passwordHash, out byte[] passwordSalt);
            user.firstName = request.firstName;
            user.lastName = request.lastName;
            user.emailAddress = request.emailAddress;
            user.passwordHash = passwordHash;
            user.passwordSalt = passwordSalt;
            MySqlDataReader myReader;
            MySqlConnection conn = new MySqlConnection(dataSource);
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("@FirstName", user.firstName);
            cmd.Parameters.AddWithValue("@LastName", user.lastName);
            cmd.Parameters.AddWithValue("@EmailAddress", user.emailAddress);
            cmd.Parameters.AddWithValue("@Password", user.passwordHash);
            conn.Open();
            myReader = cmd.ExecuteReader();
            table.Load(myReader);
            myReader.Close();
            conn.Close();
            return Ok(user);
        }

        private void createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private void connectToDatabase()
        {
            string connectionString = null;
            MySqlConnection connection;
            connectionString = "server=localhost;database=taskdb;uid=root;pwd=root;";
            connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
