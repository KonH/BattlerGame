using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GameLogics.Commands;
using GameLogics.Intents;
using GameLogics.Managers;
using GameLogics.Managers.IntentMapper;

namespace ConsoleClient {
	public class HttpIntentToCommandMapper : BaseIntentToCommandMapper {

		readonly HttpClient _client = new HttpClient();
		
		readonly string _uri;

		public HttpIntentToCommandMapper(IGameStateManager stateManager, string uri) : base(stateManager) {
			_uri = uri;
		}
		
		public override async Task<CommandResponse> RequestCommandsFromIntent(IIntent intent) {
			var body = SerializeIntent(intent);
			var content = new StringContent(body, Encoding.UTF8, "application/json");
			try {
				var result = await _client.PostAsync(_uri, content);
				if ( !result.IsSuccessStatusCode ) {
					Console.WriteLine(result);
					return CommandResponse.Failed();
				}
				var str = await result.Content.ReadAsStringAsync();
				var commands = DeserializeCommands(str);
				return CommandResponse.FromCommands(commands);
			} catch ( Exception e ) {
				Console.WriteLine(e);
				return CommandResponse.Failed();
			}
		}
	}
}