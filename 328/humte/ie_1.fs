\ ie_1.fs -- incremental encoder -- 160721rjn
\
\ 
0 [if] ---------------------------[ Notes ]------------------------------------
\ 
1. Use lead change interrupt on INT0 (PD2).  This is the "A" lead.
2. Use INT1 (PD3) as the "B" lead but don't use interrupt (not full count yet).
3. To simulate a quadrature encoder, use PD4 driving PD2 (INT0) and PD6
   driving PD3 (INT1).  Set both in quadrature in main.fs .
4. For transitions from cw to ccw or ccw to cw, not all state changes are
   possible. If the phase change is calculated on the next A edge change 
   (i.e., the ISR doesn't miss an edge), the possible state changes are:
      
      cw to ccw:      
         prev 11  00    
         cur  01  10
      (prev A xor cur B)=0, direction is ccw
                     
      ccw to cw:      
         prev 10  01    
         cur  00  11
      (prev A xor cur B)=1, direction is cw
\ 
[then] \ ----------------------------------------------------------------------
\
: /int0   $01 #, EICRA #, |! ( lead change)  $01 #, EIMSK #, |! ( enable) ;
\ : /int1   $0c #, EICRA #, |! ( rising edge)  $02 #, EIMSK #, |! ( enable) ;
\  
\ --- 32-bit accumulator
cpuHERE constant 'ecount  variable ecount   \ LSBs
cpuHERE constant 'ecount1 variable ecount1  \ MSBs
\ 
\                                                     prev  cur   
\ previous & current A&B bits and direction flag (D): D0AB  00AB
18 constant p&c  
\ 
: +iea  \ cw, increment accumulator (can't be a macro!)
   1 N ldi,    T ldx,  N T adc,   T stx+,
   0 N ldi,    T ldx,  N T adc,   T stx+,
   ( 0 N ldi,) T ldx,  N T adc,   T stx+,
   ( 0 N ldi,) T ldx,  N T adc,   T stx,
   $80 p&c ori, ( set cw flag)  \ equivalent to using sbr,
;
\ 
: -iea  \ ccw, decrement accumulator (can't be a macro!)
   1 N ldi,    T ldx,  N T sbc,   T stx+,
   0 N ldi,    T ldx,  N T sbc,   T stx+,
   ( 0 N ldi,) T ldx,  N T sbc,   T stx+,
   ( 0 N ldi,) T ldx,  N T sbc,   T stx,
   $7f p&c andi, ( ccw, so clear cw flag)  \ equivalent to using cbr,
;
\ 
:m cura  PD2 PIN sbic, 2 p&c ori, m;
:m curb  PD3 PIN sbic, 1 p&c ori, m;
\ 
\ save prev A&B to N, move to upper nibble, update current A&B bits in lower
:m ?p&c  
   p&c N' mov,  \ save current A&B to N' (N' is D0AB 00AB)
   p&c swap, $30 p&c andi,    \ move cur bits to prev (N' is 00AB 0000) 
   cura curb m; target
\    PD2 PIN sbic, 2 p&c ori,            \ get current A bit (00AB 00A0)
\    PD3 PIN sbic, 1 p&c ori, m; target  \ get current B bit (00AB 00AB)
\ 
: @p&c  ( - n)   0 #, cli, p&c T mov, sei, ;
\ 
$02 interrupt  \ --- INT0 ISR
   cli,  T push,  X push,  X' push,  N push, N' push,
   [ 'ecount $ff and ] X ldi,   [ 'ecount 256 / $ff and ] X' ldi, 
   ?p&c  \ cur A&B in lower nibble of p&c, prev A&B in lower nibble of N'
   p&c N mov, N N adc,  \ left-shifted cur A&B in N 
   \ at this point, prev A and cur B bits are in bit 1 of N' and N
   N N' xor, \ xor prev A with cur B, result in bit 1 of N'
   1 N' sbrc, +iea ( cw) 1 N' sbrs, -iea ( ccw)
   N' pop,  N pop,  X' pop,  X pop,  T pop,  sei, 
reti,
\ 
\ 
-: /encoder  
      ( $ffff) 0 #, 'ecount #, |!   ( 7fff) 0 #, 'ecount1 #, |! 
      0 p&c ldi,  cura curb ;
-: @encoder-lower  'ecount #,  cli, |@ sei, ;
-: @encoder-upper  'ecount #,  cli, |@ sei, ;
-: @encoder  ( - d )    'ecount1 #,  'ecount #,  cli, |@  swap |@ sei,  ; 
\ 
\ 
0 [if] \ --- testing
\ 
\ --- test INT1 by toggling on-board LED on rising edge
\ $04 interrupt  cli,  PB5 toggle,  sei,  reti, 
\ 
\ --- test INT0 by toggling an output on every A lead change
\ $02 interrupt  cli,  PB4 toggle, sei, reti,
\ 
:m test-add  \ 4-byte accumulator using ram variables -- ~4.32 usec
   PB4 high,
   cli,  T push,  X push,  X' push,  N push, N' push,
   [ 'ecount $ff and ] X ldi,   [ 'ecount 256 / $ff and ] X' ldi, 
   ?p&c  \ cur A&B in lower nibble of p&c, prev A&B in lower nibble of N'
   p&c N mov, N N adc,  \ left-shifted cur A&B in N 
   \ at this point, prev A and cur B bits are in bit 1 of N' and N
   N N' xor, \ xor prev A with cur B, result in bit 1 of N'
   1 N' sbrc, +iea ( cw) 1 N' sbrs, -iea ( ccw)
   N' pop,  N pop,  X' pop,  X pop,  T pop,  sei, 
   PB4 low,
m;  target
\ 
:m test-add1  \ 16 bits, 1.94 usec
   PB4 high,
   cli,  T push, T' push,  X push,  X' push,
   [ 'ecount $ff and ] X ldi,   [ 'ecount 256 / $ff and ] X' ldi,
   T ldx+, T' ldx,  1 T adiw,  T' stx, T -stx,
   X' pop, X pop, T' pop, T pop, 
   PB4 low,
m;  target
\ 
[then] \ --- testing ----------------------------------------------------------
\
0 [if] \ --- Charley's code to increment 16-bit variable using registers
\ 
18 constant E
19 constant E'

: ++  -1 E subi, 0if, -1 E' subi, then ;
: //  0 E ldi,  0 E' ldi, ;
: ??  ?dup  E T mov,  E' T' mov, . ;
[then] \ --- Charley's code to increment 16-bit variable using registers
\ 
\ 
0 [if] \ --- example code for variable manipulation
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
[then] \ --- example code for variable manipulation
\  
\ 
0 [if]  \ --- C++ examples from the web

// xor pin B with previous A -- full resolution (?)
// Interrupt on A changing state
void doEncoderA(){
  Bnew^Aold ? encoder0Pos++:encoder0Pos--;
  Aold=digitalRead(encoder0PinA);
}
// Interrupt on B changing state
void doEncoderB(){
  Bnew=digitalRead(encoder0PinB);
  Bnew^Aold ? encoder0Pos++:encoder0Pos--;
}

//  same method, better coding (?)
// Interrupt on A changing state
void doEncoderA(){
  // if Bnew = Aold, increment, otherwise decrement
  Bnew^Aold ? encoder0Pos++:encoder0Pos--;
  Aold=digitalRead(encoder0PinA);
  // check for underflow (< 0) 
  if (bitRead(encoder0Pos, 15) == 1) encoder0Pos = 0;
  // check for overflow (> 1023)
  if (bitRead(encoder0Pos, 10) == 1) encoder0Pos = 1023;
  constrain(encoder0Pos, 0, 1023);
}

// Interrupt on B changing state
void doEncoderB(){
  Bnew=digitalRead(encoder0PinB);
  // if Bnew = Aold, increment, otherwise decrement
  Bnew^Aold ? encoder0Pos++:encoder0Pos--;
  // check for underflow (< 0) 
  if (bitRead(encoder0Pos, 15) == 1) encoder0Pos = 0;
  // check for overflow (> 1023)
  if (bitRead(encoder0Pos, 10) == 1) encoder0Pos = 1023;
}
[then]  \ --- C++ examples from the web
\
\
0 [if] ---------------------[ Revision History ]-------------------------------
\ 
Date	  By  Description
======= === ===================================================================
160721  rjn fixed @encoder -- must get both bytes while interrupt disabled
160504  rjn working version
160428  rjn initial version
\ 
[then] \ ----------------------------------------------------------------------

