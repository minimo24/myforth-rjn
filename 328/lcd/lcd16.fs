\ lcd16.fs -- 16-pin lcd driver for Arduino -- 160512rjn
\  
\   
0 [if] \ ----------------[ Nibble Mode LCD, 16-pin ]---------------------------
\ 
1. LCD draws about 2.5 mA.
2. Backlight can draw up to ~16 mA.
3. Pro Mini numbering assumes that pin 1 is labeled "10" or "D10",
   with board pins progressing as with a DIP chip (pin 24 is "D9").
\ 
LCD Port/	328P    	Pro   Description
Pin Pin  	Pin      Pin
--- -------	------	---   -------------------------------------
 1  GND   	5,3,21	11,16 Vss
 2  +5V  	4,6,18	 9    Vdd
 3  ---     3        --    contrast
 4  PD7     11       22    RS - Instruction Register Select
 5  GND		--	      11,16 R/W - H=read L=write registers
 6  PB1     24, D9   24    E
 7  ---		--	            DB0 (N/C)
 8  ---		--	            DB1 (N/C)
 9  ---		--	            DB2 (N/C)
10  ---		--	            DB3 (N/C)
11  PC0	   23	 A0         DB4
12  PC1     24  A1         DB5
13  PC2     25	 A2         DB6
14  PC3     26  A3         DB7
15  +5V		--       9     LED A through 100 Ohm resistor
16  GND     --       11,16 LED K to ground via FET
\ 
\ Note: D6 drives the backlight FET
\ 
[then] \ ----------------[ Nibble Mode LCD, 16-pin ]---------------------------
\ 
\ 
\ --- LCD I/O
:m db4  pc0 m; \ 160510rjn (for incremental encoder)
:m db5  pc1 m; \ 160510rjn
:m db6  pc2 m; \ 160510rjn
:m db7  pc3 m; \ 160510rjn
:m E    pb1 m; \ 160412rjn
:m RS   pd7 m; 
:m backlight  pd6 m; target  \ 160412rjn

: instruction  RS low,  ;
: data         RS high, ;

: strobe   E high, 100 #, us  E low,  250 #, us ;

-: !nibble  ( c - c)  \ send high nibble in T to LCD nibble
   db4 low, db5 low, db6 low, db7 low,
   4 T sbrc, db4 high,  5 T sbrc, db5 high,      
   6 T sbrc, db6 high,  7 T sbrc, db7 high,  strobe ;

-: (lcd)  ( c -)   \ output high & low nibbles to DB4-DB7
   !nibble  T swap, !nibble  drop ;
-: lcd  ( c -)   (lcd)  2 #, ms ;  \ display a character ( 2 ms seems ok)

: /lcd
   30  #, ms
   $30 #, !nibble  drop strobe strobe
   $20 #, !nibble  drop
   $28 #, lcd
   $0e #, lcd
   $01 #, lcd
   $02 #, (lcd)  
   data 10 #, ms ;
\ 
-: lcd-command  ( n -)   lcd  data  10 #, ms ;  \ 5 ms not enough
\ 
\ --- backlight commands 
\
\ Note: fine control requires t0-fast-pwm.fs
\ 
\ --- backlight levels
\ : bn    backlight high, ;  \ backlight on
\ : bf    backlight low,  ;  \ backlight off
-: (bs)  ( n -)   OCR0A  #, |! ;
: bf     $01 #,  (bs) ;    \ off
: bh     $c0 #,  (bs) ;    \ high
: bl     $40 #,  (bs) ;    \ low
: bm     $7f #,  (bs) ;    \ $7f=50% PWM
: bn     $ff #,  (bs) ;    \ on
: bs    ( n/mt -)   depth if drop (bs) ; then drop ;  \ set value from stack
\ 
\ --- decided not to use timer tick for timing - 160425rjn
\ sei, & cli, not needed on tflag access -- just finished setting flag
\ : 100ms-wait  0 #, begin drop timeout? until drop  0 #, tflag |c! ; 
\ 
\ --- backlight alert
: (bas)  ( n -)   
   OCR0A #, |@  swap for 25 #, ms dup (bs)  65 #, ms  bf next  (bs) ;
