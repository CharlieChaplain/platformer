using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

	public Dialogue dialogue;
	public bool isPerson;

	public void TriggerDialogue(){
		DialogueManager.Instance.StartDialogue(dialogue);
	}
}
