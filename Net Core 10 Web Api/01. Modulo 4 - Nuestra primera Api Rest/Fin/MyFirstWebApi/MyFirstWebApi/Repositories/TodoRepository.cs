using MyFirstWebApi.Models;

using System.Xml.Linq;

namespace MyFirstWebApi.Repositories
{
    public class TodoRepository
    {
        private readonly List<TodoItem> _items = new();
        private int _nextId = 1;

        public IEnumerable<TodoItem> GetAll() => _items;

        public TodoItem? Get(int id) => _items.FirstOrDefault(x => x.Id == id);

        public TodoItem Create(TodoItem item)
        {
            item.Id = _nextId++;
            _items.Add(item);
            return item;
        }

        public bool Update(int id, TodoItem updated)
        {
            var existing = Get(id);
            if (existing == null) return false;

            existing.Title = updated.Title;
            existing.IsCompleted = updated.IsCompleted;
            return true;
        }

        public bool Delete(int id)
        {
            var existing = Get(id);
            if (existing == null) return false;

            _items.Remove(existing);
            return true;
        }
    }
}
