// Decompiled with JetBrains decompiler
// Type: WebApiBusiness.App_Data.Extensoes
// Assembly: WebApiBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2441E174-C302-4098-B419-99DA0C4BEE41
// Assembly location: G:\VOPAK\Source\sam\Api_Smart\bin\WebApiBusiness.dll

using System;

namespace WebApiBusiness.App_Data
{
  public static class Extensoes
  {
    public static void Swap(this string source, ref string target)
    {
      string str = target;
      target = source;
      source = str;
    }

    internal static double Invert(this double min, ref double max)
    {
      return max - min;
    }

    internal static float Compare(this string a, ref string b)
    {
      float num = 0.0f;
      try
      {
        string[] strArray = new string[10]
        {
          " ",
          "-",
          ".",
          ")",
          "_",
          "(",
          "]",
          "[",
          "<",
          ">"
        };
        foreach (string oldValue in strArray)
          b = b.Replace(oldValue, "");
        double min = (double) a.LevenshteinDistance(ref b);
        if (min == 0.0)
          return (float) byte.MaxValue;
        double max = (double) Math.Max(a.Length, b.Length);
        if (min == max)
          return 0.0f;
        num = (float) (byte) (min.Invert(ref max) / max * (double) byte.MaxValue) / (float) byte.MaxValue * 100f;
      }
      catch (Exception ex)
      {
      }
      return num;
    }

    internal static int LevenshteinDistance(this string source, ref string target)
    {
      int num1 = 0;
      try
      {
        if (string.IsNullOrEmpty(source))
          num1 = string.IsNullOrEmpty(target) ? 0 : target.Length;
        if (string.IsNullOrEmpty(target))
          return source.Length;
        if (source.Length > target.Length)
          source.Swap(ref target);
        int length1 = target.Length;
        int length2 = source.Length;
        int[,] numArray = new int[2, length1 + 1];
        for (int index = 1; index <= length1; ++index)
          numArray[0, index] = index;
        int index1 = 0;
        for (int index2 = 1; index2 <= length2; ++index2)
        {
          index1 = index2 & 1;
          numArray[index1, 0] = index2;
          int index3 = index1 ^ 1;
          for (int index4 = 1; index4 <= length1; ++index4)
          {
            int num2 = (int) target[index4 - 1] == (int) source[index2 - 1] ? 0 : 1;
            numArray[index1, index4] = Math.Min(Math.Min(numArray[index3, index4] + 1, numArray[index1, index4 - 1] + 1), numArray[index3, index4 - 1] + num2);
          }
        }
        num1 = numArray[index1, length1];
      }
      catch (Exception)
      {
      }
      return num1;
    }
  }
}
