using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using T1GameRoomServer;

namespace RankVerifier
{
    public partial class Form1 : Form
    {
        public string Version = "1";

        private List<int> PreviousResults;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            pbMid1.Image = Bitmap.FromFile("assets/cards/cardback.png");
            pbMid2.Image = Bitmap.FromFile("assets/cards/cardback.png");
            pbMid3.Image = Bitmap.FromFile("assets/cards/cardback.png");
            pbMid4.Image = Bitmap.FromFile("assets/cards/cardback.png");
            pbMid5.Image = Bitmap.FromFile("assets/cards/cardback.png");

            pbP0C1.Image = Bitmap.FromFile("assets/cards/cardback.png");
            pbP0C2.Image = Bitmap.FromFile("assets/cards/cardback.png");

            pbP1C1.Image = Bitmap.FromFile("assets/cards/cardback.png");
            pbP1C2.Image = Bitmap.FromFile("assets/cards/cardback.png");

            pbP2C1.Image = Bitmap.FromFile("assets/cards/cardback.png");
            pbP2C2.Image = Bitmap.FromFile("assets/cards/cardback.png");

            pbP3C1.Image = Bitmap.FromFile("assets/cards/cardback.png");
            pbP3C2.Image = Bitmap.FromFile("assets/cards/cardback.png");

            pbP4C1.Image = Bitmap.FromFile("assets/cards/cardback.png");
            pbP4C2.Image = Bitmap.FromFile("assets/cards/cardback.png");

            Text += Version;
        }

        private void pbP2C1_Click(object sender, EventArgs e)
        {
            FormCardSelection fcs = new FormCardSelection();
            if(fcs.ShowDialog() == DialogResult.OK)
            {
                ProcessResult(fcs.Result, sender);
            }
        }

        private void ProcessResult(string result, object sender)
        {
            string fileName = "cardback.png";

            switch (result)
            {
                case "pbS1C2":
                    fileName = "card_c2.png";
                    break;
                case "pbS1C3":
                    fileName = "card_c3.png";
                    break;
                case "pbS1C4":
                    fileName = "card_c4.png";
                    break;
                case "pbS1C5":
                    fileName = "card_c5.png";
                    break;
                case "pbS1C6":
                    fileName = "card_c6.png";
                    break;
                case "pbS1C7":
                    fileName = "card_c7.png";
                    break;
                case "pbS1C8":
                    fileName = "card_c8.png";
                    break;
                case "pbS1C9":
                    fileName = "card_c9.png";
                    break;
                case "pbS1C10":
                    fileName = "card_c10.png";
                    break;
                case "pbS1C11":
                    fileName = "card_cj.png";
                    break;
                case "pbS1C12":
                    fileName = "card_cq.png";
                    break;
                case "pbS1C13":
                    fileName = "card_ck.png";
                    break;
                case "pbS1C14":
                    fileName = "card_ca.png";
                    break;

                case "pbS2C2":
                    fileName = "card_d2.png";
                    break;
                case "pbS2C3":
                    fileName = "card_d3.png";
                    break;
                case "pbS2C4":
                    fileName = "card_d4.png";
                    break;
                case "pbS2C5":
                    fileName = "card_d5.png";
                    break;
                case "pbS2C6":
                    fileName = "card_d6.png";
                    break;
                case "pbS2C7":
                    fileName = "card_d7.png";
                    break;
                case "pbS2C8":
                    fileName = "card_d8.png";
                    break;
                case "pbS2C9":
                    fileName = "card_d9.png";
                    break;
                case "pbS2C10":
                    fileName = "card_d10.png";
                    break;
                case "pbS2C11":
                    fileName = "card_dj.png";
                    break;
                case "pbS2C12":
                    fileName = "card_dq.png";
                    break;
                case "pbS2C13":
                    fileName = "card_dk.png";
                    break;
                case "pbS2C14":
                    fileName = "card_da.png";
                    break;

                case "pbS3C2":
                    fileName = "card_h2.png";
                    break;
                case "pbS3C3":
                    fileName = "card_h3.png";
                    break;
                case "pbS3C4":
                    fileName = "card_h4.png";
                    break;
                case "pbS3C5":
                    fileName = "card_h5.png";
                    break;
                case "pbS3C6":
                    fileName = "card_h6.png";
                    break;
                case "pbS3C7":
                    fileName = "card_h7.png";
                    break;
                case "pbS3C8":
                    fileName = "card_h8.png";
                    break;
                case "pbS3C9":
                    fileName = "card_h9.png";
                    break;
                case "pbS3C10":
                    fileName = "card_h10.png";
                    break;
                case "pbS3C11":
                    fileName = "card_hj.png";
                    break;
                case "pbS3C12":
                    fileName = "card_hq.png";
                    break;
                case "pbS3C13":
                    fileName = "card_hk.png";
                    break;
                case "pbS3C14":
                    fileName = "card_ha.png";
                    break;

                case "pbS4C2":
                    fileName = "card_s2.png";
                    break;
                case "pbS4C3":
                    fileName = "card_s3.png";
                    break;
                case "pbS4C4":
                    fileName = "card_s4.png";
                    break;
                case "pbS4C5":
                    fileName = "card_s5.png";
                    break;
                case "pbS4C6":
                    fileName = "card_s6.png";
                    break;
                case "pbS4C7":
                    fileName = "card_s7.png";
                    break;
                case "pbS4C8":
                    fileName = "card_s8.png";
                    break;
                case "pbS4C9":
                    fileName = "card_s9.png";
                    break;
                case "pbS4C10":
                    fileName = "card_s10.png";
                    break;
                case "pbS4C11":
                    fileName = "card_sj.png";
                    break;
                case "pbS4C12":
                    fileName = "card_sq.png";
                    break;
                case "pbS4C13":
                    fileName = "card_sk.png";
                    break;
                case "pbS4C14":
                    fileName = "card_sa.png";
                    break;
            }

            (sender as PictureBox).Image = Bitmap.FromFile("assets/cards/" + fileName);
            (sender as PictureBox).Tag = result;
        }

