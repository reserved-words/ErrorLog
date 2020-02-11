using System;

namespace DatabaseMigrater
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 3)
                    throw new Exception("Incorrect numer of arguments");

                var connectionString = args[0];
                var databaseName = args[1];
                var appUser = args[2];

                CreateSchema.Run(connectionString);
                CreateUsers.Run(connectionString, databaseName, appUser);
            }
            catch (Exception exc)
            {
                Log(exc, args);
            }
        }

        private static void Log(Exception exc, string[] args)
        {
            var logFolder = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ReservedWords",
                "ErrorLog");

            System.IO.Directory.CreateDirectory(logFolder);

            var str = new System.Text.StringBuilder();

            str.Append(DateTime.Now.ToString(@"dd/MM/yy HH:mm:ss"));
            str.Append(Environment.NewLine);

            while (exc != null)
            {
                str.Append(exc.Message);
                str.Append(Environment.NewLine);

                str.Append(exc.StackTrace);
                str.Append(Environment.NewLine);

                if (exc.Data != null)
                {
                    foreach (var key in exc.Data.Keys)
                    {
                        str.Append($"{key}: {exc.Data[key]}");
                        str.Append(Environment.NewLine);
                    }
                }

                exc = exc.InnerException;
            }

            foreach (var arg in args)
            {
                str.Append($"arg: {arg}");
                str.Append(Environment.NewLine);
            }

            str.Append(Environment.NewLine);
            str.Append(Environment.NewLine);

            var logFile = System.IO.Path.Combine(logFolder, "errors.log");

            System.IO.File.AppendAllText(logFile, str.ToString());
        }
    }
}
