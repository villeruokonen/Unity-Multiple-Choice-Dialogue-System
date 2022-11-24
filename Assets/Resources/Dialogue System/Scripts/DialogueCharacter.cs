using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "Dialogue Character", menuName = "Dialogue System / Dialogue Character", order = 1)]
    public class DialogueCharacter : ScriptableObject
    {
        [Header("Character Options")]
        public string CharacterName;
        public AudioClip SpeechSoundBite;
        [Range(1,10)]
        public int TalkSpeed = 5;

        public void InitializeCharacter(string name, int talkSpeed, AudioClip speechClip = null)
        {
            CharacterName = name;
            TalkSpeed = talkSpeed;
            SpeechSoundBite = speechClip;
        }
    }
}
