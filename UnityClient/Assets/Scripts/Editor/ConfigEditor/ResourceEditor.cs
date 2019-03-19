using System;
using System.Collections.Generic;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using UnityEditor;
using UnityEngine;

namespace UnityClient.Editor.ConfigEditor {
	public class ResourceEditor {
		public Config Context = null;
		
		public bool UseVerticalLayout = false;

		bool _active = false;
		
		public void Update(string header, Dictionary<Resource, int> items) {
			_active = EditorGUILayout.Foldout(_active, header);
			if ( !_active ) {
				return;
			}
			Tuple<Resource, Resource> changedItem = null;
			Dictionary<Resource, int> newValues = new Dictionary<Resource, int>();
			foreach ( var pair in items ) {
				if ( UseVerticalLayout ) {
					GUILayout.BeginVertical();
				} else {
					GUILayout.BeginHorizontal();
				}
				{
					var key = (Resource)EditorGUILayout.EnumPopup("Key:", pair.Key);
					if ( (key != Resource.Unknown) && (key != pair.Key) && !items.ContainsKey(key) ) {
						changedItem = Tuple.Create(pair.Key, key);
						break;
					}
					newValues[key] = Update(pair.Value);
					if ( GUILayout.Button("Remove") ) {
						changedItem = Tuple.Create(pair.Key, Resource.Unknown);
						break;
					}
				}
				if ( UseVerticalLayout ) {
					GUILayout.EndVertical();
				} else {
					GUILayout.EndHorizontal();
				}
			}
			foreach ( var newPair in newValues ) {
				items[newPair.Key] = newPair.Value;
			}
			if ( changedItem != null ) {
				var (from, to) = changedItem;
				var item = items[from];
				items.Remove(from);
				if ( to != Resource.Unknown ) {
					items[to] = item;
				}
			}
			if ( !items.ContainsKey(Resource.Unknown) && GUILayout.Button("Add") ) {
				items.Add(Resource.Unknown, 0);
			}
		}
		
		int Update(int value) {
			return EditorGUILayout.IntField("Count", value);
		}
	}
}