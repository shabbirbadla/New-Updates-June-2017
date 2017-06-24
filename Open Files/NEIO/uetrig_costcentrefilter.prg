Parameters vDataSessionId
Local EXPARA

EXPARA=' '
x=""

Set DataSession To vDataSessionId

		Do FORM uefrm_costcentrefilter.scx WITH vDataSessionId To x
EXPARA=x

Replace _rstatusclonesex.xTraParam With "'"+EXPARA+"'"

