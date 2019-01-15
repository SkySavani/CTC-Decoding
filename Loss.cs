using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
     class Loss
    {
        public static List<List<float>> cache;
        private static float recLabelingProb(int t, int s, float[][] mat, List<int> labelingWithBlanks, int blank, List<List<float>> cache)
        {
            float res;
            //Console.WriteLine("t: "+t+"  "+"s: "+s);
            if (s < 0)
                return 0.0f;

            if (cache[t][s] != float.NegativeInfinity)
                return cache[t][s];

            if (t == 0)
            {
                if (s == 0)
                {
                    res = mat[0][blank];
                }
                else if (s == 1)
                {
                    res = mat[0][labelingWithBlanks[1]];
                }
                else
                {
                    res = 0.0f;
                }

                cache[t][s] = res;
                return res;
            }
            

            res = (recLabelingProb(t - 1, s, mat, labelingWithBlanks, blank, cache) + recLabelingProb(t - 1, s - 1, mat, labelingWithBlanks, blank, cache)) * mat[t][labelingWithBlanks[s]];
            //Console.WriteLine("res: "+ res);

            if (labelingWithBlanks[s] == blank || (s >= 2 && labelingWithBlanks[s - 2] == labelingWithBlanks[s]))
            {
                cache[t][s] = res;
                return res;
            }

            // otherwise, in case of a non-blank and non-repeated label, we additionally add s-2 at t-1
            res += recLabelingProb(t - 1, s - 2, mat, labelingWithBlanks, blank, cache) * mat[t][labelingWithBlanks[s]];

            cache[t][s] = res;

            return res;
        }

        private static List<List<float>> emptyCache(int maxT, List<int> labelingWithBlanks)
        {
            List<List<float>> res = new List<List<float>>();

            for (int i = 0; i < maxT; i++)
            {
                List<float> l = new List<float>();
                for (int j = 0; j < labelingWithBlanks.Count; j++)
                {
                    l.Add(float.NegativeInfinity);
                }
                res.Add(l);
            }
            return res;
        }

        public static float ctcLabelingProb(float[][] mat, string gt, string classes)
        {
            int maxT = mat.GetLength(0);
            int maxC = mat[0].Length;
            int blank = classes.Length;
            List<int> labelingWithBlanks = Common.extendByBlanks(Common.wordToLabelSeq(gt, classes), blank);
            cache = emptyCache(maxT, labelingWithBlanks);
            return recLabelingProb(maxT - 1, labelingWithBlanks.Count - 1, mat, labelingWithBlanks, blank, cache) + recLabelingProb(maxT - 1, labelingWithBlanks.Count - 2, mat, labelingWithBlanks, blank, cache);
        }

        public static float ctcLoss(float[][] mat, string gt, string classes)
        {
            return (float)-Math.Log(ctcLabelingProb(mat, gt, classes));
        }

        public static void TestLoss()
        {
            string classes = "ab";//total classes
            float[][] mat = new float[2][] { new float[] { 0.4f, 0f, 0.6f }, new float[] { 0.4f, 0f, 0.6f } };//RNN output
            float expected = 0.64f;
            Console.WriteLine("Test loss calculation");
            float actual = ctcLabelingProb(mat, "a", classes);
            Console.WriteLine("Expected       :" + expected.ToString());
            Console.WriteLine("Actual    : " + actual.ToString());
            if (expected == actual)
            {
                Console.WriteLine("OK");
            }
            else
            {
                Console.WriteLine("ERROR");
            }

            Console.ReadKey();

        }

    }
}