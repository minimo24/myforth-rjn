\ main.fs -- LCD Demo Application -- 160512rjn
\
\
: init  /chip  /timer0  ( /timer2)  /lcd  bh  ch ;

: go  init  
   begin 
      ba  2000 #, ms  \ test backlight alert
   again
\ 
\ -----------------------------------------------------------------------------
0 [if] \                      Revision History
\ -----------------------------------------------------------------------------
\ 
Date	  By    Description
======= === ===================================================================
160512  rjn changed title for github release
160424  rjn added backlight to initialization
160329  rjn initial version
\ 
[then] \ ----------------------------------------------------------------------

