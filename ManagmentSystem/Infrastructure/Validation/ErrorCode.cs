namespace Infrastructure.Validation;

public static class ErrorCode
{
    public static string KeyNotFound => "KeyNotFound";
    public static string Required => "Required";
    public static string MinimumLengthAllowedIs(int x) => $"MinimumLengthAllowedIs({x})";
    public static string MaximumLengthAllowedIs(int x) => $"MaximumLengthAllowedIs({x})";
}
