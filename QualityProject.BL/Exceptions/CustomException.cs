namespace QualityProject.BL.Exceptions;

public class CustomException : Exception
{
    public int ErrorCode { get; set; }
    
    public CustomException(string message) : base(message)
    {
        ErrorCode = 500;
    }
    
    public CustomException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
    
    public CustomException(string message, int errorCode, Exception innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}