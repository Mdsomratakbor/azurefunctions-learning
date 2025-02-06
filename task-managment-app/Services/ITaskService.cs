using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task_managment_app.Models;

namespace task_managment_app.Services;
public interface ITaskService
{
    List<TaskItem> GetTasks();
    TaskItem GetTaskById(string id);
    void AddTask(TaskItem task);
    bool UpdateTask(string id, TaskItem updatedTask);
    bool DeleteTask(string id);
}

