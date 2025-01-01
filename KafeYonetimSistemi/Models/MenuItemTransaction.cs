using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KafeYonetimSistemi.Models
{
    public class MenuItemTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("MenuItemId")]
        public MenuItem? MenuItem { get; set; } = default!;

        [Required]
        public int MenuItemId { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    public enum TransactionType
    {
        ADD,
        REMOVE
    }
}