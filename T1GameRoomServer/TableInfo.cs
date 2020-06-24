using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1GameRoomServer
{
    public class TableInfo
    {
        private string id;

        public enum TableState
        {
            Idle, Bidding, Playing
        }

        public const int MaxCapacity = 20;

        public string Id
        {
            get
            {
                return id;
            }
        }
        public int NumConnected = 0;
        public int NumSat = 0;
        public int NumSitQue = 0;
        public int CasinoId = 0;
        public long RaiseMin = 0;
        public long TotalRaiseMin = 0;
        public long SmallBlind = 0;
        public long BigBlind = 0;
        public bool Vip = false;
        public bool Private = false;
        public int OwnedBy = -1;
        public int TableSize = 5;
        public int NumHand = 0;
        public bool Fast = false;
        public long Pot;
        public int CardIndex = 0;
        public int Dealer = -1;

        public TableState State = TableState.Idle;

        public int[] PlayersConnected = new int[MaxCapacity];
        public int[] PlayersSat = new int[MaxCapacity];
        public int[] PlayersSitQue = new int[MaxCapacity];
        public CardInfo[] CardDeck = new CardInfo[52];
        public CardInfo[] CardsOnTheTable = new CardInfo[5];

        public TableInfo(string Id)
        {
            this.id = Id;
        }
    }
}
