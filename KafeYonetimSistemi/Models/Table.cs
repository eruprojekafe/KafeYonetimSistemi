using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KafeYonetimSistemi.Models;

public class Table
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Display(Name = "Masa Numarası")]
    public int TableNumber { get; set; }
    [Display(Name = "Müsait mi?")]
    public bool IsAvailable { get; set; } = true;

    [InverseProperty("Table")]
    public ICollection<Order> Orders { get; set; } = new List<Order>();

    public override string ToString()
    {
        return $"Masa {TableNumber} - {(IsAvailable ? "Müsait" : "Müsait Değil")}";
    }
}
