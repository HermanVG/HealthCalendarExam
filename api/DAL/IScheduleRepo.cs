using HealthCalendar.Models;
using HealthCalendar.Shared;

namespace HealthCalendar.DAL;

public interface IScheduleRepo
{
    Task<(List<Schedule>, OperationStatus)> getSchedulesByAvailabilityId(int availabilityId);
    Task<(List<Schedule>, OperationStatus)> getSchedulesByAvailabilityIds(int[] availabilityIds);
    Task<(List<Schedule>, OperationStatus)> getSchedulesByEventId(int eventId);
    Task<OperationStatus> updateSchedules(List<Schedule> schedules);
    Task<OperationStatus> deleteSchedules(List<Schedule> schedules);
}