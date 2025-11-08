namespace UserRequestsKafkaGenerator.Models;

public sealed record RequestSchedule(int UserId, string Endpoint, int Rpm);