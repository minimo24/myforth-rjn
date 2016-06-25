\ main.fs -- Nokia Graphics LCD Demo -- 160620rjn
\
\
: init  /chip  /pwm  /nokia  clear  nset  perm peek ;

: go  ( init)  \ init now done at cold startup
   ba  clear  16 #, 1 #, !xy  s" V" btype ver$ #btype
   16 #, 3 #, !xy s" DEMO" btype perm perm
   nset perm perm peek
   begin 
      bstring  perm peek  ~bstring peek  all-on  peek   blank peek 
      nstring  perm peek  ~nstring peek  all-on  peek   blank peek
      pattern  perm  inverse  perm  normal  blank peek
   again
\ 
\ -----------------------------------------------------------------------------
0 [if] \                      Revision History
\ -----------------------------------------------------------------------------
\ 
Date	  By    Description
======= === ===================================================================
160620  rjn added pattern to demo
160610  rjn updated init display
160609  rjn runs string demo
160529  rjn initial version
\ 
[then] \ ----------------------------------------------------------------------

