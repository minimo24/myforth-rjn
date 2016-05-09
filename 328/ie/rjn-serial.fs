\ rjn-serial.fs  -- custom serial, supress lead zeroes, etc. -- 160224rjn
\ 
\ suppress leading zeros
\
: ?0emit ( c -)   dup char 0 #, - if/ emit ; then drop space ;
\ : ?0emit  emit ;
: ?type ( adr len)  0max if  apush
      swap a! for  c@+ emit next  apop ; 
   then  2drop ;
