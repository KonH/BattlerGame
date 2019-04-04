using System.Collections.Generic;
using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public class AttackCommand : ICommand {
		public readonly ulong DealerId;
		public readonly ulong TargetId;

		public AttackCommand(ulong dealerId, ulong targetId) {
			DealerId = dealerId;
			TargetId = targetId;
		}
		
		public bool IsValid(GameState state, Config config) {
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

		public void Execute(GameState state, Config config, ICommandBuffer buffer) {
			state.Level.MovedUnits.Add(DealerId);
			var damage = GetDamage(state, config);
			var target = state.Level.FindUnitById(TargetId);
			target.Health -= damage;
			if ( target.Health <= 0 ) {
				buffer.Add(new KillUnitCommand(target.Id));
			}
		}

		public int GetDamage(GameState state, Config config) {
			var dealer = state.Level.FindUnitById(DealerId);
			return config.Units[dealer.Descriptor].BaseDamage + GetWeaponDamage(dealer.Items, config);
		}

		int GetWeaponDamage(List<ItemState> items, Config config) {
			var accum = 0;
			foreach ( var item in items ) {
				var itemConfig = config.Items[item.Descriptor];
				if ( itemConfig is WeaponConfig weapon ) {
					accum += weapon.Damage;
				}
			}
			return accum;
		}

		public override string ToString() {
			return $"{nameof(AttackCommand)} ({DealerId}, {TargetId})";
		}
	}
}