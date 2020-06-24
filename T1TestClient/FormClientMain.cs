using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T1TestClient
{
    public partial class FormClientMain : Form
    {
        private const int RESPONSE_SECONDS = 2;

        public TcpClient Client = null;
        public NetworkStream Stream = null;

        public FormClientMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using(StreamWriter sw = new StreamWriter("url.txt"))
                {
                    sw.WriteLine(txtServer.Text.Trim());
                    sw.WriteLine(txtUserID.Text.Trim());
                    sw.WriteLine(txtSessionToken.Text.Trim());
                    sw.WriteLine(txtAmountToSit.Text.Trim());
                    sw.WriteLine(chkAutoRebuy.Checked ? "Y" : "N");
                    sw.WriteLine(chkAutoTopOff.Checked ? "Y" : "N");
                }
            }
            catch
            {
                
            }

            try
            {
                Client = new TcpClient(txtServer.Text.Substring(0, txtServer.Text.IndexOf(":")), int.Parse(txtServer.Text.Substring(txtServer.Text.IndexOf(":") + 1)));
                Stream = Client.GetStream();
            } catch
            {
                WriteLine("Bağlanma girişimi başarısız", Color.Red);
                Client = null;
                Stream = null;

                return;
            }

            try
            {
                using (StreamWriter sw = new StreamWriter("settings.txt"))
                {
                    sw.WriteLine(cmbAutoResponse.SelectedIndex.ToString());
                }
            }
            catch
            {

            }

            WriteLine("Bağlandı! Welcome bilgisi gönderiliyor...", Color.Lime);

            byte[] signature = new byte[42];
            signature[0] = 115;
            signature[1] = 107;
            signature[2] = 84;
            signature[3] = 98;
            signature[4] = 1;
            signature[5] = 4;
            signature[6] = 1;
            signature[7] = 0;
            signature[8] = 0;
            signature[9] = 0;

            for (var i = 10; i < 42; i++)
            {
                signature[i] = 0;
            }

            Stream.Write(signature, 0, 42);

            new Thread(() =>
            {
                int BUFFER_SIZE = 2048;
                byte[] readBuffer = new byte[BUFFER_SIZE];
                readBuffer = Enumerable.Repeat((byte)0, BUFFER_SIZE).ToArray();

                try
                {
                    while (Stream != null && Stream.Read(readBuffer, 0, BUFFER_SIZE) != 0)
                    {
                        string msg = Encoding.ASCII.GetString(readBuffer).Replace("\0", "");
                        string[] messages = msg.Split('\n');

                        foreach (string message in messages)
                        {
                            if(string.IsNullOrEmpty(message))
                            {
                                continue;
                            }

                            ProcessMessage(message);
                        }

                        readBuffer = Enumerable.Repeat((byte)0, BUFFER_SIZE).ToArray();
                    }
                } catch(Exception ex)
                {

                }
            }).Start();
        }

        private void ProcessMessage(string msg)
        {
            if(msg.Contains(" WON"))
            {
                WriteLine("SERVER: " + msg, Color.Lime);
            }
            else if (msg.Contains(" FOLDED"))
            {
                WriteLine("SERVER: " + msg, Color.Silver);
            }
            else if (msg.Contains("NEW TURN"))
            {
                WriteLine("SERVER: " + msg, Color.Aqua);
            }
            else if (msg.Contains("YOUR TURN"))
            {
                WriteLine("SERVER: " + msg, Color.Orange);

                try
                {
                    Console.Invoke((MethodInvoker)delegate
                    {

                        if (cmbAutoResponse.SelectedItem.ToString() == "Call")
                        {
                            for (int i = 0; i < RESPONSE_SECONDS * 10; i++)
                            {
                                Thread.Sleep(100);
                                Application.DoEvents();
                            }

                            WriteToServer("CALL");
                        } else if (cmbAutoResponse.SelectedItem.ToString() == "Random")
                        {
                            for (int i = 0; i < RESPONSE_SECONDS * 10; i++)
                            {
                                Thread.Sleep(100);
                                Application.DoEvents();
                            }

                            switch (Environment.TickCount % 2) //3)
                            {
                                case 0:
                                    WriteToServer("CALL");
                                    break;
                                case 1:
                                    WriteToServer("FOLD");
                                    break;
                            }
                        }
                    });
                }
                catch
                {
                    // Could not display the message
                }
            }
            else
            {
                WriteLine("SERVER: " + msg, Color.Yellow);
            }

            if (msg.StartsWith("Welcome"))
            {
                WriteToServer("IDENTIFY " + txtUserID.Text + " " + txtSessionToken.Text);
            }

            if (msg.StartsWith("KEEP ALIVE "))
            {
                WriteToServer("IM ALIVE " + Environment.TickCount);
            }
        }

        private void WriteLine(String msg, Color color)
        {
            msg = DateTime.Now.ToString("HH:mm:ss :: ") + msg;

            try
            {
                Console.Invoke((MethodInvoker)delegate
                {
                    Console.SelectionColor = color;
                    Console.AppendText(msg + Environment.NewLine);

                    Console.SelectionColor = Color.White;
                    Console.SelectionStart = Console.Text.Length;
                    Console.ScrollToCaret();
                });
            }
            catch
            {
                // Could not display the message
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(Stream == null)
            {
                WriteLine("Bağlı değil", Color.Orange);
                return;
            }

            WriteToServer("SIT " + (chkAutoRebuy.Checked ? "ON" : "OFF") + " " + (chkAutoTopOff.Checked ? "ON" : "OFF") + " " + txtAmountToSit.Text);

            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;

            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("room.txt"))
                {
                    sw.WriteLine(txtRoomID.Text.Trim());
                }
            }
            catch
            {

            }

            if (Stream == null)
            {
                WriteLine("Bağlı değil", Color.Orange);
                return;
            }

            WriteToServer("JOIN TO " + txtRoomID.Text.Trim());
        }

        private void WriteToServer(string v)
        {
            if (Stream == null)
            {
                WriteLine("Bağlı değil", Color.Orange);
                return;
            }

            if(!v.EndsWith("\n"))
            {
                v += "\n";
            }

            byte[] writeBuffer = Encoding.ASCII.GetBytes(v);
            try
            {
                Stream.Write(writeBuffer, 0, writeBuffer.Length);
            } catch
            {
                // do something, like treating target as disconnected
            }

            WriteLine("CLIENT: " + v.Replace("\0", "").Replace("\n", ""), Color.Gray);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                using (StreamReader sr = new StreamReader("url.txt"))
                {
                    txtServer.Text = sr.ReadLine();
                    txtUserID.Text = sr.ReadLine();
                    txtSessionToken.Text = sr.ReadLine();
                    txtAmountToSit.Text = sr.ReadLine();
                    chkAutoRebuy.Checked = sr.ReadLine() == "Y";
                    chkAutoTopOff.Checked = sr.ReadLine() == "Y";
                }
            }
            catch
            {

            }

            try
            {
                using (StreamReader sr = new StreamReader("room.txt"))
                {
                    txtRoomID.Text = sr.ReadLine();
                }
            }
            catch
            {

            }

            try
            {
                cmbAutoResponse.SelectedIndex = 0;

                using (StreamReader sr = new StreamReader("settings.txt"))
                {
                    cmbAutoResponse.SelectedIndex = int.Parse(sr.ReadLine());
                }
            }
            catch
            {

            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Stream.Close();
                Client.Close();
            } catch
            {

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Stream == null)
            {
                WriteLine("Bağlı değil", Color.Orange);
                return;
            }

            WriteToServer("STAND UP");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Stream == null)
            {
                WriteLine("Bağlı değil", Color.Orange);
                return;
            }

            WriteToServer("LEAVE ROOM");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Stream == null)
            {
                WriteLine("Bağlı değil", Color.Orange);
                return;
            }

            WriteToServer("CALL");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (Stream == null)
            {
                WriteLine("Bağlı değil", Color.Orange);
                return;
            }

            WriteToServer("FOLD");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            WriteToServer("ALLIN");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            long amount = 10000;

            try
            {
                amount = long.Parse(txtRaiseAmount.Text.Trim().ToUpper().Replace("K", "000").Replace("M", "000000").Replace("B", "000000000"));
            } catch
            {

            }

            WriteToServer("RAISE TO " + amount);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            OpenFileDialog loadTestScript = new OpenFileDialog();
            loadTestScript.DefaultExt = "*.ts";
            loadTestScript.Filter = "Test Scripts (*.vtsce)|*.vtsce|Any File (*.*)|*.*";
            loadTestScript.Title = "Load Test Script";
            loadTestScript.CheckFileExists = true;
            loadTestScript.CheckPathExists = true;
            loadTestScript.Multiselect = false;

            if (loadTestScript.ShowDialog() == DialogResult.OK)
            {
                ParseTestScript(loadTestScript.FileName);
            }
        }

        private void ParseTestScript(string fileName)
        {
            WriteLine("Parsing test script " + fileName, Color.Green);

            using (StreamReader sr = new StreamReader(fileName))
            {
                string line = sr.ReadLine().ToUpper().Trim();
                while(line != null)
                {
                    if (!line.StartsWith("#"))
                    {
                        WriteToServer(line);
                    }

                    line = sr.ReadLine();
                }
            }

            WriteLine("Test script executed with success", Color.LightGreen);
        }
    }
}
