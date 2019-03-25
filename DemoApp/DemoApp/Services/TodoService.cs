using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Models;

namespace DemoApp.Services
{
    public class TodoService : ITodoService
    {
        private readonly List<TodoModel> _todoList;

        public TodoService()
        {
            _todoList = new List<TodoModel>();
        }

        public async Task<List<TodoModel>> GetAll()
        {
            //await Task.Delay(2000);
            return _todoList;
        }

        public async Task<bool> AddTodo(TodoModel todo)
        {
            try
            {
                todo.Id = Guid.NewGuid().ToString();
                await Task.Delay(2000);
                _todoList.Add(todo);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateTodo(TodoModel todo)
        {
            try
            {
                await Task.Delay(2000);
                var model = _todoList.FirstOrDefault(f => f.Id == todo.Id);
                if (model != null) model.Description = todo.Description;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Delete(TodoModel todo)
        {
            try
            {
                await Task.Delay(2000);
                _todoList.Remove(todo);
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        public async Task<bool> UpdateEstus(string id, bool estatus)
        {
            try
            {
                await Task.Delay(2000);
                var model = _todoList.FirstOrDefault(f => f.Id == id);
                if (model != null) model.IsComplete = estatus;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
