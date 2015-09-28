select *
from sourceproduct sp1 join sourceproduct sp2 on
        sp1.sourceproductid > sp2.sourceproductid and
        sp1.productid = sp2.productid and
        sp1.datasourceid = sp2.datasourceid