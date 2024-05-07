namespace Carbo.ViewModels
{
    /// <summary>
    /// Enum that represents the type of error that occurred during a request.
    /// Use this for distinguishing between request errors and socket errors.
    /// </summary>
    public enum RequestErrorType
    {
        Unknown,
        RequestError,
        SocketError,
    }
}
