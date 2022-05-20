using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Elgamal.Resources
{
    class RC4Key
    {
        public ArrayList Key3Bit() {
            Random random = new Random();
            string s = "";
            for(int i = 0; i < 8;i++)
            {
                //Phải +1 vì nó trả về giá trị từ [dau,cuoi)
                s+= random.Next(0, 8) + ",";
            }
            ArrayList list = new ArrayList();
            list.Add(s.Substring(0, s.Length - 1));
            list.Add("0,1,2,3,4,5,6,7");
            return list;
        }
        public ArrayList Key8Bit()
        {
            Random random = new Random();
            string s = "";
            string key = "";
            for (int i = 0; i < 256; i++) //Do khóa 8 bit, giá trị nằm trong khoảng từ 0,255
            {
                //Phải +1 vì nó trả về giá trị từ [dau,cuoi)
                s += random.Next(0, 256) + ",";
                key += i + ",";
            }
            ArrayList list = new ArrayList();
            list.Add(s.Substring(0, s.Length - 1)); //ĐƯa khóa ra màn hình, bỏ dấu , ở cuối nên ta substring
            list.Add(key.Substring(0,key.Length - 1)); //Tương tự với mảng key
            return list;
        }
    }
}
