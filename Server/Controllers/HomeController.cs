using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Server.Dtos;
using Server.Hubs;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly AppDbContext _context;    
    private readonly UserManager<AppUser> _userManager;
    private readonly IHubContext<ChatHub> _hubContext;
    public HomeController(AppDbContext context, UserManager<AppUser> userManager, IHubContext<ChatHub> hubContext)
    {
        _context = context;
        _userManager = userManager;
        _hubContext = hubContext;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        AppUser user = await _userManager.FindByNameAsync(loginDto.UserName);
        if (user == null) throw new Exception("Kullanıcı bulunamadı!");

        return Ok(user);
    }

    [HttpGet("[action]/{userId}")]
    public async Task<IActionResult> GetUsers(int userId, CancellationToken cancellationToken)
    {
        IList<UserDto> users =
            await _userManager.Users
            .Where(p => p.Id != userId)
            .Select(s => new UserDto
            {
                UserName = s.UserName,
                Id = s.Id
            })
            .ToListAsync(cancellationToken);

        return Ok(users);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> GetChatMessages(GetMessageDto request, CancellationToken cancellationToken)
    {
        var chatId =
            await _context.ChatParicipants
            .Where(p => p.UserId == request.UserId)
            .Select(p => p.ChatId)
            .Intersect(
                _context.ChatParicipants
                .Where(p => p.UserId == request.ToUserId)
                .Select(s => s.ChatId))
            .FirstOrDefaultAsync(); //0

        if(chatId == 0)
        {
            var chat = new Chat()
            {
                Name = $"{request.UserId} ve {request.ToUserId} arasında özel sohbet",
                CreatedDate = DateTime.Now,
            };
            await _context.Chats.AddAsync(chat, cancellationToken).ConfigureAwait(false);

            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            chatId = chat.Id;

            var userChatParticipant = new ChatParicipant() { UserId = request.UserId, ChatId = chatId };
            var toUserChatParticipant = new ChatParicipant() { UserId = request.ToUserId, ChatId = chatId };

            await _context.ChatParicipants.AddRangeAsync(userChatParticipant, toUserChatParticipant).ConfigureAwait(false);
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        var messages = await _context.Messages
            .Where(p => p.ChatId == chatId)
            .Include(p => p.User)
            .OrderBy(p => p.Timestamp)
            .ToListAsync();

        GetChatMessagesDto response = new()
        {
            Messages = messages,
            ChatId = chatId
        };

        return Ok(response);        
    }

    [HttpGet("[action]/{userId}")]
    public async Task<IActionResult> GetUser(int userId)
    {
        UserDto userDto = await _userManager.Users.Where(p => p.Id == userId).Select(s => new UserDto
        {
            Id = s.Id,
            UserName = s.UserName
        }).FirstOrDefaultAsync();

        return Ok(userDto);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> PostMessage(MessageDto request, CancellationToken cancellationToken)
    {
        Message message = new()
        {
            ChatId = request.ChatId,
            UserId = request.UserId,
            Timestamp = DateTime.Now,
            Text = request.Text
        };

        await _context.Messages.AddAsync(message, cancellationToken).ConfigureAwait(false);
        await _context.SaveChangesAsync(cancellationToken);

        AppUser user = await _userManager.Users.Where(p=> p.Id == message.UserId).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        message.User = user;

        await _hubContext.Clients.Group(message.ChatId.ToString()).SendAsync("ReceiveMessage", message);

        return Ok(new { Message = "Mesaj başarıyla gönderildi!" });        
    }
}
