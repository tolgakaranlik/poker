#define _HARDCODED_TEST_OFF

// Available levels too low!

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Script.Serialization;

namespace T1GameRoomServer
{
    public partial class FormMain : Form
    {
        public string Version = "1";
        public bool CanLog = true;
        public int Port = 3431;
        public int PlayerCapacity = 1000;
        public int TableCapacity = 100;
        public int CasinoType = 0;
        public int CasinoId = 1;
        public string Host = "http://178.157.10.75:8090/velo/";
        public decimal TaxPercent = 0.98m;

        // Use with caution! / Dikkatli kullanınız!
        public bool TestMode = false;
        // Use with caution! / Dikkatli kullanınız!
        // ============================================
        // Value of this variable must NEVER be true in production systems!
        // Bu değişkenin değeri canlı sistemlerde ASLA true olmamalıdır!

        private bool CardsHandledForTest = false;
        private bool TestScriptReadMode = false;
        private List<string> TestInstructions;
        private List<string> GameInstructions;
        private List<int> TestPlayers;
        private Thread TestThread;
        private string ExpectedResult;
        private string TestTable = null;

        private bool TestExecution = false;
        private int OccupiedThreads;
        private int OccupiedTables;
        private Thread[] ThreadList;
        private ConnectionInfo[] Connections;
        private bool CanContinue;
        private TableInfo[] TableList;
        private Thread MainThread = null;
        private TcpClient MainClient;
        private TcpListener MainListener;
        private CardInfo[] CardDeckTidy = null;

        public class WinningHand
        {
            private int playerIndex;
            private string handName;
            private string cardOrder;

            public int PlayerIndex
            {
                get
                {
                    return playerIndex;
                }
            }

            public string HandName
            {
                get
                {
                    return handName;
                }
            }

            public string CardOrder
            {
                get
                {
                    return cardOrder;
                }
            }

            public WinningHand(int playerIndex, string handName, string cardOrder)
            {
                this.playerIndex = playerIndex;
                this.cardOrder = cardOrder;
                this.handName = handName;
            }
        }

        public class TestResponseBufferItem
        {
            int id;
            DateTime timeOfProcess;
            string message;

            public int Id
            {
                get
                {
                    return id;
                }
            }

            public DateTime TimeOfProcess
            {
                get
                {
                    return timeOfProcess;
                }
            }

            public string Message
            {
                get
                {
                    return message;
                }
            }

            public TestResponseBufferItem(int id, DateTime timeOfProcess, string message)
            {
                this.id = id;
                this.timeOfProcess = timeOfProcess;
                this.message = message;
            }
        }

        volatile public List<TestResponseBufferItem> TestResponseBuffer = null;

        public enum TurnResult { Immediate, NextTurn, NextPlayer };

        public FormMain()
        {
            InitializeComponent();
        }

        private int[] LevelIntervals = new int[] { 1000, 2200, 3600, 5200, 7000, 9000, 11200, 13600, 16200, 19000, 22000, 25200, 28600, 32200, 36000, 40000, 44200, 48600, 53200, 58000, 63000, 68200, 73600, 79200, 85000, 91000, 97200, 103600, 110200, 117000, 124000, 131200, 138600, 146200, 154000, 162000, 170200, 178600, 187200, 196000, 205000, 214200, 223600, 233200, 243000, 253000, 263200, 273600, 284200, 295000, 306000, 317200, 328600, 340200, 352000, 364000, 376200, 388600, 401200, 414000, 427000, 440200, 453600, 467200, 481000, 495000, 509200, 523600, 538200, 553000, 568000, 583200, 598600, 614200, 630000, 646000, 662200, 678600, 695200, 712000, 729000, 746200, 763600, 781200, 799000, 817000, 835200, 853600, 872200, 891000, 910000, 929200, 948600, 968200, 988000, 1008000, 1028200, 1048600, 1069200, 1090000, 1111000, 1132200, 1153600, 1175200, 1197000, 1219000, 1241200, 1263600, 1286200, 1309000, 1332000, 1355200, 1378600, 1402200, 1426000, 1450000, 1474200, 1498600, 1523200, 1548000, 1573000, 1598200, 1623600, 1649200, 1675000, 1701000, 1727200, 1753600, 1780200, 1807000, 1834000, 1861200, 1888600, 1916200, 1944000, 1972000, 2000200, 2028600, 2057200, 2086000, 2115000, 2144200, 2173600, 2203200, 2233000, 2263000, 2293200, 2323600, 2354200, 2385000, 2416000, 2447200, 2478600, 2510200, 2542000, 2574000, 2606200, 2638600, 2671200, 2704000, 2737000, 2770200, 2803600, 2837200, 2871000, 2905000, 2939200, 2973600, 3008200, 3043000, 3078000, 3113200, 3148600, 3184200, 3220000, 3256000, 3292200, 3328600, 3365200, 3402000, 3439000, 3476200, 3513600, 3551200, 3589000, 3627000, 3665200, 3703600, 3742200, 3781000, 3820000, 3859200, 3898600, 3938200, 3978000, 4018000, 4058200, 4098600, 4139200, 4180000, 4221000, 4262200, 4303600, 4345200, 4387000, 4429000, 4471200, 4513600, 4556200, 4599000, 4642000, 4685200, 4728600, 4772200, 4816000, 4860000, 4904200, 4948600, 4993200, 5038000, 5083000, 5128200, 5173600, 5219200, 5265000, 5311000, 5357200, 5403600, 5450200, 5497000, 5544000, 5591200, 5638600, 5686200, 5734000, 5782000, 5830200, 5878600, 5927200, 5976000, 6025000, 6074200, 6123600, 6173200, 6223000, 6273000, 6323200, 6373600, 6424200, 6475000, 6526000, 6577200, 6628600, 6680200, 6732000, 6784000, 6836200, 6888600, 6941200, 6994000, 7047000, 7100200, 7153600, 7207200, 7261000, 7315000, 7369200, 7423600, 7478200, 7533000, 7588000, 7643200, 7698600, 7754200, 7810000, 7866000, 7922200, 7978600, 8035200, 8092000, 8149000, 8206200, 8263600, 8321200, 8379000, 8437000, 8495200, 8553600, 8612200, 8671000, 8730000, 8789200, 8848600, 8908200, 8968000, 9028000, 9088200, 9148600, 9209200, 9270000, 9331000, 9392200, 9453600, 9515200, 9577000, 9639000, 9701200, 9763600, 9826200, 9889000, 9952000, 10015200, 10078600, 10142200, 10206000, 10270000, 10334200, 10398600, 10463200, 10528000, 10593000, 10658200, 10723600, 10789200, 10855000, 10921000, 10987200, 11053600, 11120200, 11187000, 11254000, 11321200, 11388600, 11456200, 11524000, 11592000, 11660200, 11728600, 11797200, 11866000, 11935000, 12004200, 12073600, 12143200, 12213000, 12283000, 12353200, 12423600, 12494200, 12565000, 12636000, 12707200, 12778600, 12850200, 12922000, 12994000, 13066200, 13138600, 13211200, 13284000, 13357000, 13430200, 13503600, 13577200, 13651000, 13725000, 13799200, 13873600, 13948200, 14023000, 14098000, 14173200, 14248600, 14324200, 14400000, 14476000, 14552200, 14628600, 14705200, 14782000, 14859000, 14936200, 15013600, 15091200, 15169000, 15247000, 15325200, 15403600, 15482200, 15561000, 15640000, 15719200, 15798600, 15878200, 15958000, 16038000, 16118200, 16198600, 16279200, 16360000, 16441000, 16522200, 16603600, 16685200, 16767000, 16849000, 16931200, 17013600, 17096200, 17179000, 17262000, 17345200, 17428600, 17512200, 17596000, 17680000, 17764200, 17848600, 17933200, 18018000, 18103000, 18188200, 18273600, 18359200, 18445000, 18531000, 18617200, 18703600, 18790200, 18877000, 18964000, 19051200, 19138600, 19226200, 19314000, 19402000, 19490200, 19578600, 19667200, 19756000, 19845000, 19934200, 20023600, 20113200, 20203000, 20293000, 20383200, 20473600, 20564200, 20655000, 20746000, 20837200, 20928600, 21020200, 21112000, 21204000, 21296200, 21388600, 21481200, 21574000, 21667000, 21760200, 21853600, 21947200, 22041000, 22135000, 22229200, 22323600, 22418200, 22513000, 22608000, 22703200, 22798600, 22894200, 22990000, 23086000, 23182200, 23278600, 23375200, 23472000, 23569000, 23666200, 23763600, 23861200, 23959000, 24057000, 24155200, 24253600, 24352200, 24451000, 24550000, 24649200, 24748600, 24848200, 24948000, 25048000, 25148200, 25248600, 25349200, 25450000 };

        private static readonly object _syncObject = new object();
        private static readonly object _rawObject = new object();

