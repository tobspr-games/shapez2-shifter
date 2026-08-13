#!/usr/bin/env bash

CONTENT_PATH=$1

# Prints the current working directory into the variable while converting the POSIX-style path to windows-style path
# Converts /c/ to C:/
CURRENT_DIR=$(cygpath -w "$PWD")

# Composes the location of the preview image
PREVIEW_IMG=$CURRENT_DIR\\Steam\\preview.png

# Adjust paths to use double backlashes
CONTENT_PATH="${CONTENT_PATH//\\/\\\\}"
PREVIEW_IMG="${PREVIEW_IMG//\//\\}"
PREVIEW_IMG="${PREVIEW_IMG//\\/\\\\}"

echo "CONTENT_PATH: $CONTENT_PATH"
echo "PREVIEW_IMG: $PREVIEW_IMG"

export CONTENT_PATH
export PREVIEW_IMG

# Adjust temporary .vdf with absolute paths for the content and the preview image
envsubst < Steam\\base.vdf > Steam\\base.tmp.vdf

# Log the final version
cat Steam\\base.tmp.vdf

TMP_VDF=$CURRENT_DIR\\Steam\\base.tmp.vdf

# Execute
steamcmd +login lorenzo_tobspr +workshop_build_item "$TMP_VDF" +quit;

# Copy published file id back
cat Steam\\base.tmp.vdf

# Grab published file id 
FILE_ID=$(grep '"publishedfileid"' Steam\\base.tmp.vdf | sed 's/.*"publishedfileid"[ \t]*"\([0-9]\+\)".*/\1/')

# Updating original file with new published file ID
echo "New published file ID: $FILE_ID"
sed -i 's/\("publishedfileid"[ \t]*"\)[0-9]\+"/\1'"$FILE_ID"'"/'  Steam\\base.vdf

# Clean temporary files
rm Steam\\base.tmp.vdf