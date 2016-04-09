\ ATmega2560.fs
\ cloned from ATmega32U4.fs -- rjn150831 10:41:06
\ 
\ ----- "Low" Special Function Registers ----- /
\ can be addressed with IN or OUT

$20 constant PINA
$21 constant DDRA
$22 constant PORTA
$23 constant PINB
$24 constant DDRB
$25 constant PORTB
$26 constant PINC
$27 constant DDRC
$28 constant PORTC
$29 constant PIND
$2a constant DDRD
$2b constant PORTD
$2c constant PINE
$2d constant DDRE
$2e constant PORTE
$2f constant PINF
$30 constant DDRF
$31 constant PORTF
$32 constant PING
$33 constant DDRG
$34 constant PORTG

$35 constant TIFR0
$36 constant TIFR1
$37 constant TIFR2
$38 constant TIFR3
$39 constant TIFR4
$3a constant TIFR5
$3b constant PCIFR
$3c constant EIFR
$3d constant EIMSK
$3e constant GPIOR0
$3f constant EECR
$40 constant EEDR
$41 constant EEARL
$42 constant EEARH
$43 constant GTCCR
$44 constant TCCR0A
$45 constant TCCR0B
$46 constant TCNT0
$47 constant OCR0A
$48 constant OCR0B
\ $49 is reserved
$4a constant GPIOR1
$4b constant GPIOR2
$4c constant SPCR
$4d constant SPSR
$4e constant SPDR
\ $4f is reserved

$50 constant ACSR
$51 constant OCDR
\ $52 is reserved

$53 constant SMCR
$54 constant MCUSR
$55 constant MCUCR
\ $56 is reserved
$57 constant SPMCSR
\ $58 is reserved
\ $59 is reserved
\ $5a is reserved
$5b constant RAMPZ
$5c constant EIND
$5d constant SPL
$5e constant SPH
$5f constant SREG

\ ----- "High" Special Function Registers ----- /
\ can't be addressed with IN or OUT

$60 constant WDTCSR
$61 constant CLKPR
\ $62 is reserved
\ $63 is reserved
$64 constant PRR0
$65 constant PRR1
$66 constant OSCCAL
\ $67 is reserved
$68 constant PCICR
$69 constant EICRA
$6a constant EICRB
$6b constant PCMSK0
$6c constant PCMSK1
$6d constant PCMSK2
$6e constant TIMSK0
$6f constant TIMSK1
$70 constant TIMSK2
$71 constant TIMSK3
$72 constant TIMSK4
$73 constant TIMSK5
$74 constant XMCRA
$75 constant XMCRB
\ $77 is reserved

$78 constant ADCL  
$79 constant ADCH
$7a constant ADCSRA
$7b constant ADCSRB
$7c constant ADMUX
$7d constant DIDR2
$7e constant DIDR0
$7f constant DIDR1

$80 constant TCCR1A
$81 constant TCCR1B
$82 constant TCCR1C
\ $83 is reserved
$84 constant TCNT1L
$85 constant TCNT1H
$86 constant ICR1L
$87 constant ICR1H
$88 constant OCR1AL
$89 constant OCR1AH
$8a constant OCR1BL
$8b constant OCR1BH
$8c constant OCR1CL
$8d constant OCR1CH
\ $8e is reserved
\ $8f is reserved

$90 constant TCCR3A
$91 constant TCCR3B
$92 constant TCCR3C
\ $93 is reserved
$94 constant TCNT3L
$95 constant TCNT3H
$96 constant ICR3L
$97 constant ICR3H
$98 constant OCR3AL
$99 constant OCR3AH
$9a constant OCR3BL
$9b constant OCR3BH
$9c constant OCR3CL
$9d constant OCR3CH
\ $9e is reserved
\ $9f is reserved

$a0 constant TCCR4A
$a1 constant TCCR4B
$a2 constant TCCR4C
\ $a3 is reserved 
$a4 constant TCNT4L
$a5 constant TCNT4H
$a6 constant ICR4L
$a7 constant ICR4H
$a8 constant OCR4AL
$a9 constant OCR4AH
$aa constant OCR4BL
$ab constant OCR4BH
$ac constant OCR4CL
$ad constant OCR4CH
\ $ae is reserved
\ $af is reserved

$b0 constant TCCR2A
$b1 constant TCCR2B
$b2 constant TCNT2
$b3 constant OCR2A
$b4 constant OCR2B
\ $b5 is reserved
$b6 constant ASSR
\ $b7 is reserved
$b8 constant TWBR
$b9 constant TWSR
$ba constant TWAR
$bb constant TWDR
$bc constant TWCR
$bd constant TWAMR
\ $be is reserved
\ $bf is reserved

