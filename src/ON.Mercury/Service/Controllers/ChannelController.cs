using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ON.Mercury.Service.Database.Repositories;
using ON.Mercury.Service.Models.Channels;
using System.Threading;
using System.Threading.Tasks;

namespace ON.Mercury.Service.Controllers
{
    
    [ApiController]
    [Route("/api/{Controller}")]
    public class ChannelController : ControllerBase
    {
        private readonly ILogger<ChannelController> _logger;
        private readonly ChannelRepository _channels;
        
        public ChannelController(ILogger<ChannelController> logger, ChannelRepository channels)
        {
            _logger = logger;
            _channels = channels;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChannelAsync([FromBody] CreateOrUpdateChannel request, CancellationToken cancellationToken = default)
        {
            var channel = await _channels.CreateChannelAsync(request.Name, request.Description, request.Category, cancellationToken);
            return Ok(channel);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetChannelsAsync(CancellationToken cancellationToken = default)
        {
            var channels = await _channels.GetChannelsAsync(cancellationToken);
            return Ok(channels);
        }

        [HttpPut("/{channelId}")]
        public async Task<IActionResult> UpdateChannelAsync(string channelId, [FromBody] CreateOrUpdateChannel request, CancellationToken cancellationToken = default)
        {
            var channel = await _channels.UpdateChannelAsync(channelId, request.Name, request.Category, request.Description, cancellationToken);
            if (channel is null) return BadRequest("Channel failed to update");
            return Ok(channel);
        }

        [HttpDelete("/{channelId}")]
        public async Task<IActionResult> DeleteChannelAsync(string channelId, CancellationToken cancellationToken = default)
        {
            var deletedChannelId = await _channels.DeleteChannelAsync(channelId, cancellationToken);
            if (string.IsNullOrEmpty(deletedChannelId)) return BadRequest("Channel failed to delete");
            return Ok($"Channel Id: {deletedChannelId}");
        }
    }
}
