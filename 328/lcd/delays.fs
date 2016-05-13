\ delays.fs -- basic delays -- 160331rjn
\
0 [if] \ --- 16 MHz
: us ( n)  2* 2* for next ;
: ms ( n)  for 4000 #, for next next ;
[then] \ --- 16 MHz

1 [if] \ --- 8 MHz
: us ( n)  2* for next ;  \ 10 #, us ==> 13.8 usec
: ms ( n)  for 2000 #, for next next ;  \ 10 #, ms ==> 10.0 ms
[then] \ --- 8 MHz
\ 
\ 
0 [if] ---------------------[ Revision History ]-------------------------------
\ 
Date	  By    Description
======= === ===================================================================
160331  rjn versions for 8 MHz and 16 MHz.  Verified 8 MHz delays for us, ms
150926  rjn removed conditional compilation
150810  rjn initial version
\ 
[then] \ ----------------------------------------------------------------------

