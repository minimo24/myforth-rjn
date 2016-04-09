\ main.fs -- blink tester for 2560 chip -- 150830rjn
\
]  here ( *start)

: us ( n)  2* 2* for next ;
: ms ( n)  for 4000 #, for next next ;  \ timing checked with blink, below

: init  /chip  ( /step) ( /wdt) ;
\ : go_down   -pin13  10 #, for power_down next ( 80 secs) +pin13 ;

\ : go  init  reduce  go_down  begin   200 #, ms  ~pin13 ( led)  again 

: go  init  
   begin  100 #, ms  
      ~PA0 ~PA1 ~PA7  ~PB7 ( led) ~PG0 ~PG1 ~PG2
      +PH -PJ +PK -PL  100 #, ms  -PH +PJ -PK +PL
   again
   
\
\ --- conditional compilation summary ---
here  ( *stop) [ swap -  ( --)
\              ......................321
cs? [if] cr .( main.fs                 ) . .( bytes) [else] drop [then]
]
\ 
\ 
0 [if] ---------------------[ Revision History ]-------------------------------
\ 
Date	  By    Description
======= === ===================================================================
150830  rjn Initial version (with new "d" command script)
\ 
[then] \ ----------------------------------------------------------------------

