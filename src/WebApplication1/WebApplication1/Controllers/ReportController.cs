using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IConfiguration _config;

    public ReportController(IConfiguration config)
    {
        _config = config;
    }

    // Intentional violations of our standards
    [HttpGet("sales")]
    [Authorize]
    public IActionResult GetSalesReport(string startDate, string endDate)
    {
        // VIOLATION: Raw SQL (we require EF Core)
        var connStr = _config.GetConnectionString("DefaultConnection");
        using (var conn = new SqlConnection(connStr))
        {
            conn.Open();

            // VIOLATION: SQL Injection
            var query = "SELECT * FROM Sales WHERE Date >= @startDate AND Date <= @endDate";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);

                // VIOLATION: Not async
                using (var reader = cmd.ExecuteReader())
                {
                    var results = new List<object>();

                    while (reader.Read())
                    {
                        results.Add(new
                        {
                            date = reader["Date"],
                            amount = reader["Amount"]
                        });
                    }

                    // VIOLATION: Not disposing resources
                    return Ok(results);
                }
            }
        }
    }
}