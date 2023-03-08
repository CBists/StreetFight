using System.ComponentModel.DataAnnotations;

namespace TelegramGame.Data.Entity;
public class UserEntity
{
    [Key]
    public long ChatId { get; set; }
    public string? Name { get; set; }
    public int MessageId { get; set; }
    public int Strange { get; set; }
    public int Agility { get; set; }
    public int Money { get; set; }
    public string Inventory { get; set; }
    
}