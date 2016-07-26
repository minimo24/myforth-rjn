\ dht22.fs -- Driver for DHT22 temperature humidity sensor -- 160723rjn
\
\ 
0 [if] ---------------------------[ Notes ]------------------------------------
\ 
1. Uses a port pin in bidirectional mode to read the 1-wire sensor.
2. Based on example given in the March 2016 issue of Nuts & Volts.  The
   article is titled "More Hot & Sticky" by John McPhalen (page 18).
3. Data is transmitted using bit-width encoding, most significant bit
   first.
4. Every bit is preceded by a 50 usec low period (leader).  This was 
   measured at 52 microseconds.
5. A zero bit has a high period after the leader of about 28 microseconds 
   (measured 26).  A one bit has a high period of about 70 microseconds 
   (measured 76).  After the last bit is sent, the sensor releases the 
   data line and returns to its sleep state.
6. Sensor values and a checksum are transmitted as a packet of 5 bytes.
7. The first byte is humidity msb, the second byte is the humidity
   lsb.  The third byte is the temperature msb and the fourth byte is 
   the temperature lsb.  The fifth byte is a checksum.
8. The device cannot be queried too fast. For testing, the query rate
   was set at 5 seconds to ensure the device was always ready.
   The maximum read rate was not determined.
9. The bit data is accumulated by looking for the low to high edge of
   just after the leader (always low).  When the startin high edge of 
   a bit is detected, a delay is performed before sensing the DHT22's 
   output level.  
10. For a zero, the delay puts the sample in the middle of the next bit's 
    leader and the value read is low.  
    For a one, the delay puts sample approximately in the middle of a 
    the 80 usec high-level duration for the one.  
    Depending on the level, a bit is  written (or not) in a temporary 
    register.  Thus, bits are accumulated in the temporary register as 
    they are read.  Because all bits in the temporary register are 
    cleared before starting the bit-read process, only ones are written 
    when they are detected (zero bits are the default).
10. When 8 bits are accumulated in the temporary register, tbits, it  
    is written to a holding register, such as "hmsb" for the msb of the 
    humidity.  
11. After a complete 5-byte read, the holding registers are saved to 
    ram cells.  For example, the "hmsb" register is saved to the
    'hmsb cell.I
\ 
[then] \ -------------------------[ Notes ]------------------------------------
\ 
\ also see bu.1 in nokia1.fs
\ : u.1  ( n)  0 #, <# # char . #, hold # # # #> type space ;
\ 
:m dht   PB0 input,  m;  \ D8 
:m host  PB0 output, m;  target
\ 
\ use 6 temporary registers to acquire data
22 constant tbits  \ temporarily accumulates bits
21 constant hmsb
20 constant hlsb
19 constant tmsb
18 constant tlsb
17 constant chek
\ 
cpuHERE constant 'hmsb 2 cpuALLOT 
cpuHERE constant 'tmsb 2 cpuALLOT 
cpuHERE constant 'chek 2 cpuALLOT 
\ 
-: @humidity  ( - n)  'hmsb #, |@ ;
-: @ctemp     ( - n)  'tmsb #, |@ ;
-: @ftemp     ( - n)  @ctemp  400 #, |+  9 #, 5 #, */  400 #, negate |+ ;

-: by8   |2/ |2/ |2/ |2/ |2/ |2/ |2/ |2/ ;
-: @calc-chek  
   'hmsb #, |@  by8      'hmsb #, |@  $00ff #, |and  |+
   'tmsb #, |@  by8  |+  'tmsb #, |@  $00ff #, |and  |+
   $00ff #, |and ;

-: @chek   'chek #, |c@ ;

-: ?chek  \ inverted video display for bad checksum
   @chek @calc-chek |xor if/ inverse ; then normal ;
\ 
-: clipped-encoder  ( - byte)  \ clip encoder value to 0-ff range
   @encoder-lower ( - n)  $00ff #, and ;

-: ?forc  \ display F or C based on encoder position
   2 #, bas  0 #, 4 #, !xy
   clipped-encoder $7f #, negate |+  \ negative for 0-7f
   -if  drop @ctemp  bu.1  ?chek  s"  C" btype ; \ Celcius
   then drop @ftemp  bu.1  ?chek  s"  F" btype ; \ Fahrenheit
\ 
-: ie>nokia  58 #, 3 #, !xy  clipped-encoder nh. ; \ over the C or F char 
\ 
\ 
1 [if] \ --- working version with debug/test v
\ 
\ -: blip      PB2 toggle,  1 #, us  PB2 toggle, ; \ debug marker for scope
-: blip ;
-: edly  1 #, us ;  \ edge delay
:m +edge  edly begin PB0 set? until  ( PB2 high,) m;
:m -edge  edly begin PB0 clr? until m;  target
\ 
-: sample  -edge  +edge  45 #, us ;
\ 
-: @dht-bits   0 tbits ldi,
   sample  PB0 set? $80 tbits ori, blip blip  \ bit7
   sample  PB0 set? $40 tbits ori, blip       \ bit6
   sample  PB0 set? $20 tbits ori, blip       \ bit5
   sample  PB0 set? $10 tbits ori, blip       \ bit4
   sample  PB0 set? $08 tbits ori, blip       \ bit3
   sample  PB0 set? $04 tbits ori, blip       \ bit2
   sample  PB0 set? $02 tbits ori, blip       \ bit1
   sample  PB0 set? $01 tbits ori, blip       \ bit0
