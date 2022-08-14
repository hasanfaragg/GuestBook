using Dapper;
using GuestBookTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GuestBookTest.MessagesControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private const string SqlConnection = "server=.; database=GuestBook; Integrated Security=true";

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserMessages(int userId)
        {
            var query = @"Select * From messages where userId = @userId";
            var dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("userId", userId);
            using (var connection = new SqlConnection(SqlConnection))
            {
                var messages = await connection.QueryAsync<Message>(query, dynamicParameter);
                if (messages == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(messages);
                }

            }
        }


        [HttpPost]
        public async Task<IActionResult> AddMessage([FromBody] CustomMessage customMessage)
        {

            var query = @"INSERT INTO messages (userId, messageText, messageWriter) VALUES (@userId, @messageText, @messageWriter);";
            var dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("userId", customMessage.userId);
            dynamicParameter.Add("messageText", customMessage.messageText);
            dynamicParameter.Add("messageWriter", customMessage.messageWriter);

            using (var connection = new SqlConnection(SqlConnection))
            {
                var messages = await connection.ExecuteAsync(query, dynamicParameter);
                if (messages == 0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok("Added");
                }

            }

        }
        [HttpDelete("{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId) {

            var query = @"Delete From messages where messageId = @messageId";
            var dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("messageId", messageId);
            using (var connection = new SqlConnection(SqlConnection))
            {
                var messages = await connection.ExecuteAsync(query, dynamicParameter);
                if (messages == 0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok("Deleted");
                }

            }

        }

        [HttpPut("{messageId}")]
        public async Task<IActionResult> EditMessage(int messageId, [FromBody] string messageText )
        { 
            var query = @"update messages set messageText = @messageText where messageId = @messageId";
            var dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("messageId", messageId);
            dynamicParameter.Add("messageText", messageText);
            using (var connection = new SqlConnection(SqlConnection))
            {
                var messages = await connection.ExecuteAsync(query, dynamicParameter);
                if (messages == 0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok("Edited");
                }

            }

        }

        [HttpPut]
        public async Task<IActionResult> AddReply( [FromBody] Reply reply)
        {

            var query = @"Update messages set reply = @reply where messageId = @messageId";
            var dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("messageId", reply.messageId);
            dynamicParameter.Add("reply", reply.reply);

            using (var connection = new SqlConnection(SqlConnection))
            {
                var messages = await connection.ExecuteAsync(query, dynamicParameter);
                if (messages == 0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok("Reply Added");
                }

            }

        }

    }
}
