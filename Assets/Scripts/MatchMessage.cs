using Nakama.TinyJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MatchMessage<T>
{
    public static T Parse(string json)
    {
        return JsonParser.FromJson<T>(json);
    }

    public static string ToJson(T message)
    {
        return JsonWriter.ToJson(message);
    }
}
