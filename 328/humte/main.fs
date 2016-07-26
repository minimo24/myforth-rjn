\ main.fs -- HUMTE - DHT22 with encoder demo -- 160722rjn
\ 
\ Note: use /timer if using interrupt timing; /timers if using main loop
\ 
: init  /chip  /pwm  /int0  /encoder  ( /timer2)  /timers  
        /nokia  /dht  clear  bm  sei, ;
\ 
: go   init  .dht  begin 1 #, ms  ?dht again

\ 
\ -----------------------------------------------------------------------------
0 [if] \                      Revision History
\ -----------------------------------------------------------------------------
\ 
Date	  By    Description
======= === ===================================================================
160722  rjn working demo app.
160630  rjn initial version cloned from IE.
\ 
[then] \ ----------------------------------------------------------------------

