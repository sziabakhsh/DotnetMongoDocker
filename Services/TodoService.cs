// Services/TodoService.cs
using MongoDB.Driver;
using DotnetMongoDocker.Models;

namespace DotnetMongoDocker.Services
{
    public class TodoService
    {
        private readonly IMongoCollection<TodoItem> _todos;

        public TodoService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDb"));
            var database = client.GetDatabase("TodoDb");
            _todos = database.GetCollection<TodoItem>("Todos");
        }

        public List<TodoItem> Get() => _todos.Find(todo => true).ToList();
        public TodoItem Create(TodoItem todo)
        {
            _todos.InsertOne(todo);
            return todo;
        }
    }
}