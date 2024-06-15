#!/bin/bash

# package the solution
tar -czvf rightmove_console.tar.gz RightMoveConsole/bin/Release/net8.0/.
tar -czvf rightmove_webapi.tar.gz RightMove.Web/bin/Release/net8.0/.