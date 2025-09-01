using InventorySales.Models;

namespace InventorySales.Service
{
    public interface iAuditLog
    {
        Task<IEnumerable<AuditLog>> GetAllAuditLogs();
        Task LogAction(int? userId, string action, string tableName, int recordId);
    }
}
