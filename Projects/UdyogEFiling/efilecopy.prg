LPARAMETERS _SourceFile,_TargetFile
IF TYPE('_SourceFile') = 'C' AND TYPE('_TargetFile') = 'C'
	COPY FILE (_SourceFile) TO (_TargetFile)
Endif