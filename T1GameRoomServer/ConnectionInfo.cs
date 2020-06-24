using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace T1GameRoomServer
{
    public class ConnectionInfo
    {
        public enum ConnectionState
        {
            HandShake, SelfIdentify, JoinRoom, InTheRoom, DisconnectPeriod
        }

        public ConnectionState State;
        public NetworkStream Stream;
        public TcpClient Client;
        public Thread KeepAliveThread;
        public Thread TurnTimer;
        public int HandsMissed = 0;
        public bool TurnTimerOn = false;
        public bool Active = false;
        public int KeepAliveNotResponded = 0;
        public int OperationId = -1;
        public int Index = -1;
        public bool MyTurn = false;
        public bool Folded = false;
        public bool Called = false;
        public bool Allin = false;
        public bool Raised = false;
        public long RaisedTo = 0;
        public string TableId = "";
        public string IPAddress = "";
        public string EncryptionKey = "";
        public string SessionUser = "";
        public string SessionToken = "";
        public string UserID = "";
        public string FullName = "Lucky";
        public long Balance = 1000000;
        public long AmountToSit = 10000;
        public long BalanceOnTable = 0;
        public bool AutoRebuy = false;
        public bool AutoTopOff = false;
        public long BalanceOnPlay = 0;
        public int PlayerMark = 0;
        public string HandName = "";
        public string WinningCardOrder = "";
        public int Avatar = 0;
        public int Level = 0;
        public int XP = 0;
        public int XPToLevel = -1;
        public int SittingAt = -1;
        public string AvatarFile;
        public string Locale = "tr";
        public CardInfo[] CardDeck = new CardInfo[2] { null, null };

        public ConnectionInfo()
        {
            State = ConnectionState.HandShake;
        }
    }
}
