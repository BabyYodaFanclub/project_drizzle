using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcDialoguePopup : MonoBehaviour
{
    public Canvas DialogueCanvas;
    private bool _playerInRange;
    private bool _dialoguePlaying;

    private void Update()
    {
        if (DialogueCanvas.gameObject.activeSelf)
            if (!_playerInRange && !_dialoguePlaying)
                DialogueCanvas.gameObject.SetActive(false);
        
        if (!DialogueCanvas.gameObject.activeSelf)
            if (_playerInRange)
                DialogueCanvas.gameObject.SetActive(true);

        if (_playerInRange && !_dialoguePlaying && Input.GetButtonUp("Interact"))
            StartDialogue();
    }

    private void StartDialogue()
    {
        throw new NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        _playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _playerInRange = false;
    }
}
