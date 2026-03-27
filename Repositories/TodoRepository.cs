using DotnetMongoDocker.Models;
using MongoDB.Driver;

namespace DotnetMongoDocker.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly IMongoCollection<Todo> _todos;

    public TodoRepository(IMongoClient client, IConfiguration config)
    {
        var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
        _todos = database.GetCollection<Todo>("todos");
    }

    public async Task<List<Todo>> GetAllAsync() =>
        await _todos.Find(_ => true).ToListAsync();

    public async Task<Todo?> GetByIdAsync(string id) =>
        await _todos.Find(t => t.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Todo todo) =>
        await _todos.InsertOneAsync(todo);

    public async Task UpdateAsync(string id, Todo todo) =>
        await _todos.ReplaceOneAsync(t => t.Id == id, todo);

    public async Task DeleteAsync(string id) =>
        await _todos.DeleteOneAsync(t => t.Id == id);
}