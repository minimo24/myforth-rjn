\ commands.fs -- Nokia LCD misc. commands -- 160610rjn
\ 
\ ----- option?
: opt  ( - n)   0 #, PD4 PIN sbic, $ff T ldi, ;
: (nopt)  0 #, 2 #, !xy 
   opt if drop s" demo" ; then drop s" cmd" ;
: nopt  (nopt) ntype  s"  option" ntype ;
\ 
\ ----- version
15 constant ver#  \ !!!!!!!!!!!!!!!! CHANGE VERSION HERE !!!!!!!!!!!!!!!!!!!!!
-: ver$  ( - a n)  ver# #,  0 #, <# # [ char . ] #, hold # #> ; 
: nver  ( -)  \ display version
    0xy s" version " ntype  ver$ #ntype ;  
: ver  ver# #, ; \ put version # on stack   
: cver  ver$ type ;  \ display version on console
\ 
\ ----- contrast (FYI)
\ : ncon   0 #, 1 #, !xy  s" contrast" ntype  @econ nh.  ;
\
\ ----- backlight PWM
: npwm   0 #, 3 #, !xy  s" bklight" ntype  pwm  nh. ;
\ 
\ ----- settings
: nset  clear nver  ncon  nopt  npwm ;
\ 
\ -----------------------------------------------------------------------------
0 [if] \                      Revision History
\ -----------------------------------------------------------------------------
\ 
Date	  By    Description
======= === ===================================================================
160610  rjn misc. additions
160527  rjn initial version (empty)
\ 
[then] \ ----------------------------------------------------------------------

