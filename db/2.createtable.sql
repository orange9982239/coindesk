-- 建立 currencies 表
CREATE TABLE currencies (
    id INT IDENTITY(1,1) PRIMARY KEY,                  -- 主鍵，自動遞增
    currency_code VARCHAR(10) NOT NULL UNIQUE,         -- 幣別代碼，唯一值
    currency_name NVARCHAR(50) NOT NULL UNIQUE,        -- 幣別中文名稱
    exchange_rate DECIMAL(18, 6) NOT NULL,             -- 匯率，最多18位數字，6位小數
    updated_at DATETIME2 NOT NULL DEFAULT GETDATE()    -- 最後更新時間，自動更新
);

-- 插入初始可兌換貨幣（以 BTC 為基準）
INSERT INTO currencies (currency_code, currency_name, exchange_rate)
VALUES
    ('BTC', N'比特幣', 1.000000),            -- 本國貨幣，匯率為 1
    ('USD', N'美元', 0.00001056),           -- 假設 1 USD = 94,734.4777 BTC，匯率為 1/94,734.4777
    ('GBP', N'英鎊', 0.00001323),           -- 假設 1 GBP = 75,557.8511 BTC，匯率為 1/75,557.8511
    ('EUR', N'歐元', 0.00001097);           -- 假設 1 EUR = 91,109.3682 BTC，匯率為 1/91,109.3682

-- 查詢 currencies 表以確認數據
SELECT * FROM currencies;
