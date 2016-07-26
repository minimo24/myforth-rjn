\ nokia_1.fs -- Nokia graphics lcd driver, v1 -- 160721rjn
\  
\   
0 [if] \ ----------------[ Nokia LCD Connections ]---------------------------
\ 
1. For Nano
2. Also see schematic in doc directory and init.fs .
3. Nano pins per DIP convention (pins from 1 to 30, as if viewing a DIP
   from the top).
4. Because of signal inversion for level conversion, Nokia signals are 
   inverted as noted below.  Also see schematic.
\ 
LCD Fx      Port/	   Nano  Description
Pin   	   Pin      Pin
--- -------	------	---   -------------------------------------------------
 1  +3.3V  	---	   ---   NOKIA LCD is 3.3 Volts only (supplied from 78L33)
 2  GND  	---	   4,29  
 3  SCE     D9/PB1   12    driven through level converter gate
 4  RST     D7/PD7   10    driven through level converter gate
 5  D/C		D6/PD6    9    driven through level converter gate
 6  MOSI    D11/PB3  14    driven through level converter gate
 7  SCLK    D13/PB5  16    driven through level converter gate
 8  LED     PD6       9    driven by PD6 (PWM output) directly
\ 
[then] \ ----------------[ Nokia LCD Connections ]--------------------------- 
\ 
\ --- Nokia I/O
:m sce   pb1 m;
:m rst   pd7 m; 
:m d/c   pd5 m; 
:m mosi  pb3 m; 
:m sck   pb5 m; 
:m led   pd6 m; target
\ 
\ 
0 [if] \ --------------------[ Initialization ]-------------------------------
\ 
\ --- 328p
\ 
1. SPCR Register:    -SDM CCSS
                     -POS PPPP
                     -ERT OHRR
                     ||DR LA10
                     |||| ||||
                     vvvv vvvv
                     0101 0000  
                     
   + clear SPR 0,1 for 16/4 = 4 MHz SPI clock.
   + DORD=0 for data sent MSB first.
   + CPOL=0 (clk polarity) for data clock idle when low.
   + CPHA=0 (clk phase) for data sample on rising edge of clock.

2. SPSR Register:    SWxx xxxS
                     PC|| |||P
                     IO|| |||I
                     FL|| |||2
                     |||| |||X
                     |||| ||||
                     vvvv vvvv
                     0000 0000 (default)

   No need to init this register.  Default is $00 and SPIE and WCOL are flags.
   SPI2X is ok at 0 for CLK/64.   
\ 
\ --- Nokia 5110 controller
\ 
3. Datasheet for the PCD8544 controller says that (low-true) reset must be 
   applied within 100 ms of Vdd application to the chip.  See /nokia below.
4. D6 drives the Nokia's LED input directly because the 5 Volt PWM on that 
   output drives backlight LEDs.  With a 330 Ohm series resistor,
   the LEDs draw around 6 mA when the PWM signal is at max duty cycle.
5. All but SCK, MOSI & D6 assignments are somewhat arbitrary (GP I/O).
6. The Nokia can be supplied by the Nano's 3.3 Volt output which typically
   is derived from the USB converter chip and is rated at about 50 mA.
7. Note that each I/O pin is limited to 40 mA and the total 328P draw is
   limited to 200 mA.  These limits should not be pushed (e.g., limit to
   35 mA for an output pin).  The Nano usually has a resettable fuse rated
   for 500 mA in series with the 5 Volts from the USB connector to avoid
   damaging the USB port.
