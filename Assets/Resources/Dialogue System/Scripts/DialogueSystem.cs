using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DialogueSystem;

namespace DialogueSystem
{
    public class DialogueSystem : MonoBehaviour
    {
        public static DialogueSystem Instance {get; private set;}
        [Header("Dialogue Option Colors")]
        public Color BaseColor;
        public Color FillerBaseColor;
        
        [Header("Settings")]
        public bool InstantDialogue;
        //public bool UseDebug;
        //public Dialogue debugDialogue;
        private bool _instant => InstantDialogue;
        private Canvas _canvas;
        private bool _processing = false;
        private Transform _dialogueViewContainer;
        private Transform _characterDialogueContainer;
        private TMPro.TMP_Text _characterNameText;
        private TMPro.TMP_Text _characterDialogueText;
        private Transform _playerDialogueContainer;
        private GameObject _dialogueSelectionPrefab;
        private List<GameObject> _currentOptionObjects;
        private List<Dialogue.DialogueOption> _currentOptions;
        private Dialogue _currentDialogue;
        private bool _canRun;
        private bool _cutOffCharText;
       
        void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(this);
            }

            else
            {
                Instance = this;
            }
        }

        void Start()
        {
            CheckReferences();
            if(!_canRun)
            {
                Debug.LogError("Dialogue system cannot run!");
            }
            
        }

        void CheckReferences()
        {
            _canRun = false;
            _canvas = GetComponentInChildren<Canvas>();
            if(!_canvas)
            {
                _canvas = GameObject.FindWithTag("DialogueCanvas").GetComponent<Canvas>();
            }
            if(!_canvas)
            {
                Debug.LogError("Dialogue System cannot run - cannot find the \"Dialogue Canvas\"");
                return;
            }
            
            // checking transform and setting references
            _dialogueViewContainer = transform.Find("Dialogue Canvas/Dialogue View Container");
            _characterDialogueContainer = _dialogueViewContainer.transform.Find("Dialogue View Background/Character Panel");
            _characterDialogueText = _characterDialogueContainer.transform.Find("Character Dialogue").GetComponent<TMPro.TMP_Text>();
            _characterNameText = _characterDialogueContainer.transform.Find("Character Name").GetComponent<TMPro.TMP_Text>();
            _playerDialogueContainer = _dialogueViewContainer.transform.Find("Dialogue View Background/Player Panel");
            _dialogueSelectionPrefab = Resources.Load("Dialogue System/Prefabs/Dialogue Option Button") as GameObject;
            _canRun = true;
            ExitDialogue();
        }

        public void ProcessDialogue(Dialogue dialogue)
        {
            if(dialogue == null) 
            { 
                Debug.LogError("Dialogue System: ProcessDialogue called but given parameter Dialogue was null"); 
                return;
            }


            if(_processing ||!_canRun) { return; }
            _processing = true;
            _currentDialogue = dialogue;
            TriggerStateUpdate(0);
            //RefreshDialogueView(_currentDialogue);
        }

        public void TriggerStateUpdate(int triggerIndex)
        {   
            if(_currentDialogue == null) { Debug.LogError("_currentDialogue is null"); return;}
            if(_currentDialogue.UpdateState(triggerIndex))
            {
                RefreshDialogueView(_currentDialogue);
            }
            else
            {
                ExitDialogue();
            }
        }

        public void ExitDialogue()
        {
            StopAllCoroutines();
            _canvas.enabled = false;
        }

        public void RefreshDialogueView(Dialogue dialogue)
        {
            StopAllCoroutines();
            Dialogue.DialogueState currentState;
            try
            {
                currentState = dialogue.GetCurrentState();
            }

            catch (System.NullReferenceException)
            {
                Debug.LogError("Dialogue System: RefreshDialogueView called but given parameter Dialogue returned null DialogueState");
                return;
            }
            
            _canvas.enabled = true;
            _currentOptions = currentState.DialogueOptions;
            AudioClip charAudio = dialogue.DialogueCharacter.SpeechSoundBite;
            int speed = dialogue.DialogueCharacter.TalkSpeed;

            StartCoroutine(FillOutCharacterDialogue(currentState.CharacterDialogueLine, speed, charAudio));
            _characterNameText.text = dialogue.DialogueCharacter.CharacterName;

            if(_currentOptionObjects != null)
            {
                foreach(GameObject obj in _currentOptionObjects)
                {
                    Destroy(obj);
                }
            }

            _currentOptionObjects = new();

            for(int i = 0; i < _currentOptions.Count; i++)
            {
                _currentOptionObjects.Add(Instantiate(_dialogueSelectionPrefab, _playerDialogueContainer));
                DialogueSystemButtonHandler handler = _currentOptionObjects[i].GetComponent<DialogueSystemButtonHandler>();

                handler.SetUpButton(_currentOptions[i]);
            }
        }

        IEnumerator FillOutCharacterDialogue(string line, int speed = 5, AudioClip characterSoundbite = null)
        {
            string targetLine = line;
            if(InstantDialogue)
            {_characterDialogueText.text = targetLine; yield break; }

            string generatingLine = "";

            int _fpc = 18 - speed;

            for(int i = 0; i < targetLine.Length; i++)
            {
                generatingLine += targetLine[i];
                _characterDialogueText.text = generatingLine;
                
                for(int w = 0; w < _fpc; w++)
                {
                    yield return new WaitForEndOfFrame();
                }
                
                if(characterSoundbite != null && i % 3 == 0) // play sound on every third character
                {
                    GameObject audio = new GameObject("Character soundbite");
                    audio.transform.parent = _characterDialogueContainer;
                    AudioSource AS = audio.AddComponent<AudioSource>();
                    AS.clip = characterSoundbite;
                    AS.volume = 0.5f;
                    AS.pitch = RandomPitch();
                    AS.Play();
                }
            }

            yield return null;
        }

        float RandomPitch() { return Random.Range(0.95f, 1.05f); }
    }
}
