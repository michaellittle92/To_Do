using System.Net.Mime;
using To_Do_Api;

//in memory db 
var todos = new List<Todo>
{
    new Todo { Id = 1, Text = "Test text 01", IsDone = false },
    new Todo { Id = 2, Text = "Test text 02", IsDone = false },
    new Todo {Id = 3, Text = "Test text 03", IsDone = false }
};

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        c.RoutePrefix = string.Empty;
    });
}


var todoRoute = app.MapGroup("/api/todos");


//GET /todos/{id}

todoRoute.MapGet("{id:int}", (int id) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);
    if (todo == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new GetTodoResponse
    {
        Text = todo.Text,
        IsDone = todo.IsDone
    });
});

//Query string
todoRoute.MapGet(string.Empty, (int? id) =>
{
    if (id.HasValue)
    {
        var todo = todos.FirstOrDefault(t => t.Id == id.Value);
        if (todo is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(new GetTodoResponse
        {
            Text = todo.Text,
            IsDone = todo.IsDone
        });
    }

    return Results.Ok(todos.Select(todo => new GetTodoResponse
    {
        Text = todo.Text,
        IsDone = todo.IsDone
    }));
});

//POST /todos

todoRoute.MapPost(String.Empty, (CreateTodoRequest todo) =>
{
    if (string.IsNullOrWhiteSpace(todo.Text))
    {
        return Results.BadRequest("Text is required");
    }
    var newTodo = new Todo

    {
        Id = todos.Max(e => e.Id) + 1,
        Text = todo.Text,
        IsDone = false
    };
    todos.Add(newTodo);
    return Results.Created($"/api/todos/{newTodo.Id}", todo);
});


//Update /Put /todos/{id}
todoRoute.MapPut("{id:int}", (UpdateTodoRequest todo, int id) =>
{
     var existingTodo = todos.FirstOrDefault(t => t.Id == id);
     if (existingTodo == null)
     {
         return Results.NotFound();
     }
     
    existingTodo.Text = todo.Text;
    
    return Results.Ok(existingTodo);
});


//DELETE /todos/{id}
todoRoute.MapDelete("{id:int}", (int id) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);
    if (todo == null)
    {return Results.NotFound();}

    todos.Remove(todo);
    return Results.NoContent();

});

app.UseHttpsRedirection();

app.Run();

public partial class Program
{
}