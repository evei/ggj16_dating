using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Globalization;

public static class StringExtensions
{
	public static Stream ToStream (this string str)
	{
		var stream = new MemoryStream();
		var writer = new StreamWriter(stream);
		writer.Write(str);
		writer.Flush();
		stream.Position = 0;
		return stream;
	}

	public static String ToLocalUpper(this string str, CultureInfo cultureInfo)
	{
		if (cultureInfo == null) 
		{
			return str.ToUpper();
		}
		return str.ToUpper (cultureInfo);
	}

	public static bool EqualsIgnoreCase(this string str, string other)
	{
		return (str==null && other==null) || str.EqualsIgnoreCase(other, true);
	}

	public static bool EqualsIgnoreCase(this string str, string other, bool useCurrentCulture)
	{
		return (str==null && other==null) || str.Equals(other, useCurrentCulture ? StringComparison.CurrentCultureIgnoreCase : StringComparison.InvariantCultureIgnoreCase);
	}

	public static bool StartsWithIgnoreCase(this string str, string other)
	{
		return (str==null && other==null) || str.StartsWithIgnoreCase(other, true);
	}

	public static bool StartsWithIgnoreCase(this string str, string other, bool useCurrentCulture)
	{
		return (str==null && other==null) || str.StartsWith(other, useCurrentCulture ? StringComparison.CurrentCultureIgnoreCase : StringComparison.InvariantCultureIgnoreCase);
	}

	public static bool EndsWithIgnoreCase(this string str, string other)
	{
		return (str==null && other==null) || str.EndsWithIgnoreCase(other, true);
	}

	public static bool EndsWithIgnoreCase(this string str, string other, bool useCurrentCulture)
	{
		return (str==null && other==null) || str.EndsWith(other, useCurrentCulture ? StringComparison.CurrentCultureIgnoreCase : StringComparison.InvariantCultureIgnoreCase);
	}

	public static bool Contains(this string source, string toCheck, StringComparison comp)
	{
		return toCheck == null || source.IndexOf(toCheck, comp) >= 0;
	}

	public static bool ContainsIgnoreCase(this string source, string toCheck)
	{
		return source.Contains(toCheck, StringComparison.OrdinalIgnoreCase);
	}

	public static string CombinePath(this string str, params string[] otherStrings)
	{
		var result = str;
		foreach (var otherString in otherStrings)
		{
			result = Path.Combine(result, otherString);
		}
		return result;
	}

	public static bool IsNullOrEmpty(this string str)
	{
		return string.IsNullOrEmpty(str);
	}

	public static string SplitCamelCase( this string str )
	{
		return Regex.Replace(
			Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), 
			@"(\p{Ll})(\P{Ll})", 
			"$1 $2");
	}	
}