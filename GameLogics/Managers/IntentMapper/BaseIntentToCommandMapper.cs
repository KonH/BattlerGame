using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLogics.Commands;
using GameLogics.Core;
using GameLogics.DAO;
using GameLogics.Intents;
using GameLogics.UseCases;
using GameLogics.Utils;
using Newtonsoft.Json;

namespace GameLogics.Managers.IntentMapper {
	public abstract class BaseIntentToCommandMapper : IIntentToCommandMapper {
		protected readonly GameState State;
		
		readonly JsonSerializerSettings _settings = new JsonSerializerSettings {
			TypeNameHandling = TypeNameHandling.Auto
		};
		readonly Dictionary<Type, IUseCase> _useCases;
		
		public BaseIntentToCommandMapper(IGameStateManager stateManager) {
			State = stateManager.State;
			_useCases = FillUseCases();
		}

		protected List<ICommand> CreateCommandsFromIntent(IIntent intent) {
			var intentType = intent.GetType();
			var useCase = _useCases.GetOrDefault(intentType);
			if ( useCase == null ) {
				throw new InvalidOperationException("unknown intent");
			}
			return useCase.Execute(intent);
		}

		public abstract Task<CommandResponse> RequestCommandsFromIntent(IIntent intent);
		
		/// <summary>
		/// Fill use cases dictionary with types, inherited from UseCase abstract class to handle intents
		/// </summary>
		Dictionary<Type, IUseCase> FillUseCases() {
			var interfaceType = typeof(IUseCase);
			var useCaseTypes = interfaceType.Assembly.GetTypes()
				.Where(t => t.IsClass && !t.IsAbstract && interfaceType.IsAssignableFrom(t)).ToArray();
			var result = new Dictionary<Type, IUseCase>();
			foreach ( var useCaseType in useCaseTypes ) {
				var baseType = useCaseType.BaseType;
				if ( baseType == null ) {
					throw new InvalidOperationException("Use case must inherit from UseCase<T>");
				}
				var genericDefinition = baseType.GetGenericTypeDefinition();
				if ( genericDefinition != typeof(UseCase<>) ) {
					throw new InvalidOperationException("Use case must inherit from UseCase<T>");
				}
				var intentType = baseType.GetGenericArguments()[0];
				var instance   = Activator.CreateInstance(useCaseType) as IUseCase;
				result.Add(intentType, instance);
			}
			return result;
		}

		protected string SerializeIntent(IIntent intent) {
			var request = new IntentRequest { Intent = intent };
			return JsonConvert.SerializeObject(request, _settings);
		}

		protected List<ICommand> DeserializeCommands(string str) {
			var respose = JsonConvert.DeserializeObject<IntentResponse>(str, _settings);
			return respose.Commands;
		}
	}
}