using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KafeYonetimSistemi.Models;

public class MenuItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; } = true;

    // CategoryId ile Category tablosuna ilişki kuruyoruz
    public int CategoryId { get; set; } // CategoryId özelliği

    [ForeignKey("CategoryId")]
    public Category Category { get; set; } = default!;
}