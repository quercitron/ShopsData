update product
set created = a.mintimestamp
from
    (select p.productid, MIN(timestamp) as mintimestamp
    from product p join sourceproduct sp on p.productid = sp.productid
    group by p.productid) as a