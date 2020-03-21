using UnityEngine;

public class TextTokenizerTest : MonoBehaviour
{
    private void Awake()
    {
        var tk = new TextTokenizer();
        var tokens = tk.Tokenize("TESt *asdf a* ** asf weaf** `wobble`");
        foreach (var token in tokens)
        {
            Debug.Log(token.TokenType + ", " + token.OpenClosed + ": " + token.Value);
        }
    }
}