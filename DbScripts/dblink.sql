select dblink_disconnect('myconn');
select dblink_connect('myconn','host=localhost port=5433 dbname=shopsdata_restore user=postgres password=123123'); 
insert into productrecord_temp 
select *
from dblink('myconn','SELECT * FROM productrecord') as tmp(
    productrecordid integer,
    sourceproductid integer,
    price integer,
    rating real,
    "timestamp" timestamp without time zone,
    amountavailable integer,
    description text,
    name text,
    locationid integer,
    externalid text,
    brand text,
    producttypeid integer
);