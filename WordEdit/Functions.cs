﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WordEdit
{
    /// <summary>
    /// 集成了文本编辑器中所有的文字部分相关的算法
    /// 制作者：马孟凯
    /// </summary>
    public class Functions
    {
        private string NL = Environment.NewLine;
        private string TAB = "\t";
        private string EMPTY = string.Empty;
        private Dictionary<string, string> EscCharList = new Dictionary<string, string>();

        /// <summary>
        /// 一个简单的构造函数
        /// 对于程序内置的转义字符进行定义
        /// </summary>
        public Functions()
        {
            // 初始化转义字符列表
            EscCharList.Add("%return%", NL);
            EscCharList.Add("%tab%", TAB);
        }

        /// <summary>
        /// 关于括号的函数
        /// </summary>
        /// <param name="origin">原字符串</param>
        /// <param name="left">左括号</param>
        /// <param name="right">右括号</param>
        /// <param name="deleteBracket">是否删除括号</param>
        /// <returns></returns>
        public string RemoveBracket(string origin, string left, string right, bool deleteBracket)
        {
            if (left.Length > origin.Length || right.Length > origin.Length)
                return origin;
            if (left.Length == 0 || right.Length == 0)
                return origin;

            left = ChangeEscChar(left);
            right = ChangeEscChar(right);

            string sentence = "";
            // 使用正则表达式
            // 如果left、right中有特殊字符，则需要首先进行转换
            string le = left, ri = right;
            left = CheckRegExpStyle(left);
            right = CheckRegExpStyle(right);

            Regex r = new Regex(left + ".*?" + right);
            if (deleteBracket)
            {
                sentence = r.Replace(origin, "");
            }
            else
            {
                sentence = r.Replace(origin, le + ri);
            }

            return sentence;
        }

        /// <summary>
        /// 关于括号的函数
        /// </summary>
        /// <param name="origin">原字符串</param>
        /// <param name="left">左括号</param>
        /// <param name="right">右括号</param>
        /// <param name="deleteBracket">是否删除括号</param>
        /// <param name="content">被保留的括号中的内容（用换行符分开）</param>
        /// <returns></returns>
        public string RemoveBracket(string origin, string left, string right, bool deleteBracket, ref string content)
        {
            if (left.Length > origin.Length || right.Length > origin.Length)
                return origin;
            if (left.Length == 0 || right.Length == 0)
                return origin;

            left = ChangeEscChar(left);
            right = ChangeEscChar(right);

            string sentence = "";
            // 使用正则表达式
            // 如果left、right中有特殊字符，则需要首先进行转换
            string le = left, ri = right;
            left = CheckRegExpStyle(left);
            right = CheckRegExpStyle(right);

            Regex r = new Regex(left + ".*?" + right, RegexOptions.Singleline);
            // 比上面的方法多了这一个步骤：保留匹配到的内容
            MatchCollection mc = r.Matches(origin);
            if (mc.Count > 0)
            {
                StringBuilder sb = new StringBuilder(mc[0].ToString());
                for (int i = 1; i < mc.Count; i++)
                {
                    sb.Append(NL);
                    sb.Append(mc[i].ToString());
                }
                content = sb.ToString();
            }
            else
            {
                content = "";
            }
            if (deleteBracket)
            {
                sentence = r.Replace(origin, "");
            }
            else
            {
                sentence = r.Replace(origin, le + ri);
            }

            return sentence;
        }

        /// <summary>
        /// 使用正则表达式
        /// </summary>
        /// <param name="origin">原文本</param>
        /// <param name="from">被替换内容</param>
        /// <param name="to">替换为</param>
        /// <param name="options">正则表达式选项</param>
        /// <returns></returns>
        public string UseRegExp(string origin, string from, string to,
            RegexOptions options = RegexOptions.None)
        {
            to = ChangeEscChar(to);
            string temp = origin;
            try
            {
                Regex r = new Regex(from, options);
                temp = r.Replace(temp, to);
                return temp;
            }
            catch (Exception)
            {
                return origin;
            }
        }

        /// <summary>
        /// 使用正则表达式，并保留匹配到的内容
        /// </summary>
        /// <param name="origin">原文本</param>
        /// <param name="from">被替换内容</param>
        /// <param name="to">替换为</param>
        /// <param name="content">匹配到的内容（用换行符分开）</param>
        /// <param name="options">正则表达式选项</param>
        /// <returns></returns>
        public string UseRegExp(string origin, string from, string to, out string content,
            RegexOptions options = RegexOptions.None)
        {
            content = string.Empty;

            to = ChangeEscChar(to);
            string temp = origin;
            try
            {
                Regex r = new Regex(from, options);

                // 比上面多出这一步：保留Match内容
                MatchCollection mc = r.Matches(temp);
                if (mc.Count > 0)
                {
                    StringBuilder sb = new StringBuilder(mc[0].ToString());
                    for (int i = 1; i < mc.Count; i++)
                    {
                        sb.Append(NL);
                        sb.Append(mc[i].ToString());
                    }
                    content = sb.ToString();
                }

                temp = r.Replace(temp, to);
                return temp;
            }
            catch (Exception)
            {
                return origin;
            }
        }

        /// <summary>
        /// 使用正则表达式，仅获取匹配的内容，不替换
        /// </summary>
        /// <param name="origin">原文本</param>
        /// <param name="matching">查找的内容</param>
        /// <param name="content">匹配到的内容</param>
        /// <param name="options">正则表达式选项</param>
        /// <returns></returns>
        public string UseRegExp(string origin, string matching, out string content,
            RegexOptions options = RegexOptions.None)
        {
            content = string.Empty;
            
            string temp = origin;
            try
            {
                Regex r = new Regex(matching, options);
                
                MatchCollection mc = r.Matches(temp);
                if (mc.Count > 0)
                {
                    StringBuilder sb = new StringBuilder(mc[0].ToString());
                    for (int i = 1; i < mc.Count; i++)
                    {
                        sb.Append(NL);
                        sb.Append(mc[i].ToString());
                    }
                    content = sb.ToString();
                }
                return temp;
            }
            catch (Exception)
            {
                return origin;
            }
        }

        /// <summary>
        /// 使用自定义交换列表进行文字逐个匹配
        /// </summary>
        /// <param name="origin">原始段落</param>
        /// <param name="from">字符串列表1</param>
        /// <param name="to">字符串列表2</param>
        /// <param name="direct">true表示1到2，false表示2到1</param>
        /// <returns></returns>
        public string Transform(string origin, List<string> from, List<string> to, bool direct)
        {
            string temp = origin;

            if (from.Count != to.Count) throw new OverflowException("数组长度不相等");

            if (!direct) Exchange(ref from, ref to);

            for (int i = 0; i < from.Count; i++)
            {
                temp = Replace(temp, from[i], to[i]);
            }

            return temp;
        }

        /// <summary>
        /// 替换字符串
        /// </summary>
        /// <param name="origin">原字符串</param>
        /// <param name="from">需要替换的内容</param>
        /// <param name="to">替换为的内容</param>
        /// <param name="isCaseSensitive">是否区分大小写</param>
        /// <returns></returns>
        public string Replace(string origin, string from, string to, bool isCaseSensitive = true)
        {
            // 将origin去格式化，以免被正则表达式误解
            from = CheckRegExpStyle(from);
            // 如果from或to的内容包含“%return%”，则表示替换为换行符或制表符
            from = ChangeEscChar(from);
            to = ChangeEscChar(to);

            if (!isCaseSensitive)
            {
                return UseRegExp(origin, from, to, RegexOptions.IgnoreCase);
            }
            else
            {
                return UseRegExp(origin, from, to);
            }
        }

        /// <summary>
        /// 使用VB的库函数，进行文字格式转换
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="type">1简转繁，2繁转简，3小转大，4大转小</param>
        /// <returns></returns>
        public string TransformStr(string origin, int type)
        {
            string temp = origin;
            if (type == 1)
            {
                temp = Strings.StrConv(temp, VbStrConv.TraditionalChinese, 0);
            }
            else if (type == 2)
            {
                temp = Strings.StrConv(temp, VbStrConv.SimplifiedChinese, 0);
            }
            else if (type == 3)
            {
                temp = Strings.StrConv(temp, VbStrConv.Uppercase, 0);
            }
            else if (type == 4)
            {
                temp = Strings.StrConv(temp, VbStrConv.Lowercase, 0);
            }
            else if (type == 5)
            {
                temp = Strings.StrConv(temp, VbStrConv.ProperCase, 0);
            }
            return temp;
        }

        /// <summary>
        /// 隔行删除
        /// </summary>
        /// <param name="origin">原文本</param>
        /// <param name="every">每隔几行</param>
        /// <param name="delete">删除几行</param>
        /// <param name="removed">保留删除掉的内容</param>
        /// <returns></returns>
        public string RemoveLines(string origin, int every, int delete, ref string removed)
        {
            List<string> list = SplitByStr(origin, NL, true);
            int count = list.Count;
            if (count == 1) return origin;

            List<string> temp = new List<string>();
            List<string> temp2 = new List<string>();

            int i = 0;
            while (i < count)
            {
                for (int j = 0; j < every; j++)
                {
                    if (i + j < list.Count)
                    {
                        temp.Add(list[i + j]);
                    }
                    else break;
                }
                i += every;
                for (int k = 0; k < delete; k++)
                {
                    if (i + k < list.Count)
                    {
                        temp2.Add(list[i + k]);
                    }
                    else break;
                }
                i += delete;
            }

            removed = CombineStringList(temp2, NL);
            return CombineStringList(temp, NL);
        }

        /// <summary>
        /// 隔行插入内容
        /// </summary>
        /// <param name="origin1">原文本</param>
        /// <param name="origin2">插入文本</param>
        /// <param name="every">每隔几行</param>
        /// <param name="insert">插入几行</param>
        /// <returns></returns>
        public string InsertLines(string origin1, string origin2, int every, int insert)
        {
            StringBuilder sb = new StringBuilder();

            List<string> list1 = SplitByStr(origin1, NL, true);
            List<string> list2 = SplitByStr(origin2, NL, true);

            int l1 = 0, l2 = 0;
            while (l1 < list1.Count || l2 < list2.Count)
            {
                int k;
                for (k = 0; k < every; k++)
                {
                    if (l1 >= list1.Count) break;
                    sb.Append(list1[l1]);
                    l1++;
                    if (l1 < list1.Count || l2 < list2.Count)
                        sb.Append(NL);
                }
                for (k = 0; k < insert; k++)
                {
                    if (l2 >= list2.Count) break;
                    sb.Append(list2[l2]);
                    l2++;
                    if (l1 < list1.Count || l2 < list2.Count)
                        sb.Append(NL);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 在每行的指定位置添加内容
        /// </summary>
        /// <param name="origin">原文本</param>
        /// <param name="content">添加内容</param>
        /// <param name="position">位置</param>
        /// <param name="ignoreBlank">忽略空行</param>
        /// <returns></returns>
        public string AddTextAt(string origin, string content, string position, bool ignoreBlank = false)
        {
            if (content.Length == 0) return origin;
            if (!CheckPositionStyle(position)) return origin;

            List<string> list = SplitByStr(origin, NL, true);
            List<string> newlist = new List<string>();
            try
            {
                foreach (string str in list)
                {
                    // 如果忽略空格，则自动跳过
                    if (ignoreBlank)
                    {
                        if (str.Length == 0)
                        {
                            newlist.Add(EMPTY);
                            continue;
                        }
                    }
                    int i = 0;
                    if (position[0] != '-') i = Convert.ToInt32(position);
                    else i = str.Length - Convert.ToInt32(position.Substring(1, position.Length - 1));
                    if (i > str.Length) i = str.Length;
                    if (i < 0) i = 0;
                    newlist.Add(str.Insert(i, content));
                }
            }
            catch (Exception)
            {
                return origin;
            }

            return CombineStringList(newlist, NL);
        }

        /// <summary>
        /// 在每行的指定位置添加内容，但是内容是多行的，每行插入的内容都不同
        /// </summary>
        /// <param name="origin">原文本</param>
        /// <param name="content">添加内容</param>
        /// <param name="position">位置</param>
        /// <param name="ignoreBlank">忽略空行</param>
        /// <returns></returns>
        public string SpecialAddTextAt(string origin, string content, string position, bool ignoreBlank = false)
        {
            if (content.Length == 0) return origin;
            if (!CheckPositionStyle(position)) return origin;

            List<string> list = SplitByStr(origin, NL, true);
            List<string> list2 = SplitByStr(content, NL, true);
            List<string> newlist = new List<string>();
            try
            {
                for (int ii = 0; ii < list.Count; ii++)
                {
                    if (ignoreBlank)
                    {
                        if (list[ii].Length == 0)
                        {
                            newlist.Add(EMPTY);
                            continue;
                        }
                    }
                    int i = 0;
                    if (position[0] != '-') i = Convert.ToInt32(position);
                    else i = list[ii].Length - Convert.ToInt32(position.Substring(1, position.Length - 1));
                    if (i > list[ii].Length) i = list[ii].Length;
                    if (i < 0) i = 0;

                    string insertContent = string.Empty;
                    if (ii <= list2.Count - 1)
                        insertContent = list2[ii];
                    newlist.Add(list[ii].Insert(i, insertContent));
                }
            }
            catch (Exception)
            {
                return origin;
            }

            return CombineStringList(newlist, NL);
        }

        /// <summary>
        /// 逐字左右交换
        /// </summary>
        /// <param name="origin">原文本</param>
        /// <param name="style">使用功能：1、全文逐字；2、全文逐行；3、逐行逐字</param>
        /// <returns></returns>
        public string ExchangePerLetter(string origin, int style)
        {
            string temp = string.Empty;
            if (style == 1)
            {
                int length = origin.Length;
                Regex r = new Regex("\r\n");
                origin = r.Replace(origin, "\n\r");
                for (int i = length - 1; i >= 0; i--)
                {
                    temp += origin[i];
                }
            }
            else if (style == 2)
            {
                List<string> list = SplitByStr(origin, NL, true);
                list.Reverse();
                temp = CombineStringList(list, NL);
            }
            else if (style == 3)
            {
                List<string> list = SplitByStr(origin, NL, true);
                //Parallel.ForEach<string>(list, (item) =>
                //{
                //    item = ExchangePerLetter(item, 1);
                //});
                for (int i = 0; i < list.Count; i++)
                {
                    list[i] = ExchangePerLetter(list[i], 1);
                }
                temp = CombineStringList(list, NL);
            }
            return temp;
        }

        /// <summary>
        /// 添加行首序号
        /// </summary>
        /// <param name="origin">原文本</param>
        /// <param name="left">左括号</param>
        /// <param name="right">右括号</param>
        /// <param name="isAligned">是否对其数字</param>
        /// <param name="ignoreBlank">忽略空行</param>
        /// <returns></returns>
        public string AddFrontIndex(string origin, string left, string right, bool isAligned, bool ignoreBlank = false)
        {
            string sentence = "";

            List<string> list = SplitByStr(origin, NL, true);
            int len = 0, t = list.Count;
            if (ignoreBlank)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Length == 0)
                        t--;
                }
            }
            while (t > 0)
            {
                len++;
                t /= 10;
            }
            int index = 1;
            for (int i = 0; i < list.Count; i++)
            {
                if (ignoreBlank)
                {
                    if (list[i].Length == 0)
                    {
                        sentence += list[i];
                        sentence += NL;
                        continue;
                    }
                }
                sentence += left;
                string number = index.ToString();
                if (isAligned)
                {
                    if (number.Length < len)
                    {
                        int up = len - number.Length;
                        for (int j = 0; j < up; j++)
                        {
                            number = "0" + number;
                        }
                    }
                }
                sentence += number;
                sentence += right;
                sentence += list[i];
                sentence += NL;
                index++;
            }
            sentence = sentence.Remove(sentence.Length - NL.Length);

            return sentence;
        }

        /// <summary>
        /// 删除空格
        /// </summary>
        /// <param name="origin">原文本</param>
        /// <param name="includeTab">是否包括制表符</param>
        /// <param name="frontOnly">是否仅删除行首空格</param>
        /// <returns></returns>
        public string RemoveSpace(string origin, bool includeTab, bool frontOnly)
        {
            string sentence = origin;

            if (!frontOnly)
            {
                if (!includeTab)
                {
                    // 方括号里面有两种空格，分别是全角和半角
                    sentence = UseRegExp(sentence, @"[ 　]", EMPTY);
                }
                else
                {
                    sentence = UseRegExp(sentence, @"[ 　\t]", EMPTY);
                }
            }
            else
            {
                if (!includeTab)
                {
                    // 方括号里面有两种空格，分别是全角和半角
                    sentence = UseRegExp(sentence, @"^[ 　]+", EMPTY, RegexOptions.Multiline);
                }
                else
                {
                    sentence = UseRegExp(sentence, @"^[ 　\t]+", EMPTY, RegexOptions.Multiline);
                }
            }

            return sentence;
        }

        /// <summary>
        /// 删除换行符功能
        /// </summary>
        /// <param name="origin">原文本</param>
        /// <param name="uselessOnly">是否只删除多余换行符</param>
        /// <returns></returns>
        public string RemoveReturn(string origin, bool uselessOnly)
        {
            if (!uselessOnly)
            {
                return Replace(origin, NL, "");
            }
            else
            {
                return Replace(origin, NL + NL, NL);
            }
        }

        /// <summary>
        /// 使用separator字符串分割原字符串
        /// </summary>
        /// <param name="target">原字符串</param>
        /// <param name="separator">分隔符（字符串，且按照整体而非其中的每一个单个字符）</param>
        /// <param name="keepEmpty">是否保留分割出的空的子字符串</param>
        /// <returns></returns>
        public List<string> SplitByStr(string target, string separator, bool keepEmpty)
        {
            Regex r = new Regex(separator);
            List<string> list = new List<string>(r.Split(target));
            if (!keepEmpty)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] == string.Empty)
                    {
                        list.RemoveAt(i);
                        i--;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 使用separator中的所有字符分割原字符串
        /// </summary>
        /// <param name="target">原字符串</param>
        /// <param name="separator">分隔符（分别使用其中的每一个字符，而非视作整体）</param>
        /// <param name="keepEmpty">分割后，是否保留空的子字符串</param>
        /// <returns></returns>
        public List<string> SplitByChars(string target, string separator = " ", bool keepEmpty = false)
        {
            Regex r = new Regex("[" + separator + "]");
            List<string> list = new List<string>(r.Split(target));
            RemoveItemFromList(list, EMPTY);
            return list;
        }

        // 内部函数
        #region InternalFunctions

        // 交换两个变量
        private void Exchange<T>(ref T obj1, ref T obj2)
        {
            T temp = obj1;
            obj1 = obj2;
            obj2 = temp;
        }

        // 从List<string>中删除特定元素（常用于删除空元素）
        private void RemoveItemFromList(List<string> list, string element)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == element)
                {
                    list.RemoveAt(i);
                    i--;
                }
            }
        }
        // 用于检查AddTextAt函数的position格式是否正确
        private bool CheckPositionStyle(string position)
        {
            string currectList = "-0123456789";
            foreach (char c in position)
            {
                if (!currectList.Contains(c.ToString()))
                    return false;
            }
            for (int i = 1; i < position.Length; i++)
            {
                if (position[i] == '-')
                    return false;
            }
            return true;
        }
        // 用于将被SplitByStr函数拆分后的string列表重新组合为一个完整的string
        private string CombineStringList(List<string> list, string separator = "")
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                sb.Append(separator);
                sb.Append(list[i]);
            }

            return sb.ToString();
        }
        // 匹配两个字符串是否相同
        private bool CompareString(string str1, string str2, bool isCaseSensitive = true)
        {
            if (str1.Length != str2.Length)
                return false;

            if (str1 != str2)
            {
                if (isCaseSensitive)
                    return false;
                else
                {
                    if (str1.ToUpper() != str2.ToUpper())
                        return false;
                }
            }

            return true;
        }
        // 将文本中的转义字符换成相应的内容
        private string ChangeEscChar(string origin)
        {
            foreach (var pair in EscCharList)
            {
                Regex r = new Regex(pair.Key);
                origin = r.Replace(origin, pair.Value);
            }
            return origin;
        }
        // 用于在使用正则表达式的时候，将能够被正则表达式识别到的特殊符号变为非转义字符
        private string CheckRegExpStyle(string origin)
        {
            string list = @"\.*+?!=()[]|-{}<>'#^$";
            foreach (char s in list)
            {
                Regex r = new Regex(@"\" + s);
                origin = r.Replace(origin, @"\" + s);
            }
            return origin;
        }

        #endregion
    }
}
