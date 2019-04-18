using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Server.Model {
	public sealed class UserModel {
		[BsonId]
		public string Login;

		[BsonElement("user")]
		public BsonDocument User;
	}
}
