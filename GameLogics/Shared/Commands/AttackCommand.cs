using System.Collections.Generic;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public class AttackCommand : BaseCommand {
		public readonly ulong DealerId;
		public readonly ulong TargetId;

		public AttackCommand(ulong dealerId, ulong targetId) {
			DealerId = dealerId;
			TargetId = targetId;
		}
		
		public override bool IsValid(GameState state, Config config) {
			if ( state.Level == null ) {
				return false;
			}
			var dealer = state.Level.FindUnitById(DealerId);
			if ( dealer == null ) {
				return false;
			}
			if ( !config.Units.ContainsKey(dealer.Descriptor) ) {
				return false;
			}
			if ( state.Level.FindUnitById(TargetId) == null ) {
				return false;
			}
			return true;
		}

		public override List<ICommand> Execute(GameState state, Config config) {
			var dealer = state.Level.FindUnitById(DealerId);
			var damage = config.Units[dealer.Descriptor].BaseDamage;
			var target = state.Level.FindUnitById(TargetId);
			target.Health -= damage;
			if ( target.Health <= 0 ) {
				return WithSubCommand(new KillUnitCommand(target.Id));
			}
			return NoSubCommands;
		}

		public override string ToString() {
			return $"{nameof(AttackCommand)} ('{DealerId}', '{TargetId}')";
		}
	}
}