\ 
[then] \ --------------------[ Initialization ]-------------------------------
\
\ --- backlight commands 
\
\ Note: fine control requires t0-fast-pwm.fs
\ 
\ --- backlight levels, PWM on D6
\ : bn    backlight high, ;  \ backlight on
\ : bf    backlight low,  ;  \ backlight off
-: (bs)  ( n -)   OCR0A  #, |! ;
: pwm   OCR0A #, |c@ ;
: bf     $01 #,  (bs) ;    \ off
: bh     $c0 #,  (bs) ;    \ high
: bl     $40 #,  (bs) ;    \ low
: bm     $7f #,  (bs) ;    \ $7f=50% PWM
: bn     $ff #,  (bs) ;    \ on
: bs    ( n/mt -)   depth if drop (bs) ; then drop ;  \ set value from stack

e-cell backlight
: @ebl  ( - n)   backlight #, eread ;
: !ebl  ( n -)   backlight #, ewrite ;
\ : /backlight  $ff #, >backlight ;
: set-backlight  @ebl  bs ;
: save-backlight  pwm !ebl ;
\ 
\ --- backlight alert
-: (bas)  ( n -)   
   OCR0A #, |@  swap for 25 #, ms dup (bs)  65 #, ms  bf next  (bs) ;
: bas  ( n -)   depth if/ (bas) ; then ;
: ba  5 #, (bas) ;
\ 
\ --- command/data setup
-: command   d/c low,  ;
-: data      d/c high, ;
-: select    sce low,  10 #, us  ;
-: deselect  10 #, us sce high, ;

-: /spi  $50 #,  SPCR #, |!  $00 #,  SPSR #, |! ;
\ --- at 4 MHz clock, 250 ns per bit.  Sending a byte = 8x0.25 = 2 usec
-: !spi  ( n -)   SPDR #, |!  2 #, us ;  \ send spi byte, MSB first
-: 0spi  0 #, !spi ;  \ needed to clear spi output register?

-: extended  $21 #, !spi ;  \ extended instruction set
-: basic     $20 #, !spi ;  \ basic instruction set

\ chip active, horizontal addressing, basic instruction set
-: /function   command  $10 #,  !spi ;

\ --- display modes
: normal   command basic  $0c #, !spi ;
: all-on   command basic  $09 #, !spi ;
: blank    command basic  $08 #, !spi ;
: inverse  command basic  $0d #, !spi ;

\ --- positioning
cpuHere constant ~x 1 cpuALLOT  \ current x (col)
cpuHere constant ~y 1 cpuALLOT  \ current y (row)

: !x  ( n -)   dup ~x #, |c! command $7f #, and  $80 #, or  !spi  data ;
: !y  ( n -)   dup ~y #, |c! command $07 #, and  $40 #, or  !spi  data ;

: @x  ( - n)   ~x #, |c@ ;
: @y  ( -n)    ~y #, |c@ ;
: @xy ( n1 n2)   @x @y ;

: !xy  !y  !x ;  \ set xy
: 0xy   0 #, dup !xy ;  \ x,y = 0,0

\ --- fill row with character
cpuHere constant ~fill 1 cpuALLOT

-: !fill  ( n -)   ~fill #, |c! ;
-: (rfill) 
   data  84 #, for ~fill #, |c@ !spi next ;  \ fill row
: rfill  ( n -)  normal !fill (rfill) ;   
: /row   $00 #, rfill ;  \ erase row, set xy first 

\ --- erase screen (blank only blanks the display)
: erase   0xy  6 #, for /row next ;      

-: spacing  0spi 0spi ;  \ character spacing

: clear  0xy normal erase ;
: pattern  clear $a5 #, !fill  6 #, for (rfill) next ;  \ for demo

-: i>char  ( index -)  \ display char, indexed
   data 5 #, * schars #, + p!
   c@p+ !spi  c@p+ !spi  c@p+ !spi  c@p+ !spi  c@p+ !spi spacing ;
\ 
0 [if] \ --- test
\ 
-: iline  ( i -)  \ display one line of chars, starting at index
   12 #, begin swap dup i>char 1 #+ swap 1 #- while repeat drop drop ;

: stest  clear       0 #, iline  \ display schar set on display
         1 #, !y  12 #, iline 
         2 #, !y  24 #, iline  
         3 #, !y  36 #, iline  
         4 #, !y  48 #, iline  
         5 #, !y  60 #, iline  0spi ; 

