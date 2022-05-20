using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using RSA_Elgamal.Resources;
using System.IO;
namespace RSA_Elgamal.Resources
{
    class ElgamalCypto
    {
        Math math = new Math();
        public ArrayList mahoa(string nguon, string pathdich, BigInteger alpha, BigInteger pk, BigInteger p, int dodai)
        {
            ArrayList list = new ArrayList();
            try
            {
                int bitcat = 0;
                if (dodai == 1024) bitcat = 1000; //Nếu khóa là 1024 bit, cắt bản rõ 1000 bit
                else bitcat = 2000; //Nếu khóa là 2048 bit, cắt bản rõ 2000 bit
                BigInteger k;
                do
                {
                    k = math.RandomInRange(p - 1); //Random K
                } while (k < 0 || k > p - 1 || math.Euclid(k, p - 1) == false);
                list.Add(k);
                BigInteger C1 = math.Fast_Exponent(alpha, k, p); //Tính C1
                StringBuilder bitdich = new StringBuilder();
                StringBuilder bitnguon = new StringBuilder();
                FileStream fnguon = new FileStream(nguon, FileMode.Open); //Đọc file nhị phân
                long i;
                for(i = 0; i < fnguon.Length; i++)
                {
                    bitnguon.Append(Convert.ToString(fnguon.ReadByte(),2).PadLeft(8, '0')); //Đọc File và chuyển hết về bit
                }
                fnguon.Close();
                bitdich.Append(math.BigIntegertoBinary(C1).PadLeft(dodai, '0')); //Cho C1 vào bit đầu tiên của file
                string bnguon = bitnguon.ToString();
                BigInteger C2, temp = math.Fast_Exponent(pk, k, p); 
                BigInteger number;
                i = 0;
                for(i = 0; i < bnguon.Length / bitcat;i++)
                {
                    number = math.BinaryToNumber(bnguon.Substring((int)i * bitcat, bitcat)); //Cắt bản rõ
                    C2 = (number * temp) % p; //Tính C2
                    bitdich.Append(math.BigIntegertoBinary(C2).PadLeft(dodai, '0')); //Thêm vào cuối file
                } //TH file có 4230 bit, cắt 1000 bit, còn 230 bit(phần lẻ, sẽ đc xử lý ở đây)
                number = math.BinaryToNumber(bnguon.Substring((int)i * bitcat, bnguon.Length - ((int)i * bitcat))); 
                C2 = (number * temp) % p;
                bitdich.Append(math.BigIntegertoBinary(C2).PadLeft(dodai, '0'));
                string bghi = bitdich.ToString();
                long len = bghi.Length;
                FileStream fdich = new FileStream(pathdich, FileMode.Create);
                for(i = 0; i < len / 8;i++) {
                    fdich.WriteByte(Convert.ToByte(bghi.Substring(8 * (int)i, 8),2)); //Convert vê byte và ghi vào file
                }
                fdich.Close();
                list.Add("Encrypt Successfully");
                return list;
            }
            catch (Exception ex)
            {
                list.Add("Error:\n" + ex.Message);
                return list;
            }
        }
        public string giaima(string nguon, string dich, BigInteger p, BigInteger sk,int dodai)
        {
            try
            {
                int bitcat = 0;
                if (dodai == 1024) bitcat = 1000; //Khai báo số bit cắt
                else bitcat = 2000;
                Math math = new Math();
                FileStream fnguon = new FileStream(nguon, FileMode.Open);
                StringBuilder bitn = new StringBuilder();
                long i;
                for(i = 0; i < fnguon.Length;i++)
                {
                    bitn.Append(Convert.ToString(fnguon.ReadByte(), 2).PadLeft(8, '0')); //Chuyển hết về bit
                }
                fnguon.Close();
                string bitnguon = bitn.ToString();
                BigInteger C1 = math.BinaryToNumber(bitnguon.Substring(0, dodai)); //Lấy C1 từ file bản rõ
                C1 = math.Fast_Exponent(C1, (p - 1 - sk), p);
                //TH1 file bản rõ ngắn
                StringBuilder bitd = new StringBuilder();
                StringBuilder bittam = new StringBuilder();
                if(bitnguon.Length == dodai * 2) //TH 1: File có độ dài ngắn ko đủ 1000 bit
                {
                    BigInteger BM = math.BinaryToNumber(bitnguon.Substring(dodai,dodai)); //Đọc đoạn bit cắt và giải mã
                    BigInteger kqt = (BM * C1) % p;
                    bittam.Append(math.BigIntegertoBinary(kqt));
                    while (bittam.Length % 8 != 0) bittam.Insert(0, '0');
                    bitd.Append(bittam);
                }
                else
                { //TH2:File dài hơn 1000 bit
                    for(i = 1; i < bitnguon.Length / dodai - 1;i++)
                    {
                        BigInteger BM = math.BinaryToNumber(bitnguon.Substring(dodai * (int)i, dodai)); //ĐỌc đoạn bit cắt và giải mã
                        BigInteger kqt = (BM * C1) % p;
                        bitd.Append(math.BigIntegertoBinary(kqt).PadLeft(bitcat, '0')); //Thêm vào chuỗi tí nữa ghi, nếu chưa đủ 1000 bit ta thêm số 0 vào đầu
                    }
                    //Xử lý bit cuối
                    for(i = bitnguon.Length / dodai - 1; i < bitnguon.Length / dodai;i++)
                    {
                        BigInteger BM = math.BinaryToNumber(bitnguon.Substring(dodai * (int)i, dodai));
                        BigInteger kqt = (BM * C1) % p;
                        bittam.Append(math.BigIntegertoBinary(kqt));
                        while (bittam.Length % 8 != 0) bittam.Insert(0, '0');
                        bitd.Append(bittam);
                    }
                }
                string bitghi = bitd.ToString();
                FileStream fdich = new FileStream(dich, FileMode.Create);
                //Xử lý các bit lẻ
                for(i = 0; i < bitghi.Length/8;i++)
                {
                    fdich.WriteByte(Convert.ToByte(bitghi.Substring(8 * (int)i, 8),2));
                }
                fdich.Close();
                return "Decrypt Successfully";
            }
            catch (Exception ex)
            {
                return "Error:\n" + ex.Message;
            }
        }
    }
}
