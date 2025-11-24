namespace UserRequestsKafkaGenerator.Models;

public readonly struct UserRequest : IEquatable<UserRequest>
{
    public int UserId { get; }
    public string Endpoint { get; }

    public UserRequest(int userId, string endpoint)
    {
        UserId = userId;
        Endpoint = endpoint;
    }

    public bool Equals(UserRequest other)
    {
        return UserId == other.UserId && string.Equals(Endpoint, other.Endpoint, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj)
        => obj is UserRequest other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(UserId, Endpoint);

    public static bool operator ==(UserRequest left, UserRequest right)
        => left.Equals(right);

    public static bool operator !=(UserRequest left, UserRequest right)
        => !(left == right);
}