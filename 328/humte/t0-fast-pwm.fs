\ t0-fast-pwm.fs -- timer 0 setup for fast PWM -- 160616rjn
\ 
\ 
\ -----------------------------------------------------------------------------
0 [if] \                            Notes
\ -----------------------------------------------------------------------------

1. Configures for fast PWM mode.
 
2. Output appears on D5 (OCR0B) & D6 (OCR0A) -- see I/O table in init.fs.
   If OCR0A and OCR0B are set the same, output on both.  
   Otherwise, output appears on whatever pin's count is reached first.

3. See AVR328P datasheet on timer 0, page 93 -->

4. TCCR0A bits:
   1 0 0 0  0 0 1 1     COM00-01 specify non-inverted PWM
   C C          W W     WGM00-01 specify fast PWM
   O O          G G
   M M          M M
   0 0          0 0
   1 0          1 0

5. TCCR0B bits:
   0 0 0 0  0 1 0 1     FOC0A=0 & FOC0B=0 (only set for non-PWM modes)
   F F      W C C C     WGM02=0 default value (fast PWM)
   O O      G S S S     CS00-02=0x5 for clock divide by 1024 
   C C      M 0 0 0        (PWM frequency =~ 30 Hz)
   0 0      0 2 1 0
   A B      2       
   
[then] \ ----------------------------------------------------------------------
\ 
\ 
1 [if] \ ----------------------[ Timer 0 Setup ]-------------------------------
\ 
-: /pwm  \ init timer 0 in fast PWM mode
   $83 #,  TCCR0A #, |!     \ non-invert, fast PWM mode.
\   $05 #,  TCCR0B #, |!     \ PWM, clock divided by 1024 (~30 Hz)
   $03 #,  TCCR0B #, |!     \ PWM, clock divided by 64 (~488 Hz)
   $1f #,  OCR0A  #, |!     \ 50% PWM
   $01 #,  OCR0B  #, |!     \ no output on D5
;
\ 
[then] \ ----------------------------------------------------------------------
\
\  
\ -----------------------------------------------------------------------------
0 [if] \                     Revision History
\ -----------------------------------------------------------------------------

Date     By    Description
======   ===   ================================================================
160616   rjn   Made /pwm headless
160609   rjn   Changed init word to /pwm vs. /timer0
160413   rjn   Works.  Changed PWM frequency - 30 Hz too low.  Better comments.
160412   rjn   Initial version.

[then] \ ----------------------------------------------------------------------

