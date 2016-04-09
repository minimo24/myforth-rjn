\ init.fs -- DOG initialization -- 150504rjn
\
]  here  \ start marker
\ 
0 [if] ---------------------------[ Notes ]------------------------------------

1. Configures Arduino Pro Mini.  See Adafruit Arduino-Pro-Mini datasheet.

   Port	Pin      Label Type  Function
   ----	------   ----- ----  -------------------------------------------------
   PB1	JP7/1    D9    DO    GPIO
   PB5   JP6/9    SCK   DO    on-board LED
   PB6                        xtal1
   PB7                        xtal2
\ 
[then] \ ----------------------------------------------------------------------
\ 
0 [if] \ ---------------------------- Notes -----------------------------------
\
1. All of the above have been verified with a ATmega2560 board, except as 
   noted.
\
[then] \ ----------------------------------------------------------------------

:m ~PA0  PA0 toggle, m;  
:m ~PA1  PA1 toggle, m;  
:m ~PA7  PA7 toggle, m;  

\ :m ~PB7  PB7 toggle, m; 
:m ~PB7  7 #, PORTB toggle, m;  \ test -- why the #, ???????????

\ PG3, PG4 not brought out on board; no PG6 or PG7
:m ~PG0  PG0 toggle, m;  
:m ~PG1  PG1 toggle, m;  
:m ~PG2  PG2 toggle, m;  

\ PH0,1 are Tx and Rx; PH2 and PH7 not brought out to board connector
:m +PH   $fe N ldi,  N PORTH sts, m;  \ sets PH3,4,5,6
:m -PH   $00 N ldi,  N PORTH sts, m;  \ clears PH3,4,5,6

\ PJ0,1 are Tx3 and Rx3; PJ3-7 are not brought out to board connector
:m +PJ   $ff N ldi,  N PORTJ sts, m;  \ sets PJ0,1 
:m -PJ   $00 N ldi,  N PORTJ sts, m;  \ clears PJ0,1

\ PJ0,1 are Tx3 and Rx3; PJ3-7 are not brought out to board connector
:m +PK   $ff N ldi,  N PORTK sts, m;  \ sets PJ0,1 
:m -PK   $00 N ldi,  N PORTK sts, m;  \ clears PJ0,1

:m +PL   $ff N ldi,  N PORTL sts, m;  \ sets PL0-7 
:m -PL   $00 N ldi,  N PORTL sts, m;  \ clears PL0-7

-: /chip 
\ 
\ -----[ port A ]
\
    $ff N ldi, N DDRA out,  \ weak pullups default for unused pins
    PA0 output, PA1 output,  PA7 output,
\ 
\ -----[ port B ]
\
    $ff N ldi, N DDRB out,  \ weak pullups default for unused pins
    PB7 output, ( for PB7 led)
\
\ -----[ port C -- analog ]
\ 
\    0 N ldi,  N DDRC out,  \ all as inputs 
\    ( $ff N ldi,  N PORTC out,)
\ 
\ -----[ port D -- pins 0-7; 0,1 are serial]
\ 
\    0 N ldi, N DDRC out,  \ all as inputs
\    2 output,   
\
\ -----[ port G ]
\ 
    $ff N ldi,  N DDRG out,  \ PG0-5 weak pullups
    PG0 output,  PG1 output,  PG2 output, 
\
\ -----[ port H ]
\ 
   $fe N ldi,  N DDRH sts,  \ PH0=Rx input, PH1-7 weak pullups
\
\ -----[ port J ]
\ 
   $ff N ldi,  N DDRJ sts,  \ PJ0,1 weak pullups
\
\ -----[ port K ]
\ 
   $ff N ldi,  N DDRK sts,  \ PK0-7 weak pullups
\
\ -----[ port L ]
\ 
   $ff N ldi,  N DDRL sts,  \ PL0-7 weak pullups
;
\ 
\ --- conditional compilation summary --- 
here [ swap -  ( -- n)
\              ......................321
cs? [if] cr .( init.fs                  ) . .( bytes) [else] drop [then] 
]
\
0 [if] ---------------------[ Revision History ]-------------------------------
\ 
Date	  By  Description
======= === ===================================================================
150913  rjn Charley's new I/O scheme asmAVR.fs, ATmega2560.fs
150808  rjn cloned from lop
\ 
[then] \ ----------------------------------------------------------------------

