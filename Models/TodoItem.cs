// Models/TodoItem.cs
namespace DotnetMongoDocker.Models
{
    public class TodoItem
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool IsComplete { get; set; }
    }
}