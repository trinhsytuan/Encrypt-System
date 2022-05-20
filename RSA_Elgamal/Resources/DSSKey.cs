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
    internal class DSSKey
    {
        Math math = new Math();
        public BigInteger randomp(int bit) //Hàm chọn P là snt lớn
        {
            BigInteger p;
            do
            {
                p = math.randombit(bit); //Chọn P là số nguyên tố lớn
            } while (math.Miller_Rabin(p) == false);
            return p;
        }
        public ArrayList Key(int bit)
        {
            int bitq;
            if (bit == 1024 || bit == 512) bitq = 160; //Nếu khóa là 1024 bit hay 512 bit thì bitq = 160 bit
            else bitq = 224; //Ngược lại q = 224 bit
            ArrayList res = new ArrayList();
            BigInteger p, q, z;
           
            do {
                p = randomp(bit);
                q = math.randombit(bitq); //Random Q
              
            } while ((math.Miller_Rabin(q) == false) || ((p-1) % q)!= 0); //Kiểm tra đảm bảo p-1 chia hết cho Q

            res.Add(p); //Thêm vào arraylist
            res.Add(q);
            z = (p - 1) / q; //Tính Z thông qua p và q
            res.Add(z);
            BigInteger h, g;
            do
            {
                h = math.RandomInRange(p - 1); //Random H
                g = math.Fast_Exponent(h, z, p); //Tính G
                //if (g > 1) break;
            } while (g < 1);
            res.Add(g);
            BigInteger a, pk;
            do
            {
                a = math.RandomInRange(q - 1); //Random khóa bí mật
            } while (a < 2 || a > q);
            res.Add(a);
            pk = math.Fast_Exponent(g, a, p); //TÍnh khóa công khai
            res.Add(pk);
            return res;

        }
    }
}
