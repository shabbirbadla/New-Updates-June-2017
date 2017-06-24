Lparameters nPendDays,lUpgrade,lAmcFile,cRights

_Screen.Visible = .F.
Set Exclusive Off
Set Safety Off
Set Century On
Set Date To british
Set Deleted On

Do Form frmGetClientInfo With nPendDays,lUpgrade,cRights

Read Events
