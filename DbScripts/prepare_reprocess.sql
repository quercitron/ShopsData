delete from productrecord
where externalid is null;

ALTER TABLE productrecord DROP CONSTRAINT productrecord_fk_sourceproductid;

DELETE FROM sourceproduct;

DELETE FROM product;

GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO shopsuser;

GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO shopsuser;