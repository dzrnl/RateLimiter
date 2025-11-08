namespace UserRequestsKafkaGenerator.Models;

public sealed record UserRequest(int UserId, string Endpoint);