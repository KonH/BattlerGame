using GameLogics.Server.Model;
using GameLogics.Server.Repository.User;
using GameLogics.Shared.Service;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Server.Model;
using System.Linq;

namespace Server.Repository {
	public sealed class MongoUserRepository : IUserRepository {
		readonly ConvertService              _convert;
		readonly IMongoCollection<UserModel> _users;

		public MongoUserRepository(IConfiguration config, ConvertService convert) {
			_convert = convert;
			var client = new MongoClient(config.GetConnectionString("battlerGame"));
			var database = client.GetDatabase("battlerGame");
			_users = database.GetCollection<UserModel>("user");
		}

		public UserState Find(string login, string passwordHash = null) {
			var model = _users.Find(u => u.Login == login).FirstOrDefault();
			if ( model == null ) {
				return null;
			}
			var state = _convert.FromJson<UserState>(model.User.ToJson());
			if ( (passwordHash == null) || (state.PasswordHash == passwordHash) ) {
				return state;
			}
			return null;
		}

		public bool TryAdd(UserState user) {
			var doc = BsonSerializer.Deserialize<BsonDocument>(_convert.ToJson(user));
			_users.InsertOne(new UserModel { Login = user.Login, User = doc });
			return true;
		}
	}
}
