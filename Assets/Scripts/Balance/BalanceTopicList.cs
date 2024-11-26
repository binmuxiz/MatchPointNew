using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BalanceTopicList
{
    public List<Triple> triples;
}

[Serializable]
public struct Triple
{
    public string user_id;
    public string topic_id;
    public string topic;
}
