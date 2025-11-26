namespace To_Do_Api;

public class CreateTodoRequest
{
    public int Id  { get; set; }
    public required string Text { get; set; }
    public bool IsDone  { get; set; }
}

public class GetTodoResponse
{
    public required string Text { get; set; }
    public bool IsDone  { get; set; }
}

public class UpdateTodoRequest
{
    public int Id  { get; set; }
    public required string Text { get; set; }
}