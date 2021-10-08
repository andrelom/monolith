set -o allexport; source .env; set +o allexport

# Menu

help() {
  echo ""
  echo "Usage:"
  echo "apply"  "\t" "Will apply all configurations."
  echo "delete" "\t" "Will delete all configurations."
  exit 1
}

# Tasks

apply() {
  kustomize build kubernetes/overlays/dev/identity | kubectl apply -f -
}

delete() {
  kustomize build kubernetes/overlays/dev/identity | kubectl delete -f -
}

# Main

case "${1}" in
  apply  ) apply  ;;
  delete ) delete ;;
  *      ) help   ;;
esac
