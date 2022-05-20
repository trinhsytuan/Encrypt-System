using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Elgamal.Resources
{
    class RC4Crypto8Bit
    {
        int[] key = new int[256];
        int[] s = new int[256];
        public void Swap(ref int s1, ref int s2) //Hàm hoán đổi 2 phần tử s1 và s2
        {
            int tam = s1;
            s1 = s2;
            s2 = tam;
        }
        public void StringToKey(string k1, string k2) //Chuyển key từ khóa về mảng int
        {
            string[] temp1 = k1.Split(',');
            string[] temp2 = k2.Split(',');
            for (int i = 0; i < 256; i++) key[i] = int.Parse(temp1[i]);
            for (int i = 0; i < 256; i++) s[i] = int.Parse(temp2[i]);
        }
        public void Permutation() //Hoán vị khóa
        {
            int i, j = 0;
            for (i = 0; i < 256; i++)
            {
                j = (j + s[i] + key[i]) % 256;
                Swap(ref s[i], ref s[j]);
            }
        }
        public string Xor(string k1, string k2) //Hàm xor
        {
            StringBuilder tam = new StringBuilder();
            for (int i = 0; i < k1.Length; i++)
            {
                tam.Append((k1[i] - '0') ^ (k2[i] - '0'));
            }
            string res = tam.ToString();
            return res;
        }

        public string EncryptAndDecrypt(string nguon, string dich, string keyarray, string sarray)
        {
            try
            {
                StringToKey(keyarray, sarray);
                Permutation();

                byte[] t = File.ReadAllBytes(nguon);
                string bit = "";
                string res = "";
                int n = 0, j = 0;
                for (int i = 0; i < t.Length; i++)
                {
                    bit = Convert.ToString(t[i], 2).PadLeft(8, '0'); //Đọc từng byte trong file, convert về bit
                    n = (n + 1) % 256;
                    j = (j + s[n]) % 256; //Hoán vị khóa
                    Swap(ref s[n], ref s[j]);
                    int v = (s[n] + s[j]) % 256;
                    int k = s[v];
                    res = Xor(Convert.ToString(k, 2).PadLeft(8, '0'), bit); //Xor bản rõ với khóa, sau đó ghi vào file
                    t[i] = Convert.ToByte(res, 2); //Ghi vào file, mã hóa đến đâu ghi đến đó
                }
                File.WriteAllBytes(dich, t);
                return "Encrypt | Decrypt Sucessfully";
            }
            catch (Exception ex)
            {
                return "Error:\n" + ex.Message;
            }
        }
    }
}

