DELETE FROM productrecord;

DELETE FROM sourceproduct;

DELETE FROM product;

DELETE FROM datasource;
INSERT INTO datasource (name) values ('citilink'), ('dns');

DELETE FROM location;
INSERT INTO location (name) values ('vrn');

DELETE FROM producttype;
INSERT INTO producttype (name) values ('motherboard'), ('monitor'), ('powersupply');

