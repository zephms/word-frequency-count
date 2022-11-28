using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlindSearchAlgorithm
{
    static class Algo
    {
        public static String readtxt(String path)
        {
            try
            {
                if (File.Exists(path))
                {
                    string alltext = File.ReadAllText(path, Encoding.UTF8);
                    return (alltext.Replace("'", "\""));

                }
                else
                {
                    Console.WriteLine("文件不存在");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public static Dictionary<int, Dictionary<string, int>> BlindSearch(string src, int size,int stringlen)
        {
            if (size < 2)
            {
                return null;
            }
            var target = new Dictionary<int, Dictionary<string, int>>();
            var charMaps = new Dictionary<char, int>();
            var charIndex = new Dictionary<char, List<int>>();
            var doubleIndex= new Dictionary<string, List<int>>();
            var doubleCharMap = new Dictionary<string, int>();
            getBase(src, charMaps, charIndex, stringlen);
            getSecond(src, charMaps, charIndex, doubleIndex, doubleCharMap, stringlen);
            if (size == 2)
            {
                target.Add(2,doubleCharMap);
                return target;
            }
            var lastCharMap = doubleCharMap;
            var lastIndex = doubleIndex;
            for (int i=2;i<size; i++)
            {
                var lenIndex = new Dictionary<string, List<int>>();
                var lenCharMap = new Dictionary<string, int>();
                countForLen(src, lastCharMap, charMaps, lastIndex, lenIndex, lenCharMap, i, stringlen);
                lastCharMap = lenCharMap;
                lastIndex = lenIndex;
                target.Add(i + 1, lenCharMap);
            }
            return target;
        }
        public static void countForLen(string Forsearch, Dictionary<string, int> forwardCharMap, Dictionary<char, int> baseCharmap, Dictionary<string, List<int>> lastIndex, Dictionary<string, List<int>> nowIndex, Dictionary<string, int> lenCharMap,int len,int stringlen)
        {
            foreach (var pair in forwardCharMap)
            {
                foreach (int index in lastIndex[pair.Key])//遍历这个char的所有位置
                {
                    if (index < Forsearch.Length - len)//判断字符串位置.....可优化
                    {
                        if (baseCharmap.ContainsKey(Forsearch[index + len]))//如果这个char的下一个char频数也大于1
                        {
                            string now = pair.Key + Forsearch[index + len];
                            if (lenCharMap.ContainsKey(now))
                            {
                                nowIndex[now].Add(index);
                                lenCharMap[now] = lenCharMap[now] + 1;
                            }
                            else
                            {
                                nowIndex.Add(now, new List<int> { index });
                                lenCharMap.Add(now, 1);
                            }
                        }
                    }
                }

            }
            foreach (var pair in lenCharMap.ToArray())
            {
                if (pair.Value < stringlen)
                {
                    nowIndex.Remove(pair.Key);
                    lenCharMap.Remove(pair.Key);
                }
            }
        }

        public static void getSecond(string Forsearch, Dictionary<char, int> baseCharmap, Dictionary<char, List<int>> charIndex, Dictionary<string, List<int>> doubleindex, Dictionary<string, int> doubleCharMap,int stringlen)
        {
            foreach (var pair in baseCharmap)
            {
                foreach (int index in charIndex[pair.Key])//遍历这个char的所有位置
                {
                    if (index < Forsearch.Length - 1)
                    {
                        if (baseCharmap.ContainsKey(Forsearch[index + 1]))//如果这个char的下一个char频数也大于1
                        {
                            string now = new string(new char[] { Forsearch[index], Forsearch[index + 1] });
                            if (doubleCharMap.ContainsKey(now))
                            {
                                doubleindex[now].Add(index);
                                doubleCharMap[now] = doubleCharMap[now] + 1;
                            }
                            else
                            {
                                doubleindex.Add(now, new List<int>() { index });
                                doubleCharMap.Add(now, 1);
                            }
                        }
                    }
                }
            }
            foreach (var pair in doubleCharMap.ToArray())
            {
                if (pair.Value < stringlen)
                {
                    doubleindex.Remove(pair.Key);
                    doubleCharMap.Remove(pair.Key);
                }
            }
        }
        public static void getBase(string ForSearch, Dictionary<char, int> charmaps, Dictionary<char, List<int>> charIndex,int stringlen)
        {
            for (int i = 0; i < ForSearch.Length; i++)
            {
                if (charmaps.ContainsKey(ForSearch[i]))
                {
                    charmaps[ForSearch[i]] = charmaps[ForSearch[i]] + 1;
                    charIndex[ForSearch[i]].Add(i);

                }
                else
                {
                    charmaps[ForSearch[i]] = 1;
                    charIndex[ForSearch[i]] = new List<int>() { i };
                }
            }
            foreach (var pair in charmaps.ToArray())
            {
                if (pair.Value < stringlen || pair.Key == '\r' || char.IsWhiteSpace(pair.Key) || pair.Key == '\n' || pair.Key == ' ' || char.IsPunctuation(pair.Key))
                {
                    charmaps.Remove(pair.Key);
                    charIndex.Remove(pair.Key);
                }
            }
        }
    }
}
