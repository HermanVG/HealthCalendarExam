using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HealthCalendar.DAL;
using HealthCalendar.DTOs;
using HealthCalendar.Models;
using HealthCalendar.Shared;

namespace HealthCalendar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityRepo _availabilityRepo;
        private readonly ILogger<AuthController> _logger;

        public AvailabilityController(IAvailabilityRepo availabilityRepo, ILogger<AuthController> logger)
        {
            _availabilityRepo = availabilityRepo;
            _logger = logger;
        }

        
    }
}
