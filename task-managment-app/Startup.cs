using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using task_managment_app.Services; 
[assembly: FunctionsStartup(typeof(task_managment_app.Startup))]

namespace task_managment_app
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Register your services here
            builder.Services.AddSingleton<ITaskService, TaskService>();

            // Add other services if needed
            // builder.Services.AddScoped<IOtherService, OtherService>();
        }
    }
}
