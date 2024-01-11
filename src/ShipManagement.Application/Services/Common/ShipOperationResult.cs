namespace ShipManagement.Application.Services.ShipServices;
public class ShipOperationResult
{
    public bool Success { get; }
    public string Message { get; }

    private ShipOperationResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public static ShipOperationResult SuccessResult(string message = "Operation completed successfully")
    {
        return new ShipOperationResult(true, message);
    }

    public static ShipOperationResult FailureResult(string message)
    {
        return new ShipOperationResult(false, message);
    }
}