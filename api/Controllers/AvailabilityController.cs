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
        private readonly ILogger<AvailabilityController> _logger;

        public AvailabilityController(IAvailabilityRepo availabilityRepo, UserManager<User> userManager, 
                                      ILogger<AvailabilityController> logger)
        {
            _availabilityRepo = availabilityRepo;
            _userManager = userManager;
            _logger = logger;
        }

        // HTTP GET functions

        // method that retreives Worker's availability for a week
        [HttpGet("getWeeksAvailability")]
        [Authorize(Roles="Worker")]
        public async Task<IActionResult> getWeeksAvailability([FromQuery] string userId, [FromQuery] DateOnly monday)
        {
            try {
                // list of week's Availability
                var weeksAvailability = new List<Availability>();
                
                // retreives list of Worker's availability where Date = null
                var (doWAvailability, getDoWStatus) = await _availabilityRepo.getWeeksDoWAvailability(userId);
                // In case getWeeksDoWAvailability() did not succeed
                if (getDoWStatus == OperationStatus.Error)
                {
                    _logger.LogError("[AvailabilityController] Error from getWeeksAvailability(): \n" +
                                    "Could not retreive Availability with getWeeksDoWAvailability() " + 
                                    "from AvailabilityRepo.");
                    return StatusCode(500, "Something went wrong when retreiving Week's DoW Availability");
                }

                var sunday = monday.AddDays(6);
                // retreives list of Worker's availability where Date != null and between monday and sunday
                var (dateAvailability, getDateStatus) = await _availabilityRepo
                    .getWeeksDateAvailability(userId, monday, sunday);
                // In case getWeeksDateAvailability() did not succeed
                if (getDateStatus == OperationStatus.Error)
                {
                    _logger.LogError("[AvailabilityController] Error from getWeeksAvailability(): \n" +
                                    "Could not retreive Availability with getWeeksDateAvailability() " + 
                                    "from AvailabilityRepo.");
                    return StatusCode(500, "Something went wrong when retreiving Week's Date Availability");
                }

                // converts retreived Availability to AvaillibilityDTOs
                weeksAvailability.AddRange(doWAvailability);
                weeksAvailability.AddRange(dateAvailability);
                var weeksAvailabilityDTOs = weeksAvailability.Select(a => new AvailabilityDTO
                {
                    AvailabilityId = a.AvailabilityId,
                    From = a.From,
                    To = a.To,
                    DayOfWeek = a.DayOfWeek,
                    Date = a.Date,
                    UserId = userId,
                });

                return Ok(weeksAvailabilityDTOs);
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

        // method for retreiving AvailabilityIds from range of Availability 
        // The range of Availability is retreived using given DayOfWeek and From properties
        [HttpPost("getAvailabilityIdsByDoW")]
        [Authorize(Roles="Worker")]
        public async Task<IActionResult> 
            getAvailabilityIdsByDoW([FromQuery] DayOfWeek dayOfWeek,[FromQuery] TimeOnly from)
        {
            try
            {
                // retreives list of Availability
                var (availabilityRange, status) = await _availabilityRepo
                    .getAvailabilityByDoW(dayOfWeek, from);
                // In case getAvailabilityByDoW() did not succeed
                if (status == OperationStatus.Error)
                {
                    _logger.LogError("[AvailabilityController] Error from getAvailabilityByDoW(): \n" +
                                    "Could not retreive Availability with getAvailabilityByDoW() " + 
                                    "from AvailabilityRepo.");
                    return StatusCode(500, "Something went wrong when retreiving Availability");
                }

                // retreives all AvailabilityIds from availabilityRange
                var availabilityIds = availabilityRange.Select(a => a.AvailabilityId);

                return Ok(availabilityIds);
            }
            catch (Exception e) // In case of unexpected exception
            {
                _logger.LogError("[AvailabilityController] Error from getAvailabilityIdsByDoW(): \n" +
                                 "Something went wrong when trying to retreive AvailabilityIds from " + 
                                $"range of Availability where DayOfWeek = {dayOfWeek} and From = {from}, " +
                                $"Error message: {e}");
                return StatusCode(500, "Internal server error");
            }
        }



        // HTTP POST functions

        // method that creates new Availability and calls function to add it into table
        [HttpPost("createAvailability")]
        [Authorize(Roles="Worker")]
        public async Task<IActionResult> createAvailability([FromBody] AvailabilityDTO availabilityDTO)
        {
            try {
                // retreives Worker and adds it into availabilityDTO
                var userId = availabilityDTO.UserId;
                var worker = await _userManager.FindByIdAsync(userId);
                
                // creates new Availability using availabilityDTO and worker
                var availability = new Availability
                {
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
                                     "Could not create Availability with createAvailability() " + 
                                     "from AvailabilityRepo.");
                    return StatusCode(500, "Something went wrong when creating Availability");
                }
                return Ok(new { Message = "Availability has been created" });

            }
            catch (Exception e) // In case of unexpected exception
            {
                _logger.LogError("[AvailabilityController] Error from createAvailability(): \n" +
                                 "Something went wrong when trying to create new Availability " +
                                $"with AvailabilityDTO {@availabilityDTO}, Error message: {e}");
                return StatusCode(500, "Internal server error");
            }
        }


        // HTTP DELETE FUNCTIONS

        // method that deletes Availability from table by AvailabilityId
        [HttpPost("deleteAvailability/{availabilityId}")]
        [Authorize(Roles="Worker")]
        public async Task<IActionResult> deleteAvailability(int availabilityId)
        {
            try
            {
                // retreives Availability that should be deleted
                var (availability, getStatus) = await _availabilityRepo.getAvailabilityById(availabilityId);
                // In case getAvailabilityById() did not succeed
                if (getStatus == OperationStatus.Error || availability == null)
                {
                    _logger.LogError("[AvailabilityController] Error from deleteAvailability(): \n" +
                                     "Could not retreive Availability with getAvailabilityById() " + 
                                     "from AvailabilityRepo.");
                    return StatusCode(500, "Something went wrong when retreiving Availability");
                }

                // deletes availability from table
                var deleteStatus = await _availabilityRepo.deleteAvailability(availability);
                // In case deleteAvailability() did not succeed
                if (deleteStatus == OperationStatus.Error)
                {
                    _logger.LogError("[AvailabilityController] Error from deleteAvailability(): \n" +
                                     "Could not delete Availability with deleteAvailability() " + 
                                     "from AvailabilityRepo.");
                    return StatusCode(500, "Something went wrong when deleting Availability");
                }
                
                return Ok(new { Message = "Availability has been deleted" });
            }
            catch (Exception e) // In case of unexpected exception
            {
                _logger.LogError("[AvailabilityController] Error from deleteAvailability(): \n" +
                                 "Something went wrong when trying to delete Availability " +
                                $"with AvailabilityId = {availabilityId}, Error message: {e}");
                return StatusCode(500, "Internal server error");
            }
        }

        // method that deletes range of Availability from table with list of AvailabilityId
        [HttpDelete("deleteAvailabilityByIds")]
        [Authorize(Roles="Worker")]
        public async Task<IActionResult> deleteAvailabilityByIds([FromQuery] int[] availabilityIds)
        {
            try
            {
                // retreives range of Availability that should be deleted
                var (availabilityRange, getStatus) = 
                    await _availabilityRepo.getAvailabilityByIds(availabilityIds);
                // In case getAvailabilityByIds() did not succeed
                if (getStatus == OperationStatus.Error)
                {
                    _logger.LogError("[AvailabilityController] Error from deleteAvailabilityByIds(): \n" +
                                     "Could not retreive Availability with getAvailabilityByIds() " + 
                                     "from AvailabilityRepo.");
                    return StatusCode(500, "Something went wrong when retreiving Availability");
                }

                // deletes availabilityRange from table
                var deleteStatus = await _availabilityRepo.deleteAvailabilityRange(availabilityRange);
                // In case deleteAvailabilityRange() did not succeed
                if (deleteStatus == OperationStatus.Error)
                {
                    _logger.LogError("[AvailabilityController] Error from deleteAvailabilityByIds(): \n" +
                                     "Could not delete Availability with deleteAvailabilityRange() " + 
                                     "from AvailabilityRepo.");
                    return StatusCode(500, "Something went wrong when deleting Availability");
                }
                
                return Ok(new { Message = "Availability has been deleted" });
            }
            catch (Exception e) // In case of unexpected exception
            {
                // makes string listing all AvailabilityIds
                var availabilityIdsString = String.Join(", ", availabilityIds);

                _logger.LogError("[AvailabilityController] Error from deleteAvailabilityByIds(): \n" +
                                 "Something went wrong when trying to delete range of Availability " +
                                $"with AvailabilityIds {availabilityIdsString}, Error message: {e}");
                return StatusCode(500, "Internal server error");
            }
        }

        // method that deletes range of Availability from table with given DayOfWeek and From properties
        [HttpDelete("deleteAvailabilityByDoW")]
        [Authorize(Roles="Worker")]
        public async Task<IActionResult> 
            deleteAvailabilityByDoW([FromQuery] DayOfWeek dayOfWeek, [FromQuery] TimeOnly from)
        {
            try
            {
                // retreives range of Availability that should be deleted
                var (availabilityRange, getStatus) = 
                    await _availabilityRepo.getAvailabilityByDoW(dayOfWeek, from);
                // In case getAvailabilityByDoW() did not succeed
                if (getStatus == OperationStatus.Error)
                {
                    _logger.LogError("[AvailabilityController] Error from deleteAvailabilityByDoW(): \n" +
                                     "Could not retreive Availability with getAvailabilityByDoW() " + 
                                     "from AvailabilityRepo.");
                    return StatusCode(500, "Something went wrong when retreiving Availability");
                }

                // deletes availabilityRange from table
                var deleteStatus = await _availabilityRepo.deleteAvailabilityRange(availabilityRange);
                // In case deleteAvailabilityRange() did not succeed
                if (deleteStatus == OperationStatus.Error)
                {
                    _logger.LogError("[AvailabilityController] Error from deleteAvailabilityByDoW(): \n" +
                                     "Could not delete Availability with deleteAvailabilityRange() " + 
                                     "from AvailabilityRepo.");
                    return StatusCode(500, "Something went wrong when deleting Availability");
                }
                
                return Ok(new { Message = "Availability has been deleted" });
            }
            catch (Exception e) // In case of unexpected exception
            {
                _logger.LogError("[AvailabilityController] Error from deleteAvailabilityByDoW(): \n" +
                                 "Something went wrong when trying to delete range of Availability " + 
                                $"where DayOfWeek = {dayOfWeek} and From = {from}, Error message: {e}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
