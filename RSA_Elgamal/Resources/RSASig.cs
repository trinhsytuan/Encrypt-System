using System;
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
    class RSASig
    {
        Math math = new Math();
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
        public string mahoa(BigInteger d, BigInteger n, string path)
        {
            BigInteger sha1 = SHA1(path); //Lấy mã băm của file
            BigInteger k = math.Fast_Exponent(sha1, d, n); //Ký vào file
            return k.ToString(); //Return chữ ký
        }
        public bool giaima(BigInteger e, BigInteger n, string path, string chuky)
        {
            BigInteger sha1 = SHA1(path); //Lấy mã băm của file
            BigInteger ck = BigInteger.Parse(chuky); //Chuyển chữ ký từ chuỗi về dạng BigINteger
            BigInteger k = math.Fast_Exponent(ck, e, n); //Xác minh chữ ký
            if (sha1 == k) return true; //Nếu chữ ký giống với mã băm -> Xác minh chữ ký hợp lệ
            return false; //Nếu sai chũ ký không hợp lệ
        }
    }
}
