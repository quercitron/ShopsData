select *
from productrecord pr1 join productrecord pr2
	on pr1.sourceproductid = pr2.sourceproductid and pr1.price <> pr2.price and pr1.timestamp < pr2.timestamp
order by pr1.price - pr2.price
