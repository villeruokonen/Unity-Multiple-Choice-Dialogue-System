using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public partial class Dialogue : ScriptableObject
    {
        //[CreateAssetMenu(fileName = "Dialogue State", menuName = "Dialogue System / Dialogue State ", order = 2)]
        [System.Serializable]
        public class DialogueState
        {
            public int StateIndex;
            [Tooltip("What should the non-player-character say in this state?")]
            [SerializeField]
            public string CharacterDialogueLine;
            [Tooltip("The list of options of what to say for the player to choose from")]
            [SerializeField]
            public List<DialogueOption> DialogueOptions;
            public void InitializeState(string characterLine, string[] playerDialogueOptions, int[] playerOptionIds, bool[] optionsAreFiller, int stateIndex)
            {
                CharacterDialogueLine = characterLine;
                DialogueOptions = new List<DialogueOption>(playerDialogueOptions.Length);
                StateIndex = stateIndex;

                for(int i = 0; i < playerDialogueOptions.Length; i++)
                {
                    DialogueOption option = new();
                    option.InitializeOption(playerDialogueOptions[i], playerOptionIds[i], optionsAreFiller[i]);
                    DialogueOptions.Add(option);
                }
                
            }
            
            public DialogueOption[] GetOptions()
            {
                return DialogueOptions.ToArray();
            }
        }
    }
}
