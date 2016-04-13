\ job.fs -- blink tester for 2560 chip -- 160413rjn
\ 
\ changed to eliminate standalone interpreter -- won't work with 2560. 
\ 
only forth also definitions
: nowarn warnings off ; : warn warnings on ; : not 0= ;
nowarn

\ Colors are used by the decompiler/disassembler
include ansi.fs  \ Part of Gforth.
warn
variable colors
: in-color  true colors ! ;
in-color
: b/w  false colors ! ;
: color  ( n - ) create , does> colors @ if @ >fg attr! exit then drop ;
red color >red
black color >black
blue color >blue
green color >green
cyan color >cyan
yellow color >yellow

\ For navigating the source code in the VIM editor
include ../../../vtags.fs use-tags

0 constant start  \ Reset vector.
$68 constant rom-start  \ Start of code, all interrupts reserved.
$8000 constant target-size

\ Initial stack pointers
$08ff constant r0
$06ff constant s0
nowarn \ warnings off
s0 1 + constant tib  \ terminal input buffer
tib 32 + constant pad
warn \ warnings on

include ./ATmega2560.fs  \ Special Function Registers (use these for now)

30 constant Z   31 constant Z'   \ used as loop counter
28 constant Y   29 constant Y'   \ address register
26 constant X   27 constant X'   \ pointer to rest of stack
24 constant T   25 constant T'   \ top of stack
22 constant N   23 constant N'   \ next on stack (temporary)

include ../../../compiler.fs
include ../../../disAVR.fs
include ./asmAVR.fs
include ../../../miscAVR.fs

:m init-stacks
   [ r0 dup 8 rshift $ff and ] T ldi,  T SPH out,
   T ldi,  T SPL out,  \ init return stack
   [ s0 dup 8 rshift $ff and ] X' ldi, X ldi,  m;  \ init data stack

\ Default interrupt vector
0 org 0 ljmp,     \ reset vector
0 ljmp,  \ $04 
0 ljmp,  \ $08
0 ljmp,  \ $0c    \ wdt
0 ljmp,  \ $10
0 ljmp,  \ $14
0 ljmp,  \ $18
0 ljmp,  \ $1c
0 ljmp,  \ $20
0 ljmp,  \ $24
0 ljmp,  \ $28
0 ljmp,  \ $2c
0 ljmp,  \ $30
0 ljmp,  \ $34
0 ljmp,  \ $38
0 ljmp,  \ $3c
0 ljmp,  \ $40
0 ljmp,  \ $44
0 ljmp,  \ $48
0 ljmp,  \ $4c
0 ljmp,  \ $50
0 ljmp,  \ $54
0 ljmp,  \ $58
0 ljmp,  \ $5c
0 ljmp,  \ $60
0 ljmp,  \ $64

target  \ $68 org

\ ./blink.txt  \ doc file
include ../../../primitives.fs
include ./delays.fs
include ../../../math.fs
include ../../../serial.fs
include ../../../numbers.fs
include ./init.fs
include ./main.fs  \ application code, ends with go

:m init-serial
   DDRD Y ldi,  2 T ldi,  T sty,  \ TX0 is output
   UCSR0A Y ldi,  0 Y' ldi,  \ 8N1
   $20 T ldi,  T sty+,  \ ready to transmit or receive
   $18 T ldi,  T sty+,
   6 T ldi,  T sty+,
   UBRR0L Y ldi,  0 Y' ldi,  \ 9600 baud
   103 T ldi,  T sty+,  \ 9600 baud, 16 MHz crystal
\    51 T ldi,  T sty+,  \ 9600 baud, 8 MHz crystal
\   8 T ldi,  T sty+,    \ 57600 baud, 8 MHz crystal
   0 T ldi,  T sty+,  m;

\ Uncomment if you haven't already defined this word.
: init-interrupt  ;

target
\ some macros are made subroutines just for interactive testing
include ../../../interactive.fs
\ Here you decide whether to quit or go, meaning debug or run
\ : cold  entry cli, init-serial 10 #, base ! init-interrupt abort ;
: cold  entry cli, init-serial init-stacks 10 #, base ! init-interrupt go ;
here [ dup ] dict org #p! org headers  \ tack headers on end

host : .stack  depth if  >red  then  .s >black cr ;
report
save
host .( Host stack= ) .stack

