using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class ExtensionMethods
{
    public static int GetIndex<T>(this T[] array, T item) => Array.FindIndex(array, val => val.Equals(item));


    public static T GetorAddComponent<T>(this GameObject t_go) where T : Component
    {
        if (null == t_go) { return null; }

        var type = typeof(T);
        var compon = t_go.GetComponent(type);
        if (null == compon)
        {
            compon = t_go.AddComponent(type);
        }
        return compon as T;
    }
    public static T RandomElement<T>(this T[] arry)
    {
        if (arry.IsNullOrEmpty())
            throw new ArgumentNullException(nameof(arry));
        return arry[UnityEngine.Random.Range(0, arry.Length)];
    }

    public static T RandomElement<T>(this List<T> arry)
    {
        if (arry.IsNullOrEmpty())
            throw new ArgumentNullException(nameof(arry));
        return arry[UnityEngine.Random.Range(0, arry.Count)];
    }
    public static float Snap(this float value, float snapingValue)
    {
        if (snapingValue == 0) return value;
        return Mathf.Round(value / snapingValue) * snapingValue;
    }
    public static T GetorAddComponent<T>(this Component component) where T : Component
    {
        if (null == component) { return null; }

        var type = typeof(T);
        var compon = component.gameObject.GetComponent(type);
        if (null == compon)
        {
            compon = component.gameObject.AddComponent(type);
        }
        return compon as T;
    }
    public static bool IsNullOrEmpty<T>(this T[] array) => array == null || array.Length < 1;
    public static bool IsNullOrEmpty<T>(this List<T> list) => list == null || list.Count < 1;
    public static bool IsNullOrEmpty<T>(this Queue<T> queue) => queue == null || queue.Count < 1;
    public static bool IsNullOrEmpty<T1, T2>(this Dictionary<T1, T2> dictionary) => dictionary == null || dictionary.Count < 1;
    public static bool IsNull(this GameObject go)=> (go == null);
    public static bool IsNull(this Component component)=> (component == null);

    public static void Resize<T>(this List<T> list, int size, T element = default(T))
    {
        int count = list.Count;

        if (size < count)
        {
            list.RemoveRange(size, count - size);
        }
        else if (size > count)
        {
            if (size > list.Capacity)
                list.Capacity = size;

            list.AddRange(System.Linq.Enumerable.Repeat(element, size - count));
        }
    }
    public static string ReFormat(this string str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            if (str.Contains(@"\n"))
                str = str.Replace(@"\n", "\n");
            if (str.Contains(@"\t"))
                str = str.Replace(@"\t", "\t");
            if (str.Contains(@"\r"))
                str = str.Replace(@"\r", "\r");
        }
        return str;
    }
    public static void DestroyAll<T>(this T[] arr) where T : MonoBehaviour
    {
        foreach (var item in arr)
        {
            GameObject.Destroy(item.gameObject);
        }
    }
    public static void Then<T>(this ICollection<T> arry, Action<T> callback)
    {
        foreach (var item in arry)
        {
            callback?.Invoke(item);
        }
    }
    public static void DestroyAllchildern(this Transform tans)
    {
        foreach (Transform chiled in tans)
        {
            GameObject.Destroy(chiled.gameObject);
        }
    }
    public static void RemoveCloneName(this GameObject gameObject) => gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
    #region Dictionary.ForEach
    public static void ForEach<TKEY, TValue>(this Dictionary<TKEY, TValue> dict, Action<KeyValuePair<TKEY, TValue>> action)
    {
        foreach (KeyValuePair<TKEY, TValue> entery in dict)
        {
            action?.Invoke(entery);
        }
    }

    public static void ForEach<TKEY, TValue>(this Dictionary<TKEY, TValue> dict, Action<TKEY, TValue> action)
    {
        foreach (KeyValuePair<TKEY, TValue> m_type in dict)
        {
            action?.Invoke(m_type.Key, m_type.Value);
        }
    }
    public static void ForEach<TKEY, TValue>(this Dictionary<TKEY, TValue> dict, Action<TValue> value)
    {
        foreach (KeyValuePair<TKEY, TValue> m_type in dict)
        {
            value?.Invoke(m_type.Value);
        }
    }

    #endregion
}
