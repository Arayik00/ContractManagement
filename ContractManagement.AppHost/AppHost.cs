var builder = DistributedApplication.CreateBuilder(args);

var apiServer = builder.AddProject<Projects.ContractManagement_ApiServer>("apiserver")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.ContractManagement_UI>("Front")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiServer)
    .WaitFor(apiServer); ;

builder.Build().Run();
