using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelSoft.Messaging
{
    public static class MyAssert
    {
        public static void EachPair<T>(IEnumerable<T> expected, IEnumerable<T> values, Action<T, T> asserts)
        {
            if (asserts == null) throw new ArgumentNullException(nameof(asserts));
            if (expected == null) return;
            if (values == null) return;

            var expectedEnumerator = expected.GetEnumerator();
            var valuesEnumerator = values.GetEnumerator();

            while (true)
            {
                if (!expectedEnumerator.MoveNext()) break;
                if (!valuesEnumerator.MoveNext()) break;

                asserts(expectedEnumerator.Current, valuesEnumerator.Current);
            }
        }
    }
}
