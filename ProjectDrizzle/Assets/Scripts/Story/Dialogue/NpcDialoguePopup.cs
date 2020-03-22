using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BrunoMikoski.TextJuicer;
using TMPro;
using UnityEngine;

public class NpcDialoguePopup : MonoBehaviour
{
    public Canvas DialogueCanvas;
    public TMP_TextJuicer TextComponent;
    public TextJuicerCustomModifier TextModifier;
    private bool _playerInRange;
    private bool _dialoguePlaying;

    private void Update()
    {
        if (!DialogueCanvas.gameObject.activeSelf
            && _playerInRange)
        {
            DialogueCanvas.gameObject.SetActive(true);
            PlayDialogueText("`<#ff0000>Press</color>` **E** <b>to</b> `<#00ff00>chat</color>`");
        }
        else if (DialogueCanvas.gameObject.activeSelf
                 && !_playerInRange
                 && !_dialoguePlaying)
        {
            DialogueCanvas.gameObject.SetActive(false);
        }
        else if (_playerInRange
                 && Input.GetButtonUp("Interact"))
        {
            if (!_dialoguePlaying)
                StartDialogue();
            else
                ContinueDialogue();
        }
    }

    public void PlayDialogueText(string text)
    {
        var tokenizer = TextTokenizer.Instance;

        var tokens = tokenizer.Tokenize(text).ToList();
        var configs = new List<TextAnimationConfig>();
        var currentConfigs = new Dictionary<TokenType, TextAnimationConfig>();

        foreach (var token in tokens)
        {
            switch (token.TokenType)
            {
                case TokenType.Text:
                    break;
                case TokenType.Fat:
                    token.TokenType = TokenType.Text;
                    token.Value = token.OpenClosed == OpenClose.Open
                        ? "<b>"
                        : "</b>";
                    break;
                case TokenType.Italic:
                    token.TokenType = TokenType.Text;
                    token.Value = token.OpenClosed == OpenClose.Open
                        ? "<i>"
                        : "</i>";
                    break;
                case TokenType.Wobble:
                    TextAnimationConfig config;
                    if (currentConfigs.ContainsKey(TokenType.Wobble) && currentConfigs[TokenType.Wobble] != null)
                    {
                        config = currentConfigs[TokenType.Wobble];
                        config.MaxCharIndex = token.LetterIndexNoWhiteSpaces;
                        currentConfigs.Remove(TokenType.Wobble);
                    }
                    else
                    {
                        config = new TextAnimationConfig
                        {
                            MinCharIndex = token.LetterIndexNoWhiteSpaces,
                            DoWobble = true,
                            WobbleIntensity = Vector3.up * 5
                        };
                        currentConfigs[TokenType.Wobble] = config;
                        configs.Add(config);
                    }

                    break;
                case TokenType.HtmlTag:
                    token.TokenType = TokenType.Text;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        TextModifier.ConfiguredEffects = configs;

        TextComponent.Text = tokens
            .Where(t => t.TokenType == TokenType.Text)
            .Aggregate("", (s, token) => s + token.Value);
    }

    private void StartDialogue()
    {
        _dialoguePlaying = true;

        PlayDialogueText("<#00ff00>*Henlo*</color>");
    }

    private void ContinueDialogue()
    {
        PlayDialogueText("There 😘");
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