using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        
        // userManager used to retreive Users related to Availability upon creation
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthController> _logger;

        public AvailabilityController(IAvailabilityRepo availabilityRepo, UserManager<User> userManager, 
                                      ILogger<AuthController> logger)
        {
            _availabilityRepo = availabilityRepo;
            _userManager = userManager;
            _logger = logger;
        }

        // HTTP GET functions

        // method that retreives Worker's availability for a week
        [HttpGet("getAvailabilityForWeek")]
        [Authorize(Roles="Worker")]
        public async Task<IActionResult> getWeeksAvailability(string userId, DateOnly monday)
        {
            try {
                // list of week's Availability
                var weeksAvailability = new List<Availability>();
                
                // retreives list of Worker's availability where Date = null
                var (doWAvailability, status1) = await _availabilityRepo.getWeeksDoWAvailability(userId);
                // In case getWeeksDoWAvailability() did not succeed
                if (status1 == OperationStatus.Error)
                {
                    _logger.LogError("[AvailabilityController] Error from createAvailability(): \n" +
                                    "Something went wrong with getWeeksDoWAvailability() " + 
                                    "from AvailabilityRepo.");
                    return StatusCode(500, "Something went wrong when retreiving Week's DoW Availability");
                }

                var sunday = monday.AddDays(6);
                // retreives list of Worker's availability where Date != null and between monday and sunday
                var (dateAvailability, status2) = await _availabilityRepo
                    .getWeeksDateAvailability(userId, monday, sunday);
                // In case getWeeksDateAvailability() did not succeed
                if (status2 == OperationStatus.Error)
                {
                    _logger.LogError("[AvailabilityController] Error from createAvailability(): \n" +
                                    "Something went wrong with getWeeksDateAvailability() " + 
                                    "from AvailabilityRepo.");
                    return StatusCode(500, "Something went wrong when retreiving Week's Date Availability");
                }

                weeksAvailability.AddRange(doWAvailability);
                weeksAvailability.AddRange(dateAvailability);
                return Ok(weeksAvailability);
            }
            catch (Exception e) // In case of unexpected exception
            {
                _logger.LogError("[AvailabilityController] Error from getWeeksAvailability(): \n" +
                                 "Something went wrong when trying to retreive week's availability from " + 
                                $"Worker with UserId = {userId} where monday is on the {monday}, " +
                                $"Error message: {e}");
                return StatusCode(500, "Internal server error");
            }
        }



        // HTTP POST functions

        // method that creates new Availability and calls function to add it into database
        [HttpPost("createAvailability")]
        [Authorize(Roles="Worker")]
        public async Task<IActionResult> createAvailability(AvailabilityDTO availabilityDTO)
        {
            try {
                // retreives Worker related to new Availability
                var userId = availabilityDTO.UserId;
                var worker = await _userManager.FindByIdAsync(userId);
                
                // creates new Availability using availabilityDTO and worker
                var availability = new Availability
                {
                    AvailabilityId = availabilityDTO.AvailabilityId,
                    From = availabilityDTO.From,
                    To = availabilityDTO.To,
                    DayOfWeek = availabilityDTO.DayOfWeek,
                    Date = availabilityDTO.Date,
                    UserId = userId,
                    Worker = worker!
                };
                var status = await _availabilityRepo.createAvailability(availability);

                // In case createAvailability() did not succeed
                if (status == OperationStatus.Error)
                {
                    _logger.LogError("[AvailabilityController] Error from createAvailability(): \n" +
                                     "Something went wrong with createAvailability() " + 
                                     "from AvailabilityRepo.");
                    return StatusCode(500, "Something went wrong when creating Availability");
                }
                return Ok(new { Message = "Availability has been created" });

            }
            catch (Exception e) // In case of unexpected exception
            {
                _logger.LogError("[AvailabilityController] Error from createAvailability(): \n" +
                                 "Something went wrong when trying to create new Availability, " +
                                $"with AvailabilityDTO {@availabilityDTO} Error message: {e}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
