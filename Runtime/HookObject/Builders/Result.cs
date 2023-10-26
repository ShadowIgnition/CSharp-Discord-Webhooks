public record Result<TMessage>
{
    public static implicit operator Result<TMessage>(TMessage result) => new(result);
    public static implicit operator TMessage(Result<TMessage> result) => result.Message;
    public static Result<TMessage> Success { get { return m_Success ??= new(); } }
    static Result<TMessage> m_Success;

    public bool Failed { get; }
    public bool Succeeded => !Failed;
    public TMessage Message { get; }

    Result()
    {
        Failed = false;
        Message = default;
    }

    Result(TMessage message)
    {
        Failed = true;
        Message = message;
    }
}