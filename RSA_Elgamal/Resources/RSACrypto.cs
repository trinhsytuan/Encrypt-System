using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using RSA_Elgamal.Resources;
namespace RSA_Elgamal.Resources
{
    class RSACrypto
    {
        Math math = new Math();
        public string MaHoa(string filenguon, string filedich, BigInteger pk, BigInteger N, int bitlength)
        {
            try
            {
                //Khai báo biến số bit cắt bản rõ
                int sobitcat;
                if (bitlength == 1024) sobitcat = 1000; //Nếu khóa dài 1024 bit-> cắt 1000 bit
                else sobitcat = 2000; //ngược lại cắt bản rõ 2000 but
                StringBuilder bit = new StringBuilder();
                
                FileStream fdich = new FileStream(filenguon,FileMode.Open); //Đọc file nhị phân
                int dau = 0;
                long dich = fdich.Length;
                while(dau < dich)
                {
                    bit.Append(Convert.ToString(fdich.ReadByte(), 2).PadLeft(8, '0')); //Đọc hết file chuyển về nhị phân lưu vào string bit
                    dau++;
                }
                fdich.Close();
                dich = bit.Length / sobitcat; //Tính số lần lặp
                string bitthuong = bit.ToString(); //Chuyển về string thường mục đích để sử dụng lệnh substring
                StringBuilder bitghi = new StringBuilder();
                int i;
                BigInteger tam, ghi;
                for(i = 0; i < dich;i++) //Mã hóa
                {
                    tam = math.BinaryToNumber(bitthuong.Substring(sobitcat * i, sobitcat)); //Chuyển từ nhị phân thành số 
                    ghi = math.Fast_Exponent(tam, pk, N); //Thực hiện mã hóa
                    bitghi.Append(math.BigIntegertoBinary(ghi).PadLeft(bitlength, '0')); //Mã hóa xong chuyển số thành lại nhị phân
                }
                //TH2: Nếu bản rõ là 4320 bit mà ta cắt 1000 bit thì được 4 lần, còn 320 bit là phần lẻ, được xử lý ở dưới
                tam = math.BinaryToNumber(bitthuong.Substring(sobitcat * i, (int)bitthuong.Length - (sobitcat*i)));
                ghi = math.Fast_Exponent(tam, pk, N);
                bitghi.Append(math.BigIntegertoBinary(ghi).PadLeft(bitlength, '0'));
                FileStream ghif = new FileStream(filedich, FileMode.Create);
                string bitghiv = bitghi.ToString();
                for(i = 0; i < bitghiv.Length / 8;i++) //Mã hóa xong, ghi vào file
                {
                    ghif.WriteByte(Convert.ToByte(bitghiv.Substring(8 * i, 8),2));
                }
                ghif.Close();
                return "Encrypt File Successfully";

            }
            catch (Exception ex)
            {
                return "Error:\n" + ex.Message;
            }
        }
        public string giaima(string filenguon, string filedich, BigInteger sk, BigInteger n, int bitlength)
        {
           
            try
            {
                //Khai báo số bit cắt
                int sobitcat;
                if (bitlength == 1024) sobitcat = 1000;
                else sobitcat = 2000;
                FileStream fnguon = new FileStream(filenguon, FileMode.Open);
                StringBuilder dich = new StringBuilder();
                long dem = 0, len = fnguon.Length;
                while(dem < len) //Nếu chưa đọc hết file
                {
                    dem++;
                    dich.Append(Convert.ToString(fnguon.ReadByte(), 2).PadLeft(8, '0')); //Chuyển hết về dạng nhị phân, đọc được bao nhiêu chuyển về dạng nhị phân
                }
                fnguon.Close();
                string bitdoc = dich.ToString();
                BigInteger ma, ro;
                StringBuilder bitro = new StringBuilder();
                StringBuilder bithientai = new StringBuilder();
                if (bitdoc.Length == bitlength) //Nếu trường hợp bản mã có độ dài bằng độ dài bản cắt
                {
                    for (int i = 0; i < bitdoc.Length / bitlength; i++)
                    {
                        ma = math.BinaryToNumber(bitdoc.Substring(bitlength * i, bitlength)); //Chuyển về số
                        ro = math.Fast_Exponent(ma, sk, n); //Giải mã
                        bithientai.Clear(); //Làm rỗng mảng
                        bithientai.Append(math.BigIntegertoBinary(ro)); //Thêm vào string builder
                        while (bithientai.Length % 8 != 0) bithientai.Insert(0, '0'); //Nếu bit không chia hết cho 8 thêm các số 0 ở đầu
                        bitro.Append(bithientai);
                    }
                }
                else //Trường hợp bản rõ dài hơn bitcat
                {
                    for (int i = 0; i < (bitdoc.Length / bitlength) - 1; i++)
                    {
                        ma = math.BinaryToNumber(bitdoc.Substring(bitlength * i, bitlength)); //Chuyển về số
                        ro = math.Fast_Exponent(ma, sk, n); //Tiến hành mã hóa
                        bithientai.Clear(); //Clear stringbuilder
                        bithientai.Append(math.BigIntegertoBinary(ro));
                        bitro.Append(bithientai.ToString().PadLeft(sobitcat, '0')); //PadLeft để đảm bảo cho nếu không đủ bit thì thêm các con 0 ở đầu
                    }
                    //Trường hợp bit cuối phải xử lý riêng, vì nếu xử lý ở vòng for trên sẽ bị padleft -> thừa bit 0
                    for (int i = (bitdoc.Length / bitlength) - 1; i < bitdoc.Length / bitlength; i++)
                    {
                        ma = math.BinaryToNumber(bitdoc.Substring(bitlength * i, bitlength)); //Chuyển từ hệ nhị phân về số
                        ro = math.Fast_Exponent(ma, sk, n); //Tiến hành giải mã
                        bithientai.Clear();
                        bithientai.Append(math.BigIntegertoBinary(ro)); //Append vào bitht
                        while (bithientai.Length % 8 != 0) bithientai.Insert(0, '0'); //Nếu bit không chia hết cho 8-> không convert về byte được thì thêm con 0 ở đầu
                        bitro.Append(bithientai);
                    }
                }
                    FileStream filedi = new FileStream(filedich, FileMode.Create); //Đọc file nhị phân
                string bitroghi = bitro.ToString();
                for(int i = 0; i < bitroghi.Length/8;i++)
                {
                    filedi.WriteByte(Convert.ToByte(bitroghi.Substring(8 * i, 8), 2)); //Ghi lại bản rõ vào file
                }
                filedi.Close(); //ĐÓng file
                return "Decrypt Successfully";
            }
            catch (Exception ex)
            {
                return "Error:\n" + ex.Message; //Trả về lỗi
            }
        }
    }
}
