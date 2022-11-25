# Unity-Multiple-Choice-Dialogue-System
 Simple dialogue system, supports multiple player dialogue options, character soundbytes (animal crossing style).

https://user-images.githubusercontent.com/99807061/203967493-1c062f09-1b20-4149-b16a-f5e1aaa0c48a.mp4

How-to:
Use the Dialogue Builder tool found in Unity's Window / Dialogue System / Dialogue Builder to more simply create new Dialogues.
Alternatively, you can create Dialogue and Dialogue Character ScriptableObject assets manually through Assets / Create / Dialogue System.

Dialogues consist of states, with a character line, player lines and a state index or ID.
To connect different player lines and dialogue states to each other, match the lines' trigger IDs to the Dialogue State Index you want to trigger.

To use the Dialogue System, find the Dialogue Container prefab in Resources / Dialogue System / Prefabs and import it into a scene.
Then attach the DialogueInterface script to a GameObject and use the Interface script's "InitiateDialogue" method to trigger
the dialogue you've assigned to the script in the editor view.
Optionally, you can assign a different character here to override the Character held by the Dialogue object. 

You can also initiate dialogues through the DialogueSystem static instance - pass a Dialogue parameter to the DialogueSystem.Instance's "ProcessDialogue" method.

Dialogue options that do not trigger a state will cause the system to exit the dialogue.

Use the JSON Tools to create a JSON file based on your Dialogue. You can find the tools in Unity's Window / Dialogue System / Dialogue JSON Tools.
You can edit this JSON and then apply those changes by using the JSON Tools' "Update from JSON" option. Just make sure you don't move the JSON file from the folder the system creates for you.

You can find a simple example scene in the DialogueSystem / Example folder.
