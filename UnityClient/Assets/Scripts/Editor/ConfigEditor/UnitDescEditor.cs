using System;
using System.Collections.Generic;
using System.Linq;
using GameLogics.Shared.Models.Configs;
using UnityEditor;
using UnityEngine;

namespace UnityClient.Editor.ConfigEditor {
	public class UnitDescEditor : ListEditor<string> {
		public Config Context = null;
		
		protected override string New() => "";
		
		protected override void Update(List<string> items, int index) {
			var unitIds = Context.Units.Keys.ToArray();
			if ( unitIds.Length == 0 ) {
				GUILayout.Label("No units");
				return;
			}
			var unitIndex = Array.IndexOf(unitIds, items[index]);
			if ( unitIndex < 0 ) {
				unitIndex = 0;
			}
			unitIndex = EditorGUILayout.Popup("Unit", unitIndex, unitIds);
			items[index] = unitIds[unitIndex];
		}
	}
}