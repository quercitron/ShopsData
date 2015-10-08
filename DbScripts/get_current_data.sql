with list as ( 
select p.productid, p.name, p.class, p.producttypeid, sp.sourceproductid, sp.datasourceid, case when up.userproductid is null then FALSE else TRUE end as ismarked
from product p 
left join userproduct up on p.productid = up.productid and up.userid = :userid
join sourceproduct sp on p.productid = sp.productid 
where p.producttypeid = :producttypeid) 
select list.productid, list.name, list.producttypeid, list.datasourceid, pr1.price, pr1.rating, pr1.timestamp, pr1.locationid, list.class, list.ismarked 
from list 
join productrecord pr1 on list.sourceproductid = pr1.sourceproductid 
left join productrecord pr2 on(list.sourceproductid = pr2.sourceproductid and pr1.timestamp < pr2.timestamp) 
where pr2.productrecordid is null and pr1.locationid = :locationid and pr1.timestamp > current_date - 14