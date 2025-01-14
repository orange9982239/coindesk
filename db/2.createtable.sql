-- 建立 currencies 表
CREATE TABLE [currencies] (
    [Id] int NOT NULL IDENTITY,
    [CurrencyCode] nvarchar(10) NOT NULL,
    [CurrencyName] nvarchar(50) NOT NULL,
    [ExchangeRate] decimal(18,6) NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_currencies] PRIMARY KEY ([Id])
);

-- 插入初始可兌換貨幣（以 BTC 為基準）
INSERT INTO currencies (CurrencyCode, CurrencyName, ExchangeRate, UpdatedAt)
VALUES
    ('BTC', N'比特幣', 1.000000, GETDATE()),
    ('USD', N'美元', 96167.7374, GETDATE()),
    ('GBP', N'英鎊', 76700.9831, GETDATE()),
    ('EUR', N'歐元', 92487.7827, GETDATE());

-- 查詢 currencies 表以確認數據
SELECT * FROM currencies;

