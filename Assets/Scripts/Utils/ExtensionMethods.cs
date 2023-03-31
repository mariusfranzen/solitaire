using System;
using System.Collections.Generic;

namespace Assets.Scripts.Utils
{
    public static class ExtensionMethods
    {
        public static int IndexOfMin(this IList<int> self)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            if (self.Count == 0)
            {
                throw new ArgumentException("List is empty.", nameof(self));
            }

            int min = self[0];
            int minIndex = 0;

            for (int i = 1; i < self.Count; ++i)
            {
                if (self[i] >= min) continue;
                min = self[i];
                minIndex = i;
            }

            return minIndex;
        }

        public static int IndexOfMin(this IList<double> self)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            if (self.Count == 0)
            {
                throw new ArgumentException("List is empty.", nameof(self));
            }

            double min = self[0];
            int minIndex = 0;

            for (int i = 1; i < self.Count; ++i)
            {
                if (self[i] >= min) continue;
                min = self[i];
                minIndex = i;
            }

            return minIndex;
        }

        public static int IndexOfMin(this IList<float> self)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            if (self.Count == 0)
            {
                throw new ArgumentException("List is empty.", nameof(self));
            }

            float min = self[0];
            int minIndex = 0;

            for (int i = 1; i < self.Count; ++i)
            {
                if (self[i] >= min) continue;
                min = self[i];
                minIndex = i;
            }

            return minIndex;
        }
    }
}
