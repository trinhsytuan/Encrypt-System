using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RSA_Elgamal.Resources;
namespace RSA_Elgamal
{
    public partial class Form1 : Form
    {

        public Form1()
        {

            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            tc_loaihe.SelectedIndex = 7;
            btn_rsa_keylength.SelectedIndex = 0;
            cb_elgamal_dodai.SelectedIndex = 0;
            cb_elgamal_mh_dodai.SelectedIndex = 0;
            cb_rsa_mh_dodai.SelectedIndex = 0;
            rb_rsa_mh_ghide.Checked = true;
            rb_elgamal_mh_ghide.Checked = true;
            cb_a51_dodai.SelectedIndex = 0;
            rb_a51_ghide.Checked = true;
            rb_rc4_ghide.Checked = true;
            cb_rc4_dodai.SelectedIndex = 1;
            cb_dss_keylength.SelectedIndex = 0;
        }

        private void btn_rsa_taokhoa_Click(object sender, EventArgs e)
        {
            RSAKey moi = new RSAKey();
            ArrayList khoa = new ArrayList();
            int dodaikhoa;
            if (btn_rsa_keylength.SelectedIndex == 0) dodaikhoa = 1024;
            else dodaikhoa = 2048;
            khoa = moi.generateKey(dodaikhoa);
            tb_rsa_p.Text = khoa[0].ToString();
            tb_rsa_q.Text = khoa[1].ToString();
            tb_rsa_n.Text = khoa[2].ToString();
            tb_rsa_phin.Text = khoa[3].ToString();
            tb_rsa_pk.Text = khoa[4].ToString();
            tb_rsa_sk.Text = khoa[5].ToString();
        }

        private void btn_rsa_mokhoa_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "File Key .txt|*.txt";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    //FileStream -> File nhị phân
                    // StreamReader -> File văn bản
                    StreamReader k = new StreamReader(openFileDialog1.FileName, Encoding.UTF8);
                    tb_rsa_p.Text = k.ReadLine();
                    tb_rsa_q.Text = k.ReadLine();
                    tb_rsa_n.Text = k.ReadLine();
                    tb_rsa_phin.Text = k.ReadLine();
                    tb_rsa_pk.Text = k.ReadLine();
                    tb_rsa_sk.Text = k.ReadLine();
                    k.Close();
                    MessageBox.Show("Read key from file successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message, "Error");
            }
        }

