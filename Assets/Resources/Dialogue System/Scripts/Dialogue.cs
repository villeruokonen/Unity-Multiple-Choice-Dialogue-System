using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue System / Dialogue ", order = 2)]
    public partial class Dialogue : ScriptableObject
    {
        public DialogueCharacter DialogueCharacter;
        public List<DialogueState> DialogueStates;
        DialogueState _curState;
        public DialogueState GetCurrentState()
        {
            if(_curState == null) { Debug.LogError("DialogueState DefaultState null"); return null;}
            return _curState;
        }

        void Awake()
        {
            _curState = DialogueStates[0];
        }

        public bool UpdateState(int stateIndex)
        {
            foreach(DialogueState state in DialogueStates)
            {
                if(state.StateIndex == stateIndex)
                {
                    _curState = state;
                    return true;
                }
            }

            return false;
        }

        public void InitializeDialogue(DialogueCharacter character, DialogueState mainState)
        {
            DialogueStates = new();
            DialogueCharacter = character;
            _curState = mainState;
            DialogueStates.Add(_curState);
            //DefaultState = mainState;
        }

    }
}
