using HealthCalendar.Models;
using HealthCalendar.Shared;

namespace HealthCalendar.DAL;

public interface IEventRepo
{
    Task<(Event?, OperationStatus)> getEventById(int eventId);
    Task<OperationStatus> deleteEvent(Event eventt);
}