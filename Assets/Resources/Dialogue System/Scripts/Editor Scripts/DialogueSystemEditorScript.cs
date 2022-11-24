using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DialogueSystem
{
    public class DialogueSystemEditorScript : EditorWindow
    {
        List<string> lines = new();
        List<int> linesIds = new();
        List<bool> linesIsFiller = new();
        DialogueCharacter _char = null;
        Dialogue _editDialogue = null;
        string _charLine = "";
        string _charName = "";
        int _stateId;
        int _charSpeed;
        AudioClip _charClip;

        string _fileName = "";

        

        [MenuItem("Window / Dialogue System / Dialogue Builder")]
        public static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow(typeof(DialogueSystemEditorScript));
            window.titleContent = new GUIContent("Dialogue Builder");
        }

        void OnGUI()
        {
            
            //linesIds = new();
            GUILayout.Label("Dialogue Tools", EditorStyles.whiteLargeLabel);
            GUILayout.Space(8);

            EditorGUILayout.LabelField("Do you have an existing Dialogue?");
            _editDialogue = EditorGUILayout.ObjectField(_editDialogue, typeof(Dialogue), false) as Dialogue;

            bool _hasDialogue = _editDialogue != null;
            bool createNewCharacter = false;
            
            if(!_hasDialogue)
            {
                GUILayout.Label("Create New Dialogue", EditorStyles.largeLabel);
                _fileName = EditorGUILayout.TextField("Dialogue Name", _fileName);
                if(_fileName.Length == 0) { _fileName = "Dialogue"; }
                GUILayout.Space(8);

                createNewCharacter = _char == null;

                GUILayout.Label("Character", EditorStyles.largeLabel);
                if(_charName.Length == 0)
                {
                    EditorGUILayout.LabelField("Do you have an existing DialogueCharacter?");
                    _char = EditorGUILayout.ObjectField(_char, typeof(DialogueCharacter), false) as DialogueCharacter;
                    GUILayout.Space(8);
                }

                if(_char == null)
                {
                    GUILayout.Label("Create New Character", EditorStyles.boldLabel);
                    _charName = EditorGUILayout.TextField("Name", _charName);
                    _charSpeed = EditorGUILayout.IntSlider("Talk Speed", _charSpeed, 1, 10);
                    _charClip = EditorGUILayout.ObjectField("Character Soundbite", _charClip, typeof(AudioClip), false) as AudioClip;
                }
            }
            else
            {
                _char = _editDialogue.DialogueCharacter;
            }

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

            if(createNewCharacter)
            {
                DialogueCharacter character = ScriptableObject.CreateInstance<DialogueCharacter>();
                if(GUILayout.Button("Create Character"))
                {
                    if(!AssetDatabase.IsValidFolder($"Assets/Resources/Dialogue System/Dialogues/Characters"))
                    {
                        AssetDatabase.CreateFolder($"Assets/Resources/Dialogue System/Dialogues", "Characters");
                    }
                    if(!AssetDatabase.LoadAssetAtPath<ScriptableObject>($"Assets/Resources/Dialogue System/Dialogues/Characters/{_charName}.asset)"))
                    {
                        AssetDatabase.CreateAsset(character, $"Assets/Resources/Dialogue System/Dialogues/Characters/{_charName}.asset");
                    }
                    else
                    {
                        Debug.LogWarning($"A character called {_charName} already exists.");
                    }
                }
            }

            _stateId = EditorGUILayout.DelayedIntField("State ID", _stateId);

            string buttonText = _hasDialogue ? "Generate State" : "Generate Dialogue";
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

            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
