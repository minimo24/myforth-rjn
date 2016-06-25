\ t2-ticks.fs  SERDY tick timer - 160422rjn
\ 
\ 
0 [if] \ ------------------------[ Notes ]-------------------------------------
\
1. Use with PROSE for 100 ms timing.  Also see new_timer.fs and system file
   timer.fs .

2. For a clock divisor of 64:

   : init-interrupt 
      $03 ~#, TCCR2A #, and!  \ normal mode
      $08 ~#, TCCR2B #, and!  \ no wgm22, clear bit 3
      $20 ~#,   ASSR #, and!  \ source = I/O clock
      $01 ~#, TCCR2B #, and!  \ scale by 64, clear bit 0
      $02 ~#, TCCR2B #, and!  \ scale by 64, clear bit 1
      $04  #, TCCR2B #, or!   \ scale by 64, set bit 2
      131 #,   TCNT2 #, !     \ set count in TCNT2
      $01 #,  TIMSK2 #, !     \ overflow interrupt enable
      sei, ;

      TCNT2    rate     freq
      -----    -------  ---------
      131       500 us   1.0 kHz
      200       224 us   2.2 kHz
      225       124 us   4.0 kHz
      252      ( intereferes with interpreter)

3. For a clock divisor of 128:

   : init-interrupt 
      $03 ~#, TCCR2A #, and!  \ normal mode
      $08 ~#, TCCR2B #, and!  \ no wgm22, clear bit 3
      $20 ~#,   ASSR #, and!  \ source = I/O clock
      $02 ~#, TCCR2B #, and!  \ scale by 128, clear bit 1
      $05  #, TCCR2B #, or!   \ scale by 128, set bits 0,2
      131  #,  TCNT2 #, !     \ set count in TCNT2
      $01 #,  TIMSK2 #, !     \ overflow interrupt enable
      sei, ;

      TCNT2    rate     freq
      -----    -------  ---------
        1      2.02 ms  245.3 Hz
       10      1.96 ms  254.2 Hz
      100      1.24 ms  400.9 Hz
      131      1.01 ms  500.3 Hz 
      200       448 us    1.1 kHz
      250        48 us   10.4 kHz
      252        32 us   15.6 kHz
      254      ( interferes with interpreter)

4. Divisor of 1024:

      TCNT2    rate          freq
      -----    -----------   -----------
      100       9.9774  ms    50.1134 Hz  \ calc. 9.984 closest fit to 10.0 ms
                                            \ measured 50.1 Hz on counter
       99      10.0     ms    49.79   Hz
       98      10.1     ms    49.48   Hz
       97      10.1     ms    49.2    Hz
       92      10.4     ms    47.7    Hz
       85      10.9     ms    45.7    Hz
      131       7.9     ms    62.5    Hz
      145       7.00    ms    70.43   Hz
      167       5.68    ms    87.84   Hz
      176       5.08    ms    97.72   Hz
      177       5.04    ms    98.96   Hz
      178       4.96    ms   100.23   Hz  \ calculated: 256-[5000/(16/1024)]
                                            \ note: ~64 us/count
      192       4.08    ms   122.15   Hz
      254     128.0     us     3.9   kHz
\
[then] \ ----------------------------------------------------------------------
\ 
\ -----------------------------------------------------------------------------
1 [if] \                      100 ms timer on T2
\ -----------------------------------------------------------------------------
\
cpuHERE constant 'tcounter  variable  tcounter \ counts timer2 ticks
cpuHERE constant 'tflag     cvariable tflag    \ flags secondary count

: and! ( n a)  swap over  @ and swap ! ;
: or!  ( n a)  swap over  @ or  swap ! ;

: /timer2  \ ~ 1 ms tick, 8 MHz clock
      $03 ~#, TCCR2A #, and!  \ normal mode
      $08 ~#, TCCR2B #, and!  \ no wgm22, clear bit 3
      $20 ~#,   ASSR #, and!  \ source = I/O clock
