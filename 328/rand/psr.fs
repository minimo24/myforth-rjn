\ psr.fs -- Pseudo-Random Number Generator, register version -- 160303rjn
\ 
\ -----------------------------------------------------------------------------
0 [if] \                            Notes
\ -----------------------------------------------------------------------------

1. Uses registers b0 to b3 to implement a 31-bit PSR output signal on PD3.
   Also uses several scratch registers for calcs and display of registers
   with the standalone interpreter.

2. Maximum length cycle for XNOR feedback on bits 30 & 27 (stages 31 & 28).
   Per XILINX App. Note KAPP 052 by Peter Alfke.  Also see EDN Design Idea
   titled "Make noise with a PIC", August 7, 2003 by Peter Guettler.
   Cycle length is 2^31 - 1 = 2,147,483,647 states.

3. Register/Bit assignments as follows:
      b3       b2       b1       b0
   33222222 22221111 11111100 00000000
   10987654 32109876 54321098 76543210
    ^  ^ feedback bits

4. Per "max cycle rate" code below, executing psr in the "go" loop
   results in a cycle rate period of ~ 1.26 us (793.7 kHz).  
   Cycle repetition rate = (2,147,483,647)(1.26)(10e-06) 
                         = 2,705.8 seconds = 45.1 minutes 

[then] \ ----------------------------------------------------------------------
\
\
\ --- register assignments
20 constant tmp0  21 constant tmp1
18 constant b2    19 constant b3
14 constant b0    17 constant b1

15 constant temp

: /psr   $A5 b3 ldi,  $54 b2 ldi,  $40 b1 ldi,  $5A b0 ldi, ;  \ arbitrary 

\ Note: $9408 is the set carry instruction
:m feedback   tmp0 tmp0 xor,  tmp1 tmp1 xor,  \ XNOR on bits 30 & 27 -> carry
   6 b3 sbrc, $01 tmp0 ldi, 3 b3 sbrc, $01 tmp1 ldi,  clc,
   tmp0 tmp1 xor, 0 tmp1 sbrs, $9408 ,-t ( sec,) m;

:m psr  
\   PD3 toggle, nop nop \ show approximate cycle rate (low)
   feedback  b0 b0 adc,  b1 b1 adc,  b2 b2 adc,  b3 b3 adc, 
   0 tmp1 sbrs, PD3 toggle, m; target

0 [if] \ debug sequencing with standalone interpreter

: "1   [ char 1 ] #, emit ;
: "0   [ char 0 ] #, emit ;

:m .temp
   7 temp sbrc, "1  7 temp sbrs, "0
   6 temp sbrc, "1  6 temp sbrs, "0
   5 temp sbrc, "1  5 temp sbrs, "0
   4 temp sbrc, "1  4 temp sbrs, "0
   space
   3 temp sbrc, "1  3 temp sbrs, "0
   2 temp sbrc, "1  2 temp sbrs, "0
   1 temp sbrc, "1  1 temp sbrs, "0
   0 temp sbrc, "1  0 temp sbrs, "0  nop m;  target

: .psr   cr  b3  temp mov, .temp
      space  b2  temp mov, .temp
      space  b1  temp mov, .temp 
      space  b0  temp mov, .temp 
         cr  ."  ^   ^" cr ;

: tt  psr .psr ;
: ttt   tt tt tt tt tt tt tt tt tt tt ;

[then] \ ----------------------------------------------------------------------
\  
\ -----------------------------------------------------------------------------
0 [if] \                     Revision History
\ -----------------------------------------------------------------------------

Date     By    Description
======   ===   ================================================================
160301   rjn   Used different feedback bits, all on byte 3 now.
151120   rjn   Initial version
              
[then] \ ----------------------------------------------------------------------

