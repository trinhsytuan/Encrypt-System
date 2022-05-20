using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using RSA_Elgamal.Resources;
namespace RSA_Elgamal.Resources
{
    class A51Key
    {
        Math math = new Math();
        public ArrayList Key23Bit()
        {
            ArrayList result = new ArrayList();
            BigInteger k = math.randombit(24); 
            string s = math.BigIntegertoBinary(k).PadLeft(23,'0');
            result.Add(s.Substring(0, 6));
            result.Add(s.Substring(6, 8));
            result.Add(s.Substring(14, 9));
            return result;
        }
        public ArrayList Key64Bit()
        {
            ArrayList result = new ArrayList();
            BigInteger k = math.randombit(64); //Random khóa 64 bit
            string s = math.BigIntegertoBinary(k).PadLeft(64, '0'); //Chuyển từ hệ 10 sang hệ 2
            result.Add(s.Substring(0, 19)); //19 Phần tử đầu thuộc thanh ghi X
            result.Add(s.Substring(19, 22)); //22 phần tử tiếp theo thuộc thanh ghi Y
            result.Add(s.Substring(41, 23)); //23 phân tử cuối cùn thuộc thanh ghi S
            return result; //Trả kết quả
        }
    }
}
