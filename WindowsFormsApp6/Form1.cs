using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        Socket sck;
        EndPoint eplocal, epRemote;
        byte[] buffer;
        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                eplocal = new IPEndPoint(IPAddress.Parse(localIP.Text), Convert.ToInt32(localPort.Text));
                sck.Bind(eplocal);

                epRemote = new IPEndPoint(IPAddress.Parse(remoteIP.Text), Convert.ToInt32(remotePort.Text));
                sck.Connect(epRemote);
                if(sck.Connected)
                {
                    
                    button1.Text = "Connected";
                    button1.Enabled = false;
                }
                buffer = new byte[1500];
                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                MessageBox.Show("Connected to port  " + remotePort.Text + "  Enjoy!!!!");
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sck.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            localIP.Text = GetlocalIP();
            remoteIP.Text = GetlocalIP();
        }

        private string GetlocalIP()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            }
            return "127.0.0.1";
        }

        private void send_Click(object sender, EventArgs e)
        {
            if (messagetext.Text != "")
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] send = new byte[1500];
                //send = encoding.GetBytes(messagetext.Text);

                list.Items.Add("Me :  " + messagetext.Text);
                string str;
                str = encryptetext();
                send = encoding.GetBytes(str);
                sck.Send(send);
                if (list.Items.Count > 0)
                {
                    list.TopIndex = list.Items.Count - 1;
                }
                messagetext.Text = "";
            }
        }

        private void messagetext_TextChanged(object sender, EventArgs e)
        {

        }

        private void messagetext_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (messagetext.Text != "")
                {
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    byte[] send = new byte[1500];
                    //send = encoding.GetBytes(messagetext.Text);

                    
                    list.Items.Add("Me :  " + messagetext.Text);
                    string str;                    
                    str = encryptetext();
                    send = encoding.GetBytes(str);
                    sck.Send(send);
                    if (list.Items.Count > 0)
                    {
                        list.TopIndex = list.Items.Count - 1;
                    }
                    messagetext.Text = "";
                }
            }
        }

        private void MessageCallBack(IAsyncResult aResult)
        {
            try
            {
                byte[] recievedData = new byte[1500];
                recievedData = (byte[])aResult.AsyncState;

                ASCIIEncoding encoding = new ASCIIEncoding();
                string receivedMessage = encoding.GetString(recievedData);
                string str;
                str=Decryptetext(receivedMessage);
                list.Items.Add("Friend : " + str);
                if (list.Items.Count > 0)
                {
                    list.TopIndex = list.Items.Count - 1;
                }
                buffer = new byte[1500];
                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //ash algorithm
        private string encryptetext()
        {
            string s =messagetext.Text.ToString();
            char[] ch = s.ToCharArray();

            for (int i = 0; i < ch.Length; i++)
            {
                switch (ch[i])
                {
                    //blank space
                    case ' ': ch[i] = '!'; break;
                    //lower case
                    case 'a': ch[i] = '1'; break;
                    case 'b': ch[i] = 'R'; break;
                    case 'c': ch[i] = 't'; break;
                    case 'd': ch[i] = 'Z'; break;
                    case 'e': ch[i] = '7'; break;
                    case 'f': ch[i] = 'w'; break;
                    case 'g': ch[i] = 'l'; break;
                    case 'h': ch[i] = 'u'; break;
                    case 'i': ch[i] = 'D'; break;
                    case 'j': ch[i] = 'P'; break;
                    case 'k': ch[i] = 'V'; break;
                    case 'l': ch[i] = '4'; break;
                    case 'm': ch[i] = 'Q'; break;
                    case 'n': ch[i] = 'T'; break;
                    case 'o': ch[i] = 'F'; break;
                    case 'p': ch[i] = 'M'; break;
                    case 'q': ch[i] = 'G'; break;
                    case 'r': ch[i] = 'X'; break;
                    case 's': ch[i] = 'I'; break;
                    case 't': ch[i] = 'k'; break;
                    case 'u': ch[i] = 'H'; break;
                    case 'v': ch[i] = 'r'; break;
                    case 'w': ch[i] = 's'; break;
                    case 'x': ch[i] = 'U'; break;
                    case 'y': ch[i] = 'v'; break;
                    case 'z': ch[i] = 'm'; break;

                    //upper case

                    case 'A': ch[i] = 'n'; break;
                    case 'B': ch[i] = 'e'; break;
                    case 'C': ch[i] = 'i'; break;
                    case 'D': ch[i] = '6'; break;
                    case 'E': ch[i] = 'A'; break;
                    case 'F': ch[i] = '5'; break;
                    case 'G': ch[i] = 'N'; break;
                    case 'H': ch[i] = 'E'; break;
                    case 'I': ch[i] = 'c'; break;
                    case 'J': ch[i] = 'Y'; break;
                    case 'K': ch[i] = '3'; break;
                    case 'L': ch[i] = 'J'; break;
                    case 'M': ch[i] = 'j'; break;
                    case 'N': ch[i] = 'W'; break;
                    case 'O': ch[i] = 'B'; break;
                    case 'P': ch[i] = 'a'; break;
                    case 'Q': ch[i] = 'o'; break;
                    case 'R': ch[i] = 'Z'; break;
                    case 'S': ch[i] = '0'; break;
                    case 'T': ch[i] = 'C'; break;
                    case 'U': ch[i] = 'L'; break;
                    case 'V': ch[i] = 'g'; break;
                    case 'W': ch[i] = 'b'; break;
                    case 'X': ch[i] = '9'; break;
                    case 'Y': ch[i] = '8'; break;
                    case 'Z': ch[i] = '2'; break;

                    //Numeric digits

                    case '0': ch[i] = 'x'; break;
                    case '1': ch[i] = 'q'; break;
                    case '2': ch[i] = 'k'; break;
                    case '3': ch[i] = 'S'; break;
                    case '4': ch[i] = 'y'; break;
                    case '5': ch[i] = 'f'; break;
                    case '6': ch[i] = 'd'; break;
                    case '7': ch[i] = 'h'; break;
                    case '8': ch[i] = 'O'; break;
                    case '9': ch[i] = 'p'; break;

                }
            }

            string st = new string(ch);
            return st.ToString();
        }

        private string Decryptetext(string s)
        {
           
            char[] ch = s.ToCharArray();

            for (int i = 0; i < ch.Length; i++)
            {
                switch (ch[i])
                {
                    //blank space
                    case '!': ch[i] = ' '; break;
                    //lower case
                    case '1': ch[i] = 'a'; break;
                    case 'R': ch[i] = 'b'; break;
                    case 't': ch[i] = 'c'; break;
                    case 'Z': ch[i] = 'd'; break;
                    case '7': ch[i] = 'e'; break;
                    case 'w': ch[i] = 'f'; break;
                    case 'l': ch[i] = 'g'; break;
                    case 'u': ch[i] = 'h'; break;
                    case 'D': ch[i] = 'i'; break;
                    case 'P': ch[i] = 'j'; break;
                    case 'V': ch[i] = 'k'; break;
                    case '4': ch[i] = 'l'; break;
                    case 'Q': ch[i] = 'm'; break;
                    case 'T': ch[i] = 'n'; break;
                    case 'F': ch[i] = 'o'; break;
                    case 'M': ch[i] = 'p'; break;
                    case 'G': ch[i] = 'q'; break;
                    case 'X': ch[i] = 'r'; break;
                    case 'I': ch[i] = 's'; break;
                    case 'K': ch[i] = 't'; break;
                    case 'H': ch[i] = 'u'; break;
                    case 'r': ch[i] = 'v'; break;
                    case 's': ch[i] = 'w'; break;
                    case 'U': ch[i] = 'x'; break;
                    case 'v': ch[i] = 'y'; break;
                    case 'm': ch[i] = 'z'; break;

                    //upper case

                    case 'n': ch[i] = 'A'; break;
                    case 'e': ch[i] = 'B'; break;
                    case 'i': ch[i] = 'C'; break;
                    case '6': ch[i] = 'D'; break;
                    case 'A': ch[i] = 'E'; break;
                    case '5': ch[i] = 'F'; break;
                    case 'N': ch[i] = 'G'; break;
                    case 'E': ch[i] = 'H'; break;
                    case 'c': ch[i] = 'I'; break;
                    case 'Y': ch[i] = 'J'; break;
                    case '3': ch[i] = 'K'; break;
                    case 'J': ch[i] = 'L'; break;
                    case 'j': ch[i] = 'M'; break;
                    case 'W': ch[i] = 'N'; break;
                    case 'B': ch[i] = 'O'; break;
                    case 'a': ch[i] = 'P'; break;
                    case 'o': ch[i] = 'Q'; break;
                    case 'z': ch[i] = 'R'; break;
                    case '0': ch[i] = 'S'; break;
                    case 'C': ch[i] = 'T'; break;
                    case 'L': ch[i] = 'U'; break;
                    case 'g': ch[i] = 'V'; break;
                    case 'b': ch[i] = 'W'; break;
                    case '9': ch[i] = 'X'; break;
                    case '8': ch[i] = 'Y'; break;
                    case '2': ch[i] = 'Z'; break;

                    //Numeric digits

                    case 'x': ch[i] = '0'; break;
                    case 'q': ch[i] = '1'; break;
                    case 'k': ch[i] = '2'; break;
                    case 'S': ch[i] = '3'; break;
                    case 'y': ch[i] = '4'; break;
                    case 'f': ch[i] = '5'; break;
                    case 'd': ch[i] = '6'; break;
                    case 'h': ch[i] = '7'; break;
                    case 'O': ch[i] = '8'; break;
                    case 'p': ch[i] = '9'; break;

                }
            }
            string st = new string(ch);
            return st.ToString();
        }
    }
}
