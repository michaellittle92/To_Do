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


//GET /todos

todoRoute.MapGet(string.Empty, () =>
{
    return Results.Ok(todos);
});

//GET /todos/{id}

todoRoute.MapGet("{id:int}", (int id) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);
    if (todo == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(todo);
});

//POST /todos

todoRoute.MapPost(String.Empty, (Todo todo) =>
{
    todo.Id = todos.Max(e => e.Id) + 1; //  not using a database, manually assign an ID
    todos.Add(todo);
    return Results.Created($"/api/todos/{todo.Id}", todos);
});

//PUT /todos/{id}

//DELETE /todos/{id}

app.UseHttpsRedirection();

app.Run();