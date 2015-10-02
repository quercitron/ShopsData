insert into productrecord_temp
set producttypeid = p.producttypeid, datasourceid = sp.datasourceid
from product p
	join sourceproduct sp on p.productid = sp.productid
	join productrecord pr on sp.sourceproductid = pr.sourceproductid


insert into productrecord_temp (
    productrecordid,
    sourceproductid,
    price,
    rating,
    "timestamp",
    amountavailable,
    description,
    name,
    locationid,
    externalid,
    brand,
    producttypeid,
    datasourceid)
select
    pr.productrecordid,
    pr.sourceproductid,
    pr.price,
    pr.rating,
    pr."timestamp",
    pr.amountavailable,
    pr.description,
    pr.name,
    pr.locationid,
    pr.externalid,
    pr.brand,
    p.producttypeid,
    sp.datasourceid
from product p
	join sourceproduct sp on p.productid = sp.productid
	join productrecord pr on sp.sourceproductid = pr.sourceproductid