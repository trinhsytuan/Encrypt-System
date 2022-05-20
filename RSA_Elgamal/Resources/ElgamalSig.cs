using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RSA_Elgamal.Resources;
namespace RSA_Elgamal.Resources
{
    class ElgamalSig
    {
        public BigInteger SHA1(string dd)
        {
            //Lấy mã SHA1 của file
            string path = dd; //Lấy đường dẫn file
            BigInteger hash_value;
            StringBuilder formatted;
            string hash_string = "";
            using (FileStream fs = new FileStream(path, FileMode.Open)) //Đọc file nhị phân
            using (BufferedStream bs = new BufferedStream(fs)) //Đưa vào bộ nhớ đệm
            {
                using (SHA1Managed sha1 = new SHA1Managed()) //Tính mã sha1 của file
                {
                    byte[] hash = sha1.ComputeHash(fs);
                    formatted = new StringBuilder(2 * hash.Length); //Khai báo string builder độ dài bằng 2 lần độ dài mảng byte
                    foreach (byte b in hash)
                    {
                        formatted.Append(b.ToString("X2")); //Convert từ byte về hệ hexa

                    }
                    hash_string = "0" + formatted.ToString(); //Cho máy tính không nhận là dấu âm(Vì mã SHA có bit đầu là số 1)
                }
            }
            hash_value = BigInteger.Parse(hash_string, System.Globalization.NumberStyles.AllowHexSpecifier); //Convert về dạng hệ thập phân
            return hash_value;
        }
        public ArrayList sig(string path, BigInteger p, BigInteger alpha, BigInteger sk)
        {
            Math math = new Math();
            ArrayList l = new ArrayList();
            BigInteger sha = SHA1(path);
            
            BigInteger k;
            do
            {
                k = math.RandomInRange(p - 1); //Random số k sao cho k nguyên tố cùng nhau với p - 1;
            } while (k < 0 || k > p - 1 || math.Euclid(k, p - 1) == false);
            
            BigInteger S1 = math.Fast_Exponent(alpha, k, p); //Tính bản mã S1
            BigInteger kMod = (math.Inverse_Modulo(k, p - 1) % (p - 1)); 
            BigInteger S2 = (kMod * (sha - sk * S1) % (p - 1));// TÍnh S2
            if (S2 < 0)
            {
                S2 += p - 1; //Nếu S2 nhỏ hơn 0, bù với p-1
            }
            l.Add(k);
            l.Add(S1);
            l.Add(S2);
            return l;
        }
        public bool ktra(string path, BigInteger p, BigInteger alpha, BigInteger pk, BigInteger s1, BigInteger s2)
        {
            Math math = new Math();
            BigInteger sha = SHA1(path); //Lấy mã băm của file
            BigInteger v1 = math.Fast_Exponent(alpha, sha, p); //Tính V1
            BigInteger n = math.Fast_Exponent(pk, s1, p);  //Tính N
            BigInteger m = math.Fast_Exponent(s1, s2, p); //Tính M
            BigInteger v2 = (n * m) % p; //Tính V2
            if (v1 == v2) return true; //Nếu V1 == V2 -> Chữ ký hợp lệ
            return false;
        }
    }
}
