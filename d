# download to Arduino -- 160225rjn
set -x

# on-board serial programmer
avrdude -p m328p -c avrisp -P /dev/ttyUSB0 -b 57600 -F -U flash:w:chip.bin

# usbtiny programmer
# avrdude -c usbtiny -B1 -p m328p -F -U flash:w:chip.bin
