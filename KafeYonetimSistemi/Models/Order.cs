using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KafeYonetimSistemi.Models;

public enum OrderStatus
{
    Created,
    Preparing,
    Ready,
    Delivered,
    Cancelled
}

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime OrderTime { get; set; }

    public OrderStatus Status { get; set; }

    [ForeignKey("TableId")]
    public required Table Table { get; set; }

    public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}