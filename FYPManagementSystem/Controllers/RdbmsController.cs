using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using FYPManagementSystem.Data;

namespace FYPManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RdbmsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RdbmsController(AppDbContext context)
        {
            _context = context;
        }

        public class QueryRequest
        {
            public string QueryKey { get; set; }
        }

        [HttpPost("run-query")]
        public async Task<IActionResult> RunQuery([FromBody] QueryRequest request)
        {
            string sqlQuery = "";
            string description = "";

            switch (request.QueryKey)
            {
                case "projects_with_groups":
                    sqlQuery = @"
                        SELECT p.Title AS [Project Title], g.GroupName AS [Group Name], u.Name AS [Supervisor Name]
                        FROM Projects p
                        JOIN Groups g ON p.GroupId = g.Id
                        LEFT JOIN Users u ON p.SupervisorId = u.Id;";
                    description = "Demonstrates an INNER JOIN between Projects and Groups, and a LEFT JOIN with Users to retrieve the supervisor's name. This shows how relational links are resolved.";
                    break;

                case "supervisor_workload":
                    sqlQuery = @"
                        SELECT u.Name AS [Supervisor], COUNT(p.Id) AS [Assigned Projects]
                        FROM Users u
                        LEFT JOIN Projects p ON p.SupervisorId = u.Id
                        WHERE u.Role = 'Supervisor'
                        GROUP BY u.Name, u.Id
                        ORDER BY [Assigned Projects] DESC;";
                    description = "Demonstrates a LEFT JOIN, a WHERE filter, a GROUP BY aggregation, and a COUNT() function with an ORDER BY clause to find supervisor project distribution.";
                    break;

                case "group_members":
                    sqlQuery = @"
                        SELECT g.GroupName AS [Group], u.Name AS [Student Name], u.Email AS [Email Address]
                        FROM Groups g
                        JOIN GroupMembers gm ON gm.GroupId = g.Id
                        JOIN Users u ON gm.StudentId = u.Id
                        ORDER BY g.GroupName, u.Name;";
                    description = "Demonstrates a 3-table INNER JOIN resolving the many-to-many relationship between Students and Groups via the GroupMembers bridge table.";
                    break;

                case "deliverables_status":
                    sqlQuery = @"
                        SELECT p.Title AS [Project], d.Title AS [Deliverable], d.Type AS [Type], d.Status AS [Status], d.DueDate AS [Due Date]
                        FROM Deliverables d
                        JOIN Projects p ON d.ProjectId = p.Id
                        ORDER BY d.DueDate ASC;";
                    description = "Demonstrates an INNER JOIN between Deliverables and Projects, sorted by DueDate in ascending order.";
                    break;

                case "evaluations_marks":
                    sqlQuery = @"
                        SELECT p.Title AS [Project Title], e.Marks AS [Marks (Out of 100)], e.Feedback AS [Comments], e.EvaluatedAt AS [Date Evaluated]
                        FROM Evaluations e
                        JOIN Projects p ON e.ProjectId = p.Id
                        ORDER BY e.Marks DESC;";
                    description = "Demonstrates a JOIN operation between the Evaluations table and the Projects table, sorted by Marks in descending order.";
                    break;

                case "proposal_stats":
                    sqlQuery = @"
                        SELECT Status AS [Proposal Status], COUNT(Id) AS [Total Count]
                        FROM Proposals
                        GROUP BY Status;";
                    description = "Demonstrates a simple GROUP BY and COUNT() aggregation on the Proposals table to see how many project plans are Pending, Approved, or Rejected.";
                    break;

                default:
                    return BadRequest(new { message = "Invalid query selection." });
            }

            try
            {
                var columns = new List<string>();
                var rows = new List<List<object>>();

                using (var conn = _context.Database.GetDbConnection())
                {
                    if (conn.State == ConnectionState.Closed)
                        await conn.OpenAsync();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sqlQuery;
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            // Get column names
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                columns.Add(reader.GetName(i));
                            }

                            // Get row values
                            while (await reader.ReadAsync())
                            {
                                var row = new List<object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    var val = reader.GetValue(i);
                                    row.Add(val == DBNull.Value ? null : val);
                                }
                                rows.Add(row);
                            }
                        }
                    }
                }

                return Ok(new
                {
                    sql = sqlQuery,
                    description = description,
                    columns = columns,
                    rows = rows
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Database query failed: {ex.Message}" });
            }
        }
    }
}
