using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Elgamal.Resources
{
    class A51Crypto23Bit
    {
        Math math = new Math();
        //Sử dụng string để tránh phải dịch mảng nhiều lần nếu dùng mảng -> Tăng tốc độ chương trình
        string x, y, z;
        public A51Crypto23Bit(string x, string y, string z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        //Tính giá trị maj
        public int maj(int b1, int b2, int b3)
        {
            int[] a = { 0, 0 };
            a[b1]++;
            a[b2]++;
            a[b3]++;
            return a[0] < a[1] ? 1 : 0;
        }
        public void XRotate()
        {
            int t = (x[2] - '0') ^ (x[4] - '0') ^ (x[5] - '0');
            x = t.ToString() + x;
            int len = x.Length - 1;
            x = x.Remove(len, 1);
        }
        public void YRotate()
        {
            int t = (y[6] - '0') ^ (y[7] - '0');
            y = t.ToString() + y;
            int len = y.Length - 1;
            y = y.Remove(len, 1);
        }
        public void ZRotate()
        {
            int t = (z[2] - '0') ^ (z[7] - '0') ^ (z[8] - '0');
            z = t.ToString() + z;
            int len = z.Length - 1;
            z = z.Remove(len, 1);
        }
        //Xor 3 bit với nhau
        public int tong(int b1, int b2, int b3)
        {
            int k = b1 ^ b2 ^ b3;
            return k;
        }
        //Mã hóa trên từng bit
        public int CryptoBit(int b)
        {
            int k = maj(x[1] - '0', y[3] - '0', z[3] - '0');
            if (x[1] - '0' == k) XRotate();
            if (y[3] - '0' == k) YRotate();
            if (z[3] - '0' == k) ZRotate();
            int v = tong(x[5] - '0', y[7] - '0', z[8] - '0');
            return b ^ v;
        }
        public string MH(string v)
        {
            string result = "";
            for (int i = 0; i < v.Length; i++)
            {
                result += CryptoBit(v[i] - '0');
            }
            return result.PadLeft(8,'0');
        }
        public string A51Cryto(string pathnguon, string pathdich)
        {
            try
            {
                byte[] t = File.ReadAllBytes(pathnguon);
                for (int i = 0; i < t.Length; i++)
                {
                    //Đọc hết các byte sau đó convert về bit
                    string m = Convert.ToString(t[i], 2).PadLeft(8,'0');
                    //Mã hóa xong lại convert về byte
                    t[i] = Convert.ToByte(MH(m), 2);
                }
                //Ghi lại vào file
                File.WriteAllBytes(pathdich, t);
                return "Encrypt | Decrypt Successfully";
            }
            catch (Exception ex)
            {
                return "Error:\n" + ex.Message;
            }
        }
    }
}