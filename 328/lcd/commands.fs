\ commands.fs -- LCD misc. commands -- 160512rjn
\ 
\ --- version
\ pretty, but takes too long to complete -- use for demo only
: v   db ch  cf  s" LCD Demo V1.1" ltype  ch  2 #, drs  
      20 #, bas  15 #, dls  ch  16 #, dss  ch ; 
\ : v   // ch s"  LCD Demo V1.1" ltype ;  \ use for application code
\ 
\ -----------------------------------------------------------------------------
0 [if] \                      Revision History
\ -----------------------------------------------------------------------------
\ 
Date	  By    Description
======= === ===================================================================
160512  rjn two versions of "v" command
160510  rjn added display blank to "v" command (takes a long time to complete)
160422  rjn initial version
\ 
[then] \ ----------------------------------------------------------------------

