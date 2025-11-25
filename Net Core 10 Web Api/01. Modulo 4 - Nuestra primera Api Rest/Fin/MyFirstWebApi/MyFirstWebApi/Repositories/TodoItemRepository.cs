using MyFirstWebApi.Models;

namespace MyFirstWebApi.Repositories
{
    public class TodoItemRepository
    {
        private readonly List<TodoItem> _todoItems = new();
        private int _nextId = 1;

        public IEnumerable<TodoItem> GetAll()
        {
            return _todoItems;
        }

        public TodoItem? Get(int id) =>
             _todoItems.FirstOrDefault(item => item.Id == id);

        public TodoItem Create(TodoItem item)
        {
            item.Id = _nextId++;
            _todoItems.Add(item);
            return item;
        }

        public bool Update(int id, TodoItem item)
        {
            var existingItem = Get(id);
            if (existingItem == null)
                return false;

            existingItem?.Title = item.Title;
            existingItem?.IsCompleted = item.IsCompleted;

            return true;
        }

        public bool Delete(int id)
        {
            var item = Get(id);
            if (item == null)
                return false;

            _todoItems.Remove(item);

            return true;
        }
    }
}
