DO
$body$
BEGIN
   IF NOT EXISTS (SELECT 1 FROM pg_catalog.pg_user WHERE usename = 'shopsuser') THEN
      CREATE USER shopsuser WITH PASSWORD '123123';
   END IF;
END
$body$
