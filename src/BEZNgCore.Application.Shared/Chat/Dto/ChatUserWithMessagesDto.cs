using System.Collections.Generic;

namespace BEZNgCore.Chat.Dto;

public class ChatUserWithMessagesDto : ChatUserDto
{
    public List<ChatMessageDto> Messages { get; set; }
}

