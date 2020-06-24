using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1GameRoomServer
{
    public enum CardType { Hearts, Spades, Diamonds, Clubs };

    public class CardInfo
    {
        public const int CardJ = 11;
        public const int CardQ = 12;
        public const int CardK = 13;
        public const int CardA = 14;

        public CardType Type;
        public int Number;

        public CardInfo(CardType Type, int Number)
        {
            this.Type = Type;
            this.Number = Number;
        }

        internal static CardInfo Parse(string v)
        {
            try
            {
                v = v.ToUpper();
                CardType cardType = CardType.Clubs;

                if(v[0] == 'D')
                {
                    cardType = CardType.Diamonds;
                } else if(v[0] == 'H')
                {
                    cardType = CardType.Hearts;
                } else if(v[0] == 'S')
                {
                    cardType = CardType.Spades;
                }
                else if (v[0] == 'C')
                {
                    cardType = CardType.Clubs;
                }
                else
                {
                    throw new Exception("Invalid card type");
                }

                return new CardInfo(cardType, int.Parse(v.Substring(1)));
            } catch
            {
                return null;
            }
        }
    }
}
