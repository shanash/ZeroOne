using System.Collections.Generic;

namespace FluffyDuck.Util
{
    public static class ListDefine
    {
        /// <summary>
        /// ¸®½ºÆ®ÀÇ ·£´ý ¼¯±â
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;

            System.Random rnd = new System.Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

        }

    }

}
