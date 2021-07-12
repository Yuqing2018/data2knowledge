using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicKG.Service.Helpers
{
    /// <summary>
    /// Overlap Rate.
    /// </summary>
    public class OverlapRateHelper
    {
        private static List<string> GetOverlapping(int overlap, List<string> docIds)
        {
            var overlapping = new List<string>();
            var number = Convert.ToInt32(Math.Floor(docIds.Count * (overlap * 0.01)));
            if (number < 1)
                number = 1;
            int[] num = GetRandomNum(number, 0, docIds.Count - 1);
            for (int i = 0; i < num.Length; i++)
            {
                if (i > num.Length - 1)
                    break;
                overlapping.Add(docIds[num[i]]);
            }

            return overlapping;
        }

        private static int[] GetRandomNum(int num, int minValue, int maxValue)
        {
            if ((maxValue + 1 - minValue - num < 0))
                maxValue += num - (maxValue + 1 - minValue);
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            int[] arrNum = new int[num];
            StringBuilder sb = new StringBuilder(num * maxValue.ToString().Trim().Length);

            for (int i = 0; i <= num - 1; i++)
            {
                var tmp = ra.Next(minValue, maxValue);
                while (sb.ToString().Contains("#" + tmp.ToString().Trim() + "#"))
                    tmp = ra.Next(minValue, maxValue + 1);
                arrNum[i] = tmp;
                sb.Append("#" + tmp.ToString().Trim() + "#");
            }
            return arrNum;
        }

        /// <summary>
        /// Assign Document By Overlap.
        /// </summary>
        /// <param name="annotatorCount">Annotator Count.</param>
        /// <param name="overlap">Overlap.</param>
        /// <param name="docIds">Document Id List.</param>
        /// <returns></returns>
        public static List<string[]> AssignDocumentByOverlap(int annotatorCount, int overlap, List<string> docIds)
        {
            var result = new List<string[]>();

            if(annotatorCount==1)
                return new List<string[]>(){docIds.ToArray()};

            var overlappingDocIds = overlap == 0 ? null : GetOverlapping(overlap, docIds);

            var unOverlappingDocIds = overlappingDocIds == null ? docIds.ToArray() : docIds.Where(x => !overlappingDocIds.Contains(x)).ToArray();

            int countPerAnnotator = unOverlappingDocIds.Length / annotatorCount;

            int currentIndex = 0;
            int mod = unOverlappingDocIds.Length % annotatorCount;
            for (int i = 0; i < annotatorCount; i++)
            {
                List<string> docPerAnnotator = new List<string>();
                int currentCount = countPerAnnotator;
                if (mod != 0 && i < mod)
                {
                    currentCount++;
                }
                string[] unOverlappingPerAnnotator = new string[currentCount];
                Array.Copy(unOverlappingDocIds, currentIndex, unOverlappingPerAnnotator, 0, currentCount);
                currentIndex += currentCount;
                docPerAnnotator.AddRange(unOverlappingPerAnnotator);
                if (overlappingDocIds != null)
                    docPerAnnotator.AddRange(overlappingDocIds);
                result.Add(docPerAnnotator.ToArray());
            }
            return result;
        }
    }
}
