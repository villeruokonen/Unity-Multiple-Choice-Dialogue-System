using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace DialogueSystem
{
    //[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue System / Dialogue Option ", order = 3)]
    //[System.Serializable]
    public partial class Dialogue : ScriptableObject
    {
        [SerializeField]
        [System.Serializable]
        public class DialogueOption
        {
            [Tooltip("What should be the text displayed as the option for the player to say?")]
            public string PlayerDialogueLine;
            [Tooltip("Whether this option is \"filler\" or \"not important\" - visual difference")]
            public bool IsFillerOption;
            [Tooltip("Which state (by ID) should the option trigger?")]
            public int TriggerStateIndex;
            public void InitializeOption(string playerLine, int stateIndex, bool isFiller)
            {
                PlayerDialogueLine = playerLine;
                TriggerStateIndex = stateIndex;
                IsFillerOption = isFiller;
            }
        }
    }
}
