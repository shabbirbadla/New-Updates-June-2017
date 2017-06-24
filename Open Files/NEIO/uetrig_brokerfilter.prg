Parameters vDataSessionId,cEntry_ty

Do Form uefrm_ageing_out With vDataSessionId

If cEntry_ty="PT"
	Do Form uefrm_BrokerFilter With vDataSessionId ,cEntry_ty
Endif

If cEntry_ty="ST"
	Do Form uefrm_BrokerFilter With vDataSessionId ,cEntry_ty
Endif
