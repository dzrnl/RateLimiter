namespace UserRequestsKafkaGenerator;

public class ScheduleRequest
{
    public int UserId { get; set; }
    public string Endpoint { get; set; } = string.Empty;
    public int Rpm { get; set; }
}