namespace Orders.Api.Services.Dapper;

public class DapperResponse
{
    public bool Success { get; set; }
    public dynamic? Result { get; set; }
    public string? InnerException { get; set; }
}