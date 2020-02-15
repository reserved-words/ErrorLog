using PreDeploymentTools;
using System;

namespace ErrorLog.PreDeployment
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
                throw new Exception("Incorrect number of arguments");

            var appName = args[0];
            var domainName = args[1];

            var preDeploymentService = new PreDeploymentService(appName, domainName);
            preDeploymentService.CreateApi();
        }
    }
}
