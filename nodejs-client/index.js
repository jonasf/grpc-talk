var PROTO_PATH = __dirname + '/../protos/StarwarsService.proto';

var grpc = require('grpc');
var starwars_proto = grpc.load(PROTO_PATH).Starwars;

function main() {
  var client = new starwars_proto.StarwarsService('localhost:50051', grpc.credentials.createInsecure());
  
  client.GetCharacter({name: "Luke Skywalker"}, function(err, response) {
    console.log('Character info:');
    console.log('Name: ', response.Name);
    console.log('Gender: ', response.Gender);
    console.log('Height: ', response.Height);
  });
}

main();