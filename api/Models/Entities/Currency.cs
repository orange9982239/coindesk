using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("currencies")] // 對應資料表名稱
public class Currency
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 自動遞增主鍵
    public int Id { get; set; }

    [Required]
    [StringLength(10)] // 幣別代碼最大長度為 10
    public string CurrencyCode { get; set; } = string.Empty;

    [Required]
    [StringLength(50)] // 幣別名稱最大長度為 50
    public string CurrencyName { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,6)")] // 匯率精度為 18 位數字，6 位小數
    public decimal ExchangeRate { get; set; }

    [Required]
    [Column(TypeName = "datetime2")] // 使用 datetime2 類型
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}