
namespace FluffyDuck.Util
{
    public static class DateTimeDefine
    {
        /// <summary>
        /// Datetime convert to Timestamp
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToTimeStamp(this System.DateTime value)
        {
            return (long)value.Subtract(new System.DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        /// <summary>
        /// Timestamp conver to DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static System.DateTime ToDatetime(this long value)
        {
            //return new System.DateTime(1970, 1, 1).AddMilliseconds(value).ToLocalTime();
            return new System.DateTime(1970, 1, 1).AddMilliseconds(value);
        }
    }

}
