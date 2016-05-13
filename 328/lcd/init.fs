\ init.fs -- LCD initialization -- 160510rjn
\ 
\ 
\ -----------------------------------------------------------------------------
0 [if] \                            Notes
\ -----------------------------------------------------------------------------

1. Configures Nano board (328 cpu).
 
2. This version requires the revised ATmega328.fs file that allows both 
   original Arduino mapping (e.g., 13 toggle,) and port/pin mapping 
   (e.g., PB5 toggle,).

3. If pin is configured as input, writing a 1 to the port pin activates
   the pullup.  Writing a zero deactivates the pullup.
   
4. ----------------------- RAND Pin Configuration -----------------------------

Label    Name  Port  I/O      Function
=====    ===== ====  =====    ==============================================
RX0            PD0   input    RXD
TX1            PD1   output   TXD
D2             PD2   output   unused
D3             PD3   output   unused
D4             PD4   output   unused
D5       OC0B  PD5   output   unused
D6       OC0A  PD6   output   backlight
D7             PD7   output   RS
D8             PB0   output   unused (fuse option for clock echo)
D9       OC1A  PB1   output   E
D10      OC1B  PB2   output   unused
D11      OC2A  PB3   output   unused
D12      MISO  PB4   output   unused
D13       SCK  PB5   output   on-board LED indicator
         XTL1  PB6   ------
         XTL2  PB7   ------ 
A0             PC0   output   DB4
A1             PC1   output   DB5
A2             PC2   output   DB6
A3             PC3   output   DB7
A4             PC4   output   unused
A5             PC5   output   unused
RST       RST  PC6   /RESET
A6             ADC6  output   unused
A7             ADC7  output   unused
\ 
[then] \ ----------------------------------------------------------------------
\ 
\  
-: /chip   \ initialize I/O
\ 
\ -----[ port B -- pins 8-13; PB6,7 are xtal ]
\
    $ff N ldi, N DDRB out,   \ weak pullups default for unused pins
      8 output, ( 0)   9 output, ( PB1)  10 output, ( 2)  11 output, ( 3)
     12 output, ( 4)  13 output, ( 5, led)    
\
\ -----[ port C ]
\ 
    $ff N ldi,  N DDRC out,   \ weak pullups on port C pins    
    PC0 output, ( 16/A0)  PC1 output, ( 17/A1)  PC2 output, ( 18/A2)
    PC3 output, ( 19/A3)  PC4 output, ( A4)  PC5 output, ( A5)
\ 
\ -----[ port D -- pins 0-7; 0,1 are serial; 2-7 are GPIO]
\ 
    $fe N ldi, N DDRD out, \ weak pullups on 0-7
    PD0 input, ( 0/Rx)    PD1 output, ( 1/Tx)   PD2 output, ( 2/D2)   
    PD3 output, ( D3)     PD4 output, ( 4/D4)   PD5 output, ( D5)   
    PD6 output, ( D6)     PD7 output, ( 7/D7)
;
\  
\ -----------------------------------------------------------------------------
0 [if] \                     Revision History
\ -----------------------------------------------------------------------------

Date     By    Description
======   ===   ================================================================
160510   rjn   changed DB4-7 to PC0-PC3 vs. PD2-PD5 (clear INT0,1 for IE)
160408   rjn   PB1 (D9) now drives the LCD backlight.  DOC CHANGE ONLY
160329   rjn   Initial version.

[then] \ ----------------------------------------------------------------------

