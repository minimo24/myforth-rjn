\ schars.fs -- alphanumeric character set for Nokia -- 160602rjn
\  
\   
0 [if] \ ------------------------[ Notes ]------------------------------------
\ 
1. From example at playground.arduino.cc

[then] \ ------------------------[ Notes ]------------------------------------
\ 
\
\ note: 5 bytes per character -- table must be an even number!
\
here constant schars ( small characters)

$00 c,-t  $00 c,-t  $00 c,-t  $00 c,-t  $00 c,-t  \ blank  \ 20
$00 c,-t  $00 c,-t  $5f c,-t  $00 c,-t  $00 c,-t  \ !
$00 c,-t  $07 c,-t  $00 c,-t  $07 c,-t  $00 c,-t  \ *
$14 c,-t  $7f c,-t  $14 c,-t  $7f c,-t  $14 c,-t  \ #

$24 c,-t  $2a c,-t  $7f c,-t  $2a c,-t  $12 c,-t  \ $
$23 c,-t  $13 c,-t  $08 c,-t  $64 c,-t  $62 c,-t  \ %
$36 c,-t  $49 c,-t  $55 c,-t  $22 c,-t  $50 c,-t  \ &
$00 c,-t  $05 c,-t  $03 c,-t  $00 c,-t  $00 c,-t  \ '

$00 c,-t  $1c c,-t  $22 c,-t  $41 c,-t  $00 c,-t  \ (
$00 c,-t  $41 c,-t  $22 c,-t  $1c c,-t  $00 c,-t  \ )
$14 c,-t  $08 c,-t  $3e c,-t  $08 c,-t  $14 c,-t  \ 
$08 c,-t  $08 c,-t  $3e c,-t  $08 c,-t  $08 c,-t  \ +

$00 c,-t  $00 c,-t  $50 c,-t  $30 c,-t  $00 c,-t  \ ,
$08 c,-t  $08 c,-t  $08 c,-t  $08 c,-t  $08 c,-t  \ -
$00 c,-t  $60 c,-t  $60 c,-t  $00 c,-t  $00 c,-t  \ .
$20 c,-t  $10 c,-t  $08 c,-t  $04 c,-t  $02 c,-t  \ /

$3e c,-t  $51 c,-t  $49 c,-t  $45 c,-t  $3e c,-t  \ 0
$00 c,-t  $42 c,-t  $7f c,-t  $40 c,-t  $00 c,-t  \ 1
$42 c,-t  $61 c,-t  $51 c,-t  $49 c,-t  $46 c,-t  \ 2
$21 c,-t  $41 c,-t  $45 c,-t  $4b c,-t  $31 c,-t  \ 3

$18 c,-t  $14 c,-t  $12 c,-t  $7f c,-t  $10 c,-t  \ 4
$27 c,-t  $45 c,-t  $45 c,-t  $45 c,-t  $39 c,-t  \ 5
$3c c,-t  $4a c,-t  $49 c,-t  $49 c,-t  $30 c,-t  \ 6
$01 c,-t  $71 c,-t  $09 c,-t  $05 c,-t  $03 c,-t  \ 7

$36 c,-t  $49 c,-t  $49 c,-t  $49 c,-t  $36 c,-t  \ 8
$06 c,-t  $49 c,-t  $49 c,-t  $29 c,-t  $1e c,-t  \ 9
$00 c,-t  $36 c,-t  $36 c,-t  $00 c,-t  $00 c,-t  \ :
$00 c,-t  $56 c,-t  $36 c,-t  $00 c,-t  $00 c,-t  \ ;

$08 c,-t  $14 c,-t  $22 c,-t  $41 c,-t  $00 c,-t  \ <
$14 c,-t  $14 c,-t  $14 c,-t  $14 c,-t  $14 c,-t  \ =
$00 c,-t  $41 c,-t  $22 c,-t  $14 c,-t  $08 c,-t  \ >
$02 c,-t  $01 c,-t  $51 c,-t  $09 c,-t  $06 c,-t  \ ?

