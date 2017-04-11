using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public static class JsonExtensions
{
    /// <summary>
    /// 将字典类型序列化为json字符串
    /// </summary>
    /// <typeparam name="TKey">字典key</typeparam>
    /// <typeparam name="TValue">字典value</typeparam>
    /// <param name="dict">要序列化的字典数据</param>
    /// <returns>json字符串</returns>
    public static string SerializeDictionaryToJsonString<TKey, TValue>(Dictionary<TKey, TValue> dict)
    {
        if (dict.Count == 0)
            return "";

        string jsonStr = JsonConvert.SerializeObject(dict);
        return jsonStr;
    }

    /// <summary>
    /// 将json字符串反序列化为字典类型
    /// </summary>
    /// <typeparam name="TKey">字典key</typeparam>
    /// <typeparam name="TValue">字典value</typeparam>
    /// <param name="jsonStr">json字符串</param>
    /// <returns>字典数据</returns>
    public static Dictionary<TKey, TValue> DeserializeStringToDictionary<TKey, TValue>(string jsonStr)
    {
        if (string.IsNullOrEmpty(jsonStr))
            return new Dictionary<TKey, TValue>();

        Dictionary<TKey, TValue> jsonDict = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(jsonStr);

        return jsonDict;
    }

    public static string SerializeObject(object o)
    {
        string json = JsonConvert.SerializeObject(o);
        return json;
    }

    public static T DeserializeJsonToObject<T>(string json) where T : class
    {
        JsonSerializer serializer = new JsonSerializer();
        StringReader sr = new StringReader(json);
        object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
        T t = o as T;
        return t;
    }

    public static List<T> DeserializeJsonToList<T>(string json) where T : class
    {
        JsonSerializer serializer = new JsonSerializer();
        StringReader sr = new StringReader(json);
        object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
        List<T> list = o as  List<T>;
        return list;
    }

    public static T DeserializeAnoymousType<T>(string json,T anoymousTypeObject)
    {
        T t = JsonConvert.DeserializeAnonymousType(json, anoymousTypeObject);
        return t;
    }
}