;
\
: @dht   
   host PB0 low,  ( PB2 low,)  25 #, ms   ( PB2 high,)  dht  
   +edge               \ wake, high for ~30 us
   45 #, us PB0 set? ; \ exit if no response
   -edge +edge         \ response: low then high for ~80 us 
\
\ after 50 us bit leader, a zero is ~26 us high; a one is ~80 usec high
\ 
\ --- first byte (humidity msb)
   @dht-bits   tbits hmsb mov,
\ --- second byte (humidity lsb)
   @dht-bits   tbits hlsb mov,  
\ 
\ --- third byte (temperature msb)
   @dht-bits   tbits tmsb mov,
\ --- fourth byte (temperature lsb)
   @dht-bits   tbits tlsb mov,  

\ --- fifth byte (checksum of previous four bytes)
   @dht-bits   tbits chek mov,
\ 
   host PB0 high,  ;  \ enter "idle" state
\ 
\ ---[ post acquisition processing ]
\ 
-: .dht
   @dht
\   cr  \ uncomment for console display
\ 
\ --- save humidity bytes, console display   
   0 #, hmsb T' mov, hlsb T mov,  'hmsb #, |! 
\   'hmsb #, |@  u.1  \ console
\
\ --- save temperature bytes, console display
   0 #, tmsb T' mov,  tlsb T mov, 'tmsb #, |!
\   'tmsb #, |@ u.1  \ console
\ 
\ --- save checksum byte, console display
   0 #, chek T mov, 'chek #, |!  
\    @chek @calc-chek |xor h.  \ console

\ --- Nokia display   
   0xy 'hmsb #, |@  bu.1         ?chek  s"  %" btype  \ Humidity
\   0 #, 4 #, !xy  @ctemp  bu.1  ?chek  s"  C" btype  \ Celsius
\   0 #, 4 #, !xy  @ftemp  bu.1  ?chek  s"  F" btype  \ Fahrenheit
   ?forc  \ display F or C based on IE position
; 
\  
[then] \ --- working version with debug/test ^
\ 
cpuHERE constant '5sec         2 cpuALLOT  \ DHT22 5 second counter
cpuHERE constant 'ie-timer     2 cpuALLOT  \ timer for IE display
\ 
-: /dht   0 #, '5sec #, |! ;
-: /ie-timer  0 #, 'ie-timer #, |! ;
-: /timers  /dht  /ie-timer ;
\ 
\ 
0 [if]  \ ---[ DHT22 timer using T2 ]  v
\ 
\  use /timer2 in init (see main.fs)
\ 
-: ?dht  \ executes DHT22 read every 5 seconds (timer-based)  
  'tflag #, cli, |c@  dup if/     \ $ff if timeout, resets flag if t/o
      0 #, 'tflag #, |c!  ( PB2 toggle,)  \ every 100 ms
      '5sec #, |@  1 #, + '5sec #, |!
   then  sei,  ( -  ?)
\
   if/ ie>nokia then
\
   '5sec #, |@  $32 #, negate |+ if/  ;
   then  ( PB2 toggle,) /dht  cli, .dht sei, ;   
\ 
[then]  \ ---[ DHT22 timer using T2 ]  ^ 
\ 
\ 
1 [if]  \ ---[ DHT22 timer, no T2 ]  v
\ 
\ Notes:
\ 1. Use /timers in init (see main.fs)
\ 2. Assumes that the main loop is nominally 1 ms duration for no activity
\ 
-: ?display-dht
   '5sec #, |@  1 #+ dup '5sec #, |! 
   5000 #, negate |+ if/ ; then ( PB2 toggle,) /dht  .dht ; \ DHT22 display
-: ?display-ie
   'ie-timer #, |@ 1 #+ dup 'ie-timer #, |!
   100  #, negate |+ if/ ; then /ie-timer  ie>nokia ; 
-: ?dht   ?display-ie  ?display-dht ;
\ 
[then]  \ ---[ DHT22 timer, no T2 ]  ^

\
\
0 [if] ---------------------[ Revision History ]-------------------------------
\ 
Date	  By  Description
======= === ===================================================================
160723  rjn changed ie>nokia to use @encoder-lower (more efficient)
160712  rjn working version
160701  rjn initial version
\ 
[then] \ ----------------------------------------------------------------------

