set -o allexport; source .env; set +o allexport

declare -a HOSTS=(
  "identity.monolith.localhost"
  "default.monolith.localhost"
)

# Menu

help() {
  echo ""
  echo "Usage:"
  echo "add" "\t" "It add the host entries to /etc/hosts."
  exit 1
}

# Tasks

add() {
  for HOST in "${HOSTS[@]}"
  do
    if [ -n "$(grep $HOST /etc/hosts)" ]
    then
      echo "The '$HOST' already exists"
    else
      echo "Adding $HOST";
      sudo -- sh -c -e "echo '127.0.0.1\t$HOST' >> /etc/hosts";
    fi
  done
}

# Main

case "${1}" in
  add ) add  ;;
  *   ) help ;;
esac
