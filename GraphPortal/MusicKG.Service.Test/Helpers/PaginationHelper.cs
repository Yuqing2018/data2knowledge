using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Test.Helpers
{
    public static class PaginationHelper
    {
        public static int GetExpectedCount(int skipValue, int? limitValue, int totalCountExpected)
        {
            if (skipValue >= totalCountExpected)
            {
                return 0;
            }
            if (!limitValue.HasValue)
            {
                return totalCountExpected - skipValue > 0 ? totalCountExpected - skipValue : 0;
            }
            if (limitValue.Value > totalCountExpected - skipValue)
            {
                return totalCountExpected - skipValue;
            }
            else
            {
                return limitValue.Value;
            }
        }

        public static int GetSkipValue(PaginationSkip skip, int totalCount)
        {
            switch (skip)
            {
                case PaginationSkip.Zero:
                    return 0;
                case PaginationSkip.LessThanTotal:
                    return new Random().Next(1, totalCount - 2);
                case PaginationSkip.MoreThanTotal:
                    return totalCount + 1;
                default:
                    return 0;
            }
        }

        public static int? GetLimitValue(PaginationLimit limit, int totalCount, int skipCount)
        {
            switch (limit)
            {
                case PaginationLimit.Infinity:
                    return null;
                case PaginationLimit.EqualToRemain:
                    return totalCount - skipCount;
                case PaginationLimit.LessThanRemain:
                    return new Random().Next(1, Math.Max(totalCount - skipCount, 2));
                case PaginationLimit.MoreThanRemain:
                    return totalCount - skipCount + 1;
                default:
                    return null;
            }
        }
    }

    public enum PaginationSkip
    {
        Zero,

        LessThanTotal,

        MoreThanTotal
    }

    public enum PaginationLimit
    {
        LessThanRemain,

        MoreThanRemain,

        EqualToRemain,

        Infinity
    }
}
