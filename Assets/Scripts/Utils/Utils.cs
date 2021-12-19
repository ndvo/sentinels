using System;

namespace Utils
{
    public class Time
    {
        /// <summary>
        /// Returns the current time in unix timestamp as a double.
        /// 
        /// This method is based on an answer provided in StackOverflow by Bartłomiej Mucha
        /// <see>https://stackoverflow.com/a/21055459/935645</see>
        /// </summary>
        public static int UnixNow()
        {
            return (int)(
                    DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)
                    ).TotalSeconds;
        }
    }
}