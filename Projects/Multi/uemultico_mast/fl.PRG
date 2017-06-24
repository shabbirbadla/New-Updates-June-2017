constr= "Driver={SQL Server};server=tushar;database=neio;uid=sa;pwd=;"
Chqcon = Sqlstringconnect(constr)


mpath = "C:\vudyog_tushar\multi\"
mtbm = "tushar"
sele 0
use (mpath+mtbm)
dimen mFlds(1)
local runFlds
totflds=afields(mflds)
runflds=""
for K=1 to totflds
	tYp = Type(field(k))
	leng = allT(str(fsize(field(k))))
	do case
	case tYp = "C"
		runflds = runflds+mflds(k,1)+space(1)+"varchar"+space(1)+"("+leng+")"+iif(totflds=1 or k=totflds,"",",")
	case tYp = "N"
		runflds = runflds+mflds(k,1)+space(1)+"Numeric"+space(1)+"("+leng+")"+iif(totflds=1 or k=totflds,"",",")
	case tYp = "L"
		runflds = runflds+mflds(k)+"Bit"
	case tYp = "M" 
		runflds = runflds+mflds(k)+"Text"+&leng
	endcase
Next
set step on

msql = "create table "+mtbm+"("+runflds+")"
cTbale=sqlexec(chqcon,msql)
messageB("hi")
