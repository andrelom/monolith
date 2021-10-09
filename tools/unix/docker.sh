set -o allexport; source .env; set +o allexport

declare -a containers=(
  "identity"
  "default"
  "worker"
)

# Menu

help() {
  echo ""
  echo "Usage:"
  echo "build" "\t" "It will build all docker containers."
  echo "push"  "\t" "It will push all docker containers to the target registry."
  echo "up"    "\t" "It will start all docker containers."
  echo "down"  "\t" "It will stop all docker containers."
  echo "prune" "\t" "It will prune."
  exit 1
}

# Tasks

build() {
  for container in "${containers[@]}"
  do
    docker build --no-cache -t $REGISTRY/$container -f ./docker/build/$container/Dockerfile .
    docker tag $container $REGISTRY/$container
  done
}

push() {
  for container in "${containers[@]}"
  do
    docker push $REGISTRY/$container
  done
}

up() {
  docker compose up -d
}

down() {
  docker compose down
}

prune() {
  docker system prune
}

# Main

case "${1}" in
  build ) build ;;
  push  ) push  ;;
  up    ) up    ;;
  down  ) down  ;;
  prune ) prune ;;
  *     ) help  ;;
esac
