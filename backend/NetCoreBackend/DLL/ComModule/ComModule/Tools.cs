using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ComModule
{
    [Guid("738469B4-38E0-44F6-8563-1856F0C9087F")]
    [ComVisible(true)]
    public interface ITools
    {
        [DispId(1)]
        public List<string> CheckDuplicate(List<string> officerShips);
    }
    
    [Guid("04B86E1F-EF4E-4744-8914-79A9DE9C21A9")]
    [ComVisible(true)]
    [ProgId("ComModule.Tools")]
    public class Tools:ITools
    {
        public List<string> CheckDuplicate(List<string> officerShips)
        {
            List<string> singleShips=new List<string>();
            foreach(var i in officerShips)
            {
                bool judge = true;
                foreach (var j in singleShips)
                {
                    if (i == j)
                    {
                        judge = false;
                    }
                }

                if (judge)
                {
                    singleShips.Add(i);
                }
            }

            return singleShips;
        }
    }
}