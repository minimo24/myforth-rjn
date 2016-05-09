\ main.fs -- IE Demo -- 160506rjn
\ 
\ 
: init   /chip  /int0  ( ccw)  /encoder  sei, ;
\ 
: .ie  @p&c h. space  @encoder hex ud. ;
\ 
: go  init  begin  1000 #, ms  cr .ie  again
\ 
\ 
0 [if]  \ --- testing, simulation
\ 
\ to simulate, wrap pins PD4 to PD2 (INT0) and PD6 to PD3 (INT1)
\ 
: cw   PD4 high,  PD6 high, ;
: ccw  PD4 low,   PD6 high, ;
\ 
\ .ie note -- For cw, p&c alternates between $83 and $B0, acc. increments
\ For ccw, p&c alternates between $12 and $21 and acc. decrements 
: .ie  cr @p&c h. space  @encoder hex ud. ;
\ 
: go  init cr  \ simulate encoder
   begin  
      cr .ie  
      250 #, ms  PD6 toggle, ( phase B)  
      250 #, ms  PD4 toggle, ( phase A) 
   again
\ 
\ --- speed test  ( ie ISR takes ~ 4 usec
: go  init cr begin ( .ie) test-add nop nop nop again
\    
[then] \ --- testing, simulation

\ 
\ -----------------------------------------------------------------------------
0 [if] \                      Revision History
\ -----------------------------------------------------------------------------
\ 
Date	  By    Description
======= === ===================================================================
160509  rjn works on several optical encoders, weak pullups seem sufficient.
160506  rjn hooked up to Chinese optical encoder, 10K pullups on A & B
160504  rjn working simulator
160430  rjn changed A & B to INT0 & INT1, use lead change on INT0
160429  rjn working low to high lead change on INT1 (see ie.fs)
160428  rjn initial version
\ 
[then] \ ----------------------------------------------------------------------

