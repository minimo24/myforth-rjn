\ delays.fs -- basic delays -- 150926rjn
\
\ 
\ for 16 MHz Arduino
: us ( n)  2* 2* for next ;
: ms ( n)  for 4000 #, for next next ;
\ 
\ 
0 [if] ---------------------[ Revision History ]-------------------------------
\ 
Date	  By    Description
======= === ===================================================================
150926  rjn removed conditional compilation
150810  rjn initial version
\ 
[then] \ ----------------------------------------------------------------------

