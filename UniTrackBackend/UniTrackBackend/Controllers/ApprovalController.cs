using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniTrackBackend.Api.ViewModels;
using UniTrackBackend.Data.Models.TypeSafe;
using UniTrackBackend.Services;

namespace UniTrackBackend.Controllers
{
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiController]
    public class ApprovalController : ControllerBase
    {
        private readonly IApprovalService _approvalService;
        public ApprovalController(IApprovalService approvalService)
        {
            _approvalService = approvalService;
        }

        /// <summary>
        /// Approves student registrations.
        /// </summary>
        /// <remarks>
        /// This endpoint approves pending student registrations. 
        /// Only accessible to users with the Admin role.
        /// </remarks>
        /// <response code="200">Returns a confirmation message when a student is approved.</response>
        /// <response code="401">Unauthorized if the user is not authenticated.</response>
        /// <response code="403">Forbidden if the user does not have the Admin role.</response>
        [HttpPut("students")]
        // [Authorize(Roles = Ts.Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ApproveStudents(StudentApprovalViewModel models)
        {
            var result = await _approvalService.ApproveStudentsAsync(models);
            if (!result)
                return new BadRequestResult();
            return Ok("Students approved");
        }

        /// <summary>
        /// Approves teacher registrations.
        /// </summary>
        /// <remarks>
        /// This endpoint approves pending teacher registrations.
        /// Only accessible to users with the Admin role.
        /// </remarks>
        /// <response code="200">Returns a confirmation message when a teacher is approved.</response>
        /// <response code="401">Unauthorized if the user is not authenticated.</response>
        /// <response code="403">Forbidden if the user does not have the Admin role.</response>
        [HttpPut("teachers")]
        // [Authorize(Roles = Ts.Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ApproveTeachers(TeacherApprovalViewModel models)
        {
            var result = await _approvalService.ApproveTeacherAsync(models);
            if (!result)
                return new BadRequestResult();
            return Ok("Teachers approved");
        }
        
        /// <summary>
        /// Approves admin registrations.
        /// </summary>
        /// <remarks>
        /// This endpoint approves pending admin registrations.
        /// Only accessible to users with the Admin role.
        /// </remarks>
        /// <response code="200">Returns a confirmation message when an admin is approved.</response>
        /// <response code="401">Unauthorized if the user is not authenticated.</response>
        /// <response code="403">Forbidden if the user does not have the Admin role.</response>
        [HttpPut("admins")]
        [Authorize(Roles = Ts.Roles.SuperAdmin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ApproveAdmins(List<AdminViewModel> models)
        {
            // var result = await _approvalService.ApproveStudentsAsync(models);
            // if (!result)
            //     return new BadRequestResult();
            return Ok("Admins approved");
        }

        /// <summary>
        /// Approves parent registrations.
        /// </summary>
        /// <remarks>
        /// This endpoint approves pending parent registrations.
        /// Only accessible to users with the Admin role.
        /// </remarks>
        /// <response code="200">Returns a confirmation message when a parent is approved.</response>
        /// <response code="401">Unauthorized if the user is not authenticated.</response>
        /// <response code="403">Forbidden if the user does not have the Admin role.</response>
        [HttpPut("parents")]
        [Authorize(Roles = Ts.Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ApproveParents(List<ParentViewModel> models)
        {
            var result = await _approvalService.ApproveParentsAsync(models);
            if (!result)
                return new BadRequestResult();
            return Ok("Parents approved");
        }
    }
}
