\ delays.fs -- basic delays -- 150810rjn
\
]  here ( *start)
\ for 16 MHz Arduino
: us ( n)  2* 2* for next ;
: ms ( n)  for 4000 #, for next next ;
\
\ --- conditional compilation summary ---
here  ( *stop) [ swap -  ( --)
\              ......................321
cs? [if] cr .( delays.fs                 ) . .( bytes) [else] drop [then]
]
\ 
\ 
0 [if] ---------------------[ Revision History ]-------------------------------
\ 
Date	  By    Description
======= === ===================================================================
150810  rjn initial version
\ 
[then] \ ----------------------------------------------------------------------

