using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Implicit version of token hierarchy, derived from a combined tags
 * Unlike a normal node it does not know its children or parent and its Depth is given programatically
 * Using a full string instead would make changes to the Tag hard to propagate down after serialization
 */
[Serializable]
public class GameplayTagToken
{
    public string Token;
    public int Depth;
    public bool bIsFolded;

    public GameplayTagToken(string Token, int Depth, bool bIsFolded)
    {
        this.Token = Token;
        this.Depth = Depth;
        this.bIsFolded = bIsFolded;
    }

    public override string ToString()
    {
        return Token;
    }

    public override bool Equals(object obj)
    {
        if (obj is not GameplayTagToken) 
            return false;

        GameplayTagToken Other = (GameplayTagToken)obj;
        return Token.Equals(Other.Token) && Depth == Other.Depth && bIsFolded == Other.bIsFolded;
    }

    public override int GetHashCode()
    {
        return Token.GetHashCode();
    }
}
