using SE.Neo.Common.Enums;

namespace SE.Neo.WebAPI.Models.Shared;

public class LogRequest
{
    public string? Message { get; set; }

    public LoggerSourceType Source { get; set; }

    public LogLevel Level { get; set; }

    public ClientError? Error { get; set; }

    public object[]? ExtraInfo { get; set; }

    public DateTimeOffset DateTimeUtc { get; set; }
}