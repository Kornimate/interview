var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.UsersApi>("users-api");

builder.Build().Run();
