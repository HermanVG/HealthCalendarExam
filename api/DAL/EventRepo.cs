using System;
using HealthCalendar.Models;
using HealthCalendar.Shared;
using Microsoft.EntityFrameworkCore;

namespace HealthCalendar.DAL;

public class EventRepo : IEventRepo
{
    private readonly HealthCalendarDbContext _db;
    private readonly ILogger<EventRepo> _logger;
    public EventRepo(HealthCalendarDbContext db, ILogger<EventRepo> logger)
    {
        _db = db;
        _logger = logger;
    }

    // GET FUNCTIONS:

    // method for retreiving Event by eventId
    public async Task<(Event?, OperationStatus)> getEventById(int eventId)
    {
        try
        {
            var eventt = await _db.Events.FindAsync(eventId);
            return (eventt, OperationStatus.Ok);
        }
        catch (Exception e) // In case of unexpected exception
        {
            _logger.LogError("[EventRepo] Error from getEventById(): \n" +
                             "Something went wrong when retreiving Event where " +
                            $"EventId = {eventId}, Error message: {e}");
            return (null, OperationStatus.Error);
        }
    }


    // DELETE FUNCTIONS:

    // method for deleting Event from table
    public async Task<OperationStatus> deleteEvent(Event eventt)
    {
        try 
        {
            _db.Remove(eventt);
            await _db.SaveChangesAsync();
            return OperationStatus.Ok;
        }
        catch (Exception e) // In case of unexpected exception
        {
            _logger.LogError("[EventRepo] Error from deleteEvent(): \n" +
                             "Something went wrong when deleting Event " +
                            $"{@eventt}, Error message: {e}");
            return OperationStatus.Error;
        }
    }
}
