using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RSA_Elgamal.Resources;
namespace RSA_Elgamal.Resources
{
    class RC4Crypto3Bit
    {
        int[] key = new int[8];
        int[] s = new int[8];
        public void Swap(ref int s1, ref int s2)
        {
            int tam = s1;
            s1 = s2;
            s2 = tam;
        }
        public void StringToKey(string k1, string k2)
        {
            string[] temp1 = k1.Split(',');
            string[] temp2 = k2.Split(',');
            for (int i = 0; i < 8; i++) key[i] = int.Parse(temp1[i]);
            for (int i = 0; i < 8; i++) s[i] = int.Parse(temp2[i]);
        }
        public void Permutation()
        {
            int i, j = 0;
            for (i = 0; i < 8; i++)
            {
                j = (j + s[i] + key[i]) % 8;
                Swap(ref s[i], ref s[j]);
            }
        }
        public string Xor(string k1, string k2)
        {
            string res = "";
            res += (k1[0] - '0') ^ (k2[0] - '0');
            res += (k1[1] - '0') ^ (k2[1] - '0');
            res += (k1[2] - '0') ^ (k2[2] - '0');
            return res;
        }

        public string EncryptAndDecrypt(string nguon, string dich, string keyarray, string sarray)
        {
            try
            {
                StringToKey(keyarray, sarray);
                Permutation();
                byte[] t = File.ReadAllBytes(nguon);
                StringBuilder bits = new StringBuilder();
                for (int i = 0; i < t.Length; i++)
                {
                    bits.Append(Convert.ToString(t[i], 2).PadLeft(8, '0'));
                }
                string bit = bits.ToString();
                StringBuilder tam = new StringBuilder();
                long n = 0, j = 0;
                for (long i = 0; i < bit.Length / 3; i++)
                {
                    n = (n + 1) % 8;
                    j = (j + s[n]) % 8;
                    Swap(ref s[n], ref s[j]);
                    int v = (s[n] + s[j]) % 8;
                    int k = s[v];
                    tam.Append(Xor(Convert.ToString(k, 2).PadLeft(3, '0'), bit.Substring(3 * (int)i, 3)));
                }
                if (bit.Length % 3 == 1)
                {
                    n = (n + 1) % 8;
                    j = (j + s[n]) % 8;
                    Swap(ref s[n], ref s[j]);
                    int v = (s[n] + s[j]) % 8;
                    int k = s[v];
                    string tll = Convert.ToString(k, 2);
                    tam.Append((tll[0] - '0') ^ (bit[bit.Length - 1] - '0'));
                }
                else
                {
                    n = (n + 1) % 8;
                    j = (j + s[n]) % 8;
                    Swap(ref s[n], ref s[j]);
                    int v = (s[n] + s[j]) % 8;
                    int k = s[v];
                    string tll = Convert.ToString(k, 2).PadLeft(3, '0');
                    tam.Append((tll[0] - '0') ^ (bit[bit.Length - 2] - '0'));
                    tam.Append((tll[1] - '0') ^ (bit[bit.Length - 1] - '0'));
                }
                string res = tam.ToString();
                for (int i = 0; i < res.Length % 8; i++) res = "0" + res;
                for (long i = 0; i < res.Length / 8; i++)
                {
                    t[i] = (Convert.ToByte(res.Substring(8 * (int)i, 8), 2));
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
