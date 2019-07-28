using ErrorLog.Data;
using System;
using System.Threading.Tasks;

namespace ErrorLog.Data
{
    public interface ILogRepository
    {
        // General 
        Task<bool> SaveChangesAsync();

        // Apps
        void AddApp(App app);
        void DeleteApp(App app);
        Task<App[]> GetAllAppsAsync(bool includeLogs = false);
        Task<App> GetAppAsync(string moniker, bool includeLogs = false);
        //Task<App[]> GetAllAppsBy(DateTime dateTime, bool includeLogs = false);

        // Logs
        void AddLog(Log log);
        void DeleteLog(Log log);
        Task<Log[]> GetLogsAsync(bool includeAppDetails = false);
        Task<Log> GetLogByAppAsync(string moniker, int logId, bool includeAppDetails = false);
        Task<Log[]> GetLogsByAppAsync(string moniker, bool includeAppDetails = false);
        Task<Log[]> GetLogsByDateAsync(DateTime date);

    }
}