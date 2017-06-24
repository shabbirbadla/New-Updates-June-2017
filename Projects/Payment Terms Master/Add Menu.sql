DECLARE @PayTermsRang int
select @PayTermsRang=max(range)+1 from COM_MENU
IF NOT EXISTS (SELECT padname,barname,prompname FROM COM_MENU WHERE padname='SYSTEMMASTERS' AND barname='PAYMENTTERMSMASTER' AND prompname ='Payment Terms Master')
BEGIN
	INSERT INTO COM_MENU([range], [padname], [padnum], [barname], [barnum], [prompname], [numitem], [hotkey], [progname], [E_], [n_], [r_], [t_], [i_], [o_], [b_], [x_], [Menutype], [Isactive], [puser], [mdefault], [labkey], [skipfor], [cprog], [mark], [xtvs_], [dsni_], [mcur_], [tds_], [newrange], [HRPay_]) 
	VALUES (@PayTermsRang, 'SYSTEMMASTERS', 0, 'PAYMENTTERMSMASTER', 26,'Payment Terms Master', 0, '', 'DO UDCALLEXTPROG.APP WITH "UDPAYTERMSMASTER.EXE","","^'+cast(@PayTermsRang as varchar)+'"', null, null, null, null, null, null, null, null, 'General        ', null, 'Payment Terms Master                                                                                ', null, null, null, null, null, null, null, null, null, null, null)
END
