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
			var level = state.Level;
			if ( level.MovedUnits.Contains(DealerId) ) {
				return false;
			}
			var dealer = level.FindUnitById(DealerId);
			if ( dealer == null ) {
				return false;
			}
			var isPlayerUnit = level.PlayerUnits.Contains(dealer);
			if ( isPlayerUnit && !level.PlayerTurn ) {
				return false;
			}
			if ( !config.Units.ContainsKey(dealer.Descriptor) ) {
				return false;
			}
			if ( level.FindUnitById(TargetId) == null ) {
				return false;
			}
			return true;
		}

		public override List<ICommand> Execute(GameState state, Config config) {
			state.Level.MovedUnits.Add(DealerId);
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
			return $"{nameof(AttackCommand)} ({DealerId}, {TargetId})";
		}
	}
}