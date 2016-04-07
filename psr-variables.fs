\ psr.fs  Pseudo-Random Number Generator  151120rjn
\ 
\ -----------------------------------------------------------------------------
0 [if] \                            Notes
\ -----------------------------------------------------------------------------

1. 

cpuHERE constant sequence 4 cpuALLOT
\ XXXXXXXX XXXXXXXX XXXXXXXX XXXXXXXX
\ ^ bit 31      ^ bit 18
\ ^ sequence        ^ sequence+2

[then] \ ----------------------------------------------------------------------
\ 

cpuHERE constant 'psr-high  variable psr-high  \ high word
cpuHERE constant 'psr-low   variable psr-low   \ low word

: /psr  
   $8004 #, psr-high |!  $c001 #, psr-low |! ;
: .psr   psr-high |@ u.  psr-low |@ u. ;   

\ -: feedback
\   psr-high |@ $8004 #, |xor 0 #, - 0if drop sec, ; then drop clc, ;   

\ : tt+   $8004 #, psr-high |! ;  \ CAUSES A "RESET" FAILURE!
\ : tt-   $4001 #, psr-high |! ;

: psr   
\   feedback 
   0 #,
   X push,  X' push,  Z push, Z' push,
   [ 'psr-low $ff and ] X ldi,   \ shift low word first
   [ 'psr-low 256 / $ff and ] X' ldi,
   X Z movw,  \ save original X vector in Z
   \ sec, 
   N ldx+,  N' ldx,   N N adc,  N' N' adc,  \ left shift 16 bits w/carry
   Z X movw, N stx+,  N' stx,  \ restore X pointer and save back to psr-low
   [ 'psr-high $ff and ] X ldi,   \ now do high word
   [ 'psr-high 256 / $ff and ] X' ldi,
   X Z movw,  \ save original X vector in Z
   T ldx+, T' ldx, T T adc,  T' T' adc,  \ carry is set from N left shift
   Z X movw, T stx+, T' stx,  \ restore X pointer and save back to psr-high
   Z' pop,  Z pop,  X' pop,  X pop, drop ;   

0 [if] \ ----------------------------------------------------------------------

\ If all bits are clear, reseed with $aaaaaaaa.
: ?seed   sequence # a! $aa #  dup !+ dup !+ dup !+ ! ;

\ Shift once with feedback from bits 18 and 31.
:m |psr	+clue
	[ sequence 1 + ] #@  \ Get bit 18.
	[ 2 .T movbc 7 .T movcb ]  \ Move it to bit 7 of TOS.
	sequence #@ xor  \ xor bits 31 and 18.
	2*'  \ Move xored bit into carry.
	outbit1 movcb  outbit2 movcb
	\ Shift xored bit into sequence.
	[ sequence 3 + ] (#@) 2*' [ sequence 3 + ] (#!)
	[ sequence 2 + ] (#@) 2*' [ sequence 2 + ] (#!)
	[ sequence 1 + ] (#@) 2*' [ sequence 1 + ] (#!)
	[ sequence 0 + ] (#@) 2*' [ sequence 0 + ] (#!)
	drop -clue m;

: psr  |psr ;

[then] \ ----------------------------------------------------------------------

0 [if] \ ----------------------------------------------------------------------
\ View current shift register in hex.
: .psr  cr
	[ sequence 0 + ] #@ h.
	[ sequence 1 + ] #@ h.
	[ sequence 2 + ] #@ h.
	[ sequence 3 + ] #@ h.
	space
	[ sequence 0 + ] #@ u.
	[ sequence 1 + ] #@ u.
	[ sequence 2 + ] #@ u.
	[ sequence 3 + ] #@ u.
	space
	[ sequence 1 + ] #@
	[ sequence 2 + ] #@ d.
	;

\ Load a seed value in the shift register.
: psr! ( n1 n2 n3 n4 - ) sequence # a! !+ !+ !+ ! ;

\ Note that #@ and #! push and pop the data stack, but
\ (#@) and (#!) assume the top of stack is already free to be used, so
\ no need to push or pop.

: /psr  0 # dup dup dup psr! ?seed ;
: init  /psr 0 # 7 #for  psr  7 #next ;

\ : tt  13 # 7 #for psr 7 #next .psr ;  \ test

[then] \ ----------------------------------------------------------------------
\  
\ -----------------------------------------------------------------------------
0 [if] \                     Revision History
\ -----------------------------------------------------------------------------

Date     By    Description
======   ===   ================================================================
151120   rjn   Initial version
              
[then] \ ----------------------------------------------------------------------

