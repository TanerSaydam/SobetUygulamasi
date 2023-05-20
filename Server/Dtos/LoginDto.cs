using Server.Context;

namespace Server.Dtos;

public sealed class LoginDto
{
    public string UserName { get; set; }    
}

public sealed class UserDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
}

public sealed class GetMessageDto
{
    public int UserId { get; set; }
    public int ToUserId { get; set; }
}

public sealed class MessageDto
{
    public int UserId { get; set; }
    public int ChatId { get; set; }
    public string Text { get; set; }
}

public sealed class GetChatMessagesDto
{
    public IEnumerable<Message> Messages { get; set; }
    public int ChatId { get; set; }
}
