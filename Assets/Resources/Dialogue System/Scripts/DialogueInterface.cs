using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueInterface : MonoBehaviour
    {
        [SerializeField]
        public DialogueCharacter Character;
        [SerializeField]
        public Dialogue dialogue;
        
        public void InitiateDialogue()
        {
            dialogue.DialogueCharacter = Character;
            DialogueSystem.Instance.ProcessDialogue(dialogue);
        }
    }
}