$c0 constant UCSR0A
$c1 constant UCSR0B
$c2 constant UCSR0C
\ $c3 is reserved
$c4 constant UBRR0L
$c5 constant UBRR0H
$c6 constant UDR0
\ $c7 is reserved
$c8 constant UCSR1A
$c9 constant UCSR1B
$ca constant UCSR1C
\ $cb is reserved
$cc constant UBRR1L
$cd constant UBRR1H
$ce constant UDR1
\ $cf is reserved
$d0 constant UCSR2A
$d1 constant UCSR2B
$d2 constant UCSR2C
\ $d3 is reserved

$d4 constant UBRR2L
$d5 constant UBRR2H
$d6 constant UDR2
\ $d7-$ff are reserved

$100 constant PINH
$101 constant DDRH
$102 constant PORTH
$103 constant PINJ
$104 constant DDRJ
$105 constant PORTJ
$106 constant PINK
$107 constant DDRK
$108 constant PORTK
$109 constant PINL
$10a constant DDRL
$10b constant PORTL
\ $10c-$11f are reserved
$120 constant TCCR5A
$121 constant TCCR5B
$122 constant TCCR5C
\ $123 is reserved
$124 constant TCNT5L
$125 constant TCNT5H
$126 constant ICR5L
$127 constant ICR5H
$128 constant OCR5AL
$129 constant OCR5AH
$12a constant OCR5BL
$12b constant OCR5BH
$12c constant OCR5CL
$12d constant OCR5CH
\ $12e & $12f are reserved
$130 constant UCSR3A
$131 constant UCSR3B
$132 constant UCSR3C
\ $133 is reserved
$134 constant UBRR3L
$135 constant UBRR3H
$136 constant UDR3
\ $137-$1ff are reserved

: enum ( n - n')
    8 0 do  dup constant 1 + loop ;
: enum6 ( n - n')
    6 0 do  dup constant 1 + loop ;

0 
enum PA0 PA1 PA2 PA3 PA4 PA5 PA6 PA7
enum PB0 PB1 PB2 PB3 PB4 PB5 PB6 PB7
enum PC0 PC1 PC2 PC3 PC4 PC5 PC6 PC7
enum PD0 PD1 PD2 PD3 PD4 PD5 PD6 PD7
enum PE0 PE1 PE2 PE3 PE4 PE5 PE6 PE7
enum PF0 PF1 PF2 PF3 PF4 PF5 PF6 PF7
enum6 PG0 PG1 PG2 PG3 PG4 PG5
\ enum PH0 PH1 PH2 PH3 PH4 PH5 PH6 PH7
\ enum PJ0 PJ1 PJ2 PJ3 PJ4 PJ5 PJ6 PJ7
\ enum PK0 PK1 PK2 PK3 PK4 PK5 PK6 PK7
\ enum PL0 PL1 PL2 PL3 PL4 PL5 PL6 PL7
drop

: ports, ( port)
    8 0 do  i c,  dup c,  loop  drop ;

create arduino-pins
    PORTA ports,
    PORTB ports,
    PORTC ports,
    PORTD ports,
    PORTE ports,
    PORTF ports,
    PORTG ports,
\    PORTH ports,
\    PORTJ ports,
\    PORTK ports,
\    PORTL ports,

: PORT ( i - bit adr)  2* arduino-pins + dup c@ swap 1+ c@ ;
: DDR ( i - bit adr)  PORT 1 - ;
: PIN ( i - bit adr)  PORT 2 - ;

0 [if] \ --------------------------- Notes ------------------------------------
\
 1. example:  PG3 toggle,
 2. PH,J,K,L can be accessed only by ST,LD,STS,LDS,STD and LDD 
       instructions (entire port, not individual bits).  
 3. Example for note 2. :
           :m +PL   $ff N ldi,  N PORTL sts, m;  \ sets PL0-7 
           :m -PL   $00 N ldi,  N PORTL sts, m;  \ clears PL0-7
           :m init  $ff N ldi,  N DDRL  sts, m;  \ PL0-7 weak pullups
           : go  init  begin 100 #, ms  +PL  100 #, ms  -PL  again
 4. Depending on 2560 board type, some pins may not be brought out to a 
     board edge.
 5. PG6,7 do not exist.
\
[then] \ ----------------------------------------------------------------------



