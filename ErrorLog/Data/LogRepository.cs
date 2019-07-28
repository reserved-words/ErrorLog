using System;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using ErrorLog.Data;

namespace ErrorLog.Data
{
    public class LogRepository : ILogRepository
    {
        private readonly LogContext _context;

        public LogRepository(LogContext context)
        {
            _context = context;
        }

        public void AddApp(App app)
        {
            _context.Apps.Add(app);
        }

        public void AddLog(Log log)
        {
            _context.Logs.Add(log);
        }

        public void DeleteApp(App app)
        {
            _context.Apps.Remove(app);
        }

        public void DeleteLog(Log log)
        {
            _context.Logs.Remove(log);
        }

        public async Task<bool> SaveChangesAsync()
        {
            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<App[]> GetAllAppsAsync(bool includeLogs = false)
        {
            IQueryable<App> query = _context.Apps;

            if (includeLogs)
            {
                query = query
                  .Include(c => c.Logs);
            }

            // Order It
            query = query.OrderBy(c => c.Id);

            return await query.ToArrayAsync();
        }

        public async Task<App> GetAppAsync(string moniker, bool includeLogs = false)
        {
            IQueryable<App> query = _context.Apps;

            if (includeLogs)
            {
                query = query.Include(c => c.Logs);
            }

            // Query It
            query = query.Where(c => c.Moniker == moniker);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Log> GetLogByAppAsync(string moniker, int logId, bool includeAppDetails = false)
        {
            IQueryable<Log> query = _context.Logs;

            if (includeAppDetails)
            {
                query = query
                  .Include(t => t.App);
            }

            // Add Query
            query = query
              .Where(t => t.Id == logId && t.App.Moniker == moniker);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Log[]> GetLogsAsync(bool includeAppDetails = false)
        {
            IQueryable<Log> query = _context.Logs;

            if (includeAppDetails)
            {
                query = query
                  .Include(t => t.App);
            }

            query = query
              .OrderByDescending(t => t.Timestamp);

            return await query.ToArrayAsync();
        }

        public async Task<Log[]> GetLogsByAppAsync(string moniker, bool includeAppDetails = false)
        {
            IQueryable<Log> query = _context.Logs;

            if (includeAppDetails)
            {
                query = query
                  .Include(t => t.App);
            }

            query = query
              .Where(t => t.App.Moniker == moniker)
              .OrderByDescending(t => t.Timestamp);

            return await query.ToArrayAsync();
        }

        public async Task<Log[]> GetLogsByDateAsync(DateTime date)
        {
            IQueryable<Log> query = _context.Logs;

            var startDate = date.Date;
            var endDate = date.Date.AddDays(1);

            query = query
              .Where(t => t.Timestamp >= startDate && t.Timestamp < endDate)
              .OrderBy(t => t.Timestamp);

            return await query.ToArrayAsync();
        }
    }
}
