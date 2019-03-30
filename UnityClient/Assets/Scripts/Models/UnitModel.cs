﻿using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace UnityClient.Models {
	public class UnitModel {
		public bool       IsPlayerUnit { get; }
		public UnitState  State        { get; }
		public UnitConfig Config       { get; }
		public int        Index        { get; }
		
		public bool IsFake => State == null;

		public UnitModel(bool isPlayerUnit, UnitState state, UnitConfig config, int index = 0) {
			IsPlayerUnit = isPlayerUnit;
			State        = state;
			Config       = config;
			Index        = index;
		}
		
		public UnitModel(int index) {
			Index = index;
		}
	}
}