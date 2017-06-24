SELECT Entry_ty,[Date],Doc_no,Itserial,Tran_cd,Rentry_ty,Rdate,Rdoc_no,Itref_Tran,Ritserial
FROM itref
Union
SELECT Entry_ty,[Date],Doc_no,Itserial,Tran_cd,Rentry_ty,Rdate,Rdoc_no,Itref_Tran,Ritserial
FROM Stitref
Union
SELECT Entry_ty,[Date],Doc_no,Itserial,Tran_cd,Rentry_ty,Rdate,Rdoc_no,Itref_Tran,Ritserial
FROM Ptitref
Union
SELECT Entry_ty,[Date],Doc_no,Itserial,Tran_cd,Rentry_ty,Rdate,Rdoc_no,Itref_Tran,Ritserial
FROM POitref
Union
SELECT Entry_ty,[Date],Doc_no,Itserial,Tran_cd,Rentry_ty,Rdate,Rdoc_no,Itref_Tran,Ritserial
FROM SOitref

Select Cd,Code_Nm,Repo_Nm,PickupFrom From LCode Where Entry_Ty = 'SO' Or BCode_Nm = 'SO' 
Select Cd,Code_Nm,Repo_Nm,PickupFrom From LCode Where Entry_Ty = 'PO' Or BCode_Nm = 'PO'