: ftest  clear  $aa #, !fill  data
    40 #, for 0spi  next 
    44 #, for ~fill #, |c@ !spi  next ; 
\ 
[then]    
\ 
: nchar  ( ascii -)  $20 #- zpush i>char zpop ;  \ output ascii char
\ 
\ --- strings
-: ntype ( a n)  swap p! begin c@p+ nchar 1 #- while repeat drop ;
\ 
\ --- string test (zero appears at 0xy -- wrapped)
-: (nstring)  s" &1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ" ntype 
              s" abcdefghijklmnopqrstuvwxyz~!@#$%^&*0"  ntype ;
: nstring  clear (nstring) ;  
: ~nstring clear inverse (nstring) ;
: nspace  s"  " ntype ;
\  
\ --- numeric display
\
\ --- supress leading zeroes
16 constant nozap  \ only certain regs work, don't use 14, N or N'
:m /zap  0 nozap ldi, m; target
-: ?zap  ( n - n')   0 nozap ori, if, ; then, 16 #- ;  \ could be macro
-: (?nchar)  ( index -)  \ remove leading zeroes
   dup 16 #- if/ $ff nozap ldi, ; then ?zap ; 
-: ?nchar  ( ascii -)   $20 #- (?nchar) zpush i>char zpop ;
\ 
-: ~nchar  apush ?nchar apop ;
-: #ntype ( adr len)   /zap  swap a! for zpush c@+ ~nchar zpop next ;
\ 
-: ?0nh.  ( n -)  dup if drop (h.) #ntype ; then drop drop s"    0" ntype ;
-: ?0nud.  ( d -)   \ 
   2dup negate |+ if/ <# #s #> #ntype ; then 
      drop drop s"       0" ntype ;
\ 
\ note: Entering caps in a hex number results in bad display.
\       For example, try entering "hex 00AB nh." vs. "hex 00ab nh."
: nh.   ( n)  depth if/ ?0nh.  ; then ;
: nud.  ( d)  depth if/ ?0nud. ; then ;
: nu.   ( n)  0 #, ?0nud. ;
\  
0 [if] \ test
: nhtest  ( n -)   clear  nh. 0spi ;
: nudtest  ( d -)  clear nud. 0spi ;
[then]
\ 
0 [if] \ -----------------------[ Big Chars ]---------------------------------
\
1. Use 5x7 character set to generate a larger character set.  Expand 5x7
   to 10X14 .  So, each dot is now a big dot of four pixels.  
   A X4 magnification of the 5X7 character set.
2. >big -- The workhorse definition that does the following:
      + Takes a byte from the character set and creates two bytes with
        "doubled" bits.
      + The input byte from the 5X7 character set is translated to bytes in
        the upper and lower bytes of TOS.  The original input is lost.
      + The lower byte (in T) is output twice at the current cursor location
        to double up the pixels in the current row.
      + The upper byte (in T') is used to make the lower half of the
        magnified charater.  Its bits are doubled up, like the lower byte,
        and it is output twice to double up the pixels in the current row.
      + Before outputting the lower doubled byte, the row is incremented and
        the column is set back to its starting position.  This places the
        bottom pixels directly under the top pixels.
      + Before exiting, the original row is restored and the column is 
        advanced by two (stripes).  Thus, all is set up for the next doubled
        character.
3. >big coding -- 
      + The coding for >big may appear a bit non-intuitive.  The
        idea is to write two 1s or 0s for each bit in the source nibble, with
        the first nibble filling the first doubled "target" byte and the second 
        nibble filling the second doubled target.  This is done by setting all 
        of the bits in a scratch register (0 N ldi,) so that it can be used to 
        assemble the doubled byte.
      + Each of the bits in the T register are tested and bits are set in
        pairs in the scratch byte.  This is done with the sbrc, instruction,
        writing the doubled bits.  Thus, "0 T sbrc, $03 N ori," checks bit
        0 of T and skips the following instruction if it is not set, thus
        leaving the zeroes written at initialization.  If bit 0 of T is set,
        the following instruction is not skipped.  When it executes, it just
        ORs a doubled-bit mask to the scratch register.
      + Once four bits of T are processed, the scratch register is written
        to another scratch register (N').  The scratch is then processed
        as described above.
      + After T has been fully processed, N' contains the first processed
        nibble and N contains the second.  These are written to T and T',
        destroying the original top of stack.  The stack has the bytes for
        the two vertical stripes needed for a bigger character set.
\
[then] \ -----------------------[ Big Chars ]---------------------------------
\ 
-: >big  ( byte - n)  
   N push, N' push,
   0 N ldi,  \ 0 N' ldi,
   0 T sbrc, $03 N ori,  1 T sbrc, $0c N ori,  \ process LSB nibble of byte  
   2 T sbrc, $30 N ori,  3 T sbrc, $c0 N ori,
   N N' mov,  0 N ldi,
   4 T sbrc, $03 N ori,  5 T sbrc, $0c N ori,  \ process LSB nibble of byte  
   6 T sbrc, $30 N ori,  7 T sbrc, $c0 N ori,
   N' T mov,  N T' mov,
   N' pop, N pop, ;
  
\ sends the lower byte of the stack register (T) to the Nokia, twice to 
\ double up pixels horizontally.
:m !double  ( n -)   0 T' ldi, dup !spi !spi m; target

\ note: "if" is true for any non-zero value, wrap is done on zero
-: ?col  ( n -)  \ wrap col? 
   @x 84 #, |negate + if drop ; then drop 0 #, !x  @y 2 #+ !y ; 

-: ?row ( n -)   @y  6 #- if drop ; then drop 0 #, !y ; \ wrap row?

-: !big  ( n -)  \ do one big column byte, set xy and normal/inverse first  
   data
   >big  dup !double          \ do upper stripe
   @y 1 #+ !y  @x !x  \ 2nd row, prev col
   T' T mov, !double          \ swap bytes, do lower stripe 
   @y  1 #- !y  @x 2 #+ !x  \ original row, col+2, for next stripes
   ?col  ?row ;  \ check for col and row wrap

-: i>big  ( index -)  \ display char, indexed
      5 #, * schars #, + p!
      c@p+ !big  c@p+ !big  c@p+ !big  c@p+ !big  c@p+ !big  
      0 #, !big ( spacing) ;

0 [if] \ test big character set, wrapping (works 160606rjn)
\ 
: bigtest  \ first character will end up being an "E" (overwrite)
   clear  15 #, 22 #, for 1 #+ dup zpush i>big zpop next drop ;
\ 
[then]
\
: bchar  ( ASCII -)  $20 #- zpush i>big zpop ;  \ output big ascii char
\ 
\ suppresses leading zeroes
-: ?bchar  ( ascii -)   $20 #- (?nchar) zpush i>big zpop ;   
\ 
\ --- big strings
-: btype ( a n)  /zap swap p! begin c@p+ bchar 1 #- while repeat drop 0spi ;
\ 
\ --- Big string test
-: (bstring)  s" *123456789ABCDEuvwxyz0" btype 0spi ; \ 0 appears at 0xy
: bstring  clear (bstring) ;
: ~bstring  clear inverse (bstring) ;
: bspace s"  " btype ;
\  
\ --- big numeric display
\
-: ~bchar  apush ?bchar apop ;  \ suppress leading zeroes
-: #btype ( adr len)   /zap swap a! for zpush c@+ ~bchar zpop next ;
\ 
-: ?0bh.  ( n -)  dup if drop (h.) #btype ; then drop drop s"    0" btype ;
-: ?0bud.  ( d -)  \ largest for a line is $98967F (9,999,999)
   2dup negate |+ if/ <# #s #> #btype ; then drop drop s"       0" btype ;
-: ?0bu.1  ( d -)  
   2dup negate |+ if/ <# # char . #, hold # # # #> #btype ; then 
      drop drop s"   0.0" btype ;
\
: bh.     ( n)  depth if/ ?0bh.  ; then ;
: bud.    ( d)  depth if/ ?0bud. ; then ;
: bu.   ( n)  0 #, ?0bud. ;
: bu.1  ( n)  0 #, ?0bu.1 ;
\  
0 [if] \ test
: bhtest  ( n -)   clear  bh. 0spi ;
: budtest  ( d -)  clear bud. 0spi ;
[then]
\ 
0 [if] \ -----------------------[ Contrast ]----------------------------------
\ 
1. The big weakness in the Nokia display is the contrast settings.  First,
   they are temperature sensitive so may require adjustment depending on
   the application's environment.  There are temperature compensation values
   that can be loaded, but this was not implemented in this driver.
2. There also seems to be wide variations between displays and the pressure
   mounting of the escution plate may also affect the display.
3. It is recommended that there be some way for the user to adjust the
   contrast, but this normally requires a menu or an external switch or two.
4. This driver allows contrast to be set at startup by displaying the setting
   every few seconds and letting the user press the reset button on the
   Nano when the setting looks good.  This may take several passes.  The
   displayed setting is written to eeprom and before delaying and displaying
   the next value.  Thus, when the user presses the reset button, the
   last-displayed contrast setting is in eeprom, ready for restoral at startup.
\ 
[then] \ -----------------------[ Contrast ]----------------------------------
\ 
: !ncon  ( n -)  command  extended  !spi  basic  ;  
\ : /con  $bf #, !ncon  ;  \ recommended, a bit light for my display
\ : /con  $ca #, !ncon  ;   \ very sensitive to value

e-cell econ
: @econ  ( - n)   econ #, eread ;
: !econ  ( n -)   econ #, ewrite ;
: /econ  $df #, !econ ;  \ for interactive setting of default
: set-contrast   @econ !ncon ; 
: new-contrast  $ff #, !econ ;

-: 'contrast   clear s" Push reset"   ntype 
   0 #, !x  1 #, !y  s" to set value" ntype
   0 #, !x  2 #, !y  s" for contrast" ntype ;

3000 constant persistence  \ transient message persistence
: perm  persistence #, ms ;
: peek    perm clear ;

-: ic   'contrast
   $d5 #, dup !ncon 
   $18 #, for    
   8 #, !x  4 #, !y
   zpush 1 #- dup !ncon dup dup !econ  bh. zpop  perm
   next  drop ;

\ : photo2  clear 'contrast  8 #, !x  4 #, !y  @contrast bh. ; \ for photo  

\ contrast is $ff if new download or if /contrast is executed interactively
-: ?fresh   @econ $ff #, |xor if drop ; then drop ic ;

: ncon   0 #, 1 #, !xy  s" contrast" ntype  @econ nh.  ;

\ to reset contrast: new-contrast init-contrast
: init-contrast  ?fresh  @econ !ncon ncon ;

: ~reset  rst low, 1 #, ms  rst high, 10 #, us ;
\ 
-: /nokia  rst high,  sce high,  d/c high,  sck low,  mosi high,
   /spi  ~reset  select  /function  set-backlight  init-contrast ;           
\ 
\ -----------------------------------------------------------------------------
0 [if] \                         Revisions
\ -----------------------------------------------------------------------------
\ 
Date     By    Description
======   ===   ================================================================
160721   rjn   misc. cleanup, added nspace, bspace.  Fixed ?0nud., etc. (no #s)
160713   rjn   initial version, cloned from nokia.fs -- extensions for 
               DHT22 temperature, humidity display.

[then] \ ----------------------------------------------------------------------

