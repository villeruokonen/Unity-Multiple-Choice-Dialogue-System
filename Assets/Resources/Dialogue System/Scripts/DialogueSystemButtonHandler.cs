using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DialogueSystem
{
    public class DialogueSystemButtonHandler : MonoBehaviour, ISelectHandler
    {
        private Dialogue.DialogueOption thisOption;
        public int DialogueButtonIndex;
        public void SetUpButton(Dialogue.DialogueOption option)
        {
            thisOption = option;
            DialogueButtonIndex = option.TriggerStateIndex;
            Selectable selectable = GetComponent<Selectable>();
            selectable.transition = Selectable.Transition.ColorTint;
            TMPro.TMP_Text optionText = selectable.GetComponentInChildren<TMPro.TMP_Text>();
            optionText.text = option.PlayerDialogueLine;

            ColorBlock cb = new();
            Color baseColor = option.IsFillerOption ? DialogueSystem.Instance.FillerBaseColor : DialogueSystem.Instance.BaseColor;
            cb.highlightedColor = baseColor * 0.85f;
            cb.normalColor = baseColor;
            cb.selectedColor = cb.highlightedColor;
            cb.pressedColor = cb.normalColor;
            cb.colorMultiplier = 1;
            cb.fadeDuration = 0.05f;

            selectable.colors = cb;
            selectable.targetGraphic = optionText;
        }

        public void OnSelect(BaseEventData data)
        {
            DialogueSystem.Instance.TriggerStateUpdate(DialogueButtonIndex);
        }

        
    }
}
