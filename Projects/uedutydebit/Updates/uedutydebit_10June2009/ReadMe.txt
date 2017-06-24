1) Update  CLASS cmdexbtn of vouclass.vcx from code portion from tvouclass
cmdexbtn-->click.copy lines between &&Duty Debit app Call-->.....&&<--Duty Debit app Call portion.
2)copy uedutydebit.app in Company folder not in usquare\iTAX folder becauese, it is useful only in Excise Mfg. .
3)Run USP_ENT_DUTY_DEBIT.sql file.
4)Make changes in Transaction Setting for RG23-A Payment(GI),RG23C Payment(HI),PLA Receipt(RR),Service Tax Receipt(VR) for Default Debit=main_vw.party_nm like TransactionSetting.bmp
6)Remove All RG23A,RG23C,plA,Service Tax Debit menu.
7)Add New menu for RG23A,RG23C,PLA,Service Tax debit entry with  action like 

DO UEVOUCHER WITH "GI","EXCISE ACCOUNT","","","","RG23A PAYMENT ( DUTY DEBIT )",0,.T.

DO UEVOUCHER WITH "HI","EXCISE ACCOUNT","","","","RG23C PAYMENT ( DUTY DEBIT )",0,.T.

DO UEVOUCHER WITH "RR","EXCISE ACCOUNT","","","","PLA PAYMENT ( DUTY DEBIT )",0,.T.

DO UEVOUCHER WITH "VR","EXCISE ACCOUNT","","","","SERVICE TAX PAYMENT ( SERVICE TAX DEBIT )",0,.T.

8)In Transaction Setting--> Accounting Detail --> Overwrite cr/dr yes options should be selected for all credit and debit entries.





Note:-
In RG23A/RG23C/PLA/SERVICE TAX Debit entry this app will be call from Excise button.
It will allow duty debit only if duty is available. It is considering voucher date for balance.

In Menu Transaction-->Excise Transaction-->RG23a Part II only one option for Duty Debit will be enough for all EXCISE,CESS,HCESS,.. etc. Duty Debit.

It will take all acoounts related to entry from er_excise table in sql




