#!/bin/sh

DIR="$(cd "$(dirname "$0")" && pwd)"
me=`basename "$0"`
scriptPath="$DIR/$me"


echo $scriptPath
firefox &
discord &
