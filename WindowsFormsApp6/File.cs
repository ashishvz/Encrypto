using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;
using System.Threading;

namespace WindowsFormsApp6
{
    public partial class File : Form
    {
        [DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr Destination, int Length);
        public File()
        {
            InitializeComponent();
        }

        private void File_Load(object sender, EventArgs e)
        {
            pictureBox4.Image = Image.FromFile("C:\\Users\\ashis\\Desktop\\files\\data_encryption.gif");
            pictureBox4.Visible = false;
        }

        private void btnfile_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.Title = "File Encryption and Decryption";
            dlg.InitialDirectory = @"c:\";
            dlg.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            dlg.FilterIndex = 2;
            dlg.RestoreDirectory = true;
            // Nullable<bool> result = dlg.ShowDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string filename = dlg.FileName;
                txtbrowse.Text = filename;
            }
        }

        private void btnencrypt_Click(object sender, EventArgs e)
        {
            string password = txtdepass.Text.ToString();
            if (txtbrowse.Text != "")
            {
                if (txtenpass.Text != "")
                {
                    GCHandle gch = GCHandle.Alloc(password, GCHandleType.Pinned);
                    FileEncrypt(@txtbrowse.Text, password);
                    pictureBox4.Visible = true;
                    ZeroMemory(gch.AddrOfPinnedObject(), password.Length * 2);
                    gch.Free();
                    MessageBox.Show("File Encrypted !");
                    pictureBox4.Visible = false;
                }
                else
                {
                    MessageBox.Show("Please enter the password!");
                }
            }
            else
            {
                MessageBox.Show("Choose a file");
            }
        }

        private void FileEncrypt(string inputFile, string password)
        {
            try
            {
                byte[] salt = GenerateRandomSalt();

                FileStream fsCrypt = new FileStream(inputFile + ".aes", FileMode.Create);
                byte[] passwordbyte = System.Text.Encoding.UTF8.GetBytes(password);
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.Zeros;

                var key = new Rfc2898DeriveBytes(passwordbyte, salt, 50000);
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);

                aes.Mode = CipherMode.CFB;
                fsCrypt.Write(salt, 0, salt.Length);

                CryptoStream cs = new CryptoStream(fsCrypt, aes.CreateEncryptor(), CryptoStreamMode.Write);
                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                byte[] buffer = new byte[1048576];
                int read;

                try
                {
                    while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        this.Invoke(new Action(delegate { }));
                        cs.Write(buffer, 0, read);
                    }
                    fsIn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error : " + ex.Message);
                }
                finally
                {
                    cs.Close();
                    fsCrypt.Close();
                }

                string output = inputFile + ".aes";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btndecrypt_Click(object sender, EventArgs e)
        {
            string password = txtdepass.Text.ToString();
            if (txtbrowse.Text != "")
            {
                if (txtdepass.Text != "")
                {
                    GCHandle gch = GCHandle.Alloc(password, GCHandleType.Pinned);
                    string path =Path.GetFileNameWithoutExtension(txtbrowse.Text.ToString());
                    string dir = Path.GetDirectoryName(txtbrowse.Text.ToString());
                    string outPath = String.Concat(dir+"\\", path);
                    MessageBox.Show(outPath);
                    FileDecrypt(txtbrowse.Text ,outPath , password);
                    ZeroMemory(gch.AddrOfPinnedObject(), password.Length * 2);
                    gch.Free();
                }
                else
                {
                    MessageBox.Show("Please enter the password!");
                }
            }
            else
            {
                MessageBox.Show("Choose a file");
            }

        }

        private void FileDecrypt(string inputFile, string outputFile, string password)
        {

            try
            {
                try
                {
                    byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
                    byte[] salt = new byte[32];

                    FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
                    fsCrypt.Read(salt, 0, salt.Length);

                    RijndaelManaged AES = new RijndaelManaged();
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Padding = PaddingMode.Zeros;
                    AES.Mode = CipherMode.CFB;

                    CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);

                    FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                    int read;
                    byte[] buffer = new byte[1048576];

                    try
                    {
                        while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            this.Invoke(new Action(delegate { }));
                            fsOut.Write(buffer, 0, read);
                        }
                    }
                    catch (CryptographicException ex_CryptographicException)
                    {
                        MessageBox.Show("CryptographicException error: " + ex_CryptographicException.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }

                    try
                    {
                        cs.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error by closing CryptoStream: " + ex.Message);
                    }
                    finally
                    {
                        fsOut.Close();
                        fsCrypt.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                MessageBox.Show("File Decrypted");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }



    


    

        public static byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[32];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    rng.GetBytes(data);
                }
            }

            return data;
        }

    }
}

