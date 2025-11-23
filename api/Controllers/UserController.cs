using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HealthCalendar.DTOs;
using HealthCalendar.Models;

namespace HealthCalendar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<User> userManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        // HTTP GET functions

        // Retrieves Users related to Worker with given Id
        [HttpGet("getUsersByWorkerId")]
        [Authorize(Roles="Worker")]
        public async Task<IActionResult> getUsersByWorkerId([FromQuery] string workerId)
        {
            try
            {
                // retreives list of Users with "Patient" role
                // converts list into UserDTOs
                var userDTOs = _userManager.Users
                    .Where(u => u.WorkerId == workerId)
                    .Select(u => new UserDTO
                    {
                        Id = u.Id,
                        UserName = u.UserName!,
                        Name = u.Name,
                        Role = u.Role,
                        WorkerId = u.WorkerId
                    });
                return Ok(userDTOs);
            }
            catch (Exception e) // In case of unexpected exception
            {
                _logger.LogError("[UserController] Error from getIdsByWorkerId(): \n" +
                                 "Something went wrong when trying to retreive Ids from " + 
                                $"Patients where WorkerId = {workerId}, Error message: {e}");
                return StatusCode(500, "Internal server error");
            }
        }


    }
}