using InventorySales.Models;
using InventorySales.Service;
using Microsoft.EntityFrameworkCore;

namespace InventorySales.Repository
{
    public class AuditLogRepository : iAuditLog
    {
        private readonly DBContext dbContext = new DBContext();
        public async Task<IEnumerable<AuditLog>> GetAllAuditLogs() => await dbContext.AuditLogs.OrderByDescending(x => x.Timestamp).ToListAsync();

        public async Task LogAction(int? userId, string action, string tableName, int recordId)
        {
            var log = new AuditLog
            {
                UserId = userId,
                Action = action,
                TableName = tableName,
                RecordId = recordId,
                Timestamp = DateTime.UtcNow
            };
            dbContext.AuditLogs.Add(log);

            await dbContext.SaveChangesAsync();
        }
    }
}
