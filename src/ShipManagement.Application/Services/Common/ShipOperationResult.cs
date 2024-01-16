namespace ShipManagement.Application.Services.ShipServices;
public class ShipOperationResult
{
    public int StatusCode { get; }
    public bool Success { get; }
    public string Message { get; }

    private ShipOperationResult(int statusCode, bool success, string message)
    {
        StatusCode = statusCode;
        Success = success;
        Message = message;
    }

    public static ShipOperationResult SuccessResult(int statusCode, string message = "Operation completed successfully")
    {
        return new ShipOperationResult(statusCode, true, message);
    }

    public static ShipOperationResult FailureResult(int statusCode, string message)
    {
        return new ShipOperationResult(statusCode, false, message);
    }
}