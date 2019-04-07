using System;
using System.Collections.Generic;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Models.State;

namespace GameLogics.Shared.Logics {
	static class DamageLogics {
		public static int GetDamage(GameState state, Config config, ulong dealerId, ulong targetId) {
			var baseDamage = GetBaseDamage(state, config, dealerId);
			var weaponDamage = GetWeaponDamage(state, config, dealerId);
			var targetAbsorb = GetAbsorb(state, config, targetId);
			var damage = baseDamage + weaponDamage - targetAbsorb;
			return Math.Max(damage, 0);
		}

		static int GetBaseDamage(GameState state, Config config, ulong unitId) {
			var unitState = GetUnitState(state, unitId);
			return config.Units[unitState.Descriptor].BaseDamage;
		}

		static int GetWeaponDamage(GameState state, Config config, ulong unitId) {
			var items = GetUnitState(state, unitId).Items;
			var accum = 0;
			foreach ( var item in items ) {
				var itemConfig = config.Items[item.Descriptor];
				if ( itemConfig is WeaponConfig weapon ) {
					accum += weapon.Damage;
				}
			}
			return accum;
		}

		static int GetAbsorb(GameState state, Config config, ulong unitId) {
			var items = GetUnitItems(state, unitId);
			var accum = 0;
			foreach ( var item in items ) {
				var itemConfig = config.Items[item.Descriptor];
				if ( itemConfig is ArmorConfig armor ) {
					accum += armor.Absorb;
				}
			}
			return accum;
		}

		static UnitState GetUnitState(GameState state, ulong unitId) {
			return state.Level.FindUnitById(unitId);
		}

		static List<ItemState> GetUnitItems(GameState state, ulong unitId) {
			return GetUnitState(state, unitId).Items;
		}
	}
}
