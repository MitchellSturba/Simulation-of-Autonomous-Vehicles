// @khenkel 
// parabox llc

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Parabox.Road;

[CustomEditor(typeof(Road))]
public class RoadTool : Editor
{
#region Private

	Road road;
#endregion

#region Constant

	const float MIN_ROAD_WIDTH = .3f;
	const float MAX_ROAD_WIDTH = 50f;

	const float MIN_GROUND_OFFSET = .001f;
	const float MAX_GROUND_OFFSET = 1f;
#endregion

#region Shortcut

	// big list of things that cause onscenegui to loop endlessly if mixed with our code.
	public bool earlyOut
	{
		get
		{
			return (
				Event.current.alt || 
				Tools.current == Tool.View || 
				GUIUtility.hotControl > 0 || 
				(Event.current.isMouse ? Event.current.button > 1 : false) ||
				Tools.viewTool == ViewTool.FPS ||
				Tools.viewTool == ViewTool.Orbit);
		}
	}
#endregion

#region Initalization / Destruction

	[MenuItem("GameObject/Create Other/Road %#r")]
	public static void init()
	{
		GameObject go = new GameObject();
		Road r = go.AddComponent<Road>();
		r.acceptInput = true;
		r.mat = (Material)AssetDatabase.LoadAssetAtPath("Assets/RoadTool/Example/Texture/Materials/Road.mat", typeof(Material));
		go.name = "Road " + go.GetInstanceID();
		go.AddComponent<MeshRenderer>();
		Selection.activeObject = go;
	}

	public void OnEnable()
	{
		road = (Road)target;
	}
#endregion

#region Window Lifecycle

	bool showUVOptions = false;
	public override void OnInspectorGUI()
	{
		GUI.changed = false;
		road.roadWidth = EditorGUILayout.Slider("Road Width", road.roadWidth, MIN_ROAD_WIDTH, MAX_ROAD_WIDTH);
		road.groundOffset = EditorGUILayout.Slider("Ground Offset", road.groundOffset, MIN_GROUND_OFFSET, MAX_GROUND_OFFSET);

		EditorGUILayout.HelpBox("A negative value inserts new points at the end of the line.", MessageType.Info);
		road.insertPoint = EditorGUILayout.IntField("Insert At Point", road.insertPoint);

		road.connectEnds = EditorGUILayout.Toggle("Connect Ends", road.connectEnds);

		showUVOptions = EditorGUILayout.Foldout(showUVOptions, "UV Options");
		if(showUVOptions)
		{
			road.swapUV = EditorGUILayout.Toggle("Swap UV", road.swapUV);
			road.flipU = EditorGUILayout.Toggle("Flip U", road.flipU);
			road.flipV = EditorGUILayout.Toggle("Flip V", road.flipV);

			road.uvScale = EditorGUILayout.Vector2Field("Scale", road.uvScale);
			road.uvOffset = EditorGUILayout.Vector2Field("Offset", road.uvOffset);
		}

		road.mat = (Material)EditorGUILayout.ObjectField("Material", road.mat, typeof(Material), true);

		if(road.acceptInput)
		{
			GUI.backgroundColor = Color.red;
			if(GUILayout.Button("Lock"))
			{
				road.acceptInput = false;
				SceneView.RepaintAll();
			}
		}
		else
		{
			GUI.backgroundColor = Color.green;
			if(GUILayout.Button("Modify"))
			{
				road.acceptInput = true;				
				SceneView.RepaintAll();
			}
		}

		
		GUI.backgroundColor = Color.white;
		if(GUI.changed)
		{
			EditorUtility.SetDirty(road);
			road.Refresh();
			SceneView.RepaintAll();
		}
	}
#endregion

#region Scene

	Vector3 groundPoint = Vector3.zero;
	Vector3 tp;
	public void OnSceneGUI()
	{
		if(road.acceptInput == false)
			return;

		Event e = Event.current;

		if(e.type == EventType.ValidateCommand)
		{
			road.Refresh();
			SceneView.RepaintAll();
		}

		if(e.isKey && (e.keyCode == KeyCode.Escape || e.keyCode == KeyCode.Return))
		{
			road.acceptInput = false;
			SceneView.RepaintAll();
		}

		// Existing point handles
		DrawHandleGUI(road.points);

		// TODO -- figure out why Handles.PositionHandle is sooo slow when panning

		if(!e.alt)
		for(int i = 0; i < road.points.Count; i++)
		{
			tp = road.points[i];
			road.points[i] = Handles.PositionHandle(road.points[i], Quaternion.identity);
			
			if(tp != road.points[i])
			{
				Vector3 p = RoadUtils.GroundHeight(road.points[i]);
				road.points[i] = p;
				road.Refresh();
			}
		}

		if(earlyOut)
			return;

		// New point placement from here down 

		int controlID = GUIUtility.GetControlID(FocusType.Passive);
		HandleUtility.AddDefaultControl(controlID);

		if(e.modifiers != 0 || Tools.current != Tool.Move)
		{
			if(e.type == EventType.MouseUp && e.button == 0 && Tools.current != Tool.Move && e.modifiers == 0)
			{
				FindSceneView().ShowNotification(new GUIContent("Tool must be set to 'Move' to place points!", ""));
				SceneView.RepaintAll();
			}
			return;		
		}

		if( (e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 0)
		{
			Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
			RaycastHit ground = new RaycastHit();

			if( Physics.Raycast(ray.origin, ray.direction, out ground) )
				groundPoint = ground.point;
		}
			
		// Listen for mouse input
		if(e.type == EventType.MouseUp && e.button == 0)
		{
			Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
			RaycastHit ground = new RaycastHit();

			if( Physics.Raycast(ray.origin, ray.direction, out ground) )
			{
				groundPoint = ground.point;
				AddPoint(groundPoint);
			}
		}
	}

	public static SceneView FindSceneView()
	{
		return SceneView.lastActiveSceneView == null ? EditorWindow.GetWindow<SceneView>() : SceneView.lastActiveSceneView;
	}
#endregion

#region Handles

	public void DrawHandleGUI(List<Vector3> points)
	{
		if(points == null || points.Count < 1)
			return;

		Handles.BeginGUI();
		GUI.backgroundColor = Color.red;
		for(int i = 0; i < points.Count; i++)
		{

			Vector2 p = HandleUtility.WorldToGUIPoint(points[i]);

			if(GUI.Button(new Rect(p.x+10, p.y-50, 25, 25), "x"))
				DeletePoint(i);

			GUI.Label(new Rect(p.x+45, p.y-50, 200, 25), "Point: " + i.ToString());	
		}
		GUI.backgroundColor = Color.white;
		Handles.EndGUI();
	}
#endregion

#region Point Management

	public void AddPoint(Vector3 v)
	{
		Undo.RecordObject(target, "Set Point");

		if(road.insertPoint < 0 || road.insertPoint > road.points.Count)
			road.points.Add(v);
		else
			road.points.Insert(road.insertPoint, v);

		road.Refresh();
		SceneView.RepaintAll();
	}

	public void DeletePoint(int index)
	{
		Undo.RecordObject(target, "Delete Point");

		road.points.RemoveAt(index);
		road.Refresh();
		SceneView.RepaintAll();
	}
#endregion
}