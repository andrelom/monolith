set -o allexport; source .env; set +o allexport

# Menu

help() {
  echo ""
  echo "Usage:"
  echo "clear" "\t" "It will clean all untracked files and folders."
  exit 1
}

# Tasks

clear() {
  git clean -ffdx
}

# Main

case "${1}" in
  clear ) clear ;;
  *     ) help  ;;
esac
