using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Dynamic;
using System.Web.Http;

namespace DynamicService.Controllers
{
    [RoutePrefix("api/dynamicsql")]
    public class DynamicSqlApiController : ApiController
    {
        [HttpGet, Route("test")]
        public IHttpActionResult Test()
        {
            return Ok();
        }

        [HttpPost, Route("executequery")]
        public IHttpActionResult ExecuteQuery([FromBody]string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest();
            }

            List<dynamic> returnValueList = new List<dynamic>();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var result = new ExpandoObject() as IDictionary<string, object>;
                    
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        result.Add(reader.GetName(i), reader.GetValue(i));
                    }
                    
                    returnValueList.Add(result);
                }
            }

            return Ok(returnValueList);
        }
    }
}
