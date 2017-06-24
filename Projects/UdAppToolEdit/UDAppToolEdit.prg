IF 'trnamend' $ vChkprod
*  DO VouToolEdit IN MainPrg  &&Birendra : 22 Mar 2011 for Order Amendment
* The above referencing is erroneous so commented by Amrendra for Bug-1994 On 07-02-2012
  DO VouToolEdit IN ueamendment.app  && Added By Amrendra for Bug-1994 On 07-02-2012
ENDIF 

