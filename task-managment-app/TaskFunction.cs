using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using task_managment_app.Models;
using task_managment_app.Services;

namespace task_managment_app
{
    public class TaskFunction
    {
        private readonly ITaskService _taskService;
        string functionKey = null;

        public TaskFunction(ITaskService taskService)
        {
            _taskService = taskService;
               
            
            functionKey = LoadFunctionKey();
        }
        private  string LoadFunctionKey()
        {
            var json = File.ReadAllText("local.settings.json");
            var config = JsonConvert.DeserializeObject<FunctionSettings>(json);
            return config.FunctionKeys.TaskFunctionKey;
        }
      
        [FunctionName("TaskFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", "delete", Route = "tasks/{id?}")] HttpRequest req,
            ILogger log,
            string id)
        {

            log.LogInformation($"HTTP trigger function received a {req.Method} request.");
            // Validate the key from request
            string requestKey = req.Headers["code"];
            if (string.IsNullOrEmpty(requestKey) || requestKey != functionKey)
            {
                return new UnauthorizedResult();
            }
            switch (req.Method)
            {
                case "GET":
                    return GetTasks(id);

                case "POST":
                    return await CreateTask(req);

                case "PUT":
                    return await UpdateTask(req, id);

                case "DELETE":
                    return DeleteTask(id);

                default:
                    return new BadRequestObjectResult("Unsupported HTTP method.");
            }
        }

        private IActionResult GetTasks(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new OkObjectResult(_taskService.GetTasks());
            }

            var task = _taskService.GetTaskById(id);
            if (task == null)
            {
                return new NotFoundObjectResult("Task not found.");
            }
            return new OkObjectResult(task);
        }

        private async Task<IActionResult> CreateTask(HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var newTask = JsonConvert.DeserializeObject<TaskItem>(requestBody);

            if (newTask == null || string.IsNullOrEmpty(newTask.Title))
            {
                return new BadRequestObjectResult("Invalid task data.");
            }

            _taskService.AddTask(newTask);
            return new OkObjectResult(newTask);
        }

        private async Task<IActionResult> UpdateTask(HttpRequest req, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new BadRequestObjectResult("Task ID is required.");
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updatedTask = JsonConvert.DeserializeObject<TaskItem>(requestBody);

            if (updatedTask == null || string.IsNullOrEmpty(updatedTask.Title))
            {
                return new BadRequestObjectResult("Invalid task data.");
            }

            var success = _taskService.UpdateTask(id, updatedTask);
            if (!success)
            {
                return new NotFoundObjectResult("Task not found.");
            }

            return new OkObjectResult(updatedTask);
        }

        private IActionResult DeleteTask(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new BadRequestObjectResult("Task ID is required.");
            }

            var success = _taskService.DeleteTask(id);
            if (!success)
            {
                return new NotFoundObjectResult("Task not found.");
            }

            return new OkObjectResult($"Task with ID {id} deleted successfully.");
        }
    }
}
