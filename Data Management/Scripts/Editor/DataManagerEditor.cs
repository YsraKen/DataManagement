using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataManager))]
public class DataManagerEditor : Editor
{
	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		var dm = (DataManager) target;
		
		// if(GUILayout.Button("Create New User")) dm.CreateUser(dm.testSaveName);
		
		if(GUILayout.Button("Persistent Data Path")) Debug.Log(Application.persistentDataPath);
	}
}