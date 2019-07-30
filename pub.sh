docker build -f client.Dockerfile -t patobelardo/grpc-client:1.0 .
docker build -f server.Dockerfile -t patobelardo/grpc-server:1.0 .

docker push patobelardo/grpc-client:1.0
docker push patobelardo/grpc-server:1.0

docker rmi $(docker images -f "dangling=true" -q)
