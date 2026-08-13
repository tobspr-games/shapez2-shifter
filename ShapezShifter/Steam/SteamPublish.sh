#!/usr/bin/env bash

MODE=""
CONTENT_PATH=""

# Parse arguments: flag can appear in any position
for arg in "$@"; do
  case "$arg" in
    --release) MODE="release" ;;
    --develop) MODE="develop" ;;
    *) CONTENT_PATH="$arg" ;;
  esac
done

if [[ -z "$MODE" ]]; then
  echo "Usage: $0 <content_path> --release|--develop" >&2
  exit 1
fi

# Mode-specific assets
VDF_NAME="$MODE.vdf"
IMG_NAME="$MODE.png"

# Prints the current working directory into the variable while converting the POSIX-style path to windows-style path
# Converts /c/ to C:/
CURRENT_DIR=$(cygpath -w "$PWD")

# Composes the location of the preview image
PREVIEW_IMG=$CURRENT_DIR\\Steam\\$IMG_NAME

# Adjust paths to use double backlashes
CONTENT_PATH="${CONTENT_PATH//\\/\\\\}"
PREVIEW_IMG="${PREVIEW_IMG//\//\\}"

echo "MODE: $MODE"
echo "CONTENT_PATH: $CONTENT_PATH"
echo "PREVIEW_IMG: $PREVIEW_IMG"

export CONTENT_PATH
export PREVIEW_IMG

# Adjust temporary .vdf with absolute paths for the content and the preview image
envsubst < Steam\\$VDF_NAME > Steam\\base.tmp.vdf

# Log the final version
cat Steam\\base.tmp.vdf

TMP_VDF=$CURRENT_DIR\\Steam\\base.tmp.vdf

# Execute
out=$(steamcmd +login lorenzo_tobspr +workshop_build_item "$TMP_VDF" +quit 2>&1)
rc=$?
echo "$out"


if [ $rc -eq 0 ]; then
    # Copy published file id back
    cat Steam\\base.tmp.vdf
    
    # Grab published file id 
    FILE_ID=$(grep '"publishedfileid"' Steam\\base.tmp.vdf | sed 's/.*"publishedfileid"[ \t]*"\([0-9]\+\)".*/\1/')
    
    # Updating original file with new published file ID
    echo "New published file ID: $FILE_ID"
    sed -i 's/\("publishedfileid"[ \t]*"\)[0-9]\+"/\1'"$FILE_ID"'"/'  Steam\\$VDF_NAME
fi


# Clean temporary files
rm Steam\\base.tmp.vdf

if [ $rc -ne 0 ]; then
    echo "Workshop upload failed (rc=$rc)" >&2
    exit $rc
fi
