using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Elgamal.Resources
{
    class Math
    {
        public bool Euclid(BigInteger a, BigInteger b)
        {
            //Là hàm kiểm tra xem 2 số có nguyên tố cùng nhau
            BigInteger r;
            do
            {
                //Sử dụng thuật toán Euclid
                r = a % b;
                a = b;
                b = r;
            } while (r != 0);
            if (a == 1) return true; //Nếu a == 1 là nguyên tố cùng nhau
            else return false; //Nếu sai, không phải nguyên tố cùng nhau
        }
        public string BigIntegertoBinary(BigInteger v)
        {
            //Là hàm hỗ trợ chuyển từ số hệ thập phân thành nhị phân
            string m = "";
            while (v != 0)
            {
                m += (char)(v % 2) + 0; //Chia dư cho 2
                v >>= 1;  //Tương đương với v = v / 2
            }
            string h = ""; //Đảo ngược chuỗi
            for (int i = m.Length - 1; i >= 0; i--) h += m[i];
            return h;
        }
        public BigInteger BinaryToNumber(string value) //Là hàm chuyển từ hệ nhị phân sang thập phân
        {
            //res là biến kết quả
            BigInteger res = 0;

            foreach (char c in value)
            {

                res <<= 1; //Tương đương với res = res * 2;
                res += c == '1' ? 1 : 0; //Nếu c bằng 1 thi cộng 1, nếu sai cộng 0
            }

            return res;
        }
        public BigInteger randombit(int size) //Là hàm random các số nguyên lớn
        {
            //Khai báo thư viện Random
            Random random = new Random();
            BitArray bits; //Tạo 1 mảng bitarray
            byte[] byteArray = new byte[size / 8]; //Tiến hành random
            random.NextBytes(byteArray); //Random
            bits = new BitArray(byteArray); //Copy vào bit array
            //Vì khi random byte sẽ có 2 trường hợp xảy ra
            //TH1: Số nhận được là số âm
            //TH2: Số nhận được là số chẵn
            //Ta khắc phục bằng cách set bit, số chẵn thường có bit ngoài cùng = 0
            bits.Set(0, true);
            bits.Set(size - 2, true);
            bits.Set(size - 1, false); //Tiến hành set bit
            bits.CopyTo(byteArray, 0); //Copy vào mảng byte
            BigInteger k = new BigInteger(byteArray); //Covert về hệ 10
            return k;
        }
        public BigInteger RandomInRange(BigInteger a)
        {
            // Là hàm random trong đoạn
            BigInteger b;
            byte[] k = a.ToByteArray();
            Random r = new Random();
            do
            {
                r.NextBytes(k);
                k[k.Length - 1] &= (byte)0x7F; //Ép dấu thành dương
                b = new BigInteger(k);
            } while (b >= a);
            return b;
        }
        public BigInteger Fast_Exponent(BigInteger a, BigInteger fast, BigInteger mod) //Hàm tính giai thừa nhanh
        {
            string binary = BigIntegertoBinary(fast); //Chuyển số từ thập phân về nhị phân
            char[] b = binary.ToCharArray(); //Chuyển về mảng char
            BigInteger res = 1;
            for (int i = 0; i < b.Length; i++)
            {
                res = (res * res) % mod;
                if (b[i] == '1') res = (res * a) % mod;
            }
            return res;
        }
        public BigInteger Inverse_Modulo(BigInteger a, BigInteger m) //Thuật toán tìm số nghịch đảo Euclid Extends
        {
            BigInteger temp = m, y = 0, r, q, y0 = 0, y1 = 1;
            while (a > 0)
            {
                r = m % a;
                if (r == 0) break;
                q = m / a;
                y = (y0 - y1 * q) % temp;
                y0 = y1;
                y1 = y;
                m = a;
                a = r;
            }
            if (a > 1) return -1;
            if (y < 0) y += temp;
            return y;
        }
        public bool Miller_Rabin(BigInteger num)
        {
            //Kiểm tra tính nguyên tố Miller rabin
            if (num == 2 || num == 3) return true; //2 và 3 là snt
            if (num < 2 || num % 2 == 0) return false; //Các số bé hơn 0 và số chẵn không phải là snt
            BigInteger tempnum = num - 1;
            int s = 0;
            while (tempnum % 2 == 0)
            {
                tempnum /= 2; //Phân tích thành dạng 2^s
                s++;
            }
            BigInteger d = tempnum;
            int i, j;
            BigInteger a;
            for (i = 0; i < 15; i++) //Lặp miller rabin 15 lần
            {
                do
                {
                    a = RandomInRange(num - 2); //Chọn 1 số ngẫu nhiên không vượt quá num - 2
                } while (a < 2 || a > num - 2);
                BigInteger x = Fast_Exponent(a, d, num); //Tính a^d mod nuum
                if ((x == 1) || (x == (num - 1))) continue; //Nếu bằng 1 hoặc num-1 thì nghi ngờ là snt, chuyển qua bước lặp khác
                for (j = 0; j < s; j++)
                {
                    x = Fast_Exponent(x, 2, num);
                    if (x == (num - 1)) break; //nghi ngờ là snt
                    else return false;
                }
                if (j == s) return false; //Nếu j == s -> Không phải là snt
            }
            return true;
        }
    }
}
