using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.Assertions;

public class TextTokenizer
{
    private readonly List<TokenDefinition> _tokenDefinitions;

    public TextTokenizer()
    {
        _tokenDefinitions = new List<TokenDefinition>
        {
            new TokenDefinition(TokenType.Fat, "\\*", 2, isOpenCloseStyle: true),
            new TokenDefinition(TokenType.Italic, "\\*\\*", 1, isOpenCloseStyle: true),
            new TokenDefinition(TokenType.Wobble, "`", 1, isOpenCloseStyle: true),
            new TokenDefinition(TokenType.Text, ".", 100, isOpenCloseStyle: true),
        };
    }

    public IEnumerable<Token> Tokenize(string errorMessage)
    {
        var tokenMatches = FindTokenMatches(errorMessage);

        var groupedByIndex = tokenMatches.GroupBy(x => x.StartIndex)
            .OrderBy(x => x.Key)
            .ToList();

        TokenMatch lastMatch = null;
        var collectedTokens = new List<Token>();
        foreach (var tokens in groupedByIndex)
        {
            var bestMatch = tokens.OrderBy(x => x.Precedence).First();
            if (lastMatch != null && bestMatch.StartIndex < lastMatch.EndIndex)
                continue;

            collectedTokens.Add(new Token(bestMatch.TokenType, bestMatch.Value, bestMatch.OpenClosed));

            lastMatch = bestMatch;
        }

        // merging succeeding text token into one
        var withMergedTokens = new List<Token>();
        Token last = null;
        foreach (var token in collectedTokens)
        {
            if (token.TokenType == TokenType.Text 
                && last != null 
                && last.TokenType == TokenType.Text)
                last.Value += token.Value;
            else
            {
                withMergedTokens.Add(token);
                last = token;
            }
        }
        
        return withMergedTokens;
    }

    private IEnumerable<TokenMatch> FindTokenMatches(string errorMessage)
    {
        var tokenMatches = new List<TokenMatch>();

        foreach (var tokenDefinition in _tokenDefinitions)
            tokenMatches.AddRange(tokenDefinition.FindMatches(errorMessage).ToList());

        return tokenMatches;
    }
}

public class Token
{
    public Token(TokenType tokenType)
    {
        TokenType = tokenType;
        Value = string.Empty;
    }

    public Token(TokenType tokenType, string value, OpenClose openClosed)
    {
        TokenType = tokenType;
        Value = value;
        OpenClosed = openClosed;
    }

    public TokenType TokenType { get; }
    public string Value { get; set; }
    public OpenClose OpenClosed { get; }

    public Token Clone()
    {
        return new Token(TokenType, Value, OpenClosed);
    }
}

public enum TokenType
{
    Text,
    Fat,
    Italic,
    Wobble,
}

public class TokenDefinition
{
    private Regex _regex;
    private readonly TokenType _returnsToken;
    private readonly int _precedence;
    private readonly bool _isOpenCloseStyle;
    private readonly bool _doAggregate;

    public TokenDefinition(TokenType returnsToken, string regexPattern, int precedence,
        bool isOpenCloseStyle = false, bool doAggregate = false)
    {
        _regex = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        _returnsToken = returnsToken;
        _precedence = precedence;
        _isOpenCloseStyle = isOpenCloseStyle;
        _doAggregate = doAggregate;
    }

    public IEnumerable<TokenMatch> FindMatches(string inputString)
    {
        var matches = _regex.Matches(inputString);

        if (matches.Count == 0) yield break;

        var lastToken = MatchToToken(matches[0], OpenClose.Closed);
        for (int i = 1; i < matches.Count; i++)
        {
            var currentToken = MatchToToken(matches[i], lastToken.OpenClosed);
            yield return lastToken;
            lastToken = currentToken;
        }

        yield return lastToken;
    }

    private TokenMatch MatchToToken(Match match, OpenClose lastOpenClosed)
    {
        var thisOpenClosed = lastOpenClosed == OpenClose.Open
            ? OpenClose.Closed
            : OpenClose.Open;

        return new TokenMatch()
        {
            StartIndex = match.Index,
            EndIndex = match.Index + match.Length,
            TokenType = _returnsToken,
            Value = match.Value,
            Precedence = _precedence,
            OpenClosed = _isOpenCloseStyle
                ? thisOpenClosed
                : OpenClose.None,
        };
    }

    public TokenMatch CombineTokens(TokenMatch firstToken, TokenMatch secondToken)
    {
        Assert.AreEqual(firstToken.TokenType, secondToken.TokenType);
        return new TokenMatch()
        {
            StartIndex = firstToken.StartIndex,
            EndIndex = secondToken.EndIndex,
            TokenType = _returnsToken,
            Value = firstToken.Value + secondToken.Value,
            Precedence = _precedence,
            OpenClosed = firstToken.OpenClosed,
        };
    }
}

public enum OpenClose
{
    Open,
    Closed,
    None
}

public class TokenMatch
{
    public TokenType TokenType { get; set; }
    public string Value { get; set; }
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }
    public int Precedence { get; set; }
    public OpenClose OpenClosed { get; set; }
}