$32 c,-t  $49 c,-t  $79 c,-t  $41 c,-t  $3e c,-t  \ @    \ add a byte to make even

$7e c,-t  $11 c,-t  $11 c,-t  $11 c,-t  $7e c,-t  \ A
$7f c,-t  $49 c,-t  $49 c,-t  $49 c,-t  $36 c,-t  \ B
$3e c,-t  $41 c,-t  $41 c,-t  $41 c,-t  $22 c,-t  \ C
$7f c,-t  $41 c,-t  $41 c,-t  $22 c,-t  $1c c,-t  \ D

$7f c,-t  $49 c,-t  $49 c,-t  $49 c,-t  $41 c,-t  \ E
$7f c,-t  $09 c,-t  $09 c,-t  $09 c,-t  $01 c,-t  \ F
$3e c,-t  $41 c,-t  $49 c,-t  $49 c,-t  $7a c,-t  \ G
$7f c,-t  $08 c,-t  $08 c,-t  $08 c,-t  $7f c,-t  \ H

$00 c,-t  $41 c,-t  $7f c,-t  $41 c,-t  $00 c,-t  \ I
$20 c,-t  $40 c,-t  $41 c,-t  $3f c,-t  $01 c,-t  \ J
$7f c,-t  $08 c,-t  $14 c,-t  $22 c,-t  $41 c,-t  \ K
$7f c,-t  $40 c,-t  $40 c,-t  $40 c,-t  $40 c,-t  \ L

$7f c,-t  $02 c,-t  $0c c,-t  $02 c,-t  $7f c,-t  \ M
$7f c,-t  $04 c,-t  $08 c,-t  $10 c,-t  $7f c,-t  \ N
$3e c,-t  $41 c,-t  $41 c,-t  $41 c,-t  $3e c,-t  \ O
$7f c,-t  $09 c,-t  $09 c,-t  $09 c,-t  $06 c,-t  \ P

$3e c,-t  $41 c,-t  $51 c,-t  $21 c,-t  $5e c,-t  \ Q
$7f c,-t  $09 c,-t  $19 c,-t  $29 c,-t  $46 c,-t  \ R
$46 c,-t  $49 c,-t  $49 c,-t  $49 c,-t  $31 c,-t  \ S
$01 c,-t  $01 c,-t  $7f c,-t  $01 c,-t  $01 c,-t  \ T

$3f c,-t  $40 c,-t  $40 c,-t  $40 c,-t  $3f c,-t  \ U
$1f c,-t  $20 c,-t  $40 c,-t  $20 c,-t  $1f c,-t  \ V
$3f c,-t  $40 c,-t  $38 c,-t  $40 c,-t  $3f c,-t  \ W
$63 c,-t  $14 c,-t  $08 c,-t  $14 c,-t  $63 c,-t  \ X


$07 c,-t  $08 c,-t  $70 c,-t  $08 c,-t  $07 c,-t  \ Y
$61 c,-t  $51 c,-t  $49 c,-t  $45 c,-t  $43 c,-t  \ Z
$00 c,-t  $7f c,-t  $41 c,-t  $41 c,-t  $00 c,-t  \ [
$02 c,-t  $04 c,-t  $08 c,-t  $10 c,-t  $20 c,-t  \ 

$00 c,-t  $41 c,-t  $41 c,-t  $7f c,-t  $00 c,-t  \ ]
$04 c,-t  $02 c,-t  $01 c,-t  $02 c,-t  $04 c,-t  \ ^
$40 c,-t  $40 c,-t  $40 c,-t  $40 c,-t  $40 c,-t  \ _
$00 c,-t  $01 c,-t  $02 c,-t  $04 c,-t  $00 c,-t  \ '

