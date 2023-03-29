namespace Events;

public abstract class TracingEventBase
{
    public Dictionary<string, object> Headers { get; set; } = new();
}