using System;
using System.Collections.Generic;
using System.IO;
public class LTP
{
    public struct struWordDP
    {
        public int id;

        public string cont;

        public string pos;

        public int parent;

        public string relate;

        public struWordDP(string element)
        {
            var x = RegularTool.GetMultiValueBetweenMark(element, "\"", "\"");
            id = int.Parse(x[0]);
            cont = x[1];
            pos = x[2];
            parent = int.Parse(x[3]);
            relate = x[4];
        }
    }

    public struct struWordNER
    {
        public int id;

        public string cont;

        public string pos;

        public string ne;

        public struWordNER(string element)
        {
            var x = RegularTool.GetMultiValueBetweenMark(element, "\"", "\"");
            id = int.Parse(x[0]);
            cont = x[1];
            pos = x[2];
            ne = x[3];
        }
    }

    public static List<String> Anlayze(string xmlfilename)
    {
        //由于结果是多个XML构成的
        //1.掉所有的<?xml version="1.0" encoding="utf-8" ?>
        //2.加入<sentence></sentence> root节点    
        var NerList = new List<String>();

        if (!File.Exists(xmlfilename)) return NerList;

        var sr = new StreamReader(xmlfilename);
        List<struWordNER> wl = null;
        var pl = new List<List<struWordNER>>();
        var ner = "";
        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine().Trim();
            if (line.StartsWith("<sent"))
            {
                if (wl != null) pl.Add(wl);
                //一个新的句子
                wl = new List<struWordNER>();
            }
            if (line.StartsWith("<word"))
            {
                var word = new struWordNER(line);
                wl.Add(word);
                switch (word.ne)
                {
                    case "B-Ni":
                        ner = word.cont;
                        break;
                    case "I-Ni":
                        ner += word.cont;
                        break;
                    case "E-Ni":
                        ner += word.cont;
                        if (!NerList.Contains(ner)) NerList.Add(ner);
                        break;
                }
            }
        }
        if (wl != null) pl.Add(wl);
        sr.Close();
        return NerList;
    }
}