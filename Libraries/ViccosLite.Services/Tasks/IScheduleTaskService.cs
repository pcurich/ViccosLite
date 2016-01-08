using System.Collections.Generic;
using ViccosLite.Core.Domain.Task;

namespace ViccosLite.Services.Tasks
{
    public interface IScheduleTaskService
    {
        void DeleteTask(ScheduleTask task);
        ScheduleTask GetTaskById(int taskId);
        ScheduleTask GetTaskByType(string type);
        IList<ScheduleTask> GetAllTasks(bool showHidden = false);
        void InsertTask(ScheduleTask task);
        void UpdateTask(ScheduleTask task);
    }
}