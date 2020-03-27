using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;

namespace ChatApplication
{
    public partial class Form1 : Form
    {
        //HillCipher m_objCipher;

        TcpClient m_client;
        StreamReader m_sr;
        StreamWriter m_sw;
        string m_strRecieve;
        string m_strSendingText;

        public Form1()
        {
            InitializeComponent();

            IPAddress[] local_Ips = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress loop_Ips in local_Ips)
            {
                if(loop_Ips.AddressFamily == AddressFamily.InterNetwork)
                {
                    tb_Server_Ip.Text = loop_Ips.ToString();
                }
            }

           // m_objCipher = new HillCipher();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Start()
        {
            int serverPort = int.Parse(tb_Server_Port.Text);
            TcpListener listener = new TcpListener(IPAddress.Any, serverPort);

            listener.Start();
            m_client = listener.AcceptTcpClient();
            m_sr = new StreamReader(m_client.GetStream());
            m_sw = new StreamWriter(m_client.GetStream());

            m_sw.AutoFlush = true;
            backgroundWorker1.RunWorkerAsync();
            backgroundWorker2.WorkerSupportsCancellation = true;

        }
        private void Connect()
        {
            m_client = new TcpClient();
            int Port = int.Parse(tb_Local_Port.Text);
            IPEndPoint IpEP = new IPEndPoint(IPAddress.Parse(tb_Local_Ip.Text), Port);
        
            try
            {
                m_client.Connect(IpEP);
                if(m_client.Connected)
                {
                    lb_ChatWindow.Items.Add("Connected to Server");
                    m_sr = new StreamReader(m_client.GetStream());
                    m_sw = new StreamWriter(m_client.GetStream());
                    m_sw.AutoFlush = true;
                    backgroundWorker1.RunWorkerAsync();
                    backgroundWorker2.WorkerSupportsCancellation = true;

                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }
         private void Send()
        {
            if(rtb_Message.Text != "")
            {
                m_strSendingText = Encrypt(rtb_Message.Text.Replace(' ','{'));
               // m_strSendingText = rtb_Message.Text;
                
                backgroundWorker2.RunWorkerAsync();
            }
            //rtb_Message.Text = "";

        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (m_client.Connected)
            {
            
            
                try
                {
                    m_strRecieve = m_sr.ReadLine();
                    this.lb_ChatWindow.Invoke(new MethodInvoker(delegate()
                        {
                            
                            lb_ChatWindow.Items.Add("You:" + Decrypt(m_strRecieve).Replace('{',' '));
                            
                        }                        
                        ));
                    m_strRecieve = "";
                        
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if(m_client.Connected)
            {
                try
                {
                    m_sw.WriteLine(m_strSendingText);

                    this.lb_ChatWindow.Invoke(new MethodInvoker(delegate()
                    {
                        lb_ChatWindow.Items.Add("Me:" + rtb_Message.Text);
                        rtb_Message.Text = "";           }
                        ));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
                MessageBox.Show("Sending Failed");

            backgroundWorker2.CancelAsync();
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            Start();
            
        }
        
        string cip;
        private void btn_Send_Click(object sender, EventArgs e)
        {
            Send();
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            
            Connect();


         //   string d = Decrypt(cip);
         //   MessageBox.Show(d);     
            
        }

        //List<HillCipher> ciphers = new List<HillCipher>();
        //qagufmcmghth
        private void RemoveSpaces()
        {

        }
        public string Encrypt(string p_strMsg)
        {

            string str = p_strMsg;
            string cipher = "";

            while (str.Length % 3 != 0)
            {
                str += "{";
            }

          //  ciphers.Clear();
            for (int i = 0; i < str.Length; i++)
            {
                if (((i) % 3) == 0)
                {
                    HillCipher m_objCipher = new HillCipher();
                    char[] ch = new char[3];
                    ch = str.ToCharArray(i, 3);
                    string ThreeCharString = new string(ch);
                    m_objCipher.SetString(ThreeCharString);
//                    cipher += m_objCipher.encryption();
            //        ciphers.Add(m_objCipher);
                    cipher += m_objCipher.encryption();
                    //m_objCipher.de(en);
                   // MessageBox.Show("Original = " + ThreeCharString + "\n" + "Cipher = " +en + "\n" + "Decrypted = ");
                }
            }
            return cipher;
        }
        public string Decrypt(string p_strMsg)
        {
            string str = p_strMsg;
            string Text = "";

            while (str.Length % 3 != 0)
            {
                str += " ";
            }

         //   int index = 0;
            for (int i = 0; i < str.Length; i++)
            {
                
                if (((i) % 3) == 0)
                {
                    //HillCipher m_objCipher = ciphers[index++];
                    HillCipher m_objCipher = new HillCipher();
                    char[] ch = new char[3];
                    
                    ch = str.ToCharArray(i, 3);
                    string ThreeCharString = new string(ch);
                    
                    //Text += m_objCipher.decryption(ThreeCharString);
                    //                    MessageBox.Show("Original = " + ThreeCharString + "\n" + "Cipher = " + m_objCipher.encryption() + "\n" + "Decrypted = " + m_objCipher.decryption());
                    Text += m_objCipher.de(ThreeCharString);
                }
            }
            return Text;
        }
    }
}
