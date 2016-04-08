\ main.fs -- generate PSR signal -- 160303rjn
\
\

: init  /chip  /psr  PD3 low, ;

\ : go   init  begin PD3 toggle,  again   \ I/O check

-: go   init  begin psr again

\ verify that interpreter and strings work
\
\ : greet  s" This is a string!" ptype space space rstring cr ;
\ : this  ." This was also a string!" cr ;

\ 
\ -----------------------------------------------------------------------------
0 [if] \                      Revision History
\ -----------------------------------------------------------------------------
\ 
Date	  By    Description
======= === ===================================================================
160303  rjn working psr, 45 minute repetition rate
160223  rjn initial version
\ 
[then] \ ----------------------------------------------------------------------

