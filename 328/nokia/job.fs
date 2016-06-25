\ job.fs -- SPI Demo Application -- 160514rjn

only forth also definitions
: nowarn warnings off ; : warn warnings on ; : not 0= ;
nowarn

: hello  ." Forth for Arduino" ;
' hello is bootmessage

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

\ --- compilation summary
true constant cs?    \ produce compilation summary?
variable starts-here  \ start of compilation segment
: starts  ( here -)   starts-here !  cr ;
: ends  ( here -)  cs? if starts-here @ - . ." bytes"  else 2drop then ;

include ../../../ATmega328.fs  \ special function registers

\ --- register assignments
30 constant Z   31 constant Z'   \ used as loop counter
28 constant Y   29 constant Y'   \ address register
26 constant X   27 constant X'   \ pointer to rest of stack
24 constant T   25 constant T'   \ top of stack
22 constant N   23 constant N'   \ next on stack (temporary)

\ include ../../../compiler.fs
include ./xcompiler.fs  \ compiler extensions -- added c,-t for char table
include ../../../disAVR.fs
include ../../../asmAVR.fs
include ../../../miscAVR.fs

:m init-stacks
   [ r0 dup 8 rshift $ff and ] T ldi,  T SPH out,
   T ldi,  T SPL out,  \ init return stack
   [ s0 dup 8 rshift $ff and ] X' ldi, X ldi,  m;  \ init data stack

\ Default interrupt vector
0 org 0 ljmp,  \ reset vector
0 ljmp,  \ $04 
0 ljmp,  \ $08
0 ljmp,  \ $0c    \ wdt
0 ljmp,  \ $10    \ $12 is timer2 overflow interrupt
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

[ cr ] .( starts at:      ) here .

\ ./doc/nokia.txt  \ doc file

here starts .( primitives)
include ../../../primitives.fs
here 6 spaces ends

here starts .( math)
include ../../../math.fs
12 spaces here ends

here starts .( serial.fs)
include ../../../serial.fs
here 7 spaces ends

here starts .( numbers)
include ../../../numbers.fs
here 9 spaces ends

here starts .( standalone)
include ../../../standalone.fs
here 5 spaces ends

here starts .( strings)
include ../../../strings.fs
here 10 spaces ends

here starts .( delays)
include ./delays.fs
here 11 spaces ends

here starts .( t0-fast-pwm)
include ./t0-fast-pwm.fs
here 5 spaces ends

here starts .( eeprom.fs)
include ./eeprom.fs
here 8 spaces ends

here starts .( schars.fs)
include ./schars.fs
here 7 spaces ends

here starts .( nokia.fs)
include ./nokia.fs
here 7 spaces ends

here starts .( commands)
include ./commands.fs
here 8 spaces ends

here starts .( init)
include ./init.fs
here 13 spaces ends

here starts .( main)
include ./main.fs  \ application code
12 spaces  here ends 

:m init-serial
   DDRD Y ldi,  2 T ldi,  T sty,  \ TX0 is output
   UCSR0A Y ldi,  0 Y' ldi,  \ 8N1
   $20 T ldi,  T sty+,  \ ready to transmit or receive
   $18 T ldi,  T sty+,
   6 T ldi,  T sty+,
   UBRR0L Y ldi,  0 Y' ldi,
\   103 T ldi,  T sty+,         \ 9600 baud, 16 MHz
\     51 T ldi,  T sty+,        \ 9600 baud, 8 MHz
\       8 T ldi,  T sty+,       \ 57600 baud, 8 MHz
       16 T ldi,  T sty+,       \ 57600 baud, 16 MHz
   0 T ldi,  T sty+,  
\   $02 UCSR0A  ori,            \ 115200 with UBRR0L=8 (doesn't work)
m;  target

\ Uncomment if you haven't already defined this word,
\ in timer.fs for example.
\ : init-interrupt  ;

\ some macros are made subroutines just for interactive testing
here starts .( interactive)
include ../../../interactive.fs
6 spaces here ends

here starts .( cold)
\ Here you decide whether to quit or go, meaning debug or run
\ : cold  entry cli, init-serial 10 #, base ! init  abort ;

\ execute app or standalone interpreter based on option switch
: cold  entry cli, init-serial init-stacks 10 #, base ! init
        opt if drop go then drop abort ;
13 spaces here ends

here starts .( headers)
here [ dup ] dict org #p! org headers  \ tack headers on end
8 spaces here ends

[ cr ]
host : .stack  depth if  >red  then  .s >black cr ;
report
save  \ chip.bin, avrdude handles binary files just fine.
host .( Host stack= ) .stack