: bas  ( n -)   depth if drop (bas) ; then drop ;
: ba  5 #, (bas) ;
\ 
\ --- lcd commands
: 0lcd  instruction   1 #, lcd-command ;  \ reset lcd
: cf    instruction $0C #, lcd-command ;  \ cursor off  (display on)
: //    0lcd cf ;                         \ reset lcd, cursor off 
: ck    instruction $0D #, lcd-command ;  \ blinking char at cursor
: ca    instruction $14 #, lcd-command ;  \ move cursor right (advance)
: cb    instruction $10 #, lcd-command ;  \ move cursor left  (back)
: cn    instruction $0E #, lcd-command ;  \ cursor on
: ch    instruction   2 #, lcd-command ;  \ home cursor
: dl    instruction $18 #, lcd-command ;  \ scroll display one pos left
: dr    instruction $1C #, lcd-command ;  \ scroll display one pos right
: d/    ch  ck ;                          \ display home and blink
: da    instruction $0F #, lcd-command ;  \ "all on" -- display, cursor, blink
: db    instruction $0B #, lcd-command ;  \ display blank
: df    instruction   8 #, lcd-command ;  \ display off
: dn    cf ;                              \ display on, cursor off
\ 
\ --- display positioning
-: dlr-delay  400 #, ms ;  \ eyeballed display for value
-: ?drs  ( n -)   0max if for dlr-delay dr next ; then drop ;
-: ?dls  ( n -)   0max if for dlr-delay dl next ; then drop ;
\ 
: drs  ( n -)   depth if drop ?drs ; then drop ; \ n positions right
: dls  ( n -)   depth if drop ?dls ; then drop ; \ n positions left
\ 
\ --- blank(s)
: ds    $20 #, lcd ; \ send a blank to the display
-: ?dss  ( n -)  0max if for ds next ; then drop ;
: dss  ( n -)  \ n spaces from current cursor, disallows 0, empty stack 
   depth if drop ?dss ; then drop ;  \ n spaces
\ 
\ --- line and cursor positioning
: l1    instruction $80 #, lcd-command ;  \ set cursor to start of line 1
: l2    instruction $C0 #, lcd-command ;  \ set cursor to start of line 2
: /l1   l1 16 #, for ds next l1 ;
: /l2   l2 16 #, for ds next l2 ;
\   
-: (cas)  ( n -)   if for ca next then drop ;  \ cursor n right
-: (cbs)  ( n -)   if for cb next then drop ;  \ cursor n left
: cas  ( n -)   depth if drop (cas) ; then drop ;
: cbs  ( n/mt -)   depth if drop (cbs) ; then drop ;
\ 
\ --- strings, test
-: ltype ( a n)  swap p! begin c@p+ lcd 1 #- while repeat drop ;
: ltest  l1 s" 1234567890123456" ltype  l2 s" line2" ltype cf ;  \ test

\ --- numeric display
-: #ltype ( adr len)  0max if  apush
      swap a! for  c@+ lcd next  apop ; 
   then  2drop ;
: lh.   ( n)  depth if drop (h.) #ltype ds     ; then drop ;
: lud.  ( d)  depth if drop <# #s #> #ltype ds ; then drop ;
: lu.   ( n)  0 #, lud. ;

-: ?d. ( d)  dup push dabs <# #s pop sign #> ;
\ doesn't check for 2 items, just empty stack
: ld.  ( d)   depth if drop ?d. #ltype ds ; then drop ;
\ : 0< ( n - flag)  -if drop -1 #, ; then drop 0 #, ;
: l. ( n)  depth if drop dup 0< ld. ; then drop ;
\ 
\ --- text entry
\ 
\ "put" command -- enter text until CR, with terminal echo
: p    begin key dup 13 #, |xor while drop dup lcd emit repeat 2drop space ;
\  
\  
0 [if] \ -------------------------[ LCD timing ]------------------------------
\ 
-: '16-chars  lstring " 1234567890123456"
-: (0lcd-test)  line2 '16-chars ;
: 0lcd-test   begin ~clue (0lcd-test) ie-switch 0=until. ;  \ 15 ms
\ 
[then] \ -------------------------^ LCD timing ^------------------------------
\  
\ 
\ -----------------------------------------------------------------------------
0 [if] \                         Revisions
\ -----------------------------------------------------------------------------
\ 
Date     By    Description
======   ===   ================================================================
160511   rjn   added numeric display
160510   rjn   changed DB4-7 to be driven by PC0-PC3 (vs. PD2-5)
160424   rjn   added backlight alert commands (ba, bas)
160413   rjn   PWM backlight control works.  New b/l commands added
160412   rjn   changed backlight and E assignments.
160408   rjn   added backlight, changed command names
160331   rjn   Initial version.  /lcd, character I/O and commands work

[then] \ ----------------------------------------------------------------------

