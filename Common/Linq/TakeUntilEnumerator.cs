using System;
using System.Collections.Generic;

namespace Common.Linq
{
    /// <summary>
    /// Copied from here
    /// https://code.google.com/p/morelinq/source/browse/MoreLinq/TakeUntil.cs
    /// </summary>
    public static class TakeUntilEnumerator
    {
        private static IEnumerable<TSource> TakeUntilImpl<TSource>(this IEnumerable<TSource> pSource,
                                                                   Func<TSource, bool> pPredicate)
        {
            foreach (TSource item in pSource)
            {
                yield return item;
                if (pPredicate(item))
                {
                    yield break;
                }
            }
        }

        public static IEnumerable<TSource> TakeUntil<TSource>(this IEnumerable<TSource> pSource,
                                                              Func<TSource, bool> pPredicate)
        {
            if (pSource == null)
            {
                throw new ArgumentNullException("pSource");
            }
            if (pPredicate == null)
            {
                throw new ArgumentNullException("pPredicate");
            }
            return TakeUntilImpl(pSource, pPredicate);
        }
    }
}