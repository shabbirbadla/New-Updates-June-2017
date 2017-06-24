PARA caption1
  IF UPPER(caption1)=[USER]
   do form user 
   ELSE
   IF UPPER(caption1)=[KEYLOCK]
    do form keylock
    ELSE
   IF UPPER(caption1)=[LOCKING]
    do form locking
   ENDIF
 ENDI
ENDIF
    

