using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RSA_Elgamal.Resources;
namespace RSA_Elgamal.Resources
{
    class ElgamalKey
    {
        Math math = new Math();
        public bool checkSinh(BigInteger k,BigInteger p, BigInteger q)
        { //Hàm kiểm tra phần tử sinh
            BigInteger sm = (p - 1) / q;
            BigInteger kq = math.Fast_Exponent(k, sm, p);
            if (kq != 1) return true;
            return false;
        }
        public ArrayList Key(int size)
        {
            ArrayList m = new ArrayList();
            BigInteger p, q,e,alpha;
            using (var rsa = new RSACryptoServiceProvider(size))
            {
                var a = rsa.ExportParameters(true);
                p = new BigInteger(a.P.Reverse().Concat(new byte[1]).ToArray());
                

            }
            m.Add(p);
            //q thỏa mãn là ước của p - 1
            do
            {
                q = math.RandomInRange(p - 2); //Tìm q sao cho q nguyên tố cùng nhau với p
            } while (q < 2 || q > (p - 1) || math.Euclid(p, q) == false);
            do
            {
                e = math.RandomInRange(p - 1);  //Tìm e sao cho e là phần tử sinh của q
            } while (e < 2 || e > (p - 1) || checkSinh(e, p, q) == false);
            alpha = math.Fast_Exponent(e, (p - 1) / q, p); //TÍnh alpha
            m.Add(alpha);
            BigInteger pk, sk;
            do
            {
                sk = math.RandomInRange(p - 1); //Random SK
            } while (sk < 1 || sk > (p - 1) || sk == e || sk == q);
            pk = math.Fast_Exponent(alpha, sk, p); //Tính PK
            m.Add(pk);
            m.Add(sk);
            m.Add(e);
            return m;
        }
    }
}
