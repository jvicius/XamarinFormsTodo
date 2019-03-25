using DemoApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoApp.Services
{
    public interface ITodoService
    {
        Task<List<TodoModel>> GetAll();
        Task<bool> AddTodo(TodoModel todo);
        Task<bool> UpdateTodo(TodoModel todo);
        Task<bool> Delete(TodoModel todo);
        Task<bool> UpdateEstus(string id, bool estatus);
    }
}
