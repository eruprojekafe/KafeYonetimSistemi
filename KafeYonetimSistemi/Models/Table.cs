using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KafeYonetimSistemi.Models;

public class Table
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int TableNumber { get; set; }
    public bool IsAvailable { get; set; } = true;

    [InverseProperty("Table")]
    public ICollection<Order> Orders { get; set; } = new List<Order>();

    public override string ToString()
    {
        return $"Masa {TableNumber} - {(IsAvailable ? "Müsait" : "Müsait Değil")}";
    }
}
