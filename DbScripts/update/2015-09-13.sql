ALTER TABLE sourceproduct ADD COLUMN "timestamp" timestamp without time zone NULL

UPDATE sourceproduct 
SET timestamp = pr.timestamp
FROM sourceproduct sp JOIN 
    (SELECT sourceproductid, MIN(timestamp) as timestamp FROM productrecord GROUP BY sourceproductid) as pr ON sp.sourceproductid = pr.sourceproductid
    
ALTER TABLE sourceproduct ALTER COLUMN "timestamp" SET NOT NULL

GO


ALTER TABLE sourceproduct ADD COLUMN brand text NULL

UPDATE sourceproduct 
SET brand = pr.brand
FROM sourceproduct sp JOIN 
    (SELECT sourceproductid, MIN(brand) as brand FROM productrecord GROUP BY sourceproductid) as pr ON sp.sourceproductid = pr.sourceproductid
    
GO