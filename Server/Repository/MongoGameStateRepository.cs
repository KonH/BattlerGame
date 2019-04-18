using GameLogics.Server.Model;
using GameLogics.Server.Repository.State;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Service;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Server.Model;
using System;
using System.Linq;

namespace Server.Repository {
	public sealed class MongoGameStateRepository : IGameStateRepository {
		readonly ConvertService                   _convert;
		readonly IMongoCollection<GameStateModel> _states;

		public MongoGameStateRepository(IConfiguration config, ConvertService convert) {
			_convert = convert;
			var client = new MongoClient(config.GetConnectionString("battlerGame"));
			var database = client.GetDatabase("battlerGame");
			_states = database.GetCollection<GameStateModel>("state");
		}

		public GameState Find(UserState user) {
			var model = _states.Find(s => s.Login == user.Login).FirstOrDefault();
			return (model != null) ? FromBson(model.State) : null;
		}

		public GameState FindOrCreate(UserState user, Action<GameState> init) {
			var state = Find(user);
			if ( state == null ) {
				state = new GameState();
				init(state);
				_states.InsertOne(new GameStateModel { Login = user.Login, State = ToBson(state) });
			}
			return state;
		}

		public GameState Save(UserState user, GameState state) {
			_states.ReplaceOne(s => s.Login == user.Login, new GameStateModel { Login = user.Login, State = ToBson(state) });
			return state;
		}

		BsonDocument ToBson(GameState state) {
			var json = _convert.ToJson(state);
			return BsonSerializer.Deserialize<BsonDocument>(json);
		}

		GameState FromBson(BsonDocument doc) {
			return _convert.FromJson<GameState>(doc.ToJson());
		}
	}
}
