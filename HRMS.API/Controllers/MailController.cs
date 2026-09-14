using HRMS.Application.DTOs.Mail;
using HRMS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }
        [HttpGet("{threadId}")]
        [Authorize]
        public async Task<IActionResult> GetById(int threadId)
        {
            var mailThread = await _mailService.GetThreadByIdAsync(threadId);
            return mailThread == null ? NotFound() : Ok(mailThread);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyThreads()
        {
            var threads = await _mailService.GetMyThreadsAsync();
            return Ok(threads);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateThread([FromBody]CreateMailThreadDto dto)
        {
            var thread = await _mailService.CreateThreadAsync(dto);
            return CreatedAtAction(nameof(GetById), new { threadId = thread.Id }, thread);
        }
        [HttpPost("{threadId}/reply")]
        [Authorize]
        public async Task<IActionResult> Reply(int threadId, [FromBody] SendMailMessageDto dto)
        {
            var message = await _mailService.ReplyAsync(threadId, dto);
            return Ok(message);
        }
    }
}
