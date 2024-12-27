using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KafeYonetimSistemi.Models;

public class MenuItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Display(Name = "Ürün Adı")]
    public string Name { get; set; } = default!;

    [Display(Name = "Açıklama")]
    public string Description { get; set; } = default!;

    [Display(Name = "Fiyat")]
    public decimal Price { get; set; }

    [Display(Name = "Mevcut mu?")]
    public bool IsAvailable { get; set; } = true;

    // CategoryId ile Category tablosuna ilişki kuruyoruz
    public int CategoryId { get; set; } // CategoryId özelliği

    [ForeignKey("CategoryId")]
    public Category Category { get; set; } = default!;

    public string? ImageUrl { get; set; } = null!;

    public ICollection<MenuItemTransaction> MenuItemTransaction { get; set; }
}
