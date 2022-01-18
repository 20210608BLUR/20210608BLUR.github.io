using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.Utility
{
    public class RandomNum
    {
        public int[] GetRandomNumberArray(int getNums, List<int> idList)
        {
            int[] result = new int[getNums];
            for (int i = 0; i < getNums; i++)
            {
                Random number = new Random();  //產生亂數初始值
                int temp = number.Next(0, idList.Count());
                result[i] = idList[temp];
                idList.Remove(result[i]);
            }
            return result;
        }
    }
}
