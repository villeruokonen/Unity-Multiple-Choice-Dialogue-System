using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace DialogueSystem
{
    public class DialogueSystemJSONTools : EditorWindow
    {
        Dialogue _editDialogue = null;
        string _fileName = "";

        [MenuItem("Window / Dialogue System / Dialogue JSON Tools")]
        public static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow(typeof(DialogueSystemJSONTools));
            window.titleContent = new GUIContent("Dialogue JSON Tools");
        }

        void OnGUI()
        {
            
            //linesIds = new();
            GUILayout.Label("JSON Tools", EditorStyles.whiteLargeLabel);
            GUILayout.Space(8);

            EditorGUILayout.LabelField("Drag and drop or select a Dialogue.");
            _editDialogue = EditorGUILayout.ObjectField(_editDialogue, typeof(Dialogue), false) as Dialogue;

            if(_editDialogue == null) { return; }

            _fileName = _editDialogue.name;
            //Debug.Log(_fileName);

            if(GUILayout.Button("Create JSON"))
            {
                if(AssetDatabase.IsValidFolder($"Assets/Resources/Dialogue System/Dialogues/{_fileName}/{_fileName} JSON"))
                {
                    if(File.Exists(Application.dataPath + $"Resources/Dialogue System/Dialogues/{_fileName}/{_fileName} JSON/{_fileName}.json"))
                    {
                        Debug.LogWarning($"JSON for {_editDialogue} already exists.");
                    }
                    
                    else
                    {
                        string filePath = Application.dataPath + $"/Resources/Dialogue System/Dialogues/{_fileName}/{_fileName} JSON/{_fileName}.json";
                        string json = JsonUtility.ToJson(_editDialogue, true);
                        File.WriteAllText(filePath, json);
                    }
                }

                else
                {
                    AssetDatabase.CreateFolder($"Assets/Resources/Dialogue System/Dialogues/{_fileName}", $"{_fileName} JSON");
                    string filePath = Application.dataPath + $"/Resources/Dialogue System/Dialogues/{_fileName}/{_fileName} JSON/{_fileName}.json";
                    string json = JsonUtility.ToJson(_editDialogue, true);
                    File.WriteAllText(filePath, json);
                }
            }

            if(GUILayout.Button("Update from JSON"))
            {
                if(AssetDatabase.IsValidFolder($"Assets/Resources/Dialogue System/Dialogues/{_fileName}/{_fileName} JSON"))
                {
                    if(!File.Exists(Application.dataPath + $"/Resources/Dialogue System/Dialogues/{_fileName}/{_fileName} JSON/{_fileName}.json"))
                    {
                        Debug.LogWarning($"Could not find JSON file for {_fileName}. Path checked: " 
                        + Application.dataPath + $"/Resources/Dialogue System/Dialogues/{_fileName}/{_fileName} JSON/{_fileName}.json");
                    }
                    
                    else
                    {
                        string filePath = Application.dataPath + $"/Resources/Dialogue System/Dialogues/{_fileName}/{_fileName} JSON/{_fileName}.json";
                        
                        string json = File.ReadAllText(filePath);
                        JsonUtility.FromJsonOverwrite(json, _editDialogue);
                    }
                }
                else
                {
                    Debug.LogWarning($"Could not find JSON folder for {_fileName}. Path checked: " 
                        + Application.dataPath + $"/Resources/Dialogue System/Dialogues/{_fileName}/{_fileName} JSON");
                }
            }



            /*
            

            string textFieldLabel = _char == null ? "Character's line" : $"{_char.name}'s Line";
            _charLine = EditorGUILayout.TextField(textFieldLabel, _charLine);
            GUILayout.Space(8);

            GUILayout.Label("Player Lines", EditorStyles.boldLabel);
            
            int newCount = Mathf.Max(0, EditorGUILayout.DelayedIntField("Number Of Lines", lines.Count));
            while (newCount < lines.Count)
            {
                lines.RemoveAt( lines.Count - 1 );
                linesIsFiller.RemoveAt(linesIsFiller.Count - 1);
                linesIds.RemoveAt(linesIds.Count - 1);
            }
            while (newCount > lines.Count)
            {
                lines.Add("");
                linesIsFiller.Add(false);
                linesIds.Add(0);
            }
            

            for(int i = 0; i < newCount; i++)
            {
                lines[i] = EditorGUILayout.TextField($"Line {i + 1}", lines[i]);
                EditorGUI.indentLevel++;
                linesIds[i] = EditorGUILayout.DelayedIntField("Triggers ID", linesIds[i]);
                linesIsFiller[i] = EditorGUILayout.Toggle("Is Filler", linesIsFiller[i]);
                EditorGUI.indentLevel--;
            }
            

            GUILayout.Space(16); 
            _stateId = EditorGUILayout.DelayedIntField("State ID", _stateId);

            string buttonText = "";
            if(GUILayout.Button(buttonText))
            {
                DialogueCharacter character;
                if(_char == null)
                {
                    character = ScriptableObject.CreateInstance<DialogueCharacter>();
                }
                else
                {
                    character = _char;
                }
                
                
                if(!_hasDialogue)
                {
                    Dialogue dialogue = ScriptableObject.CreateInstance<Dialogue>();
                    Dialogue.DialogueState state = new Dialogue.DialogueState();
                    character.InitializeCharacter(_charName, _charSpeed, _charClip);
                    state.InitializeState(_charLine, lines.ToArray(), linesIds.ToArray(), linesIsFiller.ToArray(), _stateId);
                    dialogue.InitializeDialogue(character, state);
                    //dialogue = ScriptableObject.CreateInstance<Dialogue>();

                    if(!AssetDatabase.IsValidFolder($"Assets/Resources/Dialogue System/Dialogues/{_fileName}"))
                    {
                        AssetDatabase.CreateFolder("Assets/Resources/Dialogue System/Dialogues", $"{_fileName}");
                    }

                    AssetDatabase.CreateAsset(dialogue, $"Assets/Resources/Dialogue System/Dialogues/{_fileName}/{_fileName}.asset");
                    var dialogueObject = AssetDatabase.LoadAssetAtPath($"Assets/Resources/Dialogue System/Dialogues/{_fileName}/{_fileName}.asset",
                                                                                                    typeof(ScriptableObject)) as ScriptableObject;
                    EditorUtility.SetDirty(dialogueObject);
                    dialogueObject = dialogue;
                    AssetDatabase.SaveAssetIfDirty(dialogueObject);
                }
                else
                {
                    Dialogue dialogue = _editDialogue;
                    Dialogue.DialogueState state = new Dialogue.DialogueState();
                    character.InitializeCharacter(_charName, _charSpeed, _charClip);
                    state.InitializeState(_charLine, lines.ToArray(), linesIds.ToArray(), linesIsFiller.ToArray(), _stateId);
                    dialogue.DialogueStates.Add(state);
                }

            } */
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
