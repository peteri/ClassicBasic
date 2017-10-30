# ClassicBasic - Clone of Microsoft 8K BASIC

This version of BASIC is designed to implement most of a Microsoft 8K basic interpreter in C#. 
The initial aim is to get the code to be able to run the contents of "BASIC computer games" by David Ahl. 
See the games directory for the files.

## Todo List
* ~~Finish string functions with tests.~~
* Add tests for commands.
* Fix outstanding bugs.
* Write the following commands (loosely clustered)
* ```INPUT, DATA, READ, RESTORE```
* ```ON GOTO, ON GOSUB, ON ERR```
* ```NEW,  CLEAR, DEL, DIM```
* ```SAVE, LOAD```
* ```DEF FN```

## Known bugs
* Parsing of floating point numbers has a bug when an exponent is specified it get split into '3.03', '-', 'E04' tokens so it needs merging.
* Make ```PRINT 3.4.5.6. ,``` work the same way it does in Microsoft BASIC.
* Make ```VAL("3.4.5.6")``` work the same way it does in Microsoft BASIC.
* Use of GOSUB from an immediate line doesn't return correctly.
* LIST doesn't correctly get the beginning and end line numbers.
* RUN "filename.bas" doesn't work.
* FOR definition does not delete unwanted stack entries.
* Numeric overflow is not trapped in the basic interpreter.

## Issues  and future plans
* Allow editing of lines of code.
* Currently uses double for the underlying floating point format, probably should be a float so we can have doubles if implement Microsoft extended /  disk BASIC.
* Numbers are printed without leading/trailing space like the Apple ][ interpreter. Possibly should change to be the same as PET / 8080 BASIC.
* Need to sort out how to package the code.
* Add Appveyor integration.
* Make Microsoft extended basic with disk commands an option.