        Random r = new Random();
        string[] PossibleResults = { "pbS1C2", "pbS1C3", "pbS1C4", "pbS1C5", "pbS1C6", "pbS1C7", "pbS1C8", "pbS1C9", "pbS1C10", "pbS1C11", "pbS1C12", "pbS1C13", "pbS1C14",
                                    "pbS2C2", "pbS2C3", "pbS2C4", "pbS2C5", "pbS2C6", "pbS2C7", "pbS2C8", "pbS2C9", "pbS2C10", "pbS2C11", "pbS2C12", "pbS2C13", "pbS2C14",
                                    "pbS3C2", "pbS3C3", "pbS3C4", "pbS3C5", "pbS3C6", "pbS3C7", "pbS3C8", "pbS3C9", "pbS3C10", "pbS3C11", "pbS3C12", "pbS3C13", "pbS3C14",
                                    "pbS4C2", "pbS4C3", "pbS4C4", "pbS4C5", "pbS4C6", "pbS4C7", "pbS4C8", "pbS4C9", "pbS4C10", "pbS4C11", "pbS4C12", "pbS4C13", "pbS4C14" };

        private int NextResult()
        {
            int result = r.Next(PossibleResults.Length);

            bool found = true;
            while(found)
            {
                found = false;
                for (int i = 0; i < PreviousResults.Count; i++)
                {
                    if(PreviousResults[i] == result)
                    {
                        result = r.Next(PossibleResults.Length);
                        found = true;

                        break;
                    }
                }
            }

            PreviousResults.Add(result);
            return result;
        }

