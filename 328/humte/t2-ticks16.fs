\ t2-ticks16.fs  t2 tick timer for 16 MHz clock - 160723rjn
\ 
\ 
0 [if] \ ------------------------[ Notes ]-------------------------------------
\
1. Adjusted for 16 MHz clock, use clock divide by 1024.

2. For a clock divisor of 1024:

   : init-interrupt 
      $03 ~#, TCCR2A #, and!  \ normal mode
      $08 ~#, TCCR2B #, and!  \ no wgm22, clear bit 3
      $20 ~#,   ASSR #, and!  \ source = I/O clock
      $07  #, TCCR2B #, or!   \ scale by 1024, set bits 0,1,2
      131  #,  TCNT2 #, !     \ set count in TCNT2
      $01 #,  TIMSK2 #, !     \ overflow interrupt enable
      sei, ;

      TCNT2    rate     freq
      -----    -------  ---------
      100      10 ms    100 Hz

3. NOTE!  The timer may interfere with operation of the standalone interpreter.
   Don't know why, but it is under investigation.  
   This is no problem for turnkeyed app.  For application troubleshooting,
   disable the timer code (e.g., comment out /timer2, any application
   words using the timer.)
\
[then] \ ----------------------------------------------------------------------
\ 
\ -----------------------------------------------------------------------------
1 [if] \                      100 ms timer on T2
\ -----------------------------------------------------------------------------
\
cpuHERE constant 'tcounter  2 cpuALLOT  \ counts timer2 ticks
cpuHERE constant 'tflag     1 cpuALLOT  \ flags tick counter rollover

: and! ( n a)  swap over  @ and swap |! ;
: or!  ( n a)  swap over  @ or  swap |! ;

: /timer2  \ ~ 10 ms tick, 16 MHz clock
      $03 ~#, TCCR2A #, and!  \ normal mode
      $08 ~#, TCCR2B #, and!  \ no wgm22, clear bit 3
      $20 ~#,   ASSR #, and!  \ source = I/O clock
\     
      $07  #, TCCR2B #, or!   \ set bit 0,1,2 -- scale by 1024 ( 10 ms)
\
      100  #,  TCNT2 #, |!       \ set count for 10 ms
      $01  #, TIMSK2 #, |!       \ overflow interrupt enable
        0  #, 'tcounter #,  |!   \ init timer counter
        0  #, 'tflag    #,  |c!  \ init overflow flag
      ( sei,) ;

\ : itick ; \ marker for see

$12 interrupt  \ ~100 ms tick
   cli,  T push, X push,  X' push,  N push,
   \ --- increment timer counter
\   PB2 toggle,  \ view timer interval on scope ( 10 ms)
   [ 'tcounter $ff and ] X ldi,   [ 'tcounter 256 / $ff and ] X' ldi,
   1 N ldi, T ldx,  N T add,   T stx+,
   0 N ldi, T ldx,  N T adc,   T stx,
   \ --- conditionally reset tcounter, signal foreground with tflag
   T -ldx,  \ load lsb of tcounter
   10 N ldi,  N T sub,  0if,   \ tcounter count down
      T stx+,  T stx,  \ zero tcounter
      \ --- set tflag to signal 100 ms timeout
      [ 'tflag $ff and ] X ldi,   [ 'tflag 256 / $ff and ] X' ldi,
      $ff T ldi, T stx,  
\      PB2 toggle, \ view final 100 ms timing on scope
   then, 
   \ --- reset timer interval here:
   100 T ldi, ( 10 ms) TCNT2  X ldi,  0 X' ldi,  T stx,
   N pop,  X' pop,  X pop,  T pop,  sei,  reti,  

0 [if] \ ---[ timeout flag ]  v 
\
\ no need for assembly definition, just use |c@
: timeout?  ( - ?)  \ $ff if timeout has occurred (does not reset)
   cli, 0 #, X push,  X' push,  
   [ 'tflag $ff and ] X ldi,   [ 'tflag 256 / $ff and ] X' ldi,
   T ldx, X' pop, X pop, sei, ;
\
[then]  \ --- [timeout flag]  ^   
\ 
[then] \ --------------------- 100 ms timer on T2 -----------------------------
\
\ 
\ -----------------------------------------------------------------------------
0 [if] \                     Revision History
\ -----------------------------------------------------------------------------
\ 
Date     By    Description
======   ===   ================================================================
160723   rjn   production version for HUMTE demo.
160714   rjn   initial version, cloned from t2-timer.fs, modified for 16 MHz
\ 
[then] \ ----------------------------------------------------------------------

