using UserRequestsKafkaGenerator.Models;

namespace UserRequestsKafkaGenerator.Configuration;

public class UserScheduleOptions
{
    public List<RequestSchedule> Schedules { get; set; } = [];
}