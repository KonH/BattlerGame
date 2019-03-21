using System.Collections.Generic;
using GameLogics.Shared.Commands.Base;
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
		
		protected override bool IsValid(GameState state, Config config) {
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

		protected override void Execute(GameState state, Config config, ICommandBuffer buffer) {
			state.Level.MovedUnits.Add(DealerId);
			var dealer = state.Level.FindUnitById(DealerId);
			var damage = GetDamage(state, config);
			var target = state.Level.FindUnitById(TargetId);
			target.Health -= damage;
			if ( target.Health <= 0 ) {
				buffer.AddCommand(new KillUnitCommand(target.Id));
			}
		}

		public int GetDamage(GameState state, Config config) {
			var dealer = state.Level.FindUnitById(DealerId);
			return config.Units[dealer.Descriptor].BaseDamage;
		}

		public override string ToString() {
			return $"{nameof(AttackCommand)} ({DealerId}, {TargetId})";
		}
	}
}