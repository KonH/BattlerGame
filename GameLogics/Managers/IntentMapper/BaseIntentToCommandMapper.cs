using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLogics.Commands;
using GameLogics.Intents;
using GameLogics.UseCases;
using GameLogics.Utils;

namespace GameLogics.Managers.IntentMapper {
	public abstract class BaseIntentToCommandMapper : IIntentToCommandMapper {
		readonly Dictionary<Type, IUseCase> _useCases;
		
		public BaseIntentToCommandMapper() {
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
	}
}