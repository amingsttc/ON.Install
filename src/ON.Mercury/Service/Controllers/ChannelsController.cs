using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ON.Mercury.Service.Database.Repositories;
using ON.Mercury.Service.Models.Channels;
using System.Threading;
using System.Threading.Tasks;

namespace ON.Mercury.Service.Controllers
{
    
    [ApiController]
    [Route("/api/mercury/{Controller}")]
    public class ChannelsController : ControllerBase
    {
        private readonly ILogger<ChannelsController> _logger;
        private readonly ChannelRepository _channels;
        
        public ChannelsController(ILogger<ChannelsController> logger, ChannelRepository channels)
        {
            _logger = logger;
            _channels = channels;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChannelAsync([FromBody] CreateOrUpdateChannel request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Name is required");
            }
            
            var channel = await _channels.CreateChannelAsync(request.Name, request.Description, request.Category, request.Roles, cancellationToken);
            return Ok(channel);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetChannelsAsync(CancellationToken cancellationToken = default)
        {
            var channels = await _channels.GetChannelsAsync(cancellationToken);
            return Ok(channels);
        }

        [HttpPut("{channelId}")]
        public async Task<IActionResult> UpdateChannelAsync(string channelId, [FromBody] CreateOrUpdateChannel request, CancellationToken cancellationToken = default)
        {
            var channel = await _channels.UpdateChannelAsync(channelId, request.Name, request.Category, request.Description, cancellationToken);
            return Ok(channel);
        }

        [HttpDelete("{channelId}")]
        public async Task<IActionResult> DeleteChannelAsync(string channelId, CancellationToken cancellationToken = default)
        {
            var deletedChannelId = await _channels.DeleteChannelAsync(channelId, cancellationToken);
            return Ok($"Channel Id: {deletedChannelId}");
        }

        [HttpGet("{channelId}/messages")]
        public async Task<IActionResult> GetMessagesAsync(string channelId, [FromQuery] string lastReceivedId = null,CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(lastReceivedId))
            {
                var messages = await _channels.GetMessagesAsync(channelId, cancellationToken: cancellationToken);

                return Ok(messages);
            }
            else
            {
                var messages = await _channels.GetMessagesAsync(channelId, lastReceivedId: lastReceivedId, cancellationToken: cancellationToken);

                return Ok(messages);
            }
        }

        [HttpPut("{channelId}/{messageId}/pin")]
        public async Task<IActionResult> PinMessageAsync(string channelId, string messageId, CancellationToken cancellationToken = default)
        {
            var pinned = await _channels.PinMessageAsync(messageId, channelId, cancellationToken);
            return Ok(pinned);
        }

        [HttpGet("{channelId}/pins")]
        public async Task<IActionResult> GetPinnedMessagesAsync(string channelId, CancellationToken cancellationToken = default)
        {
            
            var pins = await _channels.GetPinnedMessagesAsync(channelId, cancellationToken);
            return Ok(pins);
        }

        [HttpPut("{channelId}/{messageId}/unpin")]
        public async Task<IActionResult> UnpinMessageAsync(string channelId, string messageId, CancellationToken cancellationToken = default)
        {
            var unpinned = await _channels.UnpinMessageAsync(channelId, messageId, cancellationToken);
            return Ok(unpinned);
        }
    }
}
