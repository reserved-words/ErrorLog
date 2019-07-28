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
        Task<App> GetAppAsync(int appId, bool includeLogs = false);
        //Task<App[]> GetAllAppsBy(DateTime dateTime, bool includeLogs = false);

        // Logs
        void AddLog(Log log);
        void DeleteLog(Log log);
        Task<Log> GetLogByAppAsync(int appId, int logId, bool includeAppDetails = false);
        Task<Log[]> GetLogsByAppAsync(int appId, bool includeAppDetails = false);

    }
}