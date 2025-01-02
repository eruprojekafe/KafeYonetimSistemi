using KafeYonetimSistemi.Data;
using KafeYonetimSistemi.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class StockChangesModel : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public StockChangesModel(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<StockReportItem> DailyStockReports { get; set; }
    public List<StockReportItem> WeeklyStockReports { get; set; }
    public List<StockReportItem> MonthlyStockReports { get; set; }

    public void OnGet()
    {
        DateTime startDateDay = DateTime.Now.AddDays(-1); // Son 1 gün (günlük)
        DateTime startDateWeek = DateTime.Now.AddDays(-7); // Son 7 gün (haftalýk)
        DateTime startDateMonth = DateTime.Now.AddMonths(-1); // Son 1 ay (aylýk)
        DateTime endDate = DateTime.Now;

        // Günlük rapor (Son 1 gün)
        DailyStockReports = _dbContext.MenuItemTransaction
            .Where(t => t.Timestamp >= startDateDay && t.Timestamp <= endDate)
            .GroupBy(t => t.MenuItemId)
            .Select(g => new StockReportItem
            {
                MenuItemName = g.FirstOrDefault().MenuItem.Name, // Ürün adý
                TotalQuantity = g.Sum(x => x.Amount) // Toplam stok deðiþimi
            })
            .ToList();

        // Haftalýk rapor (Son 7 gün)
        WeeklyStockReports = _dbContext.MenuItemTransaction
            .Where(t => t.Timestamp >= startDateWeek && t.Timestamp <= endDate)
            .GroupBy(t => t.MenuItemId)
            .Select(g => new StockReportItem
            {
                MenuItemName = g.FirstOrDefault().MenuItem.Name, // Ürün adý
                TotalQuantity = g.Sum(x => x.Amount) // Toplam stok deðiþimi
            })
            .ToList();

        // Aylýk rapor (Son 1 ay)
        MonthlyStockReports = _dbContext.MenuItemTransaction
            .Where(t => t.Timestamp >= startDateMonth && t.Timestamp <= endDate)
            .GroupBy(t => t.MenuItemId)
            .Select(g => new StockReportItem
            {
                MenuItemName = g.FirstOrDefault().MenuItem.Name, // Ürün adý
                TotalQuantity = g.Sum(x => x.Amount) // Toplam stok deðiþimi
            })
            .ToList();
    }

    // Rapor öðesi modeli
    public class StockReportItem
    {
        public string MenuItemName { get; set; }
        public decimal TotalQuantity { get; set; }
    }
}
