using DbUp;
using System.Reflection;


    var connectionString =
        args.FirstOrDefault()
        ?? "Server=localhost\\SQL2019; Database=PTJournal; Trusted_connection=true; TrustServerCertificate=True;";
    EnsureDatabase.For.SqlDatabase(connectionString);

    var upgrader =
        DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .WithExecutionTimeout(TimeSpan.FromSeconds(3600))
            .Build();

    var result = upgrader.PerformUpgrade();

    if (!result.Successful)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(result.Error);
        Console.ResetColor();
#if DEBUG
        Console.ReadLine();
#endif                
        return -1;
    }

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Success!");
    Console.ResetColor();
    return 0;