        private void btnRandomize_Click(object sender, EventArgs e)
        {
            PreviousResults = new List<int>();

            ProcessResult(PossibleResults[NextResult()], pbP0C1);
            ProcessResult(PossibleResults[NextResult()], pbP0C2);

            ProcessResult(PossibleResults[NextResult()], pbP1C1);
            ProcessResult(PossibleResults[NextResult()], pbP1C2);

            ProcessResult(PossibleResults[NextResult()], pbP2C1);
            ProcessResult(PossibleResults[NextResult()], pbP2C2);

            ProcessResult(PossibleResults[NextResult()], pbP3C1);
            ProcessResult(PossibleResults[NextResult()], pbP3C2);

            ProcessResult(PossibleResults[NextResult()], pbP4C1);
            ProcessResult(PossibleResults[NextResult()], pbP4C2);

            ProcessResult(PossibleResults[NextResult()], pbMid1);
            ProcessResult(PossibleResults[NextResult()], pbMid2);
            ProcessResult(PossibleResults[NextResult()], pbMid3);
            ProcessResult(PossibleResults[NextResult()], pbMid4);
            ProcessResult(PossibleResults[NextResult()], pbMid5);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            btnRight.Visible = false;
            btnWrong.Visible = false;

            ResetCard(pbP0C1);
            ResetCard(pbP0C2);

            ResetCard(pbP1C1);
            ResetCard(pbP1C2);

            ResetCard(pbP2C1);
            ResetCard(pbP2C2);

            ResetCard(pbP3C1);
            ResetCard(pbP3C2);

            ResetCard(pbP4C1);
            ResetCard(pbP4C2);

            ResetCard(pbMid1);
            ResetCard(pbMid2);
            ResetCard(pbMid3);
            ResetCard(pbMid4);
            ResetCard(pbMid5);
        }