        private void Log(string msg)
        {
            if (!CanLog)
            {
                return;
            }

            lock (_syncObject)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter("smlobbylog.txt", true))
                    {
                        sw.WriteLine(msg);
                        sw.Close();
                    }
                }
                catch
                {

                }
            }
        }

        private void LogRaw(int id, string msg)
        {
            if(Connections.Length <= id || Connections[id] == null || Connections[id].UserID == "")
            {
                return;
            }

            new Thread(() =>
            {
                lock (_rawObject)
                {
                    try
                    {
                        if(!Directory.Exists("rawlog/" + DateTime.Now.ToString("dd.MM.yyyy")))
                        {
                            Directory.CreateDirectory("rawlog/" + DateTime.Now.ToString("dd.MM.yyyy"));
                        }

                        using (StreamWriter sw = new StreamWriter("rawlog/" + DateTime.Now.ToString("dd.MM.yyyy") + "/" + Connections[id].UserID + ".txt", true))
                        {
                            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " :: " + msg);
                            sw.Close();
                        }
                    }
                    catch
                    {

                    }
                }
            }).Start();
        }

        private void Start()
        {
            WriteLine("SKTB Smart Lobby v" + Version);
            WriteLine("Casino ID: " + CasinoId, Color.Gray);
            WriteLine("Host: " + Host, Color.Silver);
            WriteLine("Port: " + Port, Color.Gray);
            WriteLine("Oda bilgileri okunuyor...", Color.Olive);

            if(TestMode)
            {
                TestResponseBuffer = new List<TestResponseBufferItem>();

                WriteLine("\n\n!! TEST MODE !!", Color.Red);
                WriteLine("CAUTION!! NEVER use test mode for live production servers", Color.Pink);
                WriteLine("DİKKAT!! Test modunu ASLA canlı sunucu sistemlerinde kullanmayınız\n", Color.Pink);
            }

            #if _HARDCODED_TEST
            WriteLine("\n\n!! HARDCODED TEST MODE !!", Color.Red);
            WriteLine("CAUTION!! NEVER use test mode for live production servers", Color.Pink);
            WriteLine("DİKKAT!! Test modunu ASLA canlı sunucu sistemlerinde kullanmayınız\n", Color.Pink);
            #endif

            ThreadList = new Thread[PlayerCapacity];
            Connections = new ConnectionInfo[PlayerCapacity];
            TableList = new TableInfo[TableCapacity];
            OccupiedThreads = 0;
            OccupiedTables = 0;
            CanContinue = true;
            MainListener = null;

            InitLogDirectory();
            ZeroTables();

            try
            {
                FetchRoomInfo();
            } catch(Exception ex)
            {
                WriteLine("Oda bilgileri okunamadı. Sebep: " + ex.Message);
                WriteLine("Sonlandırılıyor...", Color.Maroon);

                Close();
                return;
            }

            WriteLine("Sunucu portu açılıyor...", Color.Silver);

            try
            {
                bool interfaceFound = false;
                foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                WriteLine("Şu IP deneniyor: " + ip.Address.ToString(), Color.Silver);

                                try
                                {
                                    MainListener = new TcpListener(ip.Address, Port);
                                    MainListener.Start();

                                    WriteLine("Başlanılan IP: " + ip.Address.ToString(), Color.Lime);
                                    interfaceFound = true;
                                    break;
                                }
                                catch
                                {
                                    WriteLine("Şu IP için port açılamadı: " + ip.Address.ToString(), Color.Gray);
                                }
                            }
                        }
                    }

                    if(interfaceFound)
                    {
                        break;
                    }
                }

                if (interfaceFound)
                {
                    WriteLine("Sunucu portu başarıyla açıldı", Color.Green);
                } else
                {
                    WriteLine("Sunucu portu başarıyla açılamadı X(", Color.Red);
                    Application.DoEvents();

                    Thread.Sleep(5000);
                    Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                WriteLine("Sunucu portu açılamadı, işlem devam edemiyor (Sebep: " + ex.Message + ")", Color.Red);
                Application.DoEvents();

                Thread.Sleep(5000);
                return;
            }

            WriteLine("Bağlanmak isteyen istemciler bekleniyor...", Color.Gray);
            MainThread = new Thread(() =>
            {
                while (CanContinue)
                {
                    try
                    {
                        Byte[] buffer = new Byte[1024];
                        TcpClient mainClient = MainListener.AcceptTcpClient();
                        int connectionIndex = OccupiedThreads;

                        // Allocate a previously used socket or allocagte a new one?
                        bool emptyFound = false;
                        for (int i = 0; i < Connections.Length; i++)
                        {
                            if(Connections[i] == null || !Connections[i].Active)
                            {
                                // Is it in disactivation period?
                                emptyFound = true;
                                connectionIndex = i;

                                break;
                            }
                        }

                        if(!emptyFound)
                        {
                            OccupiedThreads++;
                        }

                        Connections[connectionIndex] = new ConnectionInfo();
                        Connections[connectionIndex].Index = connectionIndex;
                        Connections[connectionIndex].IPAddress = ((IPEndPoint)mainClient.Client.RemoteEndPoint).Address.ToString();
                        Connections[connectionIndex].Active = true;
                        Connections[connectionIndex].Level = 1;
                        Connections[connectionIndex].XPToLevel = LevelIntervals.Length > Connections[connectionIndex].Level - 1 ? LevelIntervals[Connections[connectionIndex].Level - 1] : LevelIntervals[LevelIntervals.Length - 1];

                        WriteLine("Bir istemci bağlandı (" + Connections[connectionIndex].IPAddress + ")", Color.Lime);
                        ThreadList[connectionIndex] = new Thread((threadId) =>
                        {
                            int id = (int)threadId;

                            while (CanContinue && Connections[id] != null && Connections[id].Active)
                            {
                                int dataLength;

                                try
                                {
                                    if(mainClient == null || !mainClient.Connected || mainClient.GetStream() == null)
                                    {
                                        WriteLine("İstemci " + id + " ("+ Connections[id].FullName +") bağlantıyı kaybetti", Color.Orange);
                                        break;
                                    }

                                    Connections[id].Stream = mainClient.GetStream();
                                    Connections[id].Client = mainClient;

                                    try
                                    {
                                        while (Connections[id] != null && Connections[id].Stream != null && Connections[id].Active && Connections[id].Client != null && Connections[id].Client.Connected && (dataLength = Connections[id].Stream.Read(buffer, 0, buffer.Length)) != 0)
                                        {
                                            ProcessBuffer(id, buffer, dataLength);
                                            ClearBuffer(buffer, buffer.Length);
                                        }
                                    } catch(Exception ex)
                                    {
                                        WriteLine("Bir bağlantı problemi meydana geldi: " + ex.Message + " :: " + ex.StackTrace, Color.Gray);
                                    }

                                    if (Connections[id].Active)
                                    {
                                        if (Connections[id].SittingAt != -1)
                                        {
                                            // Player is sitting in a table
                                            WriteLine("Oyuncu pasif moda alınıyor: " + id);
                                            Connections[id].State = ConnectionInfo.ConnectionState.DisconnectPeriod;
                                            //Connections[id].HandsMissed = 1;
                                        }
                                        else
                                        {
                                            // Player is not sitting on any tables
                                            WriteLine("Oyuncunun bağlantısı kesiliyor: " + id);
                                            int playerTable = FindTableFromId(Connections[id].TableId);
                                            if (playerTable != -1)
                                            {
                                                // Delete from connected players
                                                int playerPosition = -1;
                                                for (int i = 0; i < TableList[playerTable].NumConnected; i++)
                                                {
                                                    if (TableList[playerTable].PlayersConnected[i] == id)
                                                    {
                                                        playerPosition = i;
                                                        break;
                                                    }
                                                }

                                                if (playerPosition != -1)
                                                {
                                                    for (int i = playerPosition; i < TableList[playerTable].NumConnected - 1; i++)
                                                    {
                                                        TableList[playerTable].PlayersConnected[i] = TableList[playerTable].PlayersConnected[i + 1];
                                                    }

                                                    TableList[playerTable].NumConnected--;
                                                }

                                                // Delete from sitQue
                                                playerPosition = -1;
                                                for (int i = 0; i < TableList[playerTable].NumSitQue; i++)
                                                {
                                                    if (TableList[playerTable].PlayersSitQue[i] == id)
                                                    {
                                                        playerPosition = i;
                                                        break;
                                                    }
                                                }

                                                if (playerPosition != -1)
                                                {
                                                    for (int i = playerPosition; i < TableList[playerTable].NumSitQue - 1; i++)
                                                    {
                                                        TableList[playerTable].PlayersSitQue[i] = TableList[playerTable].PlayersSitQue[i + 1];
                                                    }

                                                    TableList[playerTable].NumSitQue--;
                                                }

                                                // update server
                                                using (WebClient wc = new WebClient())
                                                {
                                                    wc.Headers.Add("User-Agent", "Gravity 0.1"); // PostmanRuntime/7.19.0
                                                    string data = wc.DownloadString(Host + "srv_user_exit_room.php?user_id=" + Connections[id].UserID + "&token=" + Connections[id].SessionToken + "&room_id=" + Connections[id].TableId);
                                                }
                                            }

                                            Connections[id].State = ConnectionInfo.ConnectionState.HandShake;
                                        }
                                    } else
                                    {
                                        WriteLine("Oyuncunun bağlantısı kapatılıyor: " + id);
                                        PlayerLeave(id);
                                    }

                                    try
                                    {
                                        Connections[id].Stream.Close();
                                    } catch
                                    {

                                    }

                                    try
                                    {
                                        Connections[id].Client.Close();
                                    }
                                    catch
                                    {

                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLine("Bir istemcide hata çıktı: " + ex.Message + " (" + ex.StackTrace + ")");

                                    try
                                    {
                                        CloseConnection(id);
                                    }
                                    catch
                                    {
                                    }

                                    break;
                                }
                            }
                        });

                        ThreadList[connectionIndex].Start(connectionIndex);

                        OccupiedThreads++;
                    }
                    catch (Exception x)
                    {
                        WriteLine("Sunucu portu başlatılamadı (Sebep: " + x.Message + " :: " + x.StackTrace + ")");
                        return;
                    }
                }
            });

            MainThread.Start();
        }

        private void ZeroTables()
        {
            WriteLine("Masa kayıtları sıfırlanıyor...", Color.Silver);
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("User-Agent", "Gravity 0.1"); // PostmanRuntime/7.19.0
                string data = wc.DownloadString(Host + "srv_zero_tables.php");
            }
        }

        private void InitLogDirectory()
        {
            try
            {
                if(!Directory.Exists("rawlog"))
                {
                    Directory.CreateDirectory("rawlog");
                }
            } catch
            {
                WriteLine("Dikkat: RawLog dizini oluşturulamadı! RawLog tutulamayabilir!!!", Color.Red);
            }
        }

        private void ProcessBuffer(int id, byte[] buffer, int dataLength)
        {
            string msg = Encoding.ASCII.GetString(buffer).Substring(0, dataLength).Trim('\n');
            LogRaw(id, "INP<< " + msg);

            switch (Connections[id].State)
            {
                case ConnectionInfo.ConnectionState.HandShake:
                    HandleSocketWelcome(id, buffer, dataLength);
                    break;
                case ConnectionInfo.ConnectionState.SelfIdentify:
                    // For test purposes, this section has been skipped automatically
                    HandleClientIdentification(id, buffer, dataLength);
                    break;
                case ConnectionInfo.ConnectionState.JoinRoom:
                    HandleJoinRoomCommand(id, buffer, dataLength);
                    break;
                case ConnectionInfo.ConnectionState.InTheRoom:
                    HandleInRoomCommands(id, buffer, dataLength);
                    break;
            }
        }

        private void HandleInRoomCommands(int id, byte[] buffer, int dataLength)
        {
            string msg = Encoding.ASCII.GetString(buffer).Replace("\0", "");
            string[] messages = msg.Split('\n');

            foreach (string message in messages)
            {
                if(string.IsNullOrEmpty(message))
                {
                    continue;
                }

                if (message.StartsWith("IM ALIVE"))
                {
                    Connections[id].KeepAliveNotResponded = 0;
                }
                else if (message.StartsWith("SIT "))
                {
                    try
                    {
                        string[] sitParts = message.Split(' ');
                        bool autoRebuy = sitParts[1] == "ON";
                        bool autoTopOff = sitParts[2] == "ON";
                        long amountToSit = long.Parse(sitParts[3]);

                        if (Connections[id].SittingAt >= 0)
                        {
                            WriteToClient(id, "ALREADY SITTING AT POSITION " + Connections[id].SittingAt);
                        }
                        else
                        {
                            int found = FindTableFromId(Connections[id].TableId);
                            if (found == -1)
                            {
                                // room not found
                                WriteToClient(id, "ROOM NOT FOUND, XO");
                                return;
                            }

                            if (Connections[id].Balance < TableList[found].SmallBlind * 20)
                            {
                                WriteToClient(id, "INSUFFICIENT FOR TABLE");
                                return;
                            }

                            using (WebClient wc = new WebClient())
                            {
                                wc.Headers.Add("User-Agent", "Gravity 0.1"); // PostmanRuntime/7.19.0
                                string data = wc.DownloadString(Host + "srv_user_sit.php?user_id=" + Connections[id].UserID + "&token=" + Connections[id].SessionToken + "&room_id=" + Connections[id].TableId + "&auto_rebuy=" + (autoRebuy ? "1" : "0") + "&auto_topoff=" + (autoTopOff ? "1" : "0") + "&amount_to_sit=" + amountToSit);

                                if (data == "TABLE FULL")
                                {
                                    WriteToClient(id, "NO FREE SPACES");
                                    return;
                                }
                                else if (data == "N/A")
                                {
                                    WriteToClient(id, "ERROR");
                                    return;
                                }

                                Connections[id].SittingAt = int.Parse(data) + 1;

                                // balance info
                                Connections[id].AmountToSit = amountToSit;
                                if (!autoRebuy && !autoTopOff)
                                {
                                    Connections[id].BalanceOnTable = amountToSit;
                                }

                                Connections[id].AutoRebuy = autoRebuy;
                                Connections[id].AutoTopOff = autoTopOff;
                                int player;

                                for (int j = 0; j < TableList[found].NumSat; j++)
                                {
                                    player = TableList[found].PlayersSat[j];

                                    if (player == id)
                                    {
                                        WriteToClient(player, "BALANCE UPDATE FOR YOU " + Connections[j].AmountToSit);
                                    }
                                    else
                                    {
                                        WriteToClient(player, "BALANCE UPDATE FOR PLAYER " + Connections[id].SittingAt + " " + Connections[id].AmountToSit);
                                    }
                                }
                                // end of balance info

                                bool waitInQue = false;
                                if (TableList[found].NumSat < 2)
                                {
                                    TableList[found].NumSat++;
                                    TableList[found].PlayersSat[TableList[found].NumSat - 1] = id;
                                }
                                else
                                {
                                    waitInQue = true;
                                    TableList[found].NumSitQue++;
                                    TableList[found].PlayersSitQue[TableList[found].NumSitQue - 1] = id;
                                }

                                // PASS 1: Let the user know he is sitting
                                WriteToClient(id, "SITTING AT POSITION " + Connections[id].SittingAt);
                                if (Connections[id].Level == LevelIntervals.Length)
                                {
                                    WriteToClient(id, "YOUR LEVEL IS " + Connections[id].Level + " AT XP " + Connections[id].XP);
                                }
                                else
                                {
                                    WriteToClient(id, "YOUR LEVEL IS " + Connections[id].Level + " AT XP " + Connections[id].XP + " FROM " + (Connections[id].Level == 1 ? 0 : LevelIntervals[Connections[id].Level - 2]) + " UNTIL " + LevelIntervals[Connections[id].Level - 1]);
                                }

                                WriteToClient(id, "YOUR BALANCE IS " + Connections[id].AmountToSit);
                                // Amount you sat??

                                // PASS 2: Notify every other player about the new user's presence
                                for (int i = 0; i < TableList[found].NumConnected; i++)
                                {
                                    player = TableList[found].PlayersConnected[i];
                                    if (player == id)
                                    {
                                        continue;
                                    }
                                    else if (Connections[i] != null && Connections[i].Active && Connections[i].TableId == Connections[id].TableId)
                                    {
                                        WriteToClient(player, "PLAYER " + id + " SIT AT " + Connections[id].SittingAt);
                                        WriteToClient(player, "BALANCE UPDATE FOR PLAYER " + Connections[id].SittingAt + " " + Connections[id].BalanceOnTable);
                                    }
                                }

                                // PASS 3: Start bidding process
                                if (TableList[found].NumSat == 2 && !waitInQue)
                                {
                                    Clear(found);
                                    StartNewTurn(found);
                                }
                            }
                        }
                    } catch(Exception ex)
                    {
                        //WriteLine("İstemci " + id + " );
                        Log(ex.Message + " :: " + ex.StackTrace);
                    }

                    return;
                }
                else if (message == "CALL")
                {
                    if (Connections[id].SittingAt != -1 && Connections[id].MyTurn)
                    {
                        PlayerCall(id);
                    }
                }
                else if (message == "FOLD")
                {
                    if (Connections[id].SittingAt != -1 && Connections[id].MyTurn)
                    {
                        PlayerFold(id);
                    }
                }
                else if (message == "ALLIN")
                {
                    if (Connections[id].SittingAt != -1 && Connections[id].MyTurn)
                    {
                        //PlayerAllin(id);
                        WriteLine("Deprecated message came: ALLIN", Color.Pink);
                        WriteToClient(id, "OBSOLETE: ALLIN");
                    }
                }
                else if (message.StartsWith("RAISE BY "))
                {
                    if (Connections[id].SittingAt != -1 && Connections[id].MyTurn)
                    {
                        try
                        {
                            long amount = long.Parse(message.Substring("RAISE BY ".Length));
                            PlayerRaiseTo(id, amount);
                        }
                        catch(Exception ex)
                        {
                            WriteToClient(id, "RAISE NOT ACCEPTED: AMOUNT INVALID");
                            Log(ex.Message + " :: " + ex.StackTrace);
                        }
                    }
                }
                else if (message == "LEAVE ROOM")
                {
                    LeaveRoom(id);

                    return;
                }
                else if (message == "STAND UP" && Connections[id].SittingAt != -1)
                {
                    PlayerStandUp(id);

                    return;
                }
                else
                {
                    WriteLine("HRC INVALID (" + message + ")");
                }
            }
        }

        private void PlayerStandUp(int id)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("User-Agent", "Gravity 0.1"); // PostmanRuntime/7.19.0
                string data = wc.DownloadString(Host + "srv_user_stand_up.php?user_id=" + Connections[id].UserID + "&token=" + Connections[id].SessionToken + "&room_id=" + Connections[id].TableId);

                if (data == "N/A")
                {
                    WriteToClient(id, "ERROR");
                    return;
                }

                if (Connections[id].MyTurn)
                {
                    PlayerFold(id, false);
                }

                int found = FindTableFromId(Connections[id].TableId);

                // Remove from players sat
                int playerIndex = -1;
                for (int z = 0; z < TableList[found].PlayersSat.Length; z++)
                {
                    if (TableList[found].PlayersSat[z] == id)
                    {
                        playerIndex = z;
                        break;
                    }
                }

                if (playerIndex != -1)
                {
                    for (int h = playerIndex; h < TableInfo.MaxCapacity - 1; h++)
                    {
                        TableList[found].PlayersSat[h] = TableList[found].PlayersSat[h + 1];
                    }
                }

                Connections[id].SittingAt = -1;

                TableList[found].NumSat--;
                TableList[found].PlayersSat[TableInfo.MaxCapacity - 1] = 0;

                // Remove from sit que
                playerIndex = -1;
                for (int z = 0; z < TableList[found].NumSitQue; z++)
                {
                    if (TableList[found].PlayersSitQue[z] == id)
                    {
                        playerIndex = z;
                        break;
                    }
                }

                if (playerIndex != -1)
                {
                    for (int h = playerIndex; h < TableInfo.MaxCapacity - 1; h++)
                    {
                        TableList[found].PlayersSitQue[h] = TableList[found].PlayersSitQue[h + 1];
                    }
                }

                // PASS 1: Let the user know he is sitting
                WriteToClient(id, "STAND UP OK");

                // PASS 2: Notify every other player about the new user's presence
                int player;
                for (int i = 0; i < TableList[found].NumConnected; i++)
                {
                    player = TableList[found].PlayersConnected[i];
                    if (player == id)
                    {
                        continue;
                    }
                    else if (Connections[i] != null && Connections[i].Active && Connections[i].TableId == Connections[id].TableId)
                    {
                        WriteToClient(player, "PLAYER " + id + " HAS STAND UP " + data);
                    }
                }

                // PASS 3: Let the only remaining player win
                if (TableList[found].NumSat == 1)
                {
                    DecideWinner(found, true);
                } else
                {
                    switch (AllDone(found))
                    {
                        case TurnResult.Immediate:
                            DecideWinner(found, true);
                            break;
                        case TurnResult.NextPlayer:
                            SkipToNextPlayer(found);
                            break;
                        case TurnResult.NextTurn:
                            StartNewTurn(found);
                            break;
                    }
                }
            }
        }

        private void HandleTableCards(int tableIndex)
        {
            if(TestMode && CardsHandledForTest)
            {
                return;
            }

            // Deal cards
            MixUpCards(tableIndex);
            DealHand(tableIndex);

            #if _HARDCODED_TEST
            TableList[tableIndex].CardsOnTheTable[0] = new CardInfo(CardType.Clubs, 4);
            TableList[tableIndex].CardsOnTheTable[1] = new CardInfo(CardType.Hearts, 5);
            TableList[tableIndex].CardsOnTheTable[2] = new CardInfo(CardType.Hearts, 10);
            TableList[tableIndex].CardsOnTheTable[3] = new CardInfo(CardType.Diamonds, 8);
            TableList[tableIndex].CardsOnTheTable[4] = new CardInfo(CardType.Hearts, CardInfo.CardQ);
#else
            TableList[tableIndex].CardsOnTheTable[0] = TakeNextCard(tableIndex);
            TableList[tableIndex].CardsOnTheTable[1] = TakeNextCard(tableIndex);
            TableList[tableIndex].CardsOnTheTable[2] = TakeNextCard(tableIndex);
            TableList[tableIndex].CardsOnTheTable[3] = TakeNextCard(tableIndex);
            TableList[tableIndex].CardsOnTheTable[4] = TakeNextCard(tableIndex);
#endif

            if (TestMode)
            {
                CardsHandledForTest = false;
            }
        }

        private void LeaveRoom(int id)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("User-Agent", "Gravity 0.1"); // PostmanRuntime/7.19.0
                string data = wc.DownloadString(Host + "srv_user_leave_room.php?user_id=" + Connections[id].UserID + "&token=" + Connections[id].SessionToken + "&room_id=" + Connections[id].TableId);

                if (data == "N/A")
                {
                    WriteToClient(id, "ERROR");
                    return;
                }

                Connections[id].TableId = "";

                // PASS 1: Let the user know he is sitting
                WriteToClient(id, "LEAVE ROOM OK");

                // PASS 2: Notify every other player about the new user's presence
                for (int i = 0; i < OccupiedThreads; i++)
                {
                    if (i == id)
                    {
                        continue;
                    }
                    else if (Connections[i] != null && Connections[i].Active && Connections[i].TableId == Connections[id].TableId)
                    {
                        int found = FindTableFromId(Connections[id].TableId);

                        if (found != -1)
                        {
                            // Clear from connected players
                            int playerIndex = -1;
                            for (int z = 0; z < TableList[found].PlayersConnected.Length; z++)
                            {
                                if (TableList[found].PlayersConnected[z] == id)
                                {
                                    playerIndex = z;
                                    break;
                                }
                            }

                            if (playerIndex != -1)
                            {
                                for (int h = playerIndex; h < TableInfo.MaxCapacity - 1; h++)
                                {
                                    TableList[found].PlayersConnected[h] = TableList[found].PlayersConnected[h + 1];
                                }

                                if (Connections[id].SittingAt != -1)
                                {
                                    playerIndex = -1;
                                    for (int z = 0; z < TableList[found].PlayersSat.Length; z++)
                                    {
                                        if (TableList[found].PlayersSat[z] == id)
                                        {
                                            playerIndex = z;
                                            break;
                                        }
                                    }

                                    for (int h = playerIndex; h < TableInfo.MaxCapacity - 1; h++)
                                    {
                                        TableList[found].PlayersSat[h] = TableList[found].PlayersSat[h + 1];
                                    }

                                    Connections[id].SittingAt = -1;
                                    TableList[found].NumSat--;
                                    TableList[found].PlayersSat[TableInfo.MaxCapacity - 1] = 0;
                                }

                                TableList[found].NumConnected--;
                                TableList[found].PlayersConnected[TableInfo.MaxCapacity - 1] = 0;
                                for (int h = playerIndex; h < TableInfo.MaxCapacity - 1; h++)
                                {
                                    TableList[found].PlayersSat[h] = TableList[found].PlayersSat[h + 1];
                                }
                            }

                            // clear from sit que
                            playerIndex = -1;
                            for (int z = 0; z < TableList[found].NumSitQue; z++)
                            {
                                if (TableList[found].PlayersSitQue[z] == id)
                                {
                                    playerIndex = z;
                                    break;
                                }
                            }

                            if (playerIndex != -1)
                            {
                                for (int h = playerIndex; h < TableList[found].NumSitQue - 1; h++)
                                {
                                    TableList[found].PlayersSitQue[h] = TableList[found].PlayersSitQue[h + 1];
                                }
                            }
                        }

                        WriteToClient(i, "PLAYER " + id + " HAS LEFT ROOM");
                    }
                }
            }
        }

        private void DecideWinner(int tableIndex)
        {
            DecideWinner(tableIndex, false);
        }

        private void DecideWinner(int tableIndex, bool noDelay)
        {
            int playerToWin;
            int player;

            // BIDDING

            // Only one non-folded?
            int numFolded = 0;
            int nonFolded = -1;
            int numCalled = 0;
            int numAllin = 0;
            int numRaised = 0;

            if (!noDelay)
            {
                Thread.Sleep(3000);
            }

            for (int i = 0; i < TableList[tableIndex].NumSat; i++)
            {
                player = TableList[tableIndex].PlayersSat[i];
                Connections[player].MyTurn = false;

                if (!Connections[player].Folded)
                {
                    nonFolded = i;
                } else
                {
                    numFolded++;
                }

                if (Connections[player].Called)
                {
                    numCalled++;
                }

                if (Connections[player].Allin)
                {
                    numAllin++;
                }

                if (Connections[player].Raised)
                {
                    numRaised++;
                }
            }

            if (TableList[tableIndex].NumSat == 1)
            {
                playerToWin = TableList[tableIndex].PlayersSat[0];
                PlayerWin(new WinningHand[] { new WinningHand(playerToWin, "AITT", "N") }, false, noDelay);
            }
            else if (numFolded == TableList[tableIndex].NumSat - 1)
            {
                playerToWin = Connections[TableList[tableIndex].PlayersSat[nonFolded]].SittingAt;
                PlayerWin(new WinningHand[] { new WinningHand(playerToWin, "AITT", "N") }, false);

                // Start a new turn if there are more than one people sitting
                if (TableList[tableIndex].NumSat > 1)
                {
                    Application.DoEvents();
                    Thread.Sleep(5000);

                    Clear(tableIndex);
                    StartNewTurn(tableIndex);
                }
            }
            else if (numCalled + numFolded + numAllin + numRaised == TableList[tableIndex].NumSat)
            {
                // decide the winner among callers
                if (TableList[tableIndex].NumHand == 1 && numAllin == 0)
                {
                    StartNewTurn(tableIndex);
                }
                else
                {
                    ResetPlayerMarksOfTable(tableIndex);
                    int highestMark = -1;

                    // the ones who stood up before winners have been declared??

                    // find the highest hand
                    string wonBy;
                    string wonOrder;
                    for (int i = 0; i < TableList[tableIndex].NumSat; i++)
                    {
                        player = TableList[tableIndex].PlayersSat[i];
                        if (Connections[player].Folded)
                        {
                            Connections[player].PlayerMark = -100;
                            Connections[player].HandName = null;
                            Connections[player].WinningCardOrder = null;
                        }
                        else
                        {
                            Connections[player].PlayerMark = CalculatePlayerMark(player, out wonBy, out wonOrder);
                            Connections[player].HandName = wonBy;
                            Connections[player].WinningCardOrder = wonOrder;

                            if (Connections[player].PlayerMark > highestMark)
                            {
                                highestMark = Connections[player].PlayerMark;
                            }
                        }
                    }

                    // display cards?
                    if (TableList[tableIndex].NumHand >= 1 && numAllin != 0)
                    {
                        for (int i = 0; i < TableList[tableIndex].NumConnected; i++)
                        {
                            WriteToClient(Connections[TableList[tableIndex].PlayersConnected[i]].Index, "REVEAL HOUSE " + CardInfoToString(TableList[tableIndex].CardsOnTheTable[0]) + " " + CardInfoToString(TableList[tableIndex].CardsOnTheTable[1]) + " " + CardInfoToString(TableList[tableIndex].CardsOnTheTable[2]) + " " + CardInfoToString(TableList[tableIndex].CardsOnTheTable[3]) + " " + CardInfoToString(TableList[tableIndex].CardsOnTheTable[4]));
                        }
                    }

                    // declare winners
                    List<WinningHand> winnerList = new List<WinningHand>();
                    List<int> winnerIDs = new List<int>();
                    for (int i = 0; i < TableList[tableIndex].NumSat; i++)
                    {
                        player = TableList[tableIndex].PlayersSat[i];
                        if (Connections[player].PlayerMark == highestMark)
                        {
                            winnerIDs.Add(player);
                            winnerList.Add(new WinningHand(player, Connections[player].HandName, Connections[player].WinningCardOrder)); // Connections[player].SittingAt);
                        }
                    }

                    PlayerWin(winnerList.ToArray(), true);
                    for (int j = 0; j < TableList[tableIndex].NumSat; j++)
                    {
                        if (!winnerIDs.Contains(TableList[tableIndex].PlayersSat[j]))
                        {
                            // 40 XP if folded, 50 XP if just lost
                            IncrementPlayerXP(tableIndex, TableList[tableIndex].PlayersSat[j], Connections[TableList[tableIndex].PlayersSat[j]].Folded ? 40 : 50);
                        }
                        else
                        {
                            // 200 XP for winning
                            IncrementPlayerXP(tableIndex, TableList[tableIndex].PlayersSat[j], 200 /*Connections[TableList[tableIndex].PlayersSat[j]].PlayerMark*/);
                        }

                        Connections[TableList[tableIndex].PlayersSat[j]].PlayerMark = 0;
                    }


                    // Start a new turn if there are more than one people sitting
                    if (TableList[tableIndex].NumSat > 1)
                    {
                        Application.DoEvents();
                        Thread.Sleep(5000);

                        Clear(tableIndex);
                        StartNewTurn(tableIndex);
                    }
                }
            }
            else
            {
                StartNewTurn(tableIndex);
            }
            
        }

        private void IncrementPlayerXP(int tableIndex, int player, int playerMark)
        {
            if(playerMark <= 0)
            {
                return;
            }

            Connections[player].XP += playerMark;
            if (Connections[player].XPToLevel <= playerMark)
            {
                // level up
                PlayerLevelUp(tableIndex, player);
            }
            else
            {
                Connections[player].XPToLevel -= playerMark;
            }

            for (int i = 0; i < TableList[tableIndex].NumConnected; i++)
            {
                if (Connections[TableList[tableIndex].PlayersConnected[i]].Level < LevelIntervals.Length)
                {
                    WriteToClient(Connections[TableList[tableIndex].PlayersConnected[i]].Index, "XP GAIN " + Connections[player].SittingAt + " " + playerMark + " TO " + Connections[player].XP + " FROM " + (Connections[player].Level == 1 ? 0 : LevelIntervals[Connections[player].Level - 2]) + " UNTIL " + LevelIntervals[Connections[player].Level - 1] + " LEVEL " + Connections[player].Level);
                }
            }

            // Save
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Headers.Add("User-Agent", "Gravity 0.1"); // PostmanRuntime/7.19.0
                    wc.Encoding = Encoding.UTF8;
                    string data = wc.DownloadString(Host + "srv_set_user_xp.php?session=" + Connections[player].SessionUser + "&token=" + Connections[player].SessionToken + "&xp=" + playerMark + "&level=" + Connections[player].Level);
                }
            } catch
            {
                WriteLine("1054: INC XP Failed: PI " + player + " XP "+ playerMark, Color.Red);
            }

            Connections[player].PlayerMark = 0;
        }

        private void PlayerLevelUp(int tableIndex, int v)
        {
            Connections[v].Level++;
            Connections[v].XPToLevel = Math.Abs(Connections[v].XPToLevel - Connections[v].PlayerMark);
            Connections[v].XPToLevel = LevelIntervals.Length > Connections[v].Level - 1 ? LevelIntervals[Connections[v].Level - 1] - Connections[v].XPToLevel : LevelIntervals[LevelIntervals.Length - 1] - Connections[v].XPToLevel;
            Connections[v].PlayerMark = 0;

            // Announce
            for (int i = 0; i < TableList[tableIndex].NumConnected; i++)
            {
                if (TableList[tableIndex].PlayersConnected[i] == v)
                {
                    WriteToClient(Connections[v].Index, "YOU LEVELED UP TO " + Connections[v].Level);
                }
                else
                {
                    WriteToClient(Connections[TableList[tableIndex].PlayersConnected[i]].Index, "LEVEL UP " + Connections[v].SittingAt + " TO " + Connections[v].Level);
                }
            }
        }

        private int CalculatePlayerMark(int player, out string wonBy, out string wonOrder)
        {
            bool acesPresent;
            int mark1 = CalculatePlayerMark(player, out wonBy, out wonOrder, 14, out acesPresent);
            int mark2 = -1;

            if (acesPresent)
            {
                mark2 = CalculatePlayerMark(player, out wonBy, out wonOrder, 1, out acesPresent);
            }

            return Math.Max(mark1, mark2);
        }

        private int CalculatePlayerMark(int player, out string wonBy, out string wonOrder, int aceValue, out bool acesPresent)
        {
            wonBy = "HC";
            wonOrder = "N";

            int tableIndex = FindTableFromId(Connections[player].TableId);
            int baseValue; // = Connections[player].CardDeck[0].Number + Connections[player].CardDeck[1].Number;
            int mark = 0;

            CardInfo[] cardList = new CardInfo[7];
            CardInfo[] origList = new CardInfo[7];

            for (int i = 0; i < 5; i++)
            {
                cardList[i] = new CardInfo(TableList[tableIndex].CardsOnTheTable[i].Type, TableList[tableIndex].CardsOnTheTable[i].Number);
                origList[i] = new CardInfo(TableList[tableIndex].CardsOnTheTable[i].Type, TableList[tableIndex].CardsOnTheTable[i].Number);

                if(aceValue == 1 && cardList[i].Number == 14)
                {
                    cardList[i].Number = 1;
                    origList[i].Number = 1;
                }
            }

            cardList[5] = new CardInfo(Connections[player].CardDeck[0].Type, Connections[player].CardDeck[0].Number);
            cardList[6] = new CardInfo(Connections[player].CardDeck[1].Type, Connections[player].CardDeck[1].Number);
            origList[5] = new CardInfo(Connections[player].CardDeck[0].Type, Connections[player].CardDeck[0].Number);
            origList[6] = new CardInfo(Connections[player].CardDeck[1].Type, Connections[player].CardDeck[1].Number);

            if (aceValue == 1 && cardList[5].Number == 14)
            {
                cardList[5].Number = 1;
                origList[5].Number = 1;
            }

            if (aceValue == 1 && cardList[6].Number == 14)
            {
                cardList[6].Number = 1;
                origList[6].Number = 1;
            }

            acesPresent = false;
            for (int i = 0; i < cardList.Length; i++)
            {
                if (cardList[i].Number == CardInfo.CardA)
                {
                    acesPresent = true;
                    break;
                }
            }

            // sort list
            for (int i = 0; i < cardList.Length - 1; i++)
            {
                for (int j = i + 1; j < cardList.Length; j++)
                {
                    if (cardList[i].Number < cardList[j].Number)
                    {
                        int c = cardList[j].Number;
                        cardList[j].Number = cardList[i].Number;
                        cardList[i].Number = c;

                        CardType t = cardList[j].Type;
                        cardList[j].Type = cardList[i].Type;
                        cardList[i].Type = t;
                    }
                }
            }

            int numHearts = 0;
            int numDiamonds = 0;
            int numSpades = 0;
            int numClubs = 0;

            for (int i = 0; i < cardList.Length; i++)
            {
                switch(cardList[i].Type)
                {
                    case CardType.Diamonds:
                        numDiamonds++;
                        break;
                    case CardType.Hearts:
                        numHearts++;
                        break;
                    case CardType.Spades:
                        numSpades++;
                        break;
                    case CardType.Clubs:
                        numClubs++;
                        break;
                }
            }

            if (numSpades >= 5 || numDiamonds >= 5 || numClubs >= 5 || numHearts >= 5)
            {
                CardType biggestType = numSpades >= 5 ? CardType.Spades : (numDiamonds >= 5 ? CardType.Diamonds : (numClubs >= 5 ? CardType.Clubs : CardType.Hearts));

                // #1 Royal Flush?
                bool presentA = false;
                bool presentK = false;
                bool presentQ = false;
                bool presentJ = false;
                bool present10 = false;

                for (int i = 0; i < cardList.Length; i++)
                {
                    if (cardList[i].Type == biggestType)
                    {
                        switch (cardList[i].Number)
                        {
                            case CardInfo.CardA:
                                presentA = true;
                                break;
                            case 10:
                                present10 = true;
                                break;
                            case CardInfo.CardJ:
                                presentJ = true;
                                break;
                            case CardInfo.CardQ:
                                presentQ = true;
                                break;
                            case CardInfo.CardK:
                                presentK = true;
                                break;
                        }
                    }
                }

                if (presentA && presentK && presentQ && presentJ && present10)
                {
                    // find A, K, Q, J & 10
                    baseValue = 0;
                    for (int i = 0; i < origList.Length; i++)
                    {
                        if (origList[i].Type == biggestType && (origList[i].Number == CardInfo.CardA || origList[i].Number == CardInfo.CardK || origList[i].Number == CardInfo.CardQ || origList[i].Number == CardInfo.CardJ || origList[i].Number == 10))
                        {
                            if (wonOrder != "")
                            {
                                wonOrder += ",";
                            }

                            wonOrder += i;
                            baseValue += origList[i].Number;
                        }
                    }

                    wonBy = "RF";
                    return 10000 + baseValue;
                }
            }

            // #2 Another RF variant?
            CardType? flushType = null;
            int highestNumber;
            int flushHighest = int.MaxValue;
            int straightIndex = 1;
            for (int i = cardList.Length - 2; i >= 0; i--)
            {
                if (cardList[i + 1].Number == cardList[i].Number - 1 && cardList[i + 1].Type == cardList[i].Type)
                {
                    if (flushHighest == int.MaxValue || cardList[i].Number > flushHighest)
                    {
                        flushType = cardList[i].Type;
                        flushHighest = cardList[i].Number;
                    }

                    straightIndex++;
                } else
                {
                    if (straightIndex < 4)
                    {
                        // If straightIndex is 4 or more, that means you already found sufficient number of cards.
                        // There can be no straight so tere is no need to start over search process for adjacents

                        flushHighest = int.MaxValue;
                        flushType = null;
                        straightIndex = 0;
                    }
                }
            }

            if (straightIndex >= 4 && flushHighest != int.MaxValue)
            {
                wonOrder = "";
                for (int i_ = 0; i_ < origList.Length; i_++)
                {
                    if (origList[i_].Number >= flushHighest - 4 && origList[i_].Number <= flushHighest && origList[i_].Type == flushType)
                    {
                        if (wonOrder != "")
                        {
                            wonOrder += ",";
                        }

                        wonOrder += i_;
                    }
                }

                // straight flush
                if (flushHighest == CardInfo.CardA)
                {
                    wonBy = "RF";
                    return 10000 + flushHighest + flushHighest - 1 + flushHighest - 2 + flushHighest - 3 + flushHighest - 4;
                }
                else
                {
                    wonBy = "SF";
                    return 9500 + flushHighest + flushHighest - 1 + flushHighest - 2 + flushHighest - 3 + flushHighest - 4;
                }
            }

            highestNumber = int.MaxValue;
            straightIndex = 1;
            for (int i = cardList.Length - 1; i >= 1; i--)
            {
                //flush = true;
                for (int j = i - 1; j >= 0; j--)
                {
                    if (cardList[j].Number == cardList[i].Number + 1)
                    {
                        if (highestNumber == int.MaxValue || cardList[j].Number > highestNumber)
                        {
                            highestNumber = cardList[j].Number;
                        }

                        straightIndex++;
                        break;
                    }
                }
            }

            if (straightIndex >= 5 && highestNumber != int.MaxValue)
            {
                wonOrder = "";
                for(int highestIndex = 0; highestIndex < 5; highestIndex++)
                {
                    for (int i = 0; i < origList.Length; i++)
                    {
                        if (origList[i].Number == highestNumber - highestIndex)
                        {
                            // is there another number within the wonOrder?
                            if (wonOrder != "")
                            {
                                wonOrder += ",";
                            }

                            wonOrder += i;
                            break;
                        }
                    }

                    try
                    {
                        if(wonOrder.Split(',').Length == 5)
                        {
                            break;
                        }
                    } catch
                    {

                    }
                }

                string[] wonOrderItems = wonOrder.Split(',');
                if (wonOrderItems.Length == 5)
                {
                    wonBy = "ST";
                    return 9000 + highestNumber + highestNumber - 1 + highestNumber - 2 + highestNumber - 3 + highestNumber - 4;
                }
            }

            // others
            int numberOfSameCards;
            int highestNumberOfSameCards = 0;
            int highestCardNumber = 0;
            int cardNumber;
            wonOrder = "";
            for (int i = 0; i < cardList.Length - 1; i++)
            {
                numberOfSameCards = 0;
                cardNumber = 2;

                for (int j = 0; j < cardList.Length; j++)
                {
                    if(i == j)
                    {
                        continue;
                    }

                    if(cardList[i].Number == cardList[j].Number)
                    {
                        cardNumber = cardList[i].Number == 1 ? 14 : cardList[i].Number;
                        numberOfSameCards++;
                    }
                }

                if (highestNumberOfSameCards < numberOfSameCards)
                {
                    highestNumberOfSameCards = numberOfSameCards;
                    highestCardNumber = cardNumber;
                }
            }

            numberOfSameCards = highestNumberOfSameCards + 1;

            // #3 Four of a Kind?
            if(numberOfSameCards == 4)
            {
                baseValue = 0;
                for (int i = 0; i < origList.Length; i++)
                {
                    if (origList[i].Number == highestCardNumber)
                    {
                        if (wonOrder != "")
                        {
                            wonOrder += ",";
                        }

                        wonOrder += i;
                    } else if(baseValue == 0)
                    {
                        baseValue = origList[i].Number;
                    }
                }

                for (int i = 0; i < cardList.Length; i++)
                {
                    if(cardList[i].Number != highestCardNumber && baseValue == 0)
                    {
                        baseValue = cardList[i].Number;
                    }
                }

                wonBy = "OOK4";
                return 8000 + baseValue;
            }

            // #4 Full House?

            // #5 Flush?

            // #6 Straight?

            // #7 Three of a Kind?
            int calculatedBaseValueItems = 0;
            if (numberOfSameCards == 3)
            {
                int secondOrder = -1;

                for (int i = 0; i < origList.Length - 1; i++)
                {
                    for (int j = 1; j < origList.Length; j++)
                    {
                        if(origList[i].Number != highestCardNumber && i != j && origList[i].Number == origList[j].Number)
                        {
                            secondOrder = origList[i].Number;
                            break;
                        }
                    }
                }

                baseValue = 0;
                for (int i = 0; i < origList.Length; i++)
                {
                    if (origList[i].Number == highestCardNumber)
                    {
                        if (wonOrder != "")
                        {
                            wonOrder += ",";
                        }

                        wonOrder += i;
                    }
                }

                for (int i = 0; i < cardList.Length; i++)
                {
                    if (calculatedBaseValueItems < 2 && cardList[i].Number != highestCardNumber)
                    {
                        calculatedBaseValueItems++;
                        baseValue += cardList[i].Number;
                    }
                }

                if (secondOrder != -1)
                {
                    wonBy = "FH";
                    baseValue += 0;

                    for (int i = 0; i < origList.Length; i++)
                    {
                        if (origList[i].Number == secondOrder)
                        {
                            wonOrder += "," + i;
                        }
                    }
                }
                else
                {
                    wonBy = "OOK3";
                }

                return 4000 + baseValue;
            }

            // #8 Three Pairs? Two Pairs?
            if (numberOfSameCards == 2)
            {
                int pairNumber3 = -1;
                int pairNumber2 = -1;
                int pairNumber = -1;

                // pass 1
                for (int i = 0; i < cardList.Length - 1; i++)
                {
                    for (int j = 1; j < cardList.Length; j++)
                    {
                        if(i == j)
                        {
                            continue;
                        }

                        if(cardList[i].Number == cardList[j].Number)
                        {
                            pairNumber = cardList[j].Number;
                            break;
                        }
                    }

                    if(pairNumber != -1)
                    {
                        break;
                    }
                }

                // pass 2
                for (int i = 0; i < cardList.Length - 1; i++)
                {
                    for (int j = 1; j < cardList.Length; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        if (cardList[i].Number != pairNumber && cardList[i].Number == cardList[j].Number)
                        {
                            //return 2000 + highestCardNumber;
                            pairNumber2 = cardList[i].Number;
                            break;
                        }
                    }

                    if (pairNumber2 != -1)
                    {
                        break;
                    }
                }

                // pass 3
                for (int i = 0; i < cardList.Length - 1; i++)
                {
                    for (int j = 1; j < cardList.Length; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        if (cardList[i].Number != pairNumber && cardList[i].Number != pairNumber2 && cardList[i].Number == cardList[j].Number)
                        {
                            //return 2000 + highestCardNumber;
                            pairNumber3 = cardList[i].Number;
                            break;
                        }
                    }

                    if (pairNumber3 != -1)
                    {
                        break;
                    }
                }

                if (pairNumber3 != -1)
                {
                    wonBy = "PA2";

                    if (pairNumber3 > pairNumber2 && pairNumber3 > pairNumber)
                    {
                        baseValue = 0;
                        for (int i = 0; i < origList.Length; i++)
                        {
                            if (origList[i].Number == pairNumber3)
                            {
                                if (wonOrder != "")
                                {
                                    wonOrder += ",";
                                }

                                wonOrder += i;
                            }
                        }

                        if (pairNumber2 > pairNumber)
                        {
                            for (int i = 0; i < origList.Length; i++)
                            {
                                if (origList[i].Number == pairNumber2)
                                {
                                    if (wonOrder != "")
                                    {
                                        wonOrder += ",";
                                    }

                                    wonOrder += i;
                                }
                            }

                            for (int i = 0; i < cardList.Length; i++)
                            {
                                if (origList[i].Number != pairNumber3 && origList[i].Number != pairNumber2 && baseValue == 0)
                                {
                                    baseValue = origList[i].Number;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < origList.Length; i++)
                            {
                                if (origList[i].Number == pairNumber)
                                {
                                    if (wonOrder != "")
                                    {
                                        wonOrder += ",";
                                    }

                                    wonOrder += i;
                                }
                            }

                            for (int i = 0; i < cardList.Length; i++)
                            {
                                if (origList[i].Number != pairNumber3 && origList[i].Number != pairNumber && baseValue == 0)
                                {
                                    baseValue = origList[i].Number;
                                }
                            }
                        }

                        return 3000 + baseValue;
                    }

                    if (pairNumber2 > pairNumber3 && pairNumber2 > pairNumber)
                    {
                        baseValue = 0;
                        for (int i = 0; i < origList.Length; i++)
                        {
                            if (origList[i].Number == pairNumber2)
                            {
                                if (wonOrder != "")
                                {
                                    wonOrder += ",";
                                }

                                wonOrder += i;
                            }
                        }

                        if (pairNumber > pairNumber3)
                        {
                            for (int i = 0; i < origList.Length; i++)
                            {
                                if (origList[i].Number == pairNumber)
                                {
                                    if (wonOrder != "")
                                    {
                                        wonOrder += ",";
                                    }

                                    wonOrder += i;
                                }
                            }

                            for (int i = 0; i < cardList.Length; i++)
                            {
                                if (origList[i].Number != pairNumber3 && origList[i].Number != pairNumber2 && baseValue == 0)
                                {
                                    baseValue = origList[i].Number;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < origList.Length; i++)
                            {
                                if (origList[i].Number == pairNumber3)
                                {
                                    if (wonOrder != "")
                                    {
                                        wonOrder += ",";
                                    }

                                    wonOrder += i;
                                }
                            }

                            for (int i = 0; i < cardList.Length; i++)
                            {
                                if (origList[i].Number != pairNumber && origList[i].Number != pairNumber2 && baseValue == 0)
                                {
                                    baseValue = origList[i].Number;
                                }
                            }
                        }

                        return 3000 + baseValue;
                    }

                    if (pairNumber > pairNumber2 && pairNumber > pairNumber3)
                    {
                        baseValue = 0;
                        for (int i = 0; i < origList.Length; i++)
                        {
                            if (origList[i].Number == pairNumber)
                            {
                                if (wonOrder != "")
                                {
                                    wonOrder += ",";
                                }

                                wonOrder += i;
                            }
                        }

                        if (pairNumber2 > pairNumber3)
                        {
                            for (int i = 0; i < origList.Length; i++)
                            {
                                if (origList[i].Number == pairNumber2)
                                {
                                    if (wonOrder != "")
                                    {
                                        wonOrder += ",";
                                    }

                                    wonOrder += i;
                                }
                            }

                            for (int i = 0; i < cardList.Length; i++)
                            {
                                if (origList[i].Number != pairNumber2 && origList[i].Number != pairNumber && baseValue == 0)
                                {
                                    baseValue = origList[i].Number;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < origList.Length; i++)
                            {
                                if (origList[i].Number == pairNumber3)
                                {
                                    if (wonOrder != "")
                                    {
                                        wonOrder += ",";
                                    }

                                    wonOrder += i;
                                }
                            }

                            for (int i = 0; i < cardList.Length; i++)
                            {
                                if(origList[i].Number != pairNumber3 && origList[i].Number != pairNumber && baseValue == 0)
                                {
                                    baseValue = origList[i].Number;
                                }
                            }
                        }

                        return 3000 + baseValue;
                    }
                }

                if (pairNumber2 != -1)
                {
                    baseValue = 0;
                    for (int i = 0; i < origList.Length; i++)
                    {
                        if (origList[i].Number == pairNumber2 || origList[i].Number == pairNumber)
                        {
                            if (wonOrder != "")
                            {
                                wonOrder += ",";
                            }

                            wonOrder += i;
                        }
                    }

                    for (int i = 0; i < cardList.Length; i++)
                    {
                        if (cardList[i].Number != pairNumber2 && cardList[i].Number != pairNumber && baseValue == 0)
                        {
                            baseValue = cardList[i].Number;
                        }
                    }

                    wonBy = "PA2";
                    return 3000 + baseValue;
                }

                if (pairNumber != -1)
                {
                    baseValue = 0;
                    for (int i = 0; i < origList.Length; i++)
                    {
                        if (origList[i].Number == pairNumber)
                        {
                            if (wonOrder != "")
                            {
                                wonOrder += ",";
                            }

                            wonOrder += i;
                        }
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        if(cardList[i].Number != pairNumber && calculatedBaseValueItems < 3)
                        {
                            if(baseValue == 0)
                            {
                                baseValue = pairNumber * 2;
                            }

                            calculatedBaseValueItems++;
                            baseValue += cardList[i].Number;
                        }
                    }

                    wonBy = "PA1";
                    return 2000 + baseValue;
                }
            }

            // #10 High Card?
            int[] sortedList = new int[cardList.Length];
            for (int i = 0; i < cardList.Length; i++)
            {
                sortedList[i] = cardList[i].Number;
            }

            int temp;
            for (int i = 0; i < sortedList.Length; i++)
            {
                for (int j = i + 1; j < sortedList.Length; j++)
                {
                    if(sortedList[j] < sortedList[i])
                    {
                        temp = sortedList[j];
                        sortedList[j] = sortedList[i];
                        sortedList[i] = temp;
                    }
                }
            }

            for (int i = 0; i < sortedList.Length; i++)
            {
                mark += sortedList[i];
            }

            return mark;
        }

        private void ResetPlayerMarksOfTable(int tableIndex)
        {
            int player;
            for (int i = 0; i < TableList[tableIndex].NumSat; i++)
            {
                player = TableList[tableIndex].PlayersSat[i];
                Connections[player].PlayerMark = 0;
                Connections[player].HandName = "";
                Connections[player].WinningCardOrder = "";
            }
        }

        private void PlayerWin(WinningHand[] ids, bool showCards)
        {
            PlayerWin(ids, showCards, false);
        }

        private void PlayerWin(WinningHand[] ids, bool showCards, bool noDelay)
        {
            if(ids == null || ids.Length == 0)
            {
                return;
            }

            if(TestMode && !string.IsNullOrEmpty(ExpectedResult))
            {
                ExpectedResult = ExpectedResult.Substring(4); // eliminate "WIN "

                string[] expectedWinners = ExpectedResult.Split(' ');
                if(expectedWinners.Length != ids.Length)
                {
                    WriteLine("TEST ASSERTION FAILED 101", Color.Red);
                } else
                {
                    bool failed = false;
                    for (int i = 0; i < ids.Length; i++)
                    {
                        if(ids[i].PlayerIndex != int.Parse(expectedWinners[i].Substring(1)))
                        {
                            WriteLine("TEST ASSERTION FAILED 102", Color.Red);
                            failed = true;

                            break;
                        }
                    }

                    if(!failed)
                    {
                        WriteLine("TEST ASSERTION SUCCEEDED", Color.Lime);
                    }
                }
            }

            string tableId = Connections[ids[0].PlayerIndex].TableId;
            int tableIndex = FindTableFromId(tableId);
            if(tableIndex == -1)
            {
                return;
            }

            int player;
            string winnerList; // = "";

            if (ids.Length == 1)
            {
                winnerList = "PLAYER WON " + Connections[ids[0].PlayerIndex].SittingAt + "-" + ids[0].HandName + "-" + ids[0].CardOrder;

                Connections[ids[0].PlayerIndex].Balance += (long)((TableList[tableIndex].Pot / ids.Length) * TaxPercent);
                Connections[ids[0].PlayerIndex].BalanceOnTable += (long)((TableList[tableIndex].Pot / ids.Length) * TaxPercent);
                Connections[ids[0].PlayerIndex].BalanceOnPlay = 0;
            }
            else
            {
                winnerList = "PLAYERS WON";

                for (int i = 0; i < ids.Length; i++)
                {
                    Connections[ids[i].PlayerIndex].Balance += (long)((TableList[tableIndex].Pot / ids.Length) * TaxPercent);
                    Connections[ids[i].PlayerIndex].BalanceOnTable += (long)((TableList[tableIndex].Pot / ids.Length) * TaxPercent);
                    Connections[ids[i].PlayerIndex].BalanceOnPlay = 0;

                    winnerList += " " + Connections[ids[i].PlayerIndex].SittingAt + "-" + ids[i].HandName + "-" + ids[i].CardOrder;
                }
            }

            if (showCards)
            {
                // show cards of players
                string cards = "HANDS OF PLAYERS ";
                for (int i = 0; i < TableList[tableIndex].NumSat; i++)
                {
                    player = TableList[tableIndex].PlayersSat[i];
                    if (!Connections[player].Folded)
                    {
                        cards += Connections[player].SittingAt + " " + CardInfoToString(Connections[player].CardDeck[0]) + " " + CardInfoToString(Connections[player].CardDeck[1]) + " ";
                    }
                }

                cards.Trim();
                for (int i = 0; i < TableList[tableIndex].NumConnected; i++)
                {
                    player = TableList[tableIndex].PlayersConnected[i];
                    WriteToClient(player, cards);
                }
            }

            if (!noDelay)
            {
                Thread.Sleep(2000);
            }

            // declare winners
            //bool youWon = false;
            for (int i = 0; i < TableList[tableIndex].NumConnected; i++)
            {
                player = TableList[tableIndex].PlayersConnected[i];
                Connections[player].MyTurn = false;

                if(ids.Length == 1 && ids[0].PlayerIndex == player)
                {
                    //youWon = true;
                    WriteToClient(player, "YOU WON-" + ids[0].HandName + "-" + ids[0].CardOrder);
                }

                //if (!youWon)
                //{
                WriteToClient(player, winnerList);
                //}

                if (Connections[player].SittingAt != -1)
                {
                    Connections[player].BalanceOnPlay = 0;
                }
            }

            UpdatePlayerBalancesOnTable(tableIndex);
            Thread.Sleep(2000);
        }

        private void UpdatePlayerBalance(int playerIndex)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("User-Agent", "Gravity 0.1"); // PostmanRuntime/7.19.0
                wc.Encoding = Encoding.UTF8;
                string data = wc.DownloadString(Host + "srv_set_user_balance.php?session="+ Connections[playerIndex].SessionUser + "&balance=" + Connections[playerIndex].Balance + "&token=" + Connections[playerIndex].SessionToken);

                if(data == "INVALIDATE")
                {
                    CloseConnection(playerIndex);
                }

                int tableIndex = FindTableFromId(Connections[playerIndex].TableId);
                if(tableIndex == -1)
                {
                    WriteLine("UpdatePlayerBalance: Can't find table index " + Connections[playerIndex].TableId, Color.Red);
                }

                int player;
                for (int j = 0; j < TableList[tableIndex].NumConnected; j++)
                {
                    player = TableList[tableIndex].PlayersConnected[j];
                    if (player == playerIndex)
                    {
                        WriteToClient(player, "BALANCE UPDATE FOR YOU " + Connections[player].BalanceOnTable);
                    }
                    else
                    {
                        WriteToClient(player, "BALANCE UPDATE FOR PLAYER " + Connections[playerIndex].SittingAt + " " + Connections[playerIndex].BalanceOnTable);
                    }
                }
            }
        }

        private void UpdatePlayerBalancesOnTable(int tableIndex)
        {
            for (int i = 0; i < TableList[tableIndex].NumSat; i++)
            {
                UpdatePlayerBalance(TableList[tableIndex].PlayersSat[i]);
            }
        }

        private int FindTableFromId(string tableId)
        {
            for (int i = 0; i < TableList.Length; i++)
            {
                if(TableList[i] == null)
                {
                    break;
                }

                if(TableList[i].Id == tableId)
                {
                    return i;
                }
            }

            return -1;
        }

        private void Clear(int tableIndex)
        {
            // Put players into table who are waiting in sit que
            while(TableList[tableIndex].NumSitQue > 0)
            {
                if (TableList[tableIndex].NumSat >= TableList[tableIndex].TableSize)
                {
                    break;
                }

                TableList[tableIndex].NumSat++;
                TableList[tableIndex].PlayersSat[TableList[tableIndex].NumSat - 1] = TableList[tableIndex].PlayersSitQue[0];

                // remove from sit que
                for (int i = 0; i < TableList[tableIndex].NumSitQue - 1; i++)
                {
                    TableList[tableIndex].PlayersSitQue[i] = TableList[tableIndex].PlayersSitQue[i + 1];
                }

                TableList[tableIndex].NumSitQue--;
            }

            // Start a new hand
            int player;
            TableList[tableIndex].RaiseMin = TableList[tableIndex].SmallBlind;
            TableList[tableIndex].TotalRaiseMin = TableList[tableIndex].SmallBlind;
            TableList[tableIndex].NumHand = 0;
            TableList[tableIndex].Pot = 0;

            SetDealer(tableIndex);
            HandleTableCards(tableIndex);
            RaiseMinChanged(tableIndex);
            int playersLost = 0;

            for (int i = 0; i < TableList[tableIndex].NumConnected; i++)
            {
                player = TableList[tableIndex].PlayersConnected[i];
                WriteToClient(player, "CLEAR");

                Connections[player].MyTurn = false;
                Connections[player].TurnTimerOn = false;

                if (Connections[player].SittingAt != -1)
                {
                    if (Connections[player].BalanceOnTable < TableList[tableIndex].SmallBlind * 20 && !Connections[player].AutoRebuy)
                    {
                        // sitout
                        playersLost++;
                        PlayerLose(player);
                    }
                    else
                    {
                        Connections[player].Folded = false;
                        Connections[player].Called = false;
                        Connections[player].Allin = false;
                        Connections[player].Raised = false;
                        Connections[player].RaisedTo = 0;
                        Connections[player].BalanceOnPlay = 0;

                        // auto top off
                        if (Connections[player].BalanceOnTable < TableList[tableIndex].SmallBlind * 400 && Connections[player].AutoTopOff)
                        {
                            if (Connections[player].Balance < TableList[tableIndex].SmallBlind * 400)
                            {
                                WriteToClient(player, "PARTIAL AUTO TOP OFF: " + Connections[player].Balance);
                                Connections[player].BalanceOnTable = Connections[player].Balance;
                                Connections[player].Balance = 0;
                            }
                            else
                            {
                                WriteToClient(player, "FULL AUTO TOP OFF: " + Connections[player].Balance);
                                Connections[player].BalanceOnTable = TableList[tableIndex].SmallBlind * 400;
                                Connections[player].Balance -= Connections[player].BalanceOnTable;
                            }
                        }
                        // auto rebuy
                        else if (Connections[player].BalanceOnTable <= 0 && Connections[player].AutoRebuy)
                        {
                            if (Connections[player].Balance < TableList[tableIndex].SmallBlind * 20)
                            {
                                WriteToClient(player, "CAN'T AUTO REBUY");

                                // sitout
                                playersLost++;
                                PlayerLose(player);
                            }
                            else if (Connections[player].Balance < Connections[player].AmountToSit)
                            {
                                WriteToClient(player, "PARTIAL AUTO REBUY");

                                Connections[player].BalanceOnTable = Connections[player].Balance;
                                Connections[player].Balance = 0;
                            } else
                            {
                                WriteToClient(player, "FULL AUTO REBUY " + Connections[player].AmountToSit);
                                long diff = Connections[player].AmountToSit;
                                Connections[player].Balance -= diff;
                                Connections[player].BalanceOnTable += diff;
                            }
                        }
                    }
                }
            }

            UpdatePlayerBalancesOnTable(tableIndex);

            if (playersLost != 0)
            {
                // Allow clients to display the sitout animation
                Thread.Sleep(1000);
            }

            // Allow clients to display transition screen
            Thread.Sleep(1000);
        }

        private void SetDealer(int tableIndex)
        {
            TableList[tableIndex].Dealer = (TableList[tableIndex].Dealer + 1) % TableList[tableIndex].NumSat; // (TableList[tableIndex].Dealer + 1) % TableList[tableIndex].NumSat;
            int player;
            for (int j = 0; j < TableList[tableIndex].NumConnected; j++)
            {
                player = TableList[tableIndex].PlayersConnected[j];
                WriteToClient(player, "DEALER " + (TableList[tableIndex].Dealer + 1));
            }
        }

        private void PlayerLose(int index)
        {
            int found = FindTableFromId(Connections[index].TableId);
            if (found != -1)
            {
                // perform the operation
                for (int i = Connections[index].SittingAt; i < TableList[found].NumSat - 1; i++)
                {
                    TableList[found].PlayersSat[i] = TableList[found].PlayersSat[i + 1];
                }

                TableList[found].NumSat--;

                // notify other players
                int player;
                for (int i = 0; i < TableList[found].NumConnected; i++)
                {
                    player = TableList[found].PlayersConnected[i];
                    if (player == index)
                    {
                        WriteToClient(player, "YOU HAVE SITOUT");
                    }
                    else
                    {
                        WriteToClient(player, "PLAYER " + Connections[index].SittingAt + " HAS SITOUT");
                    }
                }
            }

            Connections[index].SittingAt = -1;
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("User-Agent", "Gravity 0.1"); // PostmanRuntime/7.19.0
                string data = wc.DownloadString(Host + "srv_user_stand_up.php?user_id=" + Connections[index].UserID + "&token=" + Connections[index].SessionToken + "&room_id=" + Connections[index].TableId);
            }
        }

        private void StartNewTurn(int tableIndex)
        {
            if(TableList[tableIndex].NumSat < 2)
            {
                return;
            }

            TableList[tableIndex].RaiseMin = TableList[tableIndex].SmallBlind;
            //TableList[tableIndex].TotalRaiseMin = TableList[tableIndex].SmallBlind;
            switch (TableList[tableIndex].NumHand)
            {
                case 2:
                    // Street
                    for (int i = 0; i < TableList[tableIndex].NumConnected; i++)
                    {
                        WriteToClient(Connections[TableList[tableIndex].PlayersConnected[i]].Index, "STREET " + CardInfoToString(TableList[tableIndex].CardsOnTheTable[3]));
                    }

                    break;
                case 3:
                    // River
                    for (int i = 0; i < TableList[tableIndex].NumConnected; i++)
                    {
                        WriteToClient(Connections[TableList[tableIndex].PlayersConnected[i]].Index, "RIVER " + CardInfoToString(TableList[tableIndex].CardsOnTheTable[4]));
                    }

                    break;
            }

            int player;
            TableList[tableIndex].NumHand++;
            for (int i = 0; i < TableList[tableIndex].NumConnected; i++)
            {
                player = TableList[tableIndex].PlayersConnected[i];

                Connections[player].TurnTimerOn = false;
                if (Connections[player] == null)
                {
                    WriteLine("Error 1005", Color.Red);
                    return;
                }

                WriteToClient(player, "NEW TURN");
                
                if (Connections[player].SittingAt != -1 && !Connections[player].Folded)
                {
                    //Connections[player].Folded = false;
                    Connections[player].Called = false;
                    Connections[player].Raised = false;
                    Connections[player].RaisedTo = 0;
                }
            }

            if(TableList[tableIndex].NumHand == 2)
            {
                // Show everyone their hands
                for (int i = 0; i < TableList[tableIndex].NumSat; i++)
                {
                    WriteToClient(Connections[TableList[tableIndex].PlayersSat[i]].Index, "YOU HAVE " + CardInfoToString(Connections[TableList[tableIndex].PlayersSat[i]].CardDeck[0]) + " " + CardInfoToString(Connections[TableList[tableIndex].PlayersSat[i]].CardDeck[1]));
                }

                for (int i = 0; i < TableList[tableIndex].NumConnected; i++)
                {
                    WriteToClient(Connections[TableList[tableIndex].PlayersConnected[i]].Index, "FLOP " + CardInfoToString(TableList[tableIndex].CardsOnTheTable[0]) + " " + CardInfoToString(TableList[tableIndex].CardsOnTheTable[1]) + " " + CardInfoToString(TableList[tableIndex].CardsOnTheTable[2]));
                }

                Thread.Sleep(3000);
            }

            int loopCount = 0;
            _newDealer:
            int whoseTurn = -1;
            for (int i = 0; i < TableList[tableIndex].NumSat; i++)
            {
                player = TableList[tableIndex].PlayersSat[i];
                Connections[player].MyTurn = TableList[tableIndex].Dealer == i;

                if(TableList[tableIndex].Dealer == i)
                {
                    whoseTurn = TableList[tableIndex].PlayersSat[i];
                }
            }

            if(whoseTurn == -1)
            {
                if(++loopCount >= 3)
                {
                    WriteLine("Kod 1025: StartNewTurn yeni dealer belirlemede sonsuz döngü oluştu", Color.Red);
                    return;
                }

                // dealer might have been disconnected
                SetDealer(tableIndex);
                goto _newDealer;
            }

            //SkipToNextPlayer(tableIndex);
            NotifyTurnAndStartTurnTime(tableIndex, whoseTurn);
        }

        private string CardInfoToString(CardInfo cardInfo)
        {
            string result = "";

            switch(cardInfo.Type)
            {
                case CardType.Clubs:
                    result += "C";
                    break;
                case CardType.Diamonds:
                    result += "D";
                    break;
                case CardType.Hearts:
                    result += "H";
                    break;
                case CardType.Spades:
                    result += "S";
                    break;
            }

            return result + cardInfo.Number.ToString();
        }

        private void DealHand(int tableIndex)
        {
            #if _HARDCODED_TEST
            Connections[TableList[tableIndex].PlayersSat[0]].CardDeck[0] = new CardInfo(CardType.Clubs, 5);
            Connections[TableList[tableIndex].PlayersSat[0]].CardDeck[1] = new CardInfo(CardType.Hearts, 6);

            Connections[TableList[tableIndex].PlayersSat[1]].CardDeck[0] = new CardInfo(CardType.Spades, 10);
            Connections[TableList[tableIndex].PlayersSat[1]].CardDeck[1] = new CardInfo(CardType.Diamonds, 3);

            if (TableList[tableIndex].NumSat > 2)
            {
                Connections[TableList[tableIndex].PlayersSat[2]].CardDeck[0] = new CardInfo(CardType.Hearts, 6);
                Connections[TableList[tableIndex].PlayersSat[2]].CardDeck[1] = new CardInfo(CardType.Diamonds, 2);
            }
            #else
            for (int i = 0; i < TableList[tableIndex].NumSat; i++)
            {
                Connections[TableList[tableIndex].PlayersSat[i]].CardDeck[0] = TakeNextCard(tableIndex);
                Connections[TableList[tableIndex].PlayersSat[i]].CardDeck[1] = TakeNextCard(tableIndex);
            }
            #endif
        }

        private CardInfo TakeNextCard(int tableIndex)
        {
            CardInfo result = TableList[tableIndex].CardIndex >= 52 ? null : TableList[tableIndex].CardDeck[TableList[tableIndex].CardIndex];

            TableList[tableIndex].CardIndex++;
            return result;
        }

        private void MixUpCards(int tableIndex)
        {
            if(CardDeckTidy == null)
            {
                CardDeckTidy = new CardInfo[52];

                CardDeckTidy[0] = new CardInfo(CardType.Clubs, CardInfo.CardA);
                CardDeckTidy[1] = new CardInfo(CardType.Clubs, 2);
                CardDeckTidy[2] = new CardInfo(CardType.Clubs, 3);
                CardDeckTidy[3] = new CardInfo(CardType.Clubs, 4);
                CardDeckTidy[4] = new CardInfo(CardType.Clubs, 5);
                CardDeckTidy[5] = new CardInfo(CardType.Clubs, 6);
                CardDeckTidy[6] = new CardInfo(CardType.Clubs, 7);
                CardDeckTidy[7] = new CardInfo(CardType.Clubs, 8);
                CardDeckTidy[8] = new CardInfo(CardType.Clubs, 9);
                CardDeckTidy[9] = new CardInfo(CardType.Clubs, 10);
                CardDeckTidy[10] = new CardInfo(CardType.Clubs, CardInfo.CardJ);
                CardDeckTidy[11] = new CardInfo(CardType.Clubs, CardInfo.CardQ);
                CardDeckTidy[12] = new CardInfo(CardType.Clubs, CardInfo.CardK);

                CardDeckTidy[13] = new CardInfo(CardType.Diamonds, CardInfo.CardA);
                CardDeckTidy[14] = new CardInfo(CardType.Diamonds, 2);
                CardDeckTidy[15] = new CardInfo(CardType.Diamonds, 3);
                CardDeckTidy[16] = new CardInfo(CardType.Diamonds, 4);
                CardDeckTidy[17] = new CardInfo(CardType.Diamonds, 5);
                CardDeckTidy[18] = new CardInfo(CardType.Diamonds, 6);
                CardDeckTidy[19] = new CardInfo(CardType.Diamonds, 7);
                CardDeckTidy[20] = new CardInfo(CardType.Diamonds, 8);
                CardDeckTidy[21] = new CardInfo(CardType.Diamonds, 9);
                CardDeckTidy[22] = new CardInfo(CardType.Diamonds, 10);
                CardDeckTidy[23] = new CardInfo(CardType.Diamonds, CardInfo.CardJ);
                CardDeckTidy[24] = new CardInfo(CardType.Diamonds, CardInfo.CardQ);
                CardDeckTidy[25] = new CardInfo(CardType.Diamonds, CardInfo.CardK);

                CardDeckTidy[26] = new CardInfo(CardType.Hearts, CardInfo.CardA);
                CardDeckTidy[27] = new CardInfo(CardType.Hearts, 2);
                CardDeckTidy[28] = new CardInfo(CardType.Hearts, 3);
                CardDeckTidy[29] = new CardInfo(CardType.Hearts, 4);
                CardDeckTidy[30] = new CardInfo(CardType.Hearts, 5);
                CardDeckTidy[31] = new CardInfo(CardType.Hearts, 6);
                CardDeckTidy[32] = new CardInfo(CardType.Hearts, 7);
                CardDeckTidy[33] = new CardInfo(CardType.Hearts, 8);
                CardDeckTidy[34] = new CardInfo(CardType.Hearts, 9);
                CardDeckTidy[35] = new CardInfo(CardType.Hearts, 10);
                CardDeckTidy[36] = new CardInfo(CardType.Hearts, CardInfo.CardJ);
                CardDeckTidy[37] = new CardInfo(CardType.Hearts, CardInfo.CardQ);
                CardDeckTidy[38] = new CardInfo(CardType.Hearts, CardInfo.CardK);

                CardDeckTidy[39] = new CardInfo(CardType.Spades, CardInfo.CardA);
                CardDeckTidy[40] = new CardInfo(CardType.Spades, 2);
                CardDeckTidy[41] = new CardInfo(CardType.Spades, 3);
                CardDeckTidy[42] = new CardInfo(CardType.Spades, 4);
                CardDeckTidy[43] = new CardInfo(CardType.Spades, 5);
                CardDeckTidy[44] = new CardInfo(CardType.Spades, 6);
                CardDeckTidy[45] = new CardInfo(CardType.Spades, 7);
                CardDeckTidy[46] = new CardInfo(CardType.Spades, 8);
                CardDeckTidy[47] = new CardInfo(CardType.Spades, 9);
                CardDeckTidy[48] = new CardInfo(CardType.Spades, 10);
                CardDeckTidy[49] = new CardInfo(CardType.Spades, CardInfo.CardJ);
                CardDeckTidy[50] = new CardInfo(CardType.Spades, CardInfo.CardQ);
                CardDeckTidy[51] = new CardInfo(CardType.Spades, CardInfo.CardK);
            }

            Random r = new Random(Environment.TickCount);
            int next;
            int[] mixedList = new int[52];

            // init
            for (int i = 0; i < 52; i++)
            {
                mixedList[i] = -1;
            }

            // do
            for (int i = 0; i < 52; i++)
            {
                do
                {
                    next = r.Next(0, 52);
                } while (mixedList.Contains(next));

                mixedList[i] = next;
            }

            for (int i = 0; i < 52; i++)
            {
                TableList[tableIndex].CardDeck[i] = CardDeckTidy[mixedList[i]];
            }

            TableList[tableIndex].CardIndex = 0;
        }

        private void SkipToNextPlayer(int currentTable)
        {
            int whoseTurn = -1;
            int player;

            for (int i = 0; i < TableList[currentTable].NumSat; i++)
            {
                player = TableList[currentTable].PlayersSat[i];
                if (Connections[player].MyTurn)
                {
                    whoseTurn = i;
                    break;
                }
            }

            if (whoseTurn == -1)
            {
                whoseTurn = 0;
            } else
            {
                do
                {
                    whoseTurn = (whoseTurn + 1) % TableList[currentTable].NumSat;
                    player = TableList[currentTable].PlayersSat[whoseTurn];
                } while (Connections[player].Folded);
            }

            NotifyTurnAndStartTurnTime(currentTable, TableList[currentTable].PlayersSat[whoseTurn]);
        }

        private void NotifyTurnAndStartTurnTime(int currentTable, int whoseTurn)
        {
            int player;

            // Let everyone know in the table about whose turn is it
            for (int i = 0; i < TableList[currentTable].NumConnected; i++)
            {
                player = TableList[currentTable].PlayersConnected[i];
                if (player == whoseTurn)
                {
                    // Notify client about its time has started
                    WriteToClient(Connections[player].Index, "YOUR TURN");

                    // Start timer as well
                    Connections[player].MyTurn = true;
                    Connections[player].Called = false;
                    Connections[player].Raised = false;
                    Connections[player].Folded = false;
                    Connections[player].RaisedTo = 0;
                    Connections[player].TurnTimerOn = true;
                    Connections[player].TurnTimer = new Thread((ind) =>
                    {
                        int index = (int)ind;

                        // Fast? Slow?
                        for (int j = 0; j < (TableList[currentTable].Fast ? 100 : 200); j++)
                        {
                            Thread.Sleep(100);
                            if (Connections[index] == null || !Connections[index].MyTurn || !Connections[index].TurnTimerOn)
                            {
                                return;
                            }
                        }

                        if (Connections[index].MyTurn)
                        {
                            int handsMissed = ++Connections[index].HandsMissed;
                            switch (handsMissed)
                            {
                                case 1:
                                    PlayerCall(index);
                                    break;
                                case 2:
                                    PlayerStandUp(index);
                                    break;
                            }

                            Connections[index].HandsMissed = handsMissed;
                        }

                        TurnResult allDone = AllDone(currentTable);
                        switch (allDone)
                        {
                            case TurnResult.NextPlayer:
                                Application.DoEvents();
                                Thread.Sleep(1000);

                                if (!Connections[index].TurnTimerOn)
                                {
                                    return;
                                }

                                Connections[index].TurnTimerOn = false;
                                SkipToNextPlayer(currentTable);
                                break;
                            default:
                                WriteLine("Unexpected result on turn timer: " + allDone.ToString());
                                break;
                        }
                    });

                    Connections[player].TurnTimerStartedAt = DateTime.Now;
                    Connections[player].TurnTimer.Start(player);
                }
                else
                {
                    WriteToClient(Connections[player].Index, "TURN OF PLAYER " + Connections[whoseTurn].SittingAt);
                    Connections[player].MyTurn = false;
                    Connections[player].TurnTimerOn = false;
                }
            }
        }

        private void PlayerAllin(int index)
        {
            if (!Connections[index].Active || Connections[index] == null)
            {
                WriteToClient(index, "PLAYER IS GONE");
                return;
            }

            Connections[index].HandsMissed = 0;
            Connections[index].Raised = false;
            Connections[index].Allin = true;

            int found = FindTableFromId(Connections[index].TableId);
            //TableList
            if (found != -1)
            {
                Connections[index].Balance -= Connections[index].BalanceOnTable;
                Connections[index].BalanceOnPlay += Connections[index].BalanceOnTable;
                Connections[index].BalanceOnTable = 0;

                //raiseMinOld = TableList[found].RaiseMin;
                //TableList[found].RaiseMin = Connections[index].BalanceOnPlay;
                TableList[found].TotalRaiseMin += Connections[index].BalanceOnPlay;
                TableList[found].Pot += TableList[found].RaiseMin;

                UpdatePlayerBalance(index);

                for (int i = 0; i < TableList[found].NumConnected; i++)
                {
                    WriteToClient(TableList[found].PlayersConnected[i], "ALLIN " + Connections[index].SittingAt + " " + TableList[found].RaiseMin);//(Connections[index].BalanceOnTable + Connections[index].BalanceOnPlay));
                }

                if (AllDone(found) == TurnResult.NextPlayer)
                {
                    Application.DoEvents();
                    Thread.Sleep(1000);

                    SkipToNextPlayer(found);
                }
                else
                {
                    DecideWinner(found);
                }
            }
        }

        private void PlayerRaiseTo(int index, long amount)
        {
            if (!Connections[index].Active || Connections[index] == null)
            {
                WriteToClient(index, "PLAYER IS GONE");
                return;
            }

            if(Connections[index].Allin)
            {
                WriteToClient(index, "CAN'T RAISE BECAUSE ALLIN");
            }

            int found = FindTableFromId(Connections[index].TableId);
            if (found != -1)
            {
                Connections[index].HandsMissed = 0;

                if (amount > TableList[found].SmallBlind * 400)
                {
                    amount = TableList[found].SmallBlind * 400;
                }

                if (amount < TableList[found].SmallBlind * 2)
                {
                    amount = TableList[found].SmallBlind * 2;
                }

                if (TableList[found].RaiseMin < amount)
                {
                    TableList[found].RaiseMin = amount;
                } else
                {
                    if (Connections[index].BalanceOnPlay + Connections[index].BalanceOnTable > amount ||
                        amount < TableList[found].SmallBlind * 400)
                    {
                        WriteToClient(index, "RAISE ERROR");
                    }

                    return;
                }

                //WriteLine("TotalRaiseMin: " + TableList[found].TotalRaiseMin);
                TableList[found].TotalRaiseMin += TableList[found].RaiseMin;
                RaiseMinChanged(found);

                Connections[index].RaisedTo += amount;
                Connections[index].Raised = true;

                if (Connections[index].BalanceOnPlay + Connections[index].BalanceOnTable <= amount ||
                    amount >= TableList[found].SmallBlind * 400 ||
                    TableList[found].TotalRaiseMin >= TableList[found].SmallBlind * 400)
                {
                    PlayerAllin(index);
                    return;
                }

                Connections[index].BalanceOnPlay += amount;
                Connections[index].BalanceOnTable -= amount;
                Connections[index].Balance -= amount;

                TableList[found].Pot += amount; //Connections[index].BalanceOnPlay;
                UpdatePlayerBalance(index);

                // Clear all other players' turn
                int player;
                for (int i = 0; i < TableList[found].NumSat; i++)
                {
                    player = TableList[found].PlayersSat[i];
                    if(player == index)
                    {
                        continue;
                    }

                    if (!Connections[player].Folded && !Connections[player].Allin && Connections[player].Raised)
                    {
                        Connections[player].Raised = false;
                    }

                    if (!Connections[player].Folded && !Connections[player].Allin && Connections[player].Called)
                    {
                        Connections[player].Called = false;
                    }
                }

                for (int i = 0; i < TableList[found].NumConnected; i++)
                {
                    WriteToClient(TableList[found].PlayersConnected[i], "RAISE BY " + Connections[index].SittingAt + " " + TableList[found].RaiseMin);
                }

                if (AllDone(found) == TurnResult.NextPlayer)
                {
                    SkipToNextPlayer(found);
                }
                else
                {
                    DecideWinner(found);
                }
            }
        }

        private void RaiseMinChanged(int tableIndex)
        {
            int player;
            for (int i = 0; i < TableList[tableIndex].NumSat; i++)
            {
                player = TableList[tableIndex].PlayersSat[i];
                WriteToClient(player, "RAISEMIN " + TableList[tableIndex].TotalRaiseMin);
            }
        }

        private TurnResult AllDone(int found)
        {
            for (int i = 0; i < TableList[found].NumSat; i++)
            {
                if (!Connections[TableList[found].PlayersSat[i]].Called && !Connections[TableList[found].PlayersSat[i]].Folded && !Connections[TableList[found].PlayersSat[i]].Raised && !Connections[TableList[found].PlayersSat[i]].Allin)
                {
                    return TurnResult.NextPlayer;
                }
            }

            for (int i = 0; i < TableList[found].NumSat; i++)
            {
                if (Connections[TableList[found].PlayersSat[i]].Allin)
                {
                    return TurnResult.Immediate;
                }
            }

            return TurnResult.NextTurn;
        }

        private void PlayerCall(int index)
        {
            if (!Connections[index].Active || Connections[index] == null)
            {
                WriteToClient(index, "PLAYER IS GONE");
                return;
            }

            Connections[index].Called = true;
            Connections[index].HandsMissed = 0;

            int found = FindTableFromId(Connections[index].TableId);
            if (found != -1)
            {
                //long raiseMinOld = Connections[index].RaiseMin;
                if (TableList[found].RaiseMin >= Connections[index].BalanceOnTable)
                {
                    // allin
                    //long diff = Connections[index].BalanceOnTable - Connections[index].BalanceOnPlay;
                    Connections[index].Called = false;
                    Connections[index].Raised = false;
                    Connections[index].Allin = true;
                    Connections[index].Balance -= TableList[found].RaiseMin;
                    Connections[index].BalanceOnPlay = Connections[index].BalanceOnTable;
                    Connections[index].BalanceOnTable = 0;

                    TableList[found].Pot += TableList[found].RaiseMin; //Connections[index].BalanceOnPlay;
                }
                else
                {
                    // raise or call
                    Connections[index].Allin = false;
                    Connections[index].BalanceOnPlay += TableList[found].RaiseMin;
                    Connections[index].BalanceOnTable -= TableList[found].RaiseMin;
                    Connections[index].Balance -= TableList[found].RaiseMin;

                    TableList[found].Pot += TableList[found].RaiseMin; //Connections[index].BalanceOnPlay;
                }

                UpdatePlayerBalance(index);

                // All done?
                TurnResult allDone = AllDone(found);
                for (int i = 0; i < TableList[found].NumConnected; i++)
                {
                    if (!Connections[index].Allin)
                    {
                        WriteToClient(TableList[found].PlayersConnected[i], "CALL " + Connections[index].SittingAt + " " + TableList[found].RaiseMin);
                    } else
                    {
                        WriteToClient(TableList[found].PlayersConnected[i], "ALLIN " + Connections[index].SittingAt + " " + (Connections[index].BalanceOnPlay + Connections[index].BalanceOnTable));
                    }
                }

                Application.DoEvents();
                Thread.Sleep(1000);

                if (allDone == TurnResult.NextPlayer)
                {
                    Application.DoEvents();
                    Thread.Sleep(1000);

                    /*if (TableList[found].NumHand < 4)
                    {*/
                        SkipToNextPlayer(found);
                    /*}
                    else
                    {
                        DecideWinner(found);
                    }*/
                }
                else
                {
                    for (int i = 0; i < TableList[found].NumConnected; i++)
                    {
                        WriteToClient(TableList[found].PlayersConnected[i], "POT " + TableList[found].Pot);
                    }

                    if (TableList[found].NumHand < 4)
                    {
                        if (allDone == TurnResult.Immediate)
                        {
                            DecideWinner(found);
                        }
                        else
                        {
                            StartNewTurn(found);
                        }
                    } else
                    {
                        DecideWinner(found);
                    }
                }
            } else
            {
                WriteLine("Table not found", Color.Red);
            }
        }

        private void PlayerFold(int index)
        {
            PlayerFold(index, true);
        }

        private void PlayerFold(int index, bool evaluateWinner)
        {
            string tableId = Connections[index].TableId;
            if(tableId == null)
            {
                return;
            }

            //
            int found = FindTableFromId(Connections[index].TableId);
            if (found != -1)
            {
                int player;
                Connections[index].HandsMissed = 0;
                for (int i = 0; i < TableList[found].NumSat; i++)
                {
                    player = TableList[found].PlayersSat[i];
                    if (Connections[player] != null && Connections[player].Active)
                    {
                        if (player == index)
                        {
                            WriteToClient(player, "YOU FOLDED");

                            Connections[player].Folded = true;
                        }
                        else
                        {
                            WriteToClient(player, "SEAT " + Connections[index].SittingAt + " FOLDED");
                        }
                    }
                }

                int numFolded = 0;
                int numCalled = 0;
                int nonFolded = -1;

                for (int i = 0; i< TableList[found].NumSat; i++)
                {
                    if(Connections[TableList[found].PlayersSat[i]] == null || Connections[TableList[found].PlayersSat[i]].Folded)
                    {
                        numFolded++;
                    } else
                    {
                        nonFolded = i;

                        if (Connections[TableList[found].PlayersSat[i]].Called)
                        {
                            numCalled++;
                        }
                    }
                }

                if(!evaluateWinner)
                {
                    return;
                }

                if (numFolded == TableList[found].NumSat - 1)
                {
                    // the one who non-folded should win
                    PlayerWin(new WinningHand[] { new WinningHand(nonFolded, "AOF", "N") }, false);
                    IncrementPlayerXP(found, TableList[found].PlayersSat[nonFolded], 200);

                    // Start a new turn if there are more than one people sitting
                    if (TableList[found].NumSat > 1)
                    {
                        for (int i = 0; i < TableList[found].NumSat; i++)
                        {
                            if(nonFolded == i)
                            {
                                continue;
                            }

                            IncrementPlayerXP(found, TableList[found].PlayersSat[i], 40);
                        }

                        Application.DoEvents();
                        Thread.Sleep(5000);

                        Clear(found);
                        StartNewTurn(found);
                    }
                }
                else
                {
                    switch(AllDone(found))
                    {
                        case TurnResult.Immediate:
                            DecideWinner(found);
                            break;
                        case TurnResult.NextPlayer:
                            SkipToNextPlayer(found);
                            break;
                        case TurnResult.NextTurn:
                            StartNewTurn(found);
                            break;
                    }
                    
                }
            }
        }

        private void ClearBuffer(byte[] buffer, int length)
        {
            for (int i = 0; i < length; i++)
            {
                buffer[i] = 0;
            }
        }

        private void HandleClientIdentification(int id, byte[] buffer, int dataLength)
        {
            string message = Encoding.ASCII.GetString(buffer);

            if(!message.StartsWith("IDENTIFY "))
            {
                WriteToClient(id, "NOT IDENTIFIED");
                CloseConnection(id);

                return;
            }

            string[] parts = message.Split(' ');
            string sessionUser = parts[1];
            string sessionToken = parts[2].Replace("\0", "").Replace("\n", "");

            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Headers.Add("User-Agent", "Gravity 0.1"); // PostmanRuntime/7.19.0
                    wc.Encoding = Encoding.UTF8;
                    string data = wc.DownloadString(Host + "srv_verify_user_info.php?user=" + sessionUser + "&token=" + sessionToken);

                    if (data == "N/A")
                    {
                        WriteToClient(id, "NOT IDENTIFIED");
                        CloseConnection(id);

                        return;
                    }

                    string[] info = data.Split('~');
                    Connections[id].SessionUser = sessionUser;
                    Connections[id].SessionToken = sessionToken;
                    Connections[id].UserID = info[0];
                    Connections[id].FullName = info[1];
                    Connections[id].Level = int.Parse(info[2]);
                    Connections[id].Balance = long.Parse(info[3]);
                    Connections[id].XP = int.Parse(info[4]);
                    Connections[id].XPToLevel = (LevelIntervals.Length > Connections[id].Level - 1 ? LevelIntervals[Connections[id].Level - 1] : LevelIntervals[LevelIntervals.Length - 1]) - Connections[id].XP;
                    Connections[id].Avatar = int.Parse(info[5]);
                    Connections[id].AvatarFile = info[6];

                    // Same user is already inside?
                    for (int i = 0; i < OccupiedThreads; i++)
                    {
                        if(i == id)
                        {
                            continue;
                        }

                        if(Connections[i] != null && Connections[i].UserID == Connections[id].UserID && Connections[i].Active)
                        {
                            CloseConnection(i);
                        }
                    }
                }
            } catch(Exception ex)
            {
                WriteToClient(id, "SERVER ERROR");
                WriteLine("Handle identify error: " + ex.Message, Color.Red);

                CloseConnection(id);
                return;
            }

            Connections[id].State = ConnectionInfo.ConnectionState.JoinRoom;
            WriteToClient(id, "Identification OK");
        }

        private void FetchRoomInfo()
        {
            using(WebClient wc = new WebClient())
            {
                wc.Headers.Add("User-Agent", "Gravity 0.1"); // PostmanRuntime/7.19.0
                string data = wc.DownloadString(Host + "srv_get_casino_info.php?casino_id="+ CasinoId +"&casino_type=" + CasinoType);

                JObject o = JObject.Parse(data);
                JArray tableList = (JArray)o["tables"];

                OccupiedTables = tableList.Count;
                for (int i = 0; i < tableList.Count; i++)
                {
                    TableList[i] = new TableInfo(tableList[i]["id"].ToString());

                    TableList[i].NumConnected = 0;
                    TableList[i].NumSat = 0;
                    TableList[i].TableSize = int.Parse(tableList[i]["size"].ToString());
                    TableList[i].Fast = tableList[i]["speed"].ToString() == "1";
                    TableList[i].SmallBlind = long.Parse(tableList[i]["small"].ToString());
                    TableList[i].BigBlind = long.Parse(tableList[i]["big"].ToString());
                    TableList[i].Private = tableList[i]["private"].ToString() == "1";
                    TableList[i].Vip = tableList[i]["vip"].ToString() == "1";
                    TableList[i].OwnedBy = int.Parse(tableList[i]["owner"].ToString());

                    TableList[i].State = TableInfo.TableState.Idle;
                }
            }
        }

        private void HandleJoinRoomCommand(int id, byte[] buffer, int dataLength)
        {
            string messages = Encoding.ASCII.GetString(buffer).Replace("\0", "");
            string[] messageList = messages.Split('\n');
            string message;

            foreach (string m in messageList)
            {
                message = m.Replace("\n","");
                if(message.Trim() == "")
                {
                    continue;
                }

                if (TestScriptReadMode)
                {
                    if (!TestMode)
                    {
                        WriteLine("TEST INSTRUCTION ATTEMPT!!!", Color.Red);
                        continue;
                    }

                    AppendToTestScript(message);
                    continue;
                }

                if (message.StartsWith("JOIN TO "))
                {
                    int found = -1;
                    string roomId = message.Substring("JOIN TO ".Length).Replace("\0", "");
                    found = FindTableFromId(roomId);

                    if (found == -1)
                    {
                        // room not found
                        WriteToClient(id, "ROOM NOT FOUND, Sorry");
                        continue;
                    }

                    // Room is full?
                    if (TableList[found].NumConnected >= 20)
                    {
                        WriteToClient(id, "TABLE FULL, Sorry");
                        continue;
                    }

                    // Room is available?
                    if (TableList[found].Vip || TableList[found].Private)
                    {
                        WriteToClient(id, "NOT ALLOWED, Sorry");
                        continue;
                    }

                    // Add player to room
                    if (SetPlayerTable(id, roomId))
                    {
                        TableList[found].NumConnected++;
                        TableList[found].PlayersConnected[TableList[found].NumConnected - 1] = id;

                        string tableStateMessage = "";
                        switch(TableList[found].NumHand)
                        {
                            case 2:
                                tableStateMessage = "TABLE STATE " + TableList[found].NumHand + " " + CardInfoToString(TableList[found].CardsOnTheTable[0]) + " " + CardInfoToString(TableList[found].CardsOnTheTable[1]) + " " + CardInfoToString(TableList[found].CardsOnTheTable[2]);
                                break;
                            case 3:
                                tableStateMessage = "TABLE STATE " + TableList[found].NumHand + " " + CardInfoToString(TableList[found].CardsOnTheTable[0]) + " " + CardInfoToString(TableList[found].CardsOnTheTable[1]) + " " + CardInfoToString(TableList[found].CardsOnTheTable[2]) + " " + CardInfoToString(TableList[found].CardsOnTheTable[3]);
                                break;
                            case 4:
                                tableStateMessage = "TABLE STATE " + TableList[found].NumHand + " " + CardInfoToString(TableList[found].CardsOnTheTable[0]) + " " + CardInfoToString(TableList[found].CardsOnTheTable[1]) + " " + CardInfoToString(TableList[found].CardsOnTheTable[2]) + " " + CardInfoToString(TableList[found].CardsOnTheTable[3]) + " " + CardInfoToString(TableList[found].CardsOnTheTable[4]);
                                break;

                        }

                        if (tableStateMessage != "")
                        {
                            // add player hands if they have been revealed

                            // send to new client
                            WriteToClient(id, tableStateMessage);
                        }

                        Connections[id].State = ConnectionInfo.ConnectionState.InTheRoom;
                    }
                    else
                    {
                        WriteToClient(id, "COULD NOT CONNECT TO TABLE");
                    }
                }
                else if (message == "LEAVE ROOM")
                {
                    LeaveRoom(id);
                }
                else if (message == "VTSCE V1")
                {
                    ReadTestInstructions(id);
                }
                else
                {
                    // refused the command
                    WriteLine("HJRC INVALID (" + message + ")");
                }
            }
        }

        private void AppendToTestScript(string message)
        {
            message = message.Replace("\n", "");
            if (message == "VTSCE RUN")
            {
                ParseTestScript();
                TestScriptReadMode = false;

                ExecuteTestScript();
            }
            else
            {
                TestInstructions.Add(message);
            }
        }

        private void ExecuteTestScript()
        {
            // Make users join
            byte[] buffer;
            TestExecution = true;

            for (int i = 0; i < TestPlayers.Count; i++)
            {
                // identify
                buffer = Encoding.UTF8.GetBytes("IDENTIFY " + Connections[TestPlayers[i]].SessionUser + " " + Connections[TestPlayers[i]].SessionToken + "\n");
                ProcessBuffer(TestPlayers[i], buffer, buffer.Length);

                // join to
                buffer = Encoding.UTF8.GetBytes("JOIN TO " + TestTable + "\n");
                ProcessBuffer(TestPlayers[i], buffer, buffer.Length);
            }

            for (int i = 0; i < TestPlayers.Count; i++)
            {
                // sit
                buffer = Encoding.UTF8.GetBytes("SIT\n");
                ProcessBuffer(TestPlayers[i], buffer, buffer.Length);
            }

            int currentInstruction = 0;
            TestThread = new Thread(() =>
            {
                try
                {
                    int currentPlayer = int.Parse(GameInstructions[currentInstruction].Split(' ')[0]) - 1;

                    TestResponseBufferItem item;
                    while (CanContinue)
                    {
                        item = PopFromResponseBuffer(TestPlayers[currentPlayer]);
                        if (item == null)
                        {
                            Application.DoEvents();
                            Thread.Sleep(100);
                        }
                        else
                        {
                            string instruction = GameInstructions[currentInstruction].Substring(GameInstructions[currentInstruction].IndexOf(" ")).Trim();
                            string[] instructionParts = instruction.Split(':');

                            if (item.Message == instructionParts[0].Trim())
                            {
                                WriteLine(item.Id + " :: " + item.Message, Color.Yellow);

                                WriteLine("Test condition " + instructionParts[0].Trim() + " satisfied, executing command: " + instructionParts[1], Color.LightGreen);
                                byte[] buff = Encoding.UTF8.GetBytes(instructionParts[1].Trim());
                                ProcessBuffer(TestPlayers[currentPlayer], buff, buff.Length);

                                currentInstruction++;
                                if (currentInstruction >= GameInstructions.Count)
                                {
                                    // Test instruction cycle ended with success
                                    break;
                                }

                                currentPlayer = int.Parse(GameInstructions[currentInstruction].Split(' ')[0]) - 1;
                            } else
                            {
                                WriteLine(item.Id + " :: " + item.Message, Color.Gray);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteLine("Error during test cycle: " + ex.Message + " (" + ex.StackTrace + ")");
                }

                TestExecution = false;
            });

            TestThread.Start();
        }

        private void ParseTestScript()
        {
            const int SCRIPTMODE_MAIN = 0;
            const int SCRIPTMODE_PLAYERS = 1;
            const int SCRIPTMODE_GAME = 2;

            //Connections = new ConnectionInfo[PlayerCapacity];
            int scriptMode = SCRIPTMODE_MAIN;
            string[] playerParams;
            byte[] signature;
            string playerId;
            int playerIndex;

            foreach (string scriptCommand in TestInstructions)
            {
                handleCommand:
                switch (scriptMode)
                {
                    case SCRIPTMODE_MAIN:
                        if (scriptCommand.StartsWith("TABLE "))
                        {
                            TestTable = scriptCommand.Substring("TABLE ".Length);
                            if(FindTableFromId(TestTable) == -1)
                            {
                                TestScriptFail("Invalid test table id " + TestTable + " 101");
                                TestTable = null;
                            }
                        }
                        else if (scriptCommand.StartsWith("PLAYERS "))
                        {
                            scriptMode = SCRIPTMODE_PLAYERS;
                        }
                        else if (scriptCommand.StartsWith("HOUSE "))
                        {
                            try
                            {
                                string[] subItems = scriptCommand.Split(' ');
                                CardInfo[] house = new CardInfo[5] { CardInfo.Parse(subItems[1]), CardInfo.Parse(subItems[2]), CardInfo.Parse(subItems[3]), CardInfo.Parse(subItems[4]), CardInfo.Parse(subItems[5])};

                                if(TestTable == null)
                                {
                                    TestScriptFail("Test table id not set" + TestTable + " 301");
                                    return;
                                }

                                int tableIndex = FindTableFromId(TestTable);
                                if (FindTableFromId(TestTable) == -1)
                                {
                                    TestScriptFail("Invalid test table id " + TestTable + " 102");
                                    TestTable = null;
                                }

                                TableList[tableIndex].CardsOnTheTable[0] = house[0];
                                TableList[tableIndex].CardsOnTheTable[1] = house[1];
                                TableList[tableIndex].CardsOnTheTable[2] = house[2];
                                TableList[tableIndex].CardsOnTheTable[3] = house[3];
                                TableList[tableIndex].CardsOnTheTable[4] = house[4];

                                CardsHandledForTest = true;
                            } catch
                            {

                            }
                        }
                        else if (scriptCommand == "GAME")
                        {
                            scriptMode = SCRIPTMODE_GAME;
                        }

                        break;
                    case SCRIPTMODE_PLAYERS:
                        if (scriptCommand.StartsWith("1 ") || scriptCommand.StartsWith("2 ") || scriptCommand.StartsWith("3 ") || scriptCommand.StartsWith("4 ") || scriptCommand.StartsWith("5 "))
                        {
                            ConnectionInfo newConnection = new ConnectionInfo();
                            newConnection.Index = OccupiedThreads;
                            newConnection.IPAddress = "0.0.0.1";
                            newConnection.Active = true;

                            playerParams = scriptCommand.Split(' ');
                            playerId = playerParams[1];
                            //playerIndex = int.Parse(playerParams[0]);
                            playerIndex = OccupiedThreads++;

                            TestPlayers.Add(playerIndex);

                            Connections[playerIndex] = newConnection;
                            Connections[playerIndex].Index = playerIndex;
                            Connections[playerIndex].UserID = playerId;
                            Connections[playerIndex].SessionUser = playerParams[5];
                            Connections[playerIndex].SessionToken = playerParams[6].Replace("\n", "").Replace("\0", "");
                            Connections[playerIndex].Balance = long.Parse(playerParams[2].ToUpper().Replace("K", "000").Replace("M", "000000").Replace("B", "000000000"));

                            signature = new byte[42];
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

                            ProcessBuffer(playerIndex, signature, signature.Length);

                            CardInfo c1 = CardInfo.Parse(playerParams[3]);
                            CardInfo c2 = CardInfo.Parse(playerParams[4]);

                            Connections[playerIndex].CardDeck = new CardInfo[2] { c1, c2 };
                        }
                        else
                        {
                            scriptMode = SCRIPTMODE_MAIN;
                            goto handleCommand;
                        }

                        break;
                    case SCRIPTMODE_GAME:
                        if (scriptCommand.StartsWith("AFTER P"))
                        {
                            GameInstructions.Add(scriptCommand.Substring("AFTER P".Length).Trim());
                        }
                        else if (scriptCommand.StartsWith("ASSERT RESULT "))
                        {
                            ExpectedResult = scriptCommand.Substring("ASSERT RESULT ".Length).Trim();
                        }
                        else
                        {
                            scriptMode = SCRIPTMODE_MAIN;
                            goto handleCommand;
                        }

                        break;
                }
            }
        }

        private void TestScriptFail(string message)
        {
            WriteLine("TEST SCRIPT COMPILER: " + message, Color.Red);
        }

        private void ReadTestInstructions(int id)
        {
            TestScriptReadMode = true;

            TestInstructions = new List<string>();
            GameInstructions = new List<string>();
            TestPlayers = new List<int>();
            ExpectedResult = "";
        }

        private bool SetPlayerTable(int id, string roomId)
        {
            int found = FindTableFromId(roomId);
            if (found == -1)
            {
                // room not found
                WriteToClient(id, "#1005 ROOM NOT FOUND, XO");
                return false;
            }

            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("User-Agent", "Gravity 0.1"); // PostmanRuntime/7.19.0
                string data = wc.DownloadString(Host + "srv_set_user_table.php?casino_id=" + CasinoId + "&room_id=" + roomId + "&user_id=" + Connections[id].UserID);

                if (data != "OK")
                {
                    return false;
                }

                Connections[id].Active = true;

                // PASS 1: Notify every other player about the new user's presence
                // PASS 2: Notify user about room status
                string message = "WELCOME TO TABLE " + roomId + " YOU ARE " + id +
                    "\n" + "THERE ARE " + TableList[found].NumConnected + " OTHER PLAYERS";

                for (int i = 0; i < TableList[found].NumConnected; i++)
                {
                    if (TableList[found].PlayersConnected[i] == id)
                    {
                        continue;
                    }
                    else 
                    {
                        WriteToClient(TableList[found].PlayersConnected[i], "PLAYER JOINED:" + Connections[id].FullName + "," + Connections[id].Avatar + "," + Connections[id].AvatarFile + "," + Connections[id].BalanceOnTable + "," + Connections[id].Level + "," + Connections[id].XP + "," + Connections[id].XPToLevel + "," + id);
                    }
                }

                // List other players
                if (TableList[found].NumConnected > 0)
                {
                    message += "\n" + "OTHER PLAYERS IN THE TABLE ARE: ";
                    int player;
                    for (int j = 0; j < TableList[found].NumConnected; j++)
                    {
                        player = TableList[found].PlayersConnected[j];
                        if (player == id || Connections[player] == null)
                        {
                            continue;
                        }

                        message += "|" + Connections[player].FullName + "," + Connections[player].Avatar + "," + Connections[player].AvatarFile + "," + Connections[player].BalanceOnTable + "," + Connections[player].Level + "," + Connections[player].XP + "," + Connections[player].XPToLevel + "," + Connections[player].SittingAt + "," + Connections[player].Index;
                    }
                }

                // Tell whose turn is it and how much time remained
                for (int i = 0; i < TableList[found].NumSat; i++)
                {
                    int player = TableList[found].PlayersSat[i];
                    if (Connections[player].MyTurn)
                    {
                        message += "\nITS TURN OF " + Connections[player].SittingAt + " REMAINING " + (21 - Math.Floor((DateTime.Now - Connections[player].TurnTimerStartedAt).TotalSeconds));
                        break;
                    }
                }

                Connections[id].TableId = roomId;

                WriteToClient(id, "JOIN OK");
                WriteToClient(id, message);

                Connections[id].KeepAliveThread = new Thread((idVal) =>
                {
                    int clientId = (int)idVal;

                    while (CanContinue && Connections[id] != null && Connections[id].Active)
                    {
                        for (int i = 0; i < 200; i++)
                        {
                            Thread.Sleep(100);
                            if(!CanContinue || Connections[id] == null || !Connections[id].Active)
                            {
                                return;
                            }
                        }

                        try
                        {
                            if(Connections[id].KeepAliveNotResponded++ > 3)
                            {
                                // Close connection
                                TableList[found].NumSat--;
                                CloseConnection(id);

                                // Notify other users about users' leave

                                return;
                            }

                            WriteToClient(clientId, "KEEP ALIVE " + Math.Abs(Environment.TickCount));
                        } catch //(Exception ex)
                        {
                            WriteLine("Görünüşe göre bir istasyon (id: "+ id+", user: "+ Connections[id].FullName +", uid: "+ Connections[id].UserID+ ") bağlantıyı kaybetti...", Color.Red);
                            //Connections[id].Active = false;
                        }
                    }
                });

                Connections[id].KeepAliveThread.Start(id);
            }

            return true;
        }

        private void PlayerLeave(int id)
        {
            // Release user from table
            if(Connections[id] == null || !Connections[id].Active)
            {
                return;
            }

            int tableIndex = FindTableFromId(Connections[id].TableId);
            if(tableIndex == -1)
            {
                return;
            }

            // Notify others about player's leave
            for (int i = 0; i < TableList[tableIndex].NumConnected; i++)
            {
                WriteToClient(TableList[tableIndex].PlayersConnected[i], "USER LEFT " + Connections[id].SittingAt);
            }

            // Perform the operation
            for (int i = 0; i < TableList[tableIndex].NumSat; i++)
            {
                if (TableList[tableIndex].PlayersSat[i] == id)
                {
                    for (int j = i; j < TableList[tableIndex].NumSat - 1; j++)
                    {
                        TableList[tableIndex].PlayersSat[j] = TableList[tableIndex].PlayersSat[j + 1];
                    }

                    break;
                }
            }

            for (int i = 0; i < TableList[tableIndex].NumConnected; i++)
            {
                if (TableList[tableIndex].PlayersConnected[i] == id)
                {
                    for (int j = i; j < TableList[tableIndex].NumConnected - 1; j++)
                    {
                        TableList[tableIndex].PlayersConnected[j] = TableList[tableIndex].PlayersConnected[j + 1];
                    }

                    break;
                }
            }

            TableList[tableIndex].NumSat--;
            TableList[tableIndex].NumConnected--;

            Connections[id].SittingAt = -1;
            Connections[id].State = ConnectionInfo.ConnectionState.SelfIdentify;
            Connections[id].Active = false;

            // if someone is alone in the table, let him win
            if (TableList[tableIndex].NumSat == 1)
            {
                PlayerWin(new WinningHand[] { new WinningHand(TableList[tableIndex].PlayersSat[0], "AITT", "N") }, false, true);
                IncrementPlayerXP(tableIndex, TableList[tableIndex].PlayersSat[0], 200);
            }
        }

        private void HandleSocketWelcome(int id, byte[] buffer, int length)
        {
            // Message content should be the following:
            // 4 bytes - signature 1 (should be "skTb")
            // 2 bytes - signature 2 (should be "14")
            // 2 bytes - protocol version (should be "1" and "0")
            // 2 bytes - operation id
            // 32 bytes - encryption key

            if (length != 42 ||

                // Check signature 1
                buffer[0] != 's' || buffer[1] != 'k' || buffer[2] != 'T' || buffer[3] != 'b' ||

                // Check signature 2
                buffer[4] != 1 || buffer[5] != 4 ||

                // Check protocol version
                buffer[6] != 1 || buffer[7] != 0)
            {
                // Invalid handshake, terminating connection
                WriteLine("Bir istemci kendini uygun şekilde tanıtamadı, iletişim kapatılıyor", Color.Red);
                CloseConnection(id);

                return;
            }

            WriteLine("Bir istemci kendini uygun şekilde tanıttı, iletişime başlanıyor", Color.Green);
            // Send back a response.
            Connections[id].OperationId = buffer[8] << 8 + buffer[9];
            switch (Connections[id].OperationId)
            {
                case 0:
                    // Test purposes only, won't work in the production
                    Connections[id].EncryptionKey = null;
                    break;
                case 1:
                    Connections[id].EncryptionKey = "kydWwCK2G6I2DBQMPV0jrtBEFp0ilaap";
                    break;
                case 2:
                    Connections[id].EncryptionKey = "zfVDwK62wkCHqfrYp03HWP4ZVr0NhSjb";
                    break;
                default:
                    // invalid operation id, terminating connection
                    CloseConnection(id);

                    return;
            }

            WriteToClient(id, "Welcome, server version " + Version);
            Connections[id].State = ConnectionInfo.ConnectionState.SelfIdentify;
        }

        private void CloseConnection(int id)
        {
            Connections[id].Active = false;
            //Connections[id].TableId = null;
            Connections[id].State = ConnectionInfo.ConnectionState.HandShake;

            try
            {
                Connections[id].KeepAliveThread.Abort();
            }
            catch
            {

            }

            try
            {
                Connections[id].Stream.Close();
            }
            catch
            {
            }

            try
            {
                Connections[id].Client.Close();
            }
            catch
            {
            }

            // notify other players in the table

            // update database
        }

        private TestResponseBufferItem PopFromResponseBuffer(int user)
        {
            for (int i = 0; i < TestResponseBuffer.Count; i++)
            {
                if(TestResponseBuffer[i].Id == user)
                {
                    TestResponseBufferItem p = TestResponseBuffer[i];
                    TestResponseBuffer.RemoveAt(i);

                    return p;
                }
            }

            return null;
        }

        private void AppendToResponseBuffer(int id, string message)
        {
            if(TestResponseBuffer == null)
            {
                return;
            }

            TestResponseBuffer.Add(new TestResponseBufferItem(id, DateTime.Now, message));
        }

        private void WriteToClient(int id, string message)
        {
            LogRaw(id, "OUT>> " + message.Replace("\n", ""));

            if (TestMode && /*!TestExecution &&*/ TestInstructions != null && TestInstructions.Count > 0)
            {
                WriteLine(id + " :: " + message, Color.OldLace);
                AppendToResponseBuffer(id, message);
                return;
            }

            if(Connections[id] == null || !Connections[id].Active || Connections[id].Stream == null)
            {
                return;
            }

            if(!message.EndsWith("\n"))
            {
                message += "\n";
            }

            message = message.Replace("İ", "~I;").Replace("ı", "~i;").Replace("Ş", "~S;").Replace("ş", "~s;").Replace("Ğ", "~G;").Replace("ğ", "~g;").Replace("Ö", "~O;").Replace("ö", "~o;").Replace("Ü", "~U;").Replace("ü", "~u;");
            byte[] response = EncryptResponse(id, message);

            if(Connections[id].Stream == null)
            {
                Connections[id].Active = false;
                CloseConnection(id);

                return;
            }

            try
            {
                Connections[id].Stream.Write(response, 0, response.Length);
            } catch
            {

            }
        }

        private byte[] EncryptResponse(int id, string v)
        {
            byte[] result = new byte[] { 0 };
            switch (Connections[id].OperationId)
            {
                case 0:
                    // Test purposes only, won't work in the production
                    result = Encoding.ASCII.GetBytes(v);
                    break;
                case 1:
                    AesManaged aes = new AesManaged();
                    aes.Key = Encoding.ASCII.GetBytes(Connections[id].EncryptionKey);
                    aes.IV = Encoding.ASCII.GetBytes(Connections[id].EncryptionKey);

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            // Create StreamWriter and write data to a stream    
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                sw.Write(v);
                            }

                            result = ms.ToArray();
                        }
                    }

                    return result;
            }

            return result;
        }

        public string Decrypt(int id, string v)
        {
            string result = null;
            switch (Connections[id].OperationId)
            {
                case 0:
                    // Test purposes only, won't work in the production
                    result = v;
                    break;
                case 1:
                    byte[] cipherText = Encoding.ASCII.GetBytes(v);

                    // Create AesManaged    
                    using (AesManaged aes = new AesManaged())
                    {
                        // Create a decryptor    
                        aes.Key = Encoding.ASCII.GetBytes(Connections[id].EncryptionKey);
                        aes.IV = Encoding.ASCII.GetBytes(Connections[id].EncryptionKey);

                        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                        // Create the streams used for decryption.    
                        using (MemoryStream ms = new MemoryStream(cipherText))
                        {
                            // Create crypto stream    
                            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                            {
                                // Read crypto stream    
                                using (StreamReader reader = new StreamReader(cs))
                                {
                                    result = reader.ReadToEnd();
                                }
                            }
                        }
                    }

                    break;
            }

            return result;
        }

        private void WriteLine(String msg)
        {
            msg = DateTime.Now.ToString("HH:mm:ss :: ") + msg;
            //AppendToLogAbove(msg);

            Log(msg);
            try
            {
                Console.Invoke((MethodInvoker)delegate
                {
                    Console.AppendText(msg + Environment.NewLine);
                    Console.SelectionStart = Console.Text.Length;

                    Console.ScrollToCaret();
                    Application.DoEvents();
                });
            }
            catch
            {
                // Could not display the message
            }

            Application.DoEvents();
        }

        private void WriteLine(String msg, Color color)
        {
            msg = DateTime.Now.ToString("HH:mm:ss :: ") + msg;
            Log(msg);
            //AppendToLogAbove(msg);

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

            Application.DoEvents();
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            Start();
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            CanContinue = false;

            for (int i = 0; i < ThreadList.Length; i++)
            {
                try
                {
                    if (Connections[i] != null)
                    {
                        Connections[i].Stream.Close();
                        Connections[i].Client.Close();
                    }

                    if (ThreadList[i] != null)
                    {
                        ThreadList[i].Abort();
                    }
                }
                catch
                {

                }
            }

            try
            {
                MainListener.Stop();
            }
            catch
            {

            }

            try
            {
                MainClient.Close();
            }
            catch
            {

            }

            try
            {
                TestThread.Abort();
            }
            catch
            {

            }

            try
            {
                MainThread.Abort();
            }
            catch
            {

            }
        }
    }
}
