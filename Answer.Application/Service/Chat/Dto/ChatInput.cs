namespace Answer.Application.Service.Chat.Dto;

public class ChatInput
{
   /// <summary>
   /// 用户ID
   /// </summary>
   public long UserId { get; set; }
   /// <summary>
   /// 用户未查看的消息数
   /// </summary>
   public int Count { get; set; }
}