        private void ResetCard(PictureBox pcb)
        {
            pcb.Image = Bitmap.FromFile("assets/cards/cardback.png");
            pcb.Tag = null;
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            btnRight.Visible = true;
            btnWrong.Visible = true;

            int[] marks = { 0, 0, 0, 0, 0 };
            string[] wonBy = { "", "", "", "", "" };
            string[] wonHand = { "", "", "", "", "" };

            marks[0] = CalculateMark(pbP0C1.Tag.ToString(), pbP0C2.Tag.ToString(), out wonBy[0], out wonHand[0]);
            marks[1] = CalculateMark(pbP1C1.Tag.ToString(), pbP1C2.Tag.ToString(), out wonBy[1], out wonHand[1]);
            marks[2] = CalculateMark(pbP2C1.Tag.ToString(), pbP2C2.Tag.ToString(), out wonBy[2], out wonHand[2]);
            marks[3] = CalculateMark(pbP3C1.Tag.ToString(), pbP3C2.Tag.ToString(), out wonBy[3], out wonHand[3]);
            marks[4] = CalculateMark(pbP4C1.Tag.ToString(), pbP4C2.Tag.ToString(), out wonBy[4], out wonHand[4]);

            int highestIndex = -1;
            int highestValue = 0;
            Bitmap image;

            for (int i = 0; i < marks.Length; i++)
            {
                if(marks[i] > highestValue)
                {
                    highestIndex = i;
                    highestValue = marks[i];
                }
            }

            for (int i = 0; i < 5; i++)
            {
                if(i == highestIndex)
                {
                    continue;
                }

                switch (i)
                {
                    case 0:
                        image = pbP0C1.Image as Bitmap;
                        pbP0C1.Image = Lighten(image, -128);
                        image = pbP0C2.Image as Bitmap;
                        pbP0C2.Image = Lighten(image, -128);

                        break;
                    case 1:
                        image = pbP1C1.Image as Bitmap;
                        pbP1C1.Image = Lighten(image, -128);
                        image = pbP1C2.Image as Bitmap;
                        pbP1C2.Image = Lighten(image, -128);

                        break;
                    case 2:
                        image = pbP2C1.Image as Bitmap;
                        pbP2C1.Image = Lighten(image, -128);
                        image = pbP2C2.Image as Bitmap;
                        pbP2C2.Image = Lighten(image, -128);

                        break;
                    case 3:
                        image = pbP3C1.Image as Bitmap;
                        pbP3C1.Image = Lighten(image, -128);
                        image = pbP3C2.Image as Bitmap;
                        pbP3C2.Image = Lighten(image, -128);

                        break;
                    case 4:
                        image = pbP4C1.Image as Bitmap;
                        pbP4C1.Image = Lighten(image, -128);
                        image = pbP4C2.Image as Bitmap;
                        pbP4C2.Image = Lighten(image, -128);

                        break;
                }
            }

            string[] wonOrder = wonHand[highestIndex].Split(',');
            bool[] shouldDarken = { true, true, true, true, true, true, true };
            for (int i = 0; i < wonOrder.Length; i++)
            {
                if(wonOrder[i] == "0")
                {
                    shouldDarken[0] = false;
                }

                if (wonOrder[i] == "1")
                {
                    shouldDarken[1] = false;
                }

                if (wonOrder[i] == "2")
                {
                    shouldDarken[2] = false;
                }

                if (wonOrder[i] == "3")
                {
                    shouldDarken[3] = false;
                }

                if (wonOrder[i] == "4")
                {
                    shouldDarken[4] = false;
                }

                if (wonOrder[i] == "5")
                {
                    shouldDarken[5] = false;
                }

                if (wonOrder[i] == "6")
                {
                    shouldDarken[6] = false;
                }
            }

            if (shouldDarken[0])
            {
                image = pbMid1.Image as Bitmap;
                pbMid1.Image = Lighten(image, -128);
            }

            if (shouldDarken[1])
            {
                image = pbMid2.Image as Bitmap;
                pbMid2.Image = Lighten(image, -128);
            }

            if (shouldDarken[2])
            {
                image = pbMid3.Image as Bitmap;
                pbMid3.Image = Lighten(image, -128);
            }

            if (shouldDarken[3])
            {
                image = pbMid4.Image as Bitmap;
                pbMid4.Image = Lighten(image, -128);
            }

            if (shouldDarken[4])
            {
                image = pbMid5.Image as Bitmap;
                pbMid5.Image = Lighten(image, -128);
            }

            if (shouldDarken[5])
            {
                switch(highestIndex)
                {
                    case 0:
                        image = pbP0C1.Image as Bitmap;
                        pbP0C1.Image = Lighten(image, -128);
                        break;
                    case 1:
                        image = pbP1C1.Image as Bitmap;
                        pbP1C1.Image = Lighten(image, -128);
                        break;
                    case 2:
                        image = pbP2C1.Image as Bitmap;
                        pbP2C1.Image = Lighten(image, -128);
                        break;
                    case 3:
                        image = pbP3C1.Image as Bitmap;
                        pbP3C1.Image = Lighten(image, -128);
                        break;
                    case 4:
                        image = pbP4C1.Image as Bitmap;
                        pbP4C1.Image = Lighten(image, -128);
                        break;
                }
            }

            if (shouldDarken[6])
            {
                switch (highestIndex)
                {
                    case 0:
                        image = pbP0C2.Image as Bitmap;
                        pbP0C2.Image = Lighten(image, -128);
                        break;
                    case 1:
                        image = pbP1C2.Image as Bitmap;
                        pbP1C2.Image = Lighten(image, -128);
                        break;
                    case 2:
                        image = pbP2C2.Image as Bitmap;
                        pbP2C2.Image = Lighten(image, -128);
                        break;
                    case 3:
                        image = pbP3C2.Image as Bitmap;
                        pbP3C2.Image = Lighten(image, -128);
                        break;
                    case 4:
                        image = pbP4C2.Image as Bitmap;
                        pbP4C2.Image = Lighten(image, -128);
                        break;
                }
            }

            // Name of hand win
            lblResult.Text = "HIGH CARD";
            switch(wonBy[highestIndex])
            {
                case "AITT":
                case "AOF":
                    lblResult.Text = "";
                    break;
                case "RF":
                    lblResult.Text = "ROYAL FLUSH";
                    break;
                case "SF":
                    lblResult.Text = "STRAIGHT FLUSH";
                    break;
                case "ST":
                    lblResult.Text = "STRAIGHT";
                    break;
                case "FH":
                    lblResult.Text = "FULL HOUSE";
                    break;
                case "OOK4":
                    lblResult.Text = "FOUR OF A KIND";
                    break;
                case "OOK3":
                    lblResult.Text = "THREE OF A KIND";
                    break;
                case "PA3":
                    lblResult.Text = "THREE PAIR";
                    break;
                case "PA2":
                    lblResult.Text = "TWO PAIR";
                    break;
                case "PA1":
                    lblResult.Text = "PAIR";
                    break;
                default:
                    lblResult.Text = "HIGH CARD";
                    break;
            }
        }

