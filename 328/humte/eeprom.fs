\ eeprom.fs -- eeprom words -- 160606rjn
\
\ 
0 [if] ---------------------------[ Notes ]------------------------------------
\
1. No clipping for valid eeprom address.  1K ($400) available in 328.
2. Mostly follows example in AVR manual but their example just does a
   one-time check for bit status.  This code loops until the appropriate
   bit clears.
3. The waits for flash and eeprom writes to complete are somewhat optional.
4. Disabling interrupts during a read may not be necessary.
5. 2015 datasheet errata says wait for write is now 10.5 ms vs. 9.
\ 
[then] \ ----------------------------------------------------------------------
\
\
:m ?ewrite-complete  begin, 1 EECR ( EEPE=0)  sbic, rjmp, m; target 

: ewrite  ( n a -) 
   cli,  ?ewrite-complete
\
\ --- set eeprom adr and data
   T  EEARL sts,  T' EEARH sts, ( read H last) drop  \ adr
   T  EEDR  sts, drop \ data
\ 
\ --- manage EEPROM control register, EECR
   \ assumes EEPM0, EEPM1 (bits 4 & 5 of EECR) are both 0 -- erase & write
\   1 EECR cbi, \ clear write bit (EEPE set to read) -- already=0, above
   2 EECR sbi, \ set enable bit (EEMPE)
   1 EECR sbi, \ set write bit (EEPE set to write)
\
\ --- wait for write to complete, EEPE=0
   ?ewrite-complete  sei, ;

: eread  ( a - n)
   cli,  ?ewrite-complete
   T' EEARH sts,  T  EEARL sts,  drop  \ load adr
   0 EECR sbi,  \ set read enable bit
   EEDR #, c@   \ fetch byte
   sei, ;
\ 
\ 
\ -----------------------------------------------------------------------------
1 [if] \                          eeprom cells
\ -----------------------------------------------------------------------------
\ 
host
variable eadr  \ holds latest evariable allocation
$00 eadr !
: e-cell  eadr @ constant 1 eadr +! ;
: e-allocate  ( n -)   eadr @  eadr +! ;
target
\ 
[then] \ ----------------------------------------------------------------------
\ 
0 [if] \ --- example
\
e-cell e-test
: write-ecell  ( n -)  e-test #, ewrite ;
: .ecell  e-test #, eread . ;
\ 
[then]
\ 
\ -----------------------------------------------------------------------------
0 [if] \                        Revision History
\ -----------------------------------------------------------------------------
\ 
Date	  By  Description
======= === ===================================================================
160606  rjn minor corrections, just need "target" after a macro
160102  rjn added note about revised wait for write -- now 10.5 ms
151030  rjn eliminated wait for flash write -- not in the manual's example
151028  rjn better coding for eeprom write complete check
151026  rjn added waits for eeprom and flash write completion
150810  rjn initial version
\ 
[then] \ ----------------------------------------------------------------------

