namespace UserRequestsKafkaGenerator.Models;

public readonly record struct UserRequest(int UserId, string Endpoint);