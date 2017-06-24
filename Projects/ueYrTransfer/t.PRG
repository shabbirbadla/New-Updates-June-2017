
oPBar=Createobject("frmProgress")
oPBar.Show()
For i=1 To 1000
	oPBar.ProgStatus("Processing: "+Alltrim(Transform(i)),i)
	WAIT WINDOW "" TIMEOUT 0.100
Endfor
oPBar.Release()
oPBar = .Null.
Release oPBar
