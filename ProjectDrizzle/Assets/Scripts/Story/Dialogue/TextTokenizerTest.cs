using UnityEngine;

public class TextTokenizerTest : MonoBehaviour
{
    [ContextMenu("testTokenizer")]
    private void Test()
    {
        var tk = TextTokenizer.Instance;
        var tokens = tk.Tokenize("`<#ff0000>Press</color>` **E** <b>to</b> `<#00ff00>chat</color>`");
        foreach (var token in tokens)
        {
            Debug.Log($"{token.TokenType}, {token.OpenClosed}, index {token.LetterIndex}, indexNoWS {token.LetterIndexNoWhiteSpaces} : {token.Value}");
        }
    }
    
    [ContextMenu("TestDialogue")]
    public void TestDialogue()
    {
        FindObjectOfType<NpcDialoguePopup>().PlayDialogueText("`<#ff0000>Press</color>` **E** <b>to</b> `<#00ff00>chat</color>`");
    }
}