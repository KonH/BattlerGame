using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Server.Model {
	public sealed class GameStateModel {
		[BsonId]
		public string Login;

		[BsonElement("state")]
		public BsonDocument State;
	}
}
