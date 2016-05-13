\ show-reg.fs -- display register bits -- 160331rjn 
\ 

14 constant temp  \ arbitrary (can't display register 14!)

: "1   [ char 1 ] #, emit ;
: "0   [ char 0 ] #, emit ;

:m .temp
   7 temp sbrc, "1  7 temp sbrs, "0
   6 temp sbrc, "1  6 temp sbrs, "0
   5 temp sbrc, "1  5 temp sbrs, "0
   4 temp sbrc, "1  4 temp sbrs, "0
   space
   3 temp sbrc, "1  3 temp sbrs, "0
   2 temp sbrc, "1  2 temp sbrs, "0
   1 temp sbrc, "1  1 temp sbrs, "0
   0 temp sbrc, "1  0 temp sbrs, "0  nop m;  target

: .T   cr  T temp mov, .temp ;  \ example     
\ 
\ 
\ -----------------------------------------------------------------------------
0 [if] \                         Revisions
\ -----------------------------------------------------------------------------

Date     By    Description
======   ===   ================================================================
160331   rjn   Initial version.  Mostly for debug

[then] \ ----------------------------------------------------------------------