        private int CalculateMark(string tag1, string tag2, out string wonBy, out string wonOrder)
        {
            int result = 0;
            int baseValue;

            wonBy = "";
            wonOrder = "";

            CardInfo[] cardList = new CardInfo[7];
            CardInfo[] origList = new CardInfo[7];
            CardInfo[] cardDeck = { StrToCard(tag1), StrToCard(tag2) };
            CardInfo[] cardsOnTheTable = { StrToCard(pbMid1.Tag.ToString()), StrToCard(pbMid2.Tag.ToString()), StrToCard(pbMid3.Tag.ToString()), StrToCard(pbMid4.Tag.ToString()), StrToCard(pbMid5.Tag.ToString()) }; //new CardInfo[5];

            for (int i = 0; i < 5; i++)
            {
                cardList[i] = new CardInfo(cardsOnTheTable[i].Type, cardsOnTheTable[i].Number);
                origList[i] = new CardInfo(cardsOnTheTable[i].Type, cardsOnTheTable[i].Number);
            }

            cardList[5] = new CardInfo(cardDeck[0].Type, cardDeck[0].Number);
            cardList[6] = new CardInfo(cardDeck[1].Type, cardDeck[1].Number);
            origList[5] = new CardInfo(cardDeck[0].Type, cardDeck[0].Number);
            origList[6] = new CardInfo(cardDeck[1].Type, cardDeck[1].Number);

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
                switch (cardList[i].Type)
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
                }
                else
                {
                    flushHighest = int.MaxValue;
                    flushType = null;
                    straightIndex = 0;
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
                for (int highestIndex = 0; highestIndex < 5; highestIndex++)
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
                        if (wonOrder.Split(',').Length == 5)
                        {
                            break;
                        }
                    }
                    catch
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
                    if (i == j)
                    {
                        continue;
                    }

                    if (cardList[i].Number == cardList[j].Number)
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
            if (numberOfSameCards == 4)
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

