\ init.fs -- RAND initialization -- 160224rjn
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
D2             PD2   input    option jumper (future)
D3             PD3   output   PSR output
D5       OC0B  PD5   output   unused
D6       OC0A  PD6   output   unused
D7             PD7   output   unused
D8             PB0   output   unused
D9       OC1A  PB1   output   unused
D10      OC1B  PB2   output   unused
D11      OC2A  PB3   output   unused
D12      MISO  PB4   output   unused
D13       SCK  PB5   output   unused
         XTL1  PB6   ------
         XTL2  PB7   ------ 
A0             PC0   output   unused
A1             PC1   output   unused
A2             PC2   output   unused
A3             PC3   output   unused
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
      8 output, ( 0)   9 output, ( 1)  10 output, ( 2)  11 output, ( 3)
     12 output, ( 4)  13 output, ( 5, led)    
\
\ -----[ port C ]
\ 
    $ff N ldi,  N DDRC out,   \ weak pullups on port C pins    
    16 output, ( PC0/A0)  17 output, ( PC1)  PC2 output, ( 18)
    19 output, ( A3)  PC4 output, ( A4)  PC5 output, ( A5)
\ 
\ -----[ port D -- pins 0-7; 0,1 are serial; 2-7 are GPIO]
\ 
    $fe N ldi, N DDRD out, \ weak pullups on 0-7
    1 output, ( Tx)  PD0 input, ( Rx)    2 input,  ( D2)   PD3 output, 
    4 output,        PD5 output, ( 5)  PD6 output, ( 6)      7 output, ( 7)
;
\  
\ -----------------------------------------------------------------------------
0 [if] \                     Revision History
\ -----------------------------------------------------------------------------

Date     By    Description
======   ===   ================================================================
160224   rjn   Initial version.

[then] \ ----------------------------------------------------------------------

