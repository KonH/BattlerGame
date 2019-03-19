using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityClient.Editor.ConfigEditor {
	abstract class DictEditor<TItem> where TItem : class {
		public bool UseVerticalLayout = false;

		bool _active = false;
		
		public void Update(string header, Dictionary<string, TItem> items) {
			_active = EditorGUILayout.Foldout(_active, header);
			if ( !_active ) {
				return;
			}
			Tuple<string, string> changedItem = null;
			foreach ( var pair in items ) {
				if ( UseVerticalLayout ) {
					GUILayout.BeginVertical();
				} else {
					GUILayout.BeginHorizontal();
				}
				{
					var key = EditorGUILayout.TextField("Key:", pair.Key);
					if ( !string.IsNullOrEmpty(key) && (key != pair.Key) && !items.ContainsKey(key) ) {
						changedItem = Tuple.Create(pair.Key, key);
						break;
					}
					Update(pair.Value);
					if ( GUILayout.Button("Remove") ) {
						changedItem = Tuple.Create(pair.Key, string.Empty);
						break;
					}
				}
				if ( UseVerticalLayout ) {
					GUILayout.EndVertical();
				} else {
					GUILayout.EndHorizontal();
				}
			}
			if ( changedItem != null ) {
				var (from, to) = changedItem;
				var item = items[from];
				items.Remove(from);
				if ( !string.IsNullOrEmpty(to) ) {
					items[to] = item;
				}
			}
			if ( !items.ContainsKey("New") && GUILayout.Button("Add") ) {
				items.Add("New", New());
			}
		}

		protected abstract TItem New();
		protected abstract void Update(TItem item);
	}
}