using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class JsonTools
{
    #region Json解析与生成
    /// <summary>
    /// 对象转为json 如何转换失败 则返回""
    /// </summary>
    /// <typeparam name="T">泛型T</typeparam>
    /// <param name="o">对象</param>
    /// <returns></returns>
    public static string ObjectToJson<T>(object o)
    {
        try
        {
            T obj = (T)o;
            //把对象转为Json
            string jsonData = JsonMapper.ToJson(obj);
            return jsonData;
        }
        catch
        {
            return "";
        }
    }
    /// <summary>
    /// json转为对象
    /// </summary>
    /// <typeparam name="T">泛型T</typeparam>
    /// <param name="jsonData">json数据</param>
    /// <returns>如果转换失败 则引用类型返回null 值类型返回默认值</returns>
    public static T JsonToObject<T>(string jsonData)
    {
        try
        {
            //转换为对象
            FormatJson(jsonData);
            T obj = JsonMapper.ToObject<T>(jsonData);
            return obj;
        }
        catch //(Exception e)
        {
            //UnityEngine.Debug.LogWarning(e.Message + " " + jsonData);
            //MainForm.Print("Json解析错误：" + e.ToString());
            return default(T);
        }
    }

    public static JsonData JsonToObject(string jsonData)
    {
        try
        {
            //转换为对象
            JsonData json = JsonMapper.ToObject(jsonData);
            return json;
        }
        catch
        {
            return null;
        }
    }
    #endregion

    #region Json格式化
    private const string INDENT_STRING = "    ";
    public static string FormatJson(string str)
    {
        var indent = 0;
        var quoted = false;
        var sb = new StringBuilder();
        for (var i = 0; i < str.Length; i++)
        {
            var ch = str[i];
            switch (ch)
            {
                case '{':
                case '[':
                    sb.Append(ch);
                    if (!quoted)
                    {
                        sb.AppendLine();
                        Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
                    }
                    break;
                case '}':
                case ']':
                    if (!quoted)
                    {
                        sb.AppendLine();
                        Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
                    }
                    sb.Append(ch);
                    break;
                case '"':
                    sb.Append(ch);
                    bool escaped = false;
                    var index = i;
                    while (index > 0 && str[--index] == '\\')
                        escaped = !escaped;
                    if (!escaped)
                        quoted = !quoted;
                    break;
                case ',':
                    sb.Append(ch);
                    if (!quoted)
                    {
                        sb.AppendLine();
                        Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
                    }
                    break;
                case ':':
                    sb.Append(ch);
                    if (!quoted)
                        sb.Append(" ");
                    break;
                default:
                    sb.Append(ch);
                    break;
            }
        }
        return sb.ToString();
    }
    #endregion
}

#region Json格式化扩展
/// <summary>
/// 扩展方法
/// </summary>
public static class Extensions
{
    public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
    {
        foreach (var i in ie)
        {
            action(i);
        }
    }
}
#endregion

