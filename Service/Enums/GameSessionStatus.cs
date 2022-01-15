namespace TestWebApi.Service.Enums
{
    public enum GameSessionStatus
    {
        Started, 
        Stopped, 
        NotFound,
        IsActiveAlready,
        IsNotActiveAlready,
        CustomerUnknown,
        GameUnknown
    }
}