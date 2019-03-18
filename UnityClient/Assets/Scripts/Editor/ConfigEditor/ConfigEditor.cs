using System.Collections.Generic;
using System.IO;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Services;
using UnityEditor;
using UnityEngine;

namespace UnityClient.Editor.ConfigEditor {
	public class ConfigEditor : EditorWindow {		
		static List<string> _defaultPathes = new List<string>{ "Assets/Resources/Config.json" };
		
		Config _config = new Config();
		
		List<string> _pathes = new List<string>(_defaultPathes);
		
		PathEditor  _pathEditor  = new PathEditor();
		ItemEditor  _itemEditor  = new ItemEditor();
		UnitEditor  _unitEditor  = new UnitEditor();
		LevelEditor _levelEditor = new LevelEditor() { UseVerticalLayout = true };
		
		[MenuItem("Utils/Config/Open Editor")]
		public static void Open() {
			var window = GetWindow<ConfigEditor>();
			window.Show();
			window.Load();
		}

		void OnGUI() {
			_pathEditor.Update("Paths", _pathes);
			EditorGUILayout.Separator();
			
			GUILayout.BeginHorizontal();
			{
				if ( GUILayout.Button("Load") ) {
					Load();
				}
				if ( GUILayout.Button("Save") ) {
					Save();
				}
			}
			GUILayout.EndHorizontal();
			EditorGUILayout.Separator();

			_config.Version = EditorGUILayout.TextField("Version:", _config.Version);
			EditorGUILayout.Separator();
			
			_itemEditor.Update("Items", _config.Items);
			EditorGUILayout.Separator();
			
			_unitEditor.Update("Units", _config.Units);
			EditorGUILayout.Separator();

			_levelEditor.Context = _config;
			_levelEditor.Update("Levels", _config.Levels);
			EditorGUILayout.Separator();
		}

		void Load() {
			if ( _pathes.Count > 0 ) {
				Load(_pathes[0]);
			}
		}
		
		void Load(string path) {
			if ( File.Exists(path) ) {
				var content = File.ReadAllText(path);
				_config = new ConvertService().FromJson<Config>(content);
			}
		}

		void Save() {
			foreach ( var path in _pathes ) {
				Save(path);
			}
		}
		
		void Save(string path) {
			var json = new ConvertService().ToJson(_config);
			File.WriteAllText(path, json);
			AssetDatabase.Refresh();
		}
	}
}