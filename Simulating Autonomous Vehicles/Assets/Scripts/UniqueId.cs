using UnityEngine;
 using System.Collections.Generic;
 using System;
 
 #if UNITY_EDITOR
 using UnityEditor;
 using UnityEditor.SceneManagement;
 #endif
 
 [ExecuteInEditMode]
 public class UniqueId : MonoBehaviour {
 
     // global lookup of IDs to Components - we can esnure at edit time that no two 
     // components which are loaded at the same time have the same ID. 
     static Dictionary<string, UniqueId> allGuids = new Dictionary<string, UniqueId> ();
 
     public string uniqueId;
 
     // Only compile the code in an editor build
     #if UNITY_EDITOR
 
     // Whenever something changes in the editor (note the [ExecuteInEditMode])
     void Update(){
         // Don't do anything when running the game
         if (Application.isPlaying)
             return;
         
         // Construct the name of the scene with an underscore to prefix to the Guid
         string sceneName = gameObject.scene.name + "_";
 
         // if we are not part of a scene then we are a prefab so do not attempt to set 
         // the id
         if  (sceneName == null) return;
 
         // Test if we need to make a new id
         bool hasSceneNameAtBeginning = (uniqueId != null && 
             uniqueId.Length > sceneName.Length && 
             uniqueId.Substring (0, sceneName.Length) == sceneName);
         
         bool anotherComponentAlreadyHasThisID = (uniqueId != null && 
             allGuids.ContainsKey (uniqueId) && 
             allGuids [uniqueId] != this);
 
         if (!hasSceneNameAtBeginning || anotherComponentAlreadyHasThisID){
             uniqueId =  sceneName + Guid.NewGuid ();
             EditorUtility.SetDirty (this);
             EditorSceneManager.MarkSceneDirty (gameObject.scene);
         }
         // We can be sure that the key is unique - now make sure we have 
         // it in our list
         if (!allGuids.ContainsKey (uniqueId)) {
             allGuids.Add(uniqueId, this);
         }
     }
 
     // When we get destroyed (which happens when unloading a level)
     // we must remove ourselves from the global list otherwise the
     // entry still hangs around when we reload the same level again
     // but now the THIS pointer has changed and we end up changing 
     // our ID unnecessarily
     void OnDestroy(){
         allGuids.Remove(uniqueId);
     }
     #endif
 }