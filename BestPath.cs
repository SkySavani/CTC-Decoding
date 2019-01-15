using System;
using System.Linq;

namespace ConsoleApp1
{
    class BestPath
    {
        public static string ctcBestPath(float[][] mat, string classes)
        {
            int maxT = mat.GetLength(0);
            int maxC = mat[0].Length;
            string label = "";
            int blankIdx = classes.Length;
            int lastMaxIdx = maxC;

            for (int i = 0; i < maxT; i++)
            {
                float m = mat[i].Max();
                // Positioning max
                int maxIdx = Array.IndexOf(mat[i], m);
                if (maxIdx != lastMaxIdx && maxIdx != blankIdx)
                {
                    label += classes[maxIdx];
                }

                lastMaxIdx = maxIdx;
            }

            return label;
        }
    }
}