        private void btn_rsa_luukhoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_rsa_n.Text) || string.IsNullOrEmpty(tb_rsa_p.Text) || string.IsNullOrEmpty(tb_rsa_phin.Text) || string.IsNullOrEmpty(tb_rsa_pk.Text) || string.IsNullOrEmpty(tb_rsa_sk.Text) || string.IsNullOrEmpty(tb_rsa_q.Text))
            {
                MessageBox.Show("You have not created a key", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            saveFileDialog1.Filter = "File Text .txt|*.txt";
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter k = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8);
                    k.WriteLine(tb_rsa_p.Text);
                    k.WriteLine(tb_rsa_q.Text);
                    k.WriteLine(tb_rsa_n.Text);
                    k.WriteLine(tb_rsa_phin.Text);
                    k.WriteLine(tb_rsa_pk.Text);
                    k.WriteLine(tb_rsa_sk.Text);
                    k.Close();
                    MessageBox.Show("Save Key to File Success", "Notification", MessageBoxButtons.OK);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message, "Error");
            }
        }

        private void btn_rsa_chonfile_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "All File|*.*";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                tb_rsa_filevanban.Text = openFileDialog2.FileName;
            }
        }

        private void btn_rsa_chonchuky_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog3.Filter = "Signature File .txt|*.txt";
                if (openFileDialog3.ShowDialog() == DialogResult.OK)
                {
                    StreamReader ck = new StreamReader(openFileDialog3.FileName, Encoding.UTF8);
                    string s = ck.ReadToEnd();
                    tb_rsa_chuky.Text = s;
                    ck.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message, "Notification");
            }
        }

        private void btn_rsa_luuchuky_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tb_rsa_chuky.Text))
                {
                    MessageBox.Show("You have not signed the document", "Notification");
                    return;
                }
                saveFileDialog2.Filter = "Signature File .txt|*.txt";
                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter ck = new StreamWriter(saveFileDialog2.FileName, false, Encoding.UTF8);
                    ck.Write(tb_rsa_chuky.Text);
                    ck.Close();
                    MessageBox.Show("Save Signature to File Success", "Notification");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message, "Error");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_rsa_filevanban.Text))
            {
                //Thông báo cho người dùng chưa chọn file chữ ký -> Vì không có chữ ký không xác minh được

                MessageBox.Show("You have not selected the file to sign", "Notification");
                return;
            }
            if (string.IsNullOrEmpty(tb_rsa_sk.Text) || string.IsNullOrEmpty(tb_rsa_n.Text))
            {
                //Kiểm tra ngươi dùng xem đã mở khóa hoặc tạo khóa chưa
                MessageBox.Show("You have not generated a key or open key from file", "Notification");
                return;
            }
            RSASig ky = new RSASig();
            BigInteger d = BigInteger.Parse(tb_rsa_sk.Text);
            BigInteger n = BigInteger.Parse(tb_rsa_n.Text);
            string chuky = ky.mahoa(d, n, tb_rsa_filevanban.Text);
            tb_rsa_chuky.Text = chuky;
            tb_rsa_ketqua.Text = "Signed Successfully";
            tb_rsa_ketqua.ForeColor = Color.Green;
            MessageBox.Show("Signed Successfully", "Notification");
        }
        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_rsa_filevanban.Text) || string.IsNullOrEmpty(tb_rsa_chuky.Text))
            {
                //Kiểm tra ng dùng đã select file hay có chữ ký chưa
                MessageBox.Show("You have not selected a file or select signature", "Notificaion");
                return;
            }
            if (string.IsNullOrEmpty(tb_rsa_pk.Text) || string.IsNullOrEmpty(tb_rsa_n.Text))
            {

                MessageBox.Show("You have not created a key or open key from file", "Notification");
                return;
            }
            RSASig ky = new RSASig();
            BigInteger enumber = BigInteger.Parse(tb_rsa_pk.Text);
            BigInteger n = BigInteger.Parse(tb_rsa_n.Text);
            if (ky.giaima(enumber, n, tb_rsa_filevanban.Text, tb_rsa_chuky.Text))
            {
                tb_rsa_ketqua.ForeColor = Color.Green;
                tb_rsa_ketqua.Text = "Signature Valid";
                MessageBox.Show("Signature Valid", "Notification");

            }
            else
            {
                tb_rsa_ketqua.Text = "Signature invalid. Wrong Signature";
                tb_rsa_ketqua.ForeColor = Color.Red;
                MessageBox.Show("Signature invalid. Wrong Signature", "Notification");
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void tb_elgamal_taokhoa_Click(object sender, EventArgs e)
        {
            ElgamalKey key = new ElgamalKey();
            int dodaikhoa = cb_elgamal_dodai.SelectedIndex == 0 ? 1024 : 2048;
            ArrayList m = key.Key(dodaikhoa);
            tb_elgamal_p.Text = m[0].ToString();
            tb_elgamal_alpha.Text = m[1].ToString();
            tb_elgamal_pk.Text = m[2].ToString();
            tb_elgamal_sk.Text = m[3].ToString();
            tb_elgamal_cannt.Text = m[4].ToString();
        }

        private void bt_elgamal_mokhoa_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "File Key .txt|*.txt";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamReader file = new StreamReader(openFileDialog1.FileName, Encoding.UTF8);
                    tb_elgamal_p.Text = file.ReadLine();
                    tb_elgamal_alpha.Text = file.ReadLine();
                    tb_elgamal_pk.Text = file.ReadLine();
                    tb_elgamal_sk.Text = file.ReadLine();
                    tb_elgamal_cannt.Text = file.ReadLine();
                    file.Close();
                    MessageBox.Show("Read key from file successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message, "Error");
            }
        }

        private void tb_elgamal_luukhoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tb_elgamal_p.Text) || string.IsNullOrEmpty(tb_elgamal_pk.Text) || string.IsNullOrEmpty(tb_elgamal_sk.Text) || string.IsNullOrEmpty(tb_elgamal_alpha.Text) || string.IsNullOrEmpty(tb_elgamal_cannt.Text))
                {
                    MessageBox.Show("You have not created a key", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                saveFileDialog1.Filter = "File Key .txt|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter file = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8);
                    file.WriteLine(tb_elgamal_p.Text);
                    file.WriteLine(tb_elgamal_alpha.Text);
                    file.WriteLine(tb_elgamal_pk.Text);
                    file.WriteLine(tb_elgamal_sk.Text);
                    file.WriteLine(tb_elgamal_cannt.Text);
                    file.Close();
                    MessageBox.Show("Save key to File Successfully", "Notification");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message, "Error");
            }
        }

        private void bt_elgamal_ky_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_elgamal_p.Text) || string.IsNullOrEmpty(tb_elgamal_alpha.Text) || string.IsNullOrEmpty(tb_elgamal_sk.Text))
            {
                MessageBox.Show("You have not created a key or open key from file", "Notification");
                return;
            }
            if (string.IsNullOrEmpty(tb_elgamal_filecanky.Text))
            {
                MessageBox.Show("You have not selected source file", "Notification");
                return;
            }
            ElgamalSig ky = new ElgamalSig();
            ArrayList k = ky.sig(tb_elgamal_filecanky.Text, BigInteger.Parse(tb_elgamal_p.Text), BigInteger.Parse(tb_elgamal_alpha.Text), BigInteger.Parse(tb_elgamal_sk.Text));
            tb_elgamal_k.Text = k[0].ToString();
            tb_elgamal_chuky1.Text = k[1].ToString();
            tb_elgamal_chuky2.Text = k[2].ToString();
            tb_elgamal_ketqua.ForeColor = Color.Green;
            tb_elgamal_ketqua.Text = "Signed Successfully";
            MessageBox.Show("Signed Successfully", "Notification");

        }

        private void bt_elgamal_chonfile_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "All File|*.*";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                tb_elgamal_filecanky.Text = openFileDialog2.FileName;
            }
        }

        private void bt_elgamal_xacminh_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_elgamal_filecanky.Text) || string.IsNullOrEmpty(tb_elgamal_chuky1.Text) || string.IsNullOrEmpty(tb_elgamal_chuky2.Text))
            {
                MessageBox.Show("You have not selected the file or select signature file", "Notification");
                return;
            }
            if (string.IsNullOrEmpty(tb_elgamal_p.Text) || string.IsNullOrEmpty(tb_elgamal_alpha.Text) || string.IsNullOrEmpty(tb_elgamal_pk.Text))
            {
                MessageBox.Show("You have not created key or open key from file", "Notification");
                return;
            }
            ElgamalSig sig = new ElgamalSig();
            if (sig.ktra((tb_elgamal_filecanky.Text), BigInteger.Parse(tb_elgamal_p.Text), BigInteger.Parse(tb_elgamal_alpha.Text), BigInteger.Parse(tb_elgamal_pk.Text), BigInteger.Parse(tb_elgamal_chuky1.Text), BigInteger.Parse(tb_elgamal_chuky2.Text)) == true)
            {
                tb_elgamal_ketqua.ForeColor = Color.Green;
                tb_elgamal_ketqua.Text = "Signature Valid";
                MessageBox.Show("Signature Valid", "Notification");

            }
            else
            {
                MessageBox.Show("Signature invalid. Wrong Signature", "Notification");
                tb_elgamal_ketqua.ForeColor = Color.Red;
                tb_elgamal_ketqua.Text = "Signature invalid. Wrong Signature";
            }
        }

        private void bt_elgamal_chonck_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog3.Filter = "File Signature .txt|*.txt";
                if (openFileDialog3.ShowDialog() == DialogResult.OK)
                {
                    StreamReader file = new StreamReader(openFileDialog3.FileName, Encoding.UTF8);
                    tb_elgamal_chuky1.Text = file.ReadLine();
                    tb_elgamal_chuky2.Text = file.ReadLine();
                    file.Close();
                    MessageBox.Show("Open file Signature Successfully", "Notification");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message, "Error");
            }
        }

        private void bt_elgamal_luuck_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(tb_elgamal_chuky1.Text) || string.IsNullOrEmpty(tb_elgamal_chuky2.Text))
            {
                MessageBox.Show("You have not signed the document", "Notification");
                return;
            }
            try
            {
                saveFileDialog2.Filter = "File Signature .txt|*.txt";
                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter file = new StreamWriter(saveFileDialog2.FileName, false, Encoding.UTF8);
                    file.WriteLine(tb_elgamal_chuky1.Text);
                    file.WriteLine(tb_elgamal_chuky2.Text);
                    file.Close();
                    MessageBox.Show("Save key to file successfully", "Notification");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message, "Error");
            }
        }

        private void bt_rsa_mh_mokhoa_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "File key .txt|*.txt";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamReader k = new StreamReader(openFileDialog1.FileName, Encoding.UTF8);
                    tb_rsa_mh_p.Text = k.ReadLine();
                    tb_rsa_mh_q.Text = k.ReadLine();
                    tb_rsa_mh_n.Text = k.ReadLine();
                    tb_rsa_mh_phin.Text = k.ReadLine();
                    tb_rsa_mh_pk.Text = k.ReadLine();
                    tb_rsa_mh_sk.Text = k.ReadLine();
                    k.Close();
                    MessageBox.Show("Read key from file successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message, "Error");
            }
        }

        private void bt_rsa_mh_taokhoa_Click(object sender, EventArgs e)
        {
            RSAKey moi = new RSAKey();
            ArrayList khoa = new ArrayList();
            int dodaikhoa;
            if (cb_rsa_mh_dodai.SelectedIndex == 0) dodaikhoa = 1024;
            else dodaikhoa = 2048;
            khoa = moi.generateKey(dodaikhoa);
            tb_rsa_mh_p.Text = khoa[0].ToString();
            tb_rsa_mh_q.Text = khoa[1].ToString();
            tb_rsa_mh_n.Text = khoa[2].ToString();
            tb_rsa_mh_phin.Text = khoa[3].ToString();
            tb_rsa_mh_pk.Text = khoa[4].ToString();
            tb_rsa_mh_sk.Text = khoa[5].ToString();
        }

        private void bt_rsa_mh_luukhoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_rsa_mh_n.Text) || string.IsNullOrEmpty(tb_rsa_mh_p.Text) || string.IsNullOrEmpty(tb_rsa_mh_phin.Text) || string.IsNullOrEmpty(tb_rsa_mh_pk.Text) || string.IsNullOrEmpty(tb_rsa_mh_sk.Text) || string.IsNullOrEmpty(tb_rsa_mh_q.Text))
            {
                MessageBox.Show("You have not created a key", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            saveFileDialog1.Filter = "File Text .txt|*.txt";
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter k = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8);
                    k.WriteLine(tb_rsa_mh_p.Text);
                    k.WriteLine(tb_rsa_mh_q.Text);
                    k.WriteLine(tb_rsa_mh_n.Text);
                    k.WriteLine(tb_rsa_mh_phin.Text);
                    k.WriteLine(tb_rsa_mh_pk.Text);
                    k.WriteLine(tb_rsa_mh_sk.Text);
                    k.Close();
                    MessageBox.Show("Save key to File Success", "Notification", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message, "Error");
            }
        }

        private void tb_elgamal_mh_mokhoa_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "File Key .txt|*.txt";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamReader file = new StreamReader(openFileDialog1.FileName, Encoding.UTF8);
                    tb_elgamal_mh_p.Text = file.ReadLine();
                    tb_elgamal_mh_alpha.Text = file.ReadLine();
                    tb_elgamal_mh_pk.Text = file.ReadLine();
                    tb_elgamal_mh_sk.Text = file.ReadLine();
                    tb_elgamal_mh_cannt.Text = file.ReadLine();
                    file.Close();
                    MessageBox.Show("Read key from file successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message, "Error");
            }
        }
        private void tb_elgamal_mh_luukhoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tb_elgamal_mh_p.Text) || string.IsNullOrEmpty(tb_elgamal_mh_pk.Text) || string.IsNullOrEmpty(tb_elgamal_mh_sk.Text) || string.IsNullOrEmpty(tb_elgamal_mh_alpha.Text) || string.IsNullOrEmpty(tb_elgamal_mh_cannt.Text))
                {
                    MessageBox.Show("You have not created a key", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                saveFileDialog1.Filter = "File Key .txt|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter file = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8);
                    file.WriteLine(tb_elgamal_mh_p.Text);
                    file.WriteLine(tb_elgamal_mh_alpha.Text);
                    file.WriteLine(tb_elgamal_mh_pk.Text);
                    file.WriteLine(tb_elgamal_mh_sk.Text);
                    file.WriteLine(tb_elgamal_mh_cannt.Text);
                    file.Close();
                }
                MessageBox.Show("Save key to file Success", "Notification");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message, "Error");
            }
        }

        private void tb_elgamal_mh_taokhoa_Click(object sender, EventArgs e)
        {
            ElgamalKey key = new ElgamalKey();
            int dodaikhoa;
            if (cb_elgamal_dodai.SelectedIndex == 0) dodaikhoa = 1024;
            else dodaikhoa = 2048;
            ArrayList m = key.Key(dodaikhoa);
            tb_elgamal_mh_p.Text = m[0].ToString();
            tb_elgamal_mh_alpha.Text = m[1].ToString();
            tb_elgamal_mh_pk.Text = m[2].ToString();
            tb_elgamal_mh_sk.Text = m[3].ToString();
            tb_elgamal_mh_cannt.Text = m[4].ToString();
        }

        private void rb_rsa_mh_ghide_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_rsa_mh_ghide.Checked)
            {
                tb_rsa_mh_filedich.Enabled = false;
                bt_rsa_mh_chonfiledich.Enabled = false;

            }
            else
            {
                tb_rsa_mh_filedich.Enabled = true;
                bt_rsa_mh_chonfiledich.Enabled = true;

            }
        }

        private void rb_elgamal_mh_ghide_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_elgamal_mh_ghide.Checked)
            {
                tb_elgamal_mh_filedich.Enabled = false;
                bt_elgamal_mh_chonfiledich.Enabled = false;
            }
            else
            {
                tb_elgamal_mh_filedich.Enabled = true;
                bt_elgamal_mh_chonfiledich.Enabled = true;
            }
        }

        private void bt_rsa_mh_chonfilenguon_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "All File|*.*";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                tb_rsa_mh_filenguon.Text = openFileDialog2.FileName;
            }
        }

        private void bt_elgamal_mh_chonfilenguon_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "All File|*.*";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                tb_elgamal_mh_filenguon.Text = openFileDialog2.FileName;
            }
        }

        private void bt_elgamal_mh_chonfiledich_Click(object sender, EventArgs e)
        {
            string s;
            //Đảm bảo rằng đuôi của file nguồn trùng với đuôi của file đích
            if (string.IsNullOrEmpty(tb_elgamal_mh_filenguon.Text)) s = "All File|*.*";
            else s = "File " + Path.GetExtension(tb_elgamal_mh_filenguon.Text) + "|*" + Path.GetExtension(tb_elgamal_mh_filenguon.Text);
            saveFileDialog2.Filter = s;
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                tb_elgamal_mh_filedich.Text = saveFileDialog2.FileName;
            }
        }

        private void bt_rsa_mh_chonfiledich_Click(object sender, EventArgs e)
        {
            string s;
            if (string.IsNullOrEmpty(tb_rsa_mh_filenguon.Text)) s = "All File|*.*";
            else s = "File " + Path.GetExtension(tb_rsa_mh_filenguon.Text) + "|*" + Path.GetExtension(tb_rsa_mh_filenguon.Text);
            saveFileDialog2.Filter = s;
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                tb_rsa_mh_filedich.Text = saveFileDialog2.FileName;
            }
        }

        private void bt_rsa_mh_mahoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_rsa_mh_n.Text) || string.IsNullOrEmpty(tb_rsa_mh_pk.Text) || string.IsNullOrEmpty(tb_rsa_mh_sk.Text))
            {
                MessageBox.Show("You have not created key or open key from file", "Notification");
                return;
            }
            if (string.IsNullOrEmpty(tb_rsa_mh_filenguon.Text))
            {
                MessageBox.Show("You have not selected source file");
                return;
            }
            RSACrypto mh = new RSACrypto();
            if (rb_rsa_mh_ghide.Checked)
            {
                int dodaikhoa;
                if (cb_rsa_mh_dodai.SelectedIndex == 0) dodaikhoa = 1024;
                else dodaikhoa = 2048;
                string s = mh.MaHoa(tb_rsa_mh_filenguon.Text, tb_rsa_mh_filenguon.Text + ".temp", BigInteger.Parse(tb_rsa_mh_pk.Text), BigInteger.Parse(tb_rsa_mh_n.Text), dodaikhoa);
                File.Delete(tb_rsa_mh_filenguon.Text);
                System.IO.File.Move(tb_rsa_mh_filenguon.Text + ".temp", tb_rsa_mh_filenguon.Text);
                MessageBox.Show(s, "Notification");

            }
            else
            {
                if (string.IsNullOrEmpty(tb_rsa_mh_filedich.Text))
                {
                    MessageBox.Show("You have not selected destination file", "Notification");
                    return;
                }
                int dodaikhoa;
                if (cb_rsa_mh_dodai.SelectedIndex == 0) dodaikhoa = 1024;
                else dodaikhoa = 2048;
                string s = mh.MaHoa(tb_rsa_mh_filenguon.Text, tb_rsa_mh_filedich.Text, BigInteger.Parse(tb_rsa_mh_pk.Text), BigInteger.Parse(tb_rsa_mh_n.Text), dodaikhoa);
                MessageBox.Show(s, "Notification");

            }

        }

        private void bt_rsa_mh_giaima_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_rsa_mh_n.Text) || string.IsNullOrEmpty(tb_rsa_mh_pk.Text) || string.IsNullOrEmpty(tb_rsa_mh_sk.Text))
            {
                MessageBox.Show("You have not created key or open key from file", "Notification");
                return;
            }
            if (string.IsNullOrEmpty(tb_rsa_mh_filenguon.Text))
            {
                MessageBox.Show("You have not selected source file");
                return;
            }
            RSACrypto mh = new RSACrypto();
            if (rb_rsa_mh_ghide.Checked)
            {
                int dodaikhoa;
                if (cb_rsa_mh_dodai.SelectedIndex == 0) dodaikhoa = 1024;
                else dodaikhoa = 2048;
                string s = mh.giaima(tb_rsa_mh_filenguon.Text, tb_rsa_mh_filenguon.Text + ".temp", BigInteger.Parse(tb_rsa_mh_sk.Text), BigInteger.Parse(tb_rsa_mh_n.Text), dodaikhoa);
                File.Delete(tb_rsa_mh_filenguon.Text);
                System.IO.File.Move(tb_rsa_mh_filenguon.Text + ".temp", tb_rsa_mh_filenguon.Text);
                MessageBox.Show(s, "Notification");
            }
            else
            {
                if (string.IsNullOrEmpty(tb_rsa_mh_filedich.Text))
                {
                    MessageBox.Show("You have not selected destination file", "Notification");
                    return;
                }
                int dodaikhoa;
                if (cb_rsa_mh_dodai.SelectedIndex == 0) dodaikhoa = 1024;
                else dodaikhoa = 2048;
                string s = mh.giaima(tb_rsa_mh_filenguon.Text, tb_rsa_mh_filedich.Text, BigInteger.Parse(tb_rsa_mh_sk.Text), BigInteger.Parse(tb_rsa_mh_n.Text), dodaikhoa);
                MessageBox.Show(s, "Notification");
            }
        }

        private void bt_elgamal_mh_mahoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_elgamal_mh_alpha.Text) || string.IsNullOrEmpty(tb_elgamal_mh_pk.Text) || string.IsNullOrEmpty(tb_elgamal_mh_p.Text))
            {
                MessageBox.Show("You have not created key or open key from file", "Notification");
                return;
            }
            ElgamalCypto mh = new ElgamalCypto();
            int dodai = 0;
            if (string.IsNullOrEmpty(tb_elgamal_mh_filenguon.Text))
            {
                MessageBox.Show("You have not selected source file");
                return;
            }
            if (rb_elgamal_mh_ghide.Checked)
            {
                if (cb_elgamal_mh_dodai.SelectedIndex == 0) dodai = 1024;
                else dodai = 2048;
                ArrayList v = mh.mahoa(tb_elgamal_mh_filenguon.Text, tb_elgamal_mh_filenguon.Text + ".temp", BigInteger.Parse(tb_elgamal_mh_alpha.Text), BigInteger.Parse(tb_elgamal_mh_pk.Text), BigInteger.Parse(tb_elgamal_mh_p.Text), dodai);
                tb_elgamal_mh_k.Text = v[0].ToString();
                File.Delete(tb_elgamal_mh_filenguon.Text);
                File.Move(tb_elgamal_mh_filenguon.Text + ".temp", tb_elgamal_mh_filenguon.Text);
                MessageBox.Show(v[1].ToString(), "Notification");
            }
            else
            {
                if (string.IsNullOrEmpty(tb_elgamal_mh_filedich.Text))
                {
                    MessageBox.Show("You have not selected destination file", "Notification");
                    return;
                }
                if (cb_elgamal_mh_dodai.SelectedIndex == 0) dodai = 1024;
                else dodai = 2048;
                ArrayList v = mh.mahoa(tb_elgamal_mh_filenguon.Text, tb_elgamal_mh_filedich.Text, BigInteger.Parse(tb_elgamal_mh_alpha.Text), BigInteger.Parse(tb_elgamal_mh_pk.Text), BigInteger.Parse(tb_elgamal_mh_p.Text), dodai);
                tb_elgamal_mh_k.Text = v[0].ToString();
                MessageBox.Show(v[1].ToString(), "Notification");
            }
        }

        private void bt_a51_taokhoa_Click(object sender, EventArgs e)
        {
            A51Key key = new A51Key();
            if (cb_a51_dodai.SelectedIndex == 0)
            {
                ArrayList kq = key.Key23Bit();
                tb_a51_x.Text = kq[0].ToString();
                tb_a51_y.Text = kq[1].ToString();
                tb_a51_z.Text = kq[2].ToString();
            }
            else
            {
                ArrayList kq = key.Key64Bit();
                tb_a51_x.Text = kq[0].ToString();
                tb_a51_y.Text = kq[1].ToString();
                tb_a51_z.Text = kq[2].ToString();
            }
        }

        private void bt_a51_luukhoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_a51_x.Text) || string.IsNullOrEmpty(tb_a51_y.Text) || string.IsNullOrEmpty(tb_a51_z.Text))
            {
                MessageBox.Show("You have not created a key", "Notification");
                return;
            }
            try
            {
                saveFileDialog1.Filter = "File key .txt|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter file = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8);
                    file.WriteLine(tb_a51_x.Text);
                    file.WriteLine(tb_a51_y.Text);
                    file.WriteLine(tb_a51_z.Text);

                    file.Close();
                    MessageBox.Show("Save key to file success", "Notification");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message, "Error");
            }
        }

        private void bt_a51_mokhoa_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Reset();    //Reset all setting openFileDialog
                openFileDialog1.Filter = "File Key .txt|*.txt";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamReader doc = new StreamReader(openFileDialog1.FileName, Encoding.UTF8);
                    tb_a51_x.Text = doc.ReadLine();
                    tb_a51_y.Text = doc.ReadLine();
                    tb_a51_z.Text = doc.ReadLine();
                    doc.Close();
                    if (tb_a51_x.Text.Length == 6) cb_a51_dodai.SelectedIndex = 0;
                    else cb_a51_dodai.SelectedIndex = 1;
                    MessageBox.Show("Read key from file successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message, "Error");
            }
        }

        private void bt_a51_chonfilenguon_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "All File|*.*";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                tb_a51_filenguon.Text = openFileDialog2.FileName;
            }
        }

        private void bt_a51_chonfiledich_Click(object sender, EventArgs e)
        {
            string s;
            if (tb_a51_filenguon.Text == "") s = "All File|*.*";
            else s = "File " + Path.GetExtension(tb_a51_filenguon.Text) + "|*" + Path.GetExtension(tb_a51_filenguon.Text);
            saveFileDialog2.Filter = s;
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                tb_a51_filedich.Text = saveFileDialog2.FileName;
            }

        }

        private void bt_a51_mahoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_a51_x.Text) || string.IsNullOrEmpty(tb_a51_y.Text) || string.IsNullOrEmpty(tb_a51_z.Text))
            {
                MessageBox.Show("You have not generated key or open key from file", "Notification");
                return;
            }
            if (string.IsNullOrEmpty(tb_a51_filenguon.Text))
            {
                MessageBox.Show("You have not selected source file", "Notification");
                return;
            }

            if (cb_a51_dodai.SelectedIndex == 0)
            {
                if (tb_a51_x.Text.Length > 6 || tb_a51_y.Text.Length > 8 || tb_a51_z.Text.Length > 9)
                {
                    MessageBox.Show("Your key is currently a 64-bit key, please check the key again", "Notification");
                    return;
                }
                if (rb_a51_ghide.Checked)
                {
                    A51Crypto23Bit moi = new A51Crypto23Bit(tb_a51_x.Text, tb_a51_y.Text, tb_a51_z.Text);
                    string m = moi.A51Cryto(tb_a51_filenguon.Text, tb_a51_filenguon.Text + ".temp");
                    File.Delete(tb_a51_filenguon.Text);
                    System.IO.File.Move(tb_a51_filenguon.Text + ".temp", tb_a51_filenguon.Text);
                    MessageBox.Show(m, "Notiication");
                }
                else
                {

                    if (string.IsNullOrEmpty(tb_a51_filedich.Text))
                    {
                        MessageBox.Show("You have not selected destination file", "Notification");
                        return;
                    }
                    A51Crypto23Bit moi = new A51Crypto23Bit(tb_a51_x.Text, tb_a51_y.Text, tb_a51_z.Text);
                    string m = moi.A51Cryto(tb_a51_filenguon.Text, tb_a51_filedich.Text);
                    MessageBox.Show(m, "Notification");
                }
            }
            else
            {
                if (tb_a51_x.Text.Length < 19 || tb_a51_y.Text.Length < 22 || tb_a51_z.Text.Length < 23)
                {
                    MessageBox.Show("Your key is currently a 23-bit key, please check the key again", "Notification");
                    return;
                }
                if (rb_a51_ghide.Checked)
                {
                    A51Crypto64Bit moi = new A51Crypto64Bit(tb_a51_x.Text, tb_a51_y.Text, tb_a51_z.Text);
                    string m = moi.A51Cryto(tb_a51_filenguon.Text, tb_a51_filenguon.Text + ".temp");
                    File.Delete(tb_a51_filenguon.Text);
                    System.IO.File.Move(tb_a51_filenguon.Text + ".temp", tb_a51_filenguon.Text);
                    MessageBox.Show(m, "Notification");
                }
                else
                {
                    if (string.IsNullOrEmpty(tb_a51_filedich.Text))
                    {
                        MessageBox.Show("You have not selected destination file", "Notification");
                        return;
                    }
                    A51Crypto64Bit moi = new A51Crypto64Bit(tb_a51_x.Text, tb_a51_y.Text, tb_a51_z.Text);
                    string m = moi.A51Cryto(tb_a51_filenguon.Text, tb_a51_filedich.Text);
                    MessageBox.Show(m, "Notification");
                }
            }

        }

        private void rb_a51_ghide_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_a51_ghide.Checked)
            {
                tb_a51_filedich.Enabled = false;
                bt_a51_chonfiledich.Enabled = false;
            }
            else
            {
                tb_a51_filedich.Enabled = true;
                bt_a51_chonfiledich.Enabled = true;
            }
        }

        private void bt_elgamal_mh_giaima_Click(object sender, EventArgs e)
        {

            ElgamalCypto mh = new ElgamalCypto();
            int dodai = 0;
            if (rb_elgamal_mh_ghide.Checked)
            {
                if (cb_elgamal_mh_dodai.SelectedIndex == 0) dodai = 1024;
                else dodai = 2048;
                string s = mh.giaima(tb_elgamal_mh_filenguon.Text, tb_elgamal_mh_filenguon.Text + ".temp", BigInteger.Parse(tb_elgamal_mh_p.Text), BigInteger.Parse(tb_elgamal_mh_sk.Text), dodai);
                File.Delete(tb_elgamal_mh_filenguon.Text);
                File.Move(tb_elgamal_mh_filenguon.Text + ".temp", tb_elgamal_mh_filenguon.Text);
                MessageBox.Show(s, "Notification");
            }
            else
            {
                if (string.IsNullOrEmpty(tb_elgamal_mh_filedich.Text))
                {
                    MessageBox.Show("You have not selected destination file", "Notification");
                    return;
                }
                if (cb_elgamal_mh_dodai.SelectedIndex == 0) dodai = 1024;
                else dodai = 2048;
                string s = mh.giaima(tb_elgamal_mh_filenguon.Text, tb_elgamal_mh_filenguon.Text, BigInteger.Parse(tb_elgamal_mh_p.Text), BigInteger.Parse(tb_elgamal_mh_sk.Text), dodai);

                MessageBox.Show(s, "Notification");
            }
        }

        private void bt_rc4_mokhoa_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "File key .txt|*.txt";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamReader doc = new StreamReader(openFileDialog1.FileName, Encoding.UTF8);
                    tb_rc4_keyarray.Text = doc.ReadLine();
                    tb_rc4_sarray.Text = doc.ReadLine();
                    doc.Close();
                    if (tb_rc4_keyarray.Text.Length == 15) cb_rc4_dodai.SelectedIndex = 0;
                    else cb_rc4_dodai.SelectedIndex = 1;
                    MessageBox.Show("Read key from file successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message);
            }
        }

        private void bt_rc4_luukhoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_rc4_sarray.Text) || string.IsNullOrEmpty(tb_rc4_keyarray.Text))
            {
                MessageBox.Show("You have not created key", "Notification");
                return;
            }
            try
            {
                saveFileDialog1.Filter = "File key .txt|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter file = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8);
                    file.WriteLine(tb_rc4_keyarray.Text);
                    file.WriteLine(tb_rc4_sarray.Text);
                    file.Close();
                    MessageBox.Show("Save key to file successfully", "Notification");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message);
            }
        }

        private void rb_rc4_ghide_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_rc4_ghide.Checked)
            {
                tb_rc4_filedich.Enabled = false;
                bt_rc4_chonfiledich.Enabled = false;
            }
            else
            {
                tb_rc4_filedich.Enabled = true;
                bt_rc4_chonfiledich.Enabled = true;
            }
        }

        private void bt_rc4_taokhoa_Click(object sender, EventArgs e)
        {
            if (cb_rc4_dodai.SelectedIndex == 0)
            {
                RC4Key key = new RC4Key();
                ArrayList k = key.Key3Bit();
                tb_rc4_keyarray.Text = k[0].ToString();
                tb_rc4_sarray.Text = k[1].ToString();
            }
            else
            {
                RC4Key key = new RC4Key();
                ArrayList k = key.Key8Bit();
                tb_rc4_keyarray.Text = k[0].ToString();
                tb_rc4_sarray.Text = k[1].ToString();
            }
        }

        private void bt_rc4_chonfilenguon_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "All File|*.*";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                tb_rc4_filenguon.Text = openFileDialog2.FileName;
            }
        }

        private void bt_rc4_chonfiledich_Click(object sender, EventArgs e)
        {
            string s;
            if (tb_rc4_filenguon.Text == "") s = "All File|*.*";
            else s = "File " + Path.GetExtension(tb_rc4_filenguon.Text) + "|*" + Path.GetExtension(tb_rc4_filenguon.Text);
            saveFileDialog2.Filter = s;
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                tb_rc4_filedich.Text = saveFileDialog2.FileName;
            }
        }

        private void bt_rc4_mahoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_rc4_filenguon.Text))
            {
                MessageBox.Show("You have not selected source file", "Notification");
                return;
            }
            if (cb_rc4_dodai.SelectedIndex == 0)
            {
                if (rb_rc4_ghide.Checked)
                {
                    RC4Crypto3Bit mh = new RC4Crypto3Bit();
                    string s = mh.EncryptAndDecrypt(tb_rc4_filenguon.Text, tb_rc4_filenguon.Text + ".temp", tb_rc4_keyarray.Text, tb_rc4_sarray.Text);
                    File.Delete(tb_rc4_filenguon.Text);
                    File.Move(tb_rc4_filenguon.Text + ".temp", tb_rc4_filenguon.Text);
                    MessageBox.Show(s, "Notification");
                }
                else
                {
                    if (string.IsNullOrEmpty(tb_rc4_filedich.Text))
                    {
                        MessageBox.Show("You have not selected destination file", "Notification");
                        return;
                    }
                    RC4Crypto3Bit mh = new RC4Crypto3Bit();
                    string s = mh.EncryptAndDecrypt(tb_rc4_filenguon.Text, tb_rc4_filedich.Text, tb_rc4_keyarray.Text, tb_rc4_sarray.Text);

                    MessageBox.Show(s, "Notification");
                }
            }
            else
            {
                if (rb_rc4_ghide.Checked)
                {
                    RC4Crypto8Bit mh = new RC4Crypto8Bit();
                    string s = mh.EncryptAndDecrypt(tb_rc4_filenguon.Text, tb_rc4_filenguon.Text + ".temp", tb_rc4_keyarray.Text, tb_rc4_sarray.Text);
                    File.Delete(tb_rc4_filenguon.Text);
                    File.Move(tb_rc4_filenguon.Text + ".temp", tb_rc4_filenguon.Text);
                    MessageBox.Show(s, "Notification");
                }
                else
                {
                    if (string.IsNullOrEmpty(tb_rc4_filedich.Text))
                    {
                        MessageBox.Show("You have not selected destination file", "Notification");
                        return;
                    }
                    RC4Crypto8Bit mh = new RC4Crypto8Bit();
                    string s = mh.EncryptAndDecrypt(tb_rc4_filenguon.Text, tb_rc4_filedich.Text, tb_rc4_keyarray.Text, tb_rc4_sarray.Text);
                    MessageBox.Show(s, "Notification");
                }
            }
        }

        private void groupBox11_Enter(object sender, EventArgs e)
        {

        }

        private void label58_Click(object sender, EventArgs e)
        {

        }

        private void groupBox12_Enter(object sender, EventArgs e)
        {

        }

        private void bt_dss_selectfile_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "All File|*.*";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                tb_dss_sourcefile.Text = openFileDialog2.FileName;
            }
        }

        private void tb_dss_openkey_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "File key .txt|*.txt";
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader fi = new StreamReader(openFileDialog1.FileName,Encoding.UTF8);
                tb_dss_p.Text = fi.ReadLine();
                tb_dss_q.Text = fi.ReadLine();
                tb_dss_z.Text = fi.ReadLine();
                tb_dss_g1.Text = fi.ReadLine();
                tb_dss_pk1.Text = fi.ReadLine();
                tb_dss_sk1.Text = fi.ReadLine();
                fi.Close();
            }
        }

        private void tb_dss_savekey_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(tb_dss_z.Text)||string.IsNullOrEmpty(tb_dss_p.Text) || string.IsNullOrEmpty(tb_dss_q.Text) || string.IsNullOrEmpty(tb_dss_pk1.Text) || string.IsNullOrEmpty(tb_dss_g1.Text) || string.IsNullOrEmpty(tb_dss_sk1.Text))
            {
                MessageBox.Show("You have not created a key", "Notification");
                return;
            }
            saveFileDialog1.Filter = "File Key |*.txt";
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter fi = new StreamWriter(saveFileDialog1.FileName);
                fi.WriteLine(tb_dss_p.Text);
                fi.WriteLine(tb_dss_q.Text);
                fi.WriteLine(tb_dss_z.Text);
                fi.WriteLine(tb_dss_g1.Text);
                fi.WriteLine(tb_dss_pk1.Text);
                fi.WriteLine(tb_dss_sk1.Text);
                fi.Close();
                MessageBox.Show("Save key Successfully", "Notification");

            }
        }

        private void bt_dss_selectsig_Click(object sender, EventArgs e)
        {
            openFileDialog3.Filter = "File Signature | *.txt";
            if(openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                StreamReader fi = new StreamReader(openFileDialog3.FileName,Encoding.UTF8);
                tb_dss_sig1.Text = fi.ReadLine();
                tb_dss_sig2.Text = fi.ReadLine();
                fi.Close();
                MessageBox.Show("Read Signature from file Successfully", "Notification");
            }
        }

        private void bt_dss_savesig_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(tb_dss_sig1.Text) || string.IsNullOrEmpty(tb_dss_sig2.Text))
            {
                MessageBox.Show("You have not signed the document", "Notification");
                return;
            }
            saveFileDialog2.Filter = "File Signature | * .txt";
            if(saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                StreamWriter fi = new StreamWriter(saveFileDialog2.FileName);
                fi.WriteLine(tb_dss_sig1.Text);
                fi.WriteLine(tb_dss_sig2.Text);
                fi.Close();
                MessageBox.Show("Save Signature from file successfully", "Notification");
            }
        }

        private void tb_dss_generatekey_Click(object sender, EventArgs e)
        {
            DSSKey k = new DSSKey();
            ArrayList res;
            if (cb_dss_keylength.SelectedIndex == 0) res = k.Key(512);
            else if (cb_dss_keylength.SelectedIndex == 0) res = k.Key(1024);
            else res = k.Key(2048);
            tb_dss_p.Text = res[0].ToString();
            tb_dss_q.Text = res[1].ToString();
            tb_dss_z.Text = res[2].ToString();
            tb_dss_g1.Text = res[3].ToString();
            tb_dss_pk1.Text = res[5].ToString();
            tb_dss_sk1.Text = res[4].ToString();
        }

        private void label45_Click(object sender, EventArgs e)
        {

        }

        private void bt_dss_verifysig_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(tb_dss_sourcefile.Text) || string.IsNullOrEmpty(tb_dss_sig1.Text) || string.IsNullOrEmpty(tb_dss_sig2.Text))
            {
                MessageBox.Show("You have not selected source file or selected signature");
                return;
            }
            DSSSig sig = new DSSSig();
            if(sig.verifysig(tb_dss_sourcefile.Text,BigInteger.Parse(tb_dss_p.Text),BigInteger.Parse(tb_dss_q.Text),BigInteger.Parse(tb_dss_sig1.Text),BigInteger.Parse(tb_dss_sig2.Text),BigInteger.Parse(tb_dss_g1.Text),BigInteger.Parse(tb_dss_pk1.Text) ) == true)
            {
                tb_dss_res.ForeColor = Color.Green;
                tb_dss_res.Text = "Signature Valid";
                MessageBox.Show("Signature Valid", "Notification");
            }
            else
            {
                tb_dss_res.ForeColor = Color.Red;
                tb_dss_res.Text = "Signature Invalid";
                MessageBox.Show("Signature Invalid", "Notification");
            }
        }

        private void bt_dss_sig_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(tb_dss_q.Text) || string.IsNullOrEmpty(tb_dss_p.Text) || string.IsNullOrEmpty(tb_dss_g1.Text) || string.IsNullOrEmpty(tb_dss_pk1.Text))
            {
                MessageBox.Show("You have not created a key or open key from file", "Notification");
                return;
            }
            if(string.IsNullOrEmpty(tb_dss_sourcefile.Text)) {
                MessageBox.Show("You have not selected source file");
                return;
            }
            DSSSig sig = new DSSSig();
            ArrayList res;
            res = sig.sig(tb_dss_sourcefile.Text, BigInteger.Parse(tb_dss_q.Text), BigInteger.Parse(tb_dss_p.Text), BigInteger.Parse(tb_dss_g1.Text), BigInteger.Parse(tb_dss_sk1.Text));
            tb_dss_randomk1.Text = res[0].ToString();
            tb_dss_sig1.Text = res[1].ToString();
            tb_dss_sig2.Text = res[2].ToString();
            tb_dss_res.ForeColor = Color.Green;
            tb_dss_res.Text = "Signature Successfully";
            MessageBox.Show("Signature Successfully", "Notification");
            
        }
    }
}
