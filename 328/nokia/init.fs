\ init.fs -- Nokia Graphics LCD initialization -- 160609rjn
\ 
\ 
\ -----------------------------------------------------------------------------
0 [if] \                            Notes
\ -----------------------------------------------------------------------------

1. Configures Nano board (328p cpu).
 
2. This uses the revised ATmega328.fs file that allows both the original 
   Arduino terminology (e.g., 13 toggle,) and port/pin mapping 
   (e.g., PB5 toggle,).

3. If pin is configured as input, writing a 1 to the port pin activates
   the pullup.  Writing a zero deactivates the pullup.
   
4. ----------------------- Nokia Pin Configuration ---------------------------

Label    Name  Port  I/O      Function
=====    ===== ====  =====    ================================================
RX0            PD0   input    RXD
TX1            PD1   output   TXD
D2             PD2   output   unused (reserved for IE)
D3             PD3   output   unused (reserved for IE)
D4             PD4   input    GP input (e.g., option switch)
D5       OC0B  PD5   output   ND/C Nokia pin 5
D6       OC0A  PD6   output   5V PWM backlight, Nokia pin 6 -- direct drive
D7             PD7   output   NRST Nokia pin 4
D8             PB0   output   unused (fuse option for clock echo)
D9       OC1A  PB1   output   NSCE Nokia pin 3
D10      OC1B  PB2   output   unused
D11      OC2A  PB3   output   NMOSI Nokia pin 6
D12      MISO  PB4   output   unused
D13       SCK  PB5   output   SCLK Nokia pin 7
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
: /chip   \ initialize I/O
\ 
\ -----[ port B -- pins 8-13; PB6,7 are xtal ]
\
    $ff N ldi, N DDRB out,   \ weak pullups default for unused pins
      PB0 output, ( D8)   PB1 output, ( D9)  PB2 output, ( D10)  
      PB3 output, ( D11)  PB4 output, ( D12)  PB5 output, ( D13, led)    
\
\ -----[ port C ]
\ 
    $ff N ldi,  N DDRC out,   \ weak pullups on port C pins    
    PC0 output, ( A0)  PC1 output, ( A1)  PC2 output, ( A2)
    PC3 output, ( A3)  PC4 output, ( A4)  PC5 output, ( A5)
\ 
\ -----[ port D -- pins 0-7; 0,1 are serial; 2-7 are GPIO]
\ 
    $ee N ldi, N DDRD out, \ weak pullups on 1-3, 5-7
    PD0 input, ( 0/Rx)    PD1 output, ( 1/Tx)   PD2 output, ( 2/D2)   
    PD3 output, ( D3)     PD4 input,  ( 4/D4)   PD5 output, ( D5)   
    PD6 output, ( D6)     PD7 output, ( 7/D7)
    
\    PD4 high, ( set as input, internal pullup)
\    PD2 high,  PD3 high,  PD5 high,  PD6 high,  PD7 high,
\    PB1 high,  PB3 high,  PB5 high,
;
\  
\ -----------------------------------------------------------------------------
0 [if] \                     Revision History
\ -----------------------------------------------------------------------------

Date     By    Description
======   ===   ================================================================
160609   rjn   Added PD4 as an option input
160527   rjn   Initial version, cloned from LCD.

[then] \ ----------------------------------------------------------------------

