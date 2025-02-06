using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task_managment_app.Models;

namespace task_managment_app.Services
{
    public class TaskService : ITaskService
    {
        private readonly List<TaskItem> _tasks = new List<TaskItem>();

        public List<TaskItem> GetTasks() => _tasks;

        public TaskItem GetTaskById(string id) => _tasks.FirstOrDefault(t => t.Id == id);

        public void AddTask(TaskItem task)
        {
            task.Id = System.Guid.NewGuid().ToString();
            _tasks.Add(task);
        }

        public bool UpdateTask(string id, TaskItem updatedTask)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return false;

            task.Title = updatedTask.Title;
            task.IsCompleted = updatedTask.IsCompleted;
            return true;
        }

        public bool DeleteTask(string id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return false;

            _tasks.Remove(task);
            return true;
        }
    }
}
