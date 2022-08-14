using Dapper;
using GuestBookTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GuestBookTest.Controllers
{
  
    [Route("api/[controller]/[action]")]
    [ApiController]
  
public class UsersController : ControllerBase
    {
        private const string SqlConnection = "server=.; database=GuestBook; Integrated Security=true";

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            var query = @"Select * From users where userName=@userName and password = @password ";
            var dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("userName", userLogin.userName);
            dynamicParameter.Add("Password", userLogin.password);
            using (var connection = new SqlConnection(SqlConnection)) {

                var user = await connection.QueryFirstOrDefaultAsync<User>(query, dynamicParameter);
                if (user == null)
                {
                    return NotFound();
                }
                else {
                    return Ok(user);
                }
            }
               
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var query = @"Select * From users where userId = @userId";
            var dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("userId", userId);
            using (var connection = new SqlConnection(SqlConnection))
            {
                var user = await connection.QueryFirstOrDefaultAsync<User>(query, dynamicParameter);
                if (user == null)
                {
                    return NotFound();
                }
                else {
                    return Ok(new { userName = user.nickName, userId = user.userId });
                }
                
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            var query = @"INSERT INTO users (userName, nickName, password) VALUES (@userName, @nickName, @password);";

            var dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("userName", user.userName);
            dynamicParameter.Add("nickName", user.nickName);
            dynamicParameter.Add("password", user.password);

            using (var connection = new SqlConnection(SqlConnection))
            {
               var result =   await connection.ExecuteAsync(query, dynamicParameter);

                if (result == 0) {
                    return NotFound();
                }
                else
                {
                    return Ok("Added");
                }
                
            }

        }
    }
}
