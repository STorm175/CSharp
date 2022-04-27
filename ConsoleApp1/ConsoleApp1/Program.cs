using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var BASE_COUNT = 3000;
            var userIds = new List<string>();

            for (int i = 0; i < BASE_COUNT + 1; i++)
            {
                userIds.Add(i.ToString());
            }

            var createList = userIds.Count > BASE_COUNT ? userIds.GetRange(0, BASE_COUNT) : userIds;

            for (int i = BASE_COUNT; i < userIds.Count; i += BASE_COUNT)
            {
                var range = i + BASE_COUNT < userIds.Count ? BASE_COUNT : userIds.Count - i;
                var lstUsers = userIds.GetRange(i, range);
            }
        }
    }
}
