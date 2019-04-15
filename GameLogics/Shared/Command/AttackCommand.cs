using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Logic;

namespace GameLogics.Shared.Command {
	public sealed class AttackCommand : ICommand {
		public readonly ulong DealerId;
		public readonly ulong TargetId;

		public AttackCommand(ulong dealerId, ulong targetId) {
			DealerId = dealerId;
			TargetId = targetId;
		}
		
		public bool IsValid(GameState state, ConfigRoot config) {
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

		public void Execute(GameState state, ConfigRoot config, ICommandBuffer buffer) {
			state.Level.MovedUnits.Add(DealerId);
			var damage = DamageLogic.GetDamage(state, config, DealerId, TargetId);
			var target = state.Level.FindUnitById(TargetId);
			target.Health -= damage;
			if ( target.Health <= 0 ) {
				buffer.Add(new KillUnitCommand(target.Id));
			}
		}

		public override string ToString() {
			return $"{nameof(AttackCommand)} ({DealerId}, {TargetId})";
		}
	}
}