$20 c,-t  $54 c,-t  $54 c,-t  $54 c,-t  $78 c,-t  \ a
$7f c,-t  $48 c,-t  $44 c,-t  $44 c,-t  $38 c,-t  \ b
$38 c,-t  $44 c,-t  $44 c,-t  $44 c,-t  $20 c,-t  \ c
$38 c,-t  $44 c,-t  $44 c,-t  $48 c,-t  $7f c,-t  \ d

$38 c,-t  $54 c,-t  $54 c,-t  $54 c,-t  $18 c,-t  \ e
$08 c,-t  $7e c,-t  $09 c,-t  $01 c,-t  $02 c,-t  \ f
$0c c,-t  $52 c,-t  $52 c,-t  $52 c,-t  $3e c,-t  \ g
$7f c,-t  $08 c,-t  $04 c,-t  $04 c,-t  $78 c,-t  \ h

$00 c,-t  $44 c,-t  $7d c,-t  $40 c,-t  $00 c,-t  \ i
$20 c,-t  $40 c,-t  $44 c,-t  $3d c,-t  $00 c,-t  \ j
$7f c,-t  $10 c,-t  $28 c,-t  $44 c,-t  $00 c,-t  \ k
$00 c,-t  $41 c,-t  $7f c,-t  $40 c,-t  $00 c,-t  \ l

$7c c,-t  $04 c,-t  $18 c,-t  $04 c,-t  $78 c,-t  \ m
$7c c,-t  $08 c,-t  $04 c,-t  $04 c,-t  $78 c,-t  \ n
$38 c,-t  $44 c,-t  $44 c,-t  $44 c,-t  $38 c,-t  \ o
$7c c,-t  $14 c,-t  $14 c,-t  $14 c,-t  $08 c,-t  \ p

$08 c,-t  $14 c,-t  $14 c,-t  $08 c,-t  $7c c,-t  \ q
$7c c,-t  $08 c,-t  $04 c,-t  $04 c,-t  $08 c,-t  \ r
$48 c,-t  $54 c,-t  $54 c,-t  $54 c,-t  $20 c,-t  \ s
$04 c,-t  $3f c,-t  $44 c,-t  $40 c,-t  $20 c,-t  \ t

$3c c,-t  $40 c,-t  $40 c,-t  $20 c,-t  $7c c,-t  \ u
$1c c,-t  $40 c,-t  $20 c,-t  $20 c,-t  $1c c,-t  \ v
$3c c,-t  $40 c,-t  $30 c,-t  $40 c,-t  $3c c,-t  \ w
$44 c,-t  $28 c,-t  $10 c,-t  $28 c,-t  $44 c,-t  \ x

$0c c,-t  $50 c,-t  $50 c,-t  $50 c,-t  $3c c,-t  \ y
$44 c,-t  $64 c,-t  $54 c,-t  $4c c,-t  $44 c,-t  \ z
$00 c,-t  $08 c,-t  $36 c,-t  $41 c,-t  $00 c,-t  \ {
$00 c,-t  $00 c,-t  $7f c,-t  $00 c,-t  $00 c,-t  \ |

$00 c,-t  $41 c,-t  $36 c,-t  $08 c,-t  $00 c,-t  \ }      \ 7d
$10 c,-t  $08 c,-t  $08 c,-t  $10 c,-t  $08 c,-t  \ ~
$78 c,-t  $46 c,-t  $41 c,-t  $46 c,-t  $78 c,-t  \ lock
$ff c,-t  $ff c,-t  $ff c,-t  $ff c,-t  $ff c,-t  \ block  \ 80

$00 c,-t ( makes table even) 

\ : atest  schars #, p!  c@p+ . c@p+ . c@p+ . c@p+ . c@p+ . ; 

\ -----------------------------------------------------------------------------
0 [if] \                         Revisions
\ -----------------------------------------------------------------------------
\ 
Date     By    Description
======   ===   ================================================================
160602   rjn   renamed to schars.fs
160531   rjn   original version

[then] \ ----------------------------------------------------------------------

