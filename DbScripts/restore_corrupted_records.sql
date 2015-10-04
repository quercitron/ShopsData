insert into productrecord (sourceproductid, price, rating, timestamp, amountavailable, description, name, locationid, externalid, brand, producttypeid, datasourceid)
select pr1.sourceproductid, prt.price, prt.rating, prt.timestamp, prt.amountavailable, prt.description, pr1.name, prt.locationid, prt.externalid, prt.brand, pr1.producttypeid, pr1.datasourceid
from productrecord_temp prt
    join productrecord pr1 on prt.externalid = pr1.externalid
    left join productrecord pr2 on pr1.externalid = pr2.externalid and pr1.timestamp < pr2.timestamp
where
    pr2.productrecordid is null and (
    prt.name = '19.5' or
    prt.name = '21.5' or
    prt.name = '23' or
    prt.name = '23.6' or
    prt.name = '24' or
    prt.name = '27' or
    prt.name = '28' or
    prt.name = '34')
