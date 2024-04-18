using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Class wrapper for tokens and reflective storage, similar to a Trie */
public class GameplayTagNode
{
    public string Token = string.Empty;
    public List<GameplayTagNode> Children = new();
    public GameplayTagNode Parent = null;
    public bool bIsFolded;

    public GameplayTagNode(string Token, bool bIsFolded = false)
    {
        this.Token = new(Token);
        this.bIsFolded = bIsFolded;
    }

    public bool Matches(string Token)
    {
        return this.Token.Equals(Token);
    }

    public GameplayTagNode GetOrAddChild(string Token, bool bIsFolded = false)
    {
        foreach (GameplayTagNode Child in Children)
        {
            if (Child.Matches(Token))
                return Child;
        }

        GameplayTagNode NewNode = new(Token, bIsFolded);
        NewNode.Parent = this;
        Children.Add(NewNode);
        return NewNode;
    }

    private static string[] ConvertToTokens(string String)
    {
        return String.Split(Divisor);
    }

    public override string ToString()
    {
        return Token;
    }

    private int GetDepth()
    {
        return GetTagFromToken().Split(Divisor).Length - 1;
    }

    private string GetTagFromToken()
    {
        string ParentTag = Parent != null && !Parent.Token.Equals(string.Empty) ? Parent.GetTagFromToken() + Divisor : "";
        return ParentTag + Token;
    }

    public GameplayTagToken ToGameplayTagToken()
    {
        return new GameplayTagToken(Token, GetDepth(), bIsFolded);
    }

    public void AddRange(List<string> Tags)
    {
        foreach (string Tag in Tags)
        {
            Add(Tag);
        }
    }

    public void Add(string Tag)
    {
        string[] Tokens = ConvertToTokens(Tag);
        GameplayTagNode Temp = this;
        foreach (string Token in Tokens)
        {
            Temp = Temp.GetOrAddChild(Token);
        }
    }

    public static GameplayTagNode ConvertToDeserializedNodes(List<GameplayTagToken> Tokens)
    {
        GameplayTagNode Root = new(string.Empty);
        Dictionary<int, string> DepthTags = new();
        foreach (GameplayTagToken Token in Tokens)
        {
            GameplayTagNode Temp = Root;
            for (int i = 0; i < Token.Depth; i++)
            {
                Temp = Temp.GetOrAddChild(DepthTags[i]);
            }

            if (!DepthTags.ContainsKey(Token.Depth))
            {
                DepthTags.Add(Token.Depth, Token.Token);
            }
            DepthTags[Token.Depth] = Token.Token;
            Temp.GetOrAddChild(Token.Token, Token.bIsFolded);
        }
        return Root;
    }

    public static List<GameplayTagToken> ConvertToSerializedList(GameplayTagNode Node)
    {
        List<GameplayTagToken> List = new();
        if (!Node.Token.Equals(string.Empty))
        {
            List.Add(Node.ToGameplayTagToken());
        }

        foreach (GameplayTagNode Child in Node.Children)
        {
            List.AddRange(ConvertToSerializedList(Child));
        }
        return List;
    }

    public static char Divisor = '.';
}
