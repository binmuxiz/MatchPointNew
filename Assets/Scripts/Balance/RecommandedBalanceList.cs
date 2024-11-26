using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RecommandedBalanceList
{
    private string topic_id;
    public List<Pair> options;
}

[Serializable]
public struct Pair
{
    public string left;
    public string right;
}