                        baseValue += origList[i].Number;
                        wonOrder += i;
                    }
                }

                wonBy = "OOK4";
                return 8000 + baseValue;
            }

            // #4 Full House?

            // #5 Flush?

            // #6 Straight?

            // #7 Three of a Kind?
            if (numberOfSameCards == 3)
            {
                int secondOrder = -1;

                for (int i = 0; i < origList.Length - 1; i++)
                {
                    for (int j = 1; j < origList.Length; j++)
                    {
                        if (origList[i].Number != highestCardNumber && i != j && origList[i].Number == origList[j].Number)
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

                        baseValue += origList[i].Number;
                        wonOrder += i;
                    }
                }

                if (secondOrder != -1)
                {
                    wonBy = "FH";
                    //baseValue += 90;

                    for (int i = 0; i < origList.Length; i++)
                    {
                        if (origList[i].Number == secondOrder)
                        {
                            wonOrder += "," + i;
                            baseValue += origList[i].Number;
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
                        if (i == j)
                        {
                            continue;
                        }

                        if (cardList[i].Number == cardList[j].Number)
                        {
                            pairNumber = cardList[j].Number;
                            break;
                        }
                    }

                    if (pairNumber != -1)
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

                                baseValue += origList[i].Number;
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

                                    baseValue += origList[i].Number;
                                    wonOrder += i;
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

                                    baseValue += origList[i].Number;
                                    wonOrder += i;
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

                                baseValue += origList[i].Number;
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

                                    baseValue += origList[i].Number;
                                    wonOrder += i;
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

                                    baseValue += origList[i].Number;
                                    wonOrder += i;
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

                                baseValue += origList[i].Number;
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

                                    baseValue += origList[i].Number;
                                    wonOrder += i;
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

                                    baseValue += origList[i].Number;
                                    wonOrder += i;
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

                            baseValue += origList[i].Number;
                            wonOrder += i;
                        }
                    }

                    wonBy = "PA2";
                    return 2000 + baseValue;
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

                            baseValue += origList[i].Number;
                            wonOrder += i;
                        }
                    }

                    wonBy = "PA1";
                    return 1000 + baseValue;
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
                    if (sortedList[j] < sortedList[i])
                    {
                        temp = sortedList[j];
                        sortedList[j] = sortedList[i];
                        sortedList[i] = temp;
                    }
                }
            }

            for (int i = 0; i < sortedList.Length; i++)
            {
                result += sortedList[i];
            }

            return result;
        }

        private CardInfo StrToCard(string tag)
        {
            switch(tag)
            {
                case "pbS1C2":
                    return new CardInfo(CardType.Clubs, 2);
                case "pbS1C3":
                    return new CardInfo(CardType.Clubs, 3);
                case "pbS1C4":
                    return new CardInfo(CardType.Clubs, 4);
                case "pbS1C5":
                    return new CardInfo(CardType.Clubs, 5);
                case "pbS1C6":
                    return new CardInfo(CardType.Clubs, 6);
                case "pbS1C7":
                    return new CardInfo(CardType.Clubs, 7);
                case "pbS1C8":
                    return new CardInfo(CardType.Clubs, 8);
                case "pbS1C9":
                    return new CardInfo(CardType.Clubs, 9);
                case "pbS1C10":
                    return new CardInfo(CardType.Clubs, 10);
                case "pbS1C11":
                    return new CardInfo(CardType.Clubs, CardInfo.CardJ);
                case "pbS1C12":
                    return new CardInfo(CardType.Clubs, CardInfo.CardQ);
                case "pbS1C13":
                    return new CardInfo(CardType.Clubs, CardInfo.CardK);
                case "pbS1C14":
                    return new CardInfo(CardType.Clubs, CardInfo.CardA);

                case "pbS2C2":
                    return new CardInfo(CardType.Diamonds, 2);
                case "pbS2C3":
                    return new CardInfo(CardType.Diamonds, 3);
                case "pbS2C4":
                    return new CardInfo(CardType.Diamonds, 4);
                case "pbS2C5":
                    return new CardInfo(CardType.Diamonds, 5);
                case "pbS2C6":
                    return new CardInfo(CardType.Diamonds, 6);
                case "pbS2C7":
                    return new CardInfo(CardType.Diamonds, 7);
                case "pbS2C8":
                    return new CardInfo(CardType.Diamonds, 8);
                case "pbS2C9":
                    return new CardInfo(CardType.Diamonds, 9);
                case "pbS2C10":
                    return new CardInfo(CardType.Diamonds, 10);
                case "pbS2C11":
                    return new CardInfo(CardType.Diamonds, CardInfo.CardJ);
                case "pbS2C12":
                    return new CardInfo(CardType.Diamonds, CardInfo.CardQ);
                case "pbS2C13":
                    return new CardInfo(CardType.Diamonds, CardInfo.CardK);
                case "pbS2C14":
                    return new CardInfo(CardType.Diamonds, CardInfo.CardA);

                case "pbS3C2":
                    return new CardInfo(CardType.Hearts, 2);
                case "pbS3C3":
                    return new CardInfo(CardType.Hearts, 3);
                case "pbS3C4":
                    return new CardInfo(CardType.Hearts, 4);
                case "pbS3C5":
                    return new CardInfo(CardType.Hearts, 5);
                case "pbS3C6":
                    return new CardInfo(CardType.Hearts, 6);
                case "pbS3C7":
                    return new CardInfo(CardType.Hearts, 7);
                case "pbS3C8":
                    return new CardInfo(CardType.Hearts, 8);
                case "pbS3C9":
                    return new CardInfo(CardType.Hearts, 9);
                case "pbS3C10":
                    return new CardInfo(CardType.Hearts, 10);
                case "pbS3C11":
                    return new CardInfo(CardType.Hearts, CardInfo.CardJ);
                case "pbS3C12":
                    return new CardInfo(CardType.Hearts, CardInfo.CardQ);
                case "pbS3C13":
                    return new CardInfo(CardType.Hearts, CardInfo.CardK);
                case "pbS3C14":
                    return new CardInfo(CardType.Hearts, CardInfo.CardA);

                case "pbS4C2":
                    return new CardInfo(CardType.Spades, 2);
                case "pbS4C3":
                    return new CardInfo(CardType.Spades, 3);
                case "pbS4C4":
                    return new CardInfo(CardType.Spades, 4);
                case "pbS4C5":
                    return new CardInfo(CardType.Spades, 5);
                case "pbS4C6":
                    return new CardInfo(CardType.Spades, 6);
                case "pbS4C7":
                    return new CardInfo(CardType.Spades, 7);
                case "pbS4C8":
                    return new CardInfo(CardType.Spades, 8);
                case "pbS4C9":
                    return new CardInfo(CardType.Spades, 9);
                case "pbS4C10":
                    return new CardInfo(CardType.Spades, 10);
                case "pbS4C11":
                    return new CardInfo(CardType.Spades, CardInfo.CardJ);
                case "pbS4C12":
                    return new CardInfo(CardType.Spades, CardInfo.CardQ);
                case "pbS4C13":
                    return new CardInfo(CardType.Spades, CardInfo.CardK);
                case "pbS4C14":
                    return new CardInfo(CardType.Spades, CardInfo.CardA);
            }

            return null;
        }

        public Bitmap Lighten(Bitmap bitmap, int amount)
        {
            if (amount < -255 || amount > 255)
                return bitmap;

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;

            int nVal = 0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                int nOffset = stride - bitmap.Width * 3;
                int nWidth = bitmap.Width * 3;

                for (int y = 0; y < bitmap.Height; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        nVal = (int)(p[0] + amount);

                        if (nVal < 0) nVal = 0;
                        if (nVal > 255) nVal = 255;

                        p[0] = (byte)nVal;

                        ++p;
                    }

                    p += nOffset;
                }
            }

            bitmap.UnlockBits(bmData);

            return bitmap;
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            using (WebClient wc = new WebClient())
            {
                string p1c1 = pbP0C1.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string p1c2 = pbP0C2.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string p2c1 = pbP1C1.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string p2c2 = pbP1C2.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string p3c1 = pbP2C1.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string p3c2 = pbP2C2.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string p4c1 = pbP3C1.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string p4c2 = pbP3C2.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string p5c1 = pbP4C1.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string p5c2 = pbP4C2.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string pMid1 = pbMid1.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string pMid2 = pbMid2.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string pMid3 = pbMid3.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string pMid4 = pbMid4.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string pMid5 = pbMid5.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");

                wc.Headers.Add("User-Agent: Gravity 0.1");
                string s = wc.DownloadString("http://178.157.10.75:8090/velo/add_test_stat.php?p1c1=" + p1c1 + "&p1c2=" + p1c2 + "&p2c1=" + p2c1 + "&p2c2=" + p2c2 + "&p3c1=" + p3c1 + "&p3c2=" + p3c2 + "&p4c1=" + p4c1 + "&p4c2=" + p4c2 + "&p5c1=" + p5c1 + "&p5c2=" + p5c2 + "&pMid1=" + pMid1 + "&pMid2=" + pMid2 + "&pMid3=" + pMid3 + "&pMid4=" + pMid4 + "&pMid5=" + pMid5 + "&calc=" + lblResult.Text + "&corr=1&comm=");

                MessageBox.Show("Görüşünüz kaydedildi");
            }
        }

        private void btnWrong_Click(object sender, EventArgs e)
        {
            FormComments fc = new FormComments();
            fc.ShowDialog();

            using (WebClient wc = new WebClient())
            {
                string p1c1 = pbP0C1.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string p1c2 = pbP0C2.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string p2c1 = pbP1C1.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string p2c2 = pbP1C2.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string p3c1 = pbP2C1.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string p3c2 = pbP2C2.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string p4c1 = pbP3C1.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string p4c2 = pbP3C2.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string p5c1 = pbP4C1.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string p5c2 = pbP4C2.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string pMid1 = pbMid1.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string pMid2 = pbMid2.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string pMid3 = pbMid3.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string pMid4 = pbMid4.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");
                string pMid5 = pbMid5.Tag.ToString().Replace("pbS1C", "C").Replace("pbS2C", "D").Replace("pbS3C", "H").Replace("pbS4C", "S");

                wc.Headers.Add("User-Agent: Gravity 0.1");
                string s = wc.DownloadString("http://178.157.10.75:8090/velo/add_test_stat.php?p1c1=" + p1c1 + "&p1c2=" + p1c2 + "&p2c1=" + p2c1 + "&p2c2=" + p2c2 + "&p3c1=" + p3c1 + "&p3c2=" + p3c2 + "&p4c1=" + p4c1 + "&p4c2=" + p4c2 + "&p5c1=" + p5c1 + "&p5c2=" + p5c2 + "&pMid1=" + pMid1 + "&pMid2=" + pMid2 + "&pMid3=" + pMid3 + "&pMid4=" + pMid4 + "&pMid5=" + pMid5 + "&calc=" + lblResult.Text + "&corr=0&comm=" + fc.richTextBox1.Text.Trim().Replace("'", "''"));

                MessageBox.Show("Görüşünüz kaydedildi");
            }
        }
    }
}
