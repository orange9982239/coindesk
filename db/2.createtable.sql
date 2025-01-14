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
    ('BTC', N'比特幣', 1.000000,GETDATE()),            -- 本國貨幣，匯率為 1
    ('USD', N'美元', 0.00001056,GETDATE()),           -- 假設 1 USD = 94,734.4777 BTC，匯率為 1/94,734.4777
    ('GBP', N'英鎊', 0.00001323,GETDATE()),           -- 假設 1 GBP = 75,557.8511 BTC，匯率為 1/75,557.8511
    ('EUR', N'歐元', 0.00001097,GETDATE());           -- 假設 1 EUR = 91,109.3682 BTC，匯率為 1/91,109.3682

-- 查詢 currencies 表以確認數據
SELECT * FROM currencies;

