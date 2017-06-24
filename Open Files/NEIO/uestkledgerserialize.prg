Parameters vDataSessionId
Local EXPARA

EXPARA=' '
x=""

Set DataSession To vDataSessionId

Do Form uefrm_serialnofilter.scx With vDataSessionId To x

EXPARA=x

Replace _rstatusclonesex.xTraParam With "'"+EXPARA+"'"

