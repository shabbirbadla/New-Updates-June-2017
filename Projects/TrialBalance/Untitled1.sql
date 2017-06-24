SELECT *
FROM r_status
WHERE ([group] = 'Trial Balance') AND (rep_nm = 'TRIAL_Z' AND [Desc] = 'Zoom In')
--WHERE ([group] = 'Balance Sheet ') AND (rep_nm = 'BALSH_Z' AND [Desc] = 'Zoom In')

SELECT *
FROM r_status
WHERE ([Desc] = 'Zoom In')



/* Trial Balance */
Update R_Status Set IsMethod = 1,SqlQuery = '',MethodOf ='trialbalance.app WITH _TmpVar.SDate, _TmpVar.EDate, "T","",ThisForm.DataSessionId'
	WHERE ([group] = 'Trial Balance') AND (rep_nm = 'TRIAL_Z' AND [Desc] = 'Zoom In')

/* Balance Sheet*/
Update R_Status Set IsMethod = 1,SqlQuery = '',MethodOf ='trialbalance.app WITH _TmpVar.SDate, _TmpVar.EDate, "B","",ThisForm.DataSessionId'
WHERE ([group] = 'Balance Sheet ') AND (rep_nm = 'BALSH_Z' AND [Desc] = 'Zoom In')

                                                     
 