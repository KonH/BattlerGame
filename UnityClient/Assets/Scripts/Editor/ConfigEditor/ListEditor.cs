using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityClient.Editor.ConfigEditor {
	public abstract class ListEditor<T> where T : class {
		public bool UseVerticalLayout = false;

		bool _active = false;
		
		public void Update(string header, List<T> items) {
			_active = EditorGUILayout.Foldout(_active, header);
			if ( !_active ) {
				return;
			}
			T removedItem = null;
			for ( var i = 0; i < items.Count; i++ ) {
				if ( UseVerticalLayout ) {
					GUILayout.BeginVertical();
				} else {
					GUILayout.BeginHorizontal();
				}
				{
					Update(items, i);
					if ( GUILayout.Button("Remove") ) {
						removedItem = items[i];
					}
				}
				if ( UseVerticalLayout ) {
					GUILayout.EndVertical();
				} else {
					GUILayout.EndHorizontal();
				}
			}
			if ( removedItem != null ) {
				items.Remove(removedItem);
			}
			if ( GUILayout.Button("Add") ) {
				items.Add(New());
			}
		}

		protected abstract T New();
		protected abstract void Update(List<T> items, int index);
	}
}