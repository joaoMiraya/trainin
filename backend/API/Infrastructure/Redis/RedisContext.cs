using StackExchange.Redis;

namespace API.Infrastructure.Redis;

public class RedisContext
{
    private readonly Lazy<ConnectionMultiplexer> _lazyConnection;

    public RedisContext(string connectionString)
    {
        _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            ConnectionMultiplexer.Connect(connectionString));
    }

    public ConnectionMultiplexer Connection => _lazyConnection.Value;
    public IDatabase Database => Connection.GetDatabase();
}
