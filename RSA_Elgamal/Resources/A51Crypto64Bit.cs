using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Elgamal.Resources
{
    class A51Crypto64Bit
    {
        Math math = new Math();
        string x, y, z;
        public A51Crypto64Bit(string x, string y, string z)
        {
            this.x = x; //Lấy các giá trị khóa từ form giao diện
            this.y = y;
            this.z = z;
        }
        public int maj(int b1, int b2, int b3)
        {
            int[] a = { 0, 0 }; //Do bit chỉ có 0 va 1, ta tạo 2 mảng
            a[b1]++;
            a[b2]++; //Tăng lần lượt các giá trị
            a[b3]++;
            return a[0] < a[1] ? 1 : 0; //Trả về kết quả hàm maj
        }
        public void XRotate()
        { //Xoay X, Xor lần lượt các thanh ghi
            int t = (x[13] - '0') ^ (x[16] - '0') ^ (x[17] - '0') ^ (x[18] - '0');
            x = t.ToString() + x;
            int len = x.Length - 1;
            x = x.Remove(len, 1);
        }
        public void YRotate()
        { //Xoay Y
            int t = (y[20] - '0') ^ (y[21] - '0');
            y = t.ToString() + y;
            int len = y.Length - 1;
            y = y.Remove(len, 1);
        }
        public void ZRotate()
        { //Xoay Z
            int t = (z[7] - '0') ^ (z[20] - '0') ^ (z[21] - '0') ^ (z[22] - '0');
            z = t.ToString() + z;
            int len = z.Length - 1;
            z = z.Remove(len, 1);
        }
        public int tong(int b1, int b2, int b3)
        { //hàm xor 3 bit với nhau
            int k = b1 ^ b2 ^ b3;
            return k;
        }
        public int CryptoBit(int b)
        { //Xoay lần lượt thanh ghi, sau đó xor bit bản rõ với x18,y21,z22
            int k = maj(x[8] - '0', y[10] - '0', z[10] - '0');
            if (x[8] - '0' == k) XRotate();
            if (y[10] - '0' == k) YRotate();
            if (z[10] - '0' == k) ZRotate();
            int v = tong(x[18] - '0', y[21] - '0', z[22] - '0');
            return b ^ v;
        }
        public string MH(string v)
        {
            string result = "";
            for (int i = 0; i < v.Length; i++)
            {
                result += CryptoBit(v[i] - '0');
            }
            return result.PadLeft(8, '0');
        }
        public string A51Cryto(string pathnguon, string pathdich)
        {
            try
            {
                byte[] t = File.ReadAllBytes(pathnguon); //Đọc toàn bộ file vào mảng byte
                for (int i = 0; i < t.Length; i++)
                {
                    string m = Convert.ToString(t[i], 2).PadLeft(8, '0'); //Convert về hệ nhị phân
                    t[i] = Convert.ToByte(MH(m), 2); //Mã hóa xong(Gọi hàm MH(m), convert về  byte)
                }
                File.WriteAllBytes(pathdich, t); //Ghi file
                return "Encrypt | Decrypt Successfully";
            }
            catch (Exception ex)
            {
                return "Error:\n" + ex.Message;
            }
        }
    }
}