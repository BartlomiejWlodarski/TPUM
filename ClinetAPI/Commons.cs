using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientAPI
{
    public static class Commons
    {
        public static ArraySegment<byte> ToArraySegment(this string toConvert)
        {
            byte[] stringBuffer = Encoding.UTF8.GetBytes(toConvert);    
            return new ArraySegment<byte>(stringBuffer);
        }
    }
}