\ --- scale by 64      
      $03 ~#, TCCR2B #, and!  \ clear bits 0,1
      $04  #, TCCR2B #, or!   \ set bit 2
      131  #,  TCNT2 #, !     \ set count for ~ 1 ms (not exact!)
      $01  #, TIMSK2 #, !     \ overflow interrupt enable
        0  #, tcounter  !     \ init timer counter
        0  #, tflag     c!
      sei, ;

\ : itick ; \ marker for see

$12 interrupt  \ ~100 ms tick
   cli,  T push, X push,  X' push,  N push,
   \ --- increment timer counter
\   PB5 toggle,  \ view timer interval on scope (498.586, tick not exact)
   [ 'tcounter $ff and ] X ldi,   [ 'tcounter 256 / $ff and ] X' ldi,
   1 N ldi, T ldx,  N T add,   T stx+,
   0 N ldi, T ldx,  N T adc,   T stx,
   \ --- conditionally reset tcounter, signal foreground with tflag
   T -ldx,  \ load lsb of tcounter
   100 N ldi,  N T sub,  0if,   \ tcounter count down
      T stx+,  T stx,  \ zero tcounter
      \ --- set tflag to signal 100 ms timeout
      [ 'tflag $ff and ] X ldi,   [ 'tflag 256 / $ff and ] X' ldi,
      $ff T ldi, T stx,  
      \ PB5 toggle, \ view final 100 ms timing on scope
   then, 
   \ --- reset timer interval here:
    131 T ldi, ( 1 ms) TCNT2  X ldi,  0 X' ldi,  T stx,
   N pop,  X' pop,  X pop,  T pop,  sei,  reti,  
      
: timeout?  ( - ?)  
   cli, 0 #, X push,  X' push,  
   [ 'tflag $ff and ] X ldi,   [ 'tflag 256 / $ff and ] X' ldi,
   T ldx, X' pop, X pop, sei, ;

\ -: ?100ms  tflag cli, |c@  if/ 0 #, tflag |c! PB5 toggle, then sei, ; ( ok)
\ -: ?100ms  timeout? if/ 0 #, cli, tflag |c! sei, PB5 toggle, then ;
\ 
[then] \ ----------------------------------------------------------------------
\
\ -----------------------------------------------------------------------------
0 [if] \                            test 1
\ -----------------------------------------------------------------------------
\ 
cpuHERE constant 'tens  variable tens
\ 
: tens+
   X push,  X' push,  T push,  
   [ 'tens $ff and ] X ldi,   
   [ 'tens 256 / $ff and ] X' ldi,
   1 N ldi,
   T ldx,  N T add,  T stx+,  0 N ldi,
   T ldx,  N T adc,  T stx, 
   T pop,  X' pop,  X pop, ;

: /tens  0 #, tens ! ;
: @tens   tens @ ;
: tt   10 #, for @tens u. 200 #, ms  tens+ next ;
\
[then] \ ----------------------------------------------------------------------
\  
\ -----------------------------------------------------------------------------
0 [if] \                              test 2
\ -----------------------------------------------------------------------------
\ 
cpuHERE constant 'tens  variable tens

$12 interrupt  
   cli,  N push,  T push,  X push,  X' push,
\
\ --- tens counter for 100 ms tick
   [ 'tens $ff and ] X ldi,   
   [ 'tens 256 / $ff and ] X' ldi,
   1 N ldi,
   T ldx,  N T add,  T stx+,  0 N ldi,
   T ldx,  N T adc,  T stx, 
\ 
\ note: change timer interval here:
   100 T ldi, TCNT2 X ldi,  0 X' ldi,  T stx,
   X' pop,  X pop,  T pop,  N pop,
   ( ~pin8)  sei,  reti,

: .tens  cli, 'tens #, @ u. sei, ; 
\
[then] \ ----------------------------------------------------------------------
\ 
\ -----------------------------------------------------------------------------
0 [if] \                     Revision History
\ -----------------------------------------------------------------------------
\ 
Date     By    Description
======   ===   ================================================================
160422   rjn   initial version, cloned frem prose-timer.fs
\ 
[then] \ ----------------------------------------------------------------------

