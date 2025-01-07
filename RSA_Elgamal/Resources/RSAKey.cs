using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Elgamal.Resources
{

    class RSAKey
    {
        Math math = new Math();
        public ArrayList generateKey(int size)
        {
            BigInteger p, q, PK, SK, N;
            ArrayList list = new ArrayList();
            //Random 2 snt p và q có độ dài bằng nhau
            do //Random SNT p có độ dài bằng size / 2
            {
                p = math.randombit(size / 2);
            } while (math.Miller_Rabin(p) == false);
            do //Random SNT q có độ dài bằng size / 2
            {
                q = math.randombit(size / 2);
            } while (math.Miller_Rabin(q) == false);
            N = p * q; //Tính N = p * q
            //using (var rsa = new RSACryptoServiceProvider(size))
            //{
            //    var a = rsa.ExportParameters(true);
            //    p = new BigInteger(a.P.Reverse().Concat(new byte[1]).ToArray());
            //    q = new BigInteger(a.Q.Reverse().Concat(new byte[1]).ToArray());
            //    N = new BigInteger(a.Modulus.Reverse().Concat(new byte[1]).ToArray());
            //}
            BigInteger PhiN = (p - 1) * (q - 1); //Tính PhiN
            list.Add(p); //Thêm p và q vào ArrayList
            list.Add(q);
            list.Add(N);
            list.Add(PhiN);
            do
            {
                PK = math.RandomInRange(PhiN);
            } while (math.Euclid(PK, PhiN) == false); //Tìm PK nguyên tố cùng nhau với PhiN
            list.Add(PK);
            SK = math.Inverse_Modulo(PK, PhiN);
            list.Add(SK); //Tìm SK bằng cách tìm số nghịch đảo của PK và PhiN
            return list;
        }
    }
}
