using System;
using System.Collections.Generic;
using System.Linq;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Intents;
using GameLogics.Shared.Models;
using GameLogics.Shared.UseCases;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Services {
	public sealed class IntentToCommandMapper {
		readonly Dictionary<Type, IUseCase> _useCases;
		
		public IntentToCommandMapper() {
			_useCases = FillUseCases();
		}

		public List<ICommand> CreateCommandsFromIntent(GameState state, IIntent intent) {
			var intentType = intent.GetType();
			var useCase = _useCases.GetOrDefault(intentType);
			if ( useCase == null ) {
				throw new InvalidOperationException("unknown intent");
			}
			return useCase.Execute(state, intent);
		}
		
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
	}
}