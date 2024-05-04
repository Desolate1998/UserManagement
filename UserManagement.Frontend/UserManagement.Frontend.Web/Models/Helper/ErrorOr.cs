namespace UserManagement.Frontend.Web.Models.Helper;

public class ErrorOr<T>
{
    public bool IsError { get; set; }
    public List<Error>? Errors { get; set; }
    public T? Value { get; set; }
}
