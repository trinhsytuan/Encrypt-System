using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Elgamal.Resources
{
    internal class DSSSig
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
        public ArrayList sig(string path, BigInteger q, BigInteger p, BigInteger g, BigInteger a )
        {
            ArrayList res = new ArrayList();
            BigInteger k;
            Math math = new Math();
            BigInteger m = SHA1(path);
            BigInteger r, s;
            do
            {
                k = math.RandomInRange(q - 1); //Chọn K
                r = math.Fast_Exponent(g, k, p) % q; //Tính r
                BigInteger temp = math.Inverse_Modulo(k, q); //Tính số nghịch đảo của k mũ -1 và q
                s = ((m + a * r) * temp) % q; //Tính S
            } while (k < 2 || k > q - 1 || r < 0 || s < 0); //Nếu cặp r,s bé hơn 0, chọn k khác và ký lại
            res.Add(k);
            res.Add(r);
            res.Add(s);
            return res;
        }
        public bool verifysig(string path, BigInteger p, BigInteger q, BigInteger r, BigInteger s, BigInteger g,BigInteger a)
        {
            Math math = new Math();
            BigInteger w = math.Inverse_Modulo(s, q); //W là s -1 mod q
            BigInteger m = SHA1(path); //Lấy mã băm
            BigInteger u1 = (m * w) % q; //Tính U1
            BigInteger u2 = (r * w) % q; //TÍnh U2
            BigInteger v1 = math.Fast_Exponent(g, u1, p); //Tính V1
            BigInteger v2 = math.Fast_Exponent(a, u2, p); //Tính V2
            BigInteger v = (v1 * v2) % p; //Tính V
            v = v % q; //mod v cho q
            if (v == r) return true; //Nếu v == r chữ ký đúng
            else return false; //Ngược lại chữ ký sai
        }
    }
}
