/* Written By Tyler Besecker
 * 08/01/2019
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace WAR
{
    class Program
    {
        #region Card Class

        public class Card
        {
            public int Rank = 0;
            public int Suit = 0;
            public bool faceUp = true;

            //0-4 Heart,Diamond,Spade,Club
            //2-14 (Rank 2 = 2, Rank 14 = Ace)
            public Card(int rank, int suit)
            {
                Rank = rank;
                Suit = suit;
            }

        }

        #endregion

        #region CardToString
        /*****************************************
        * String CardToString(Card)              *
        * Takes integer rank and suit value      *
        * and converts it to an easily readable  *
        * face value.                            *
        *****************************************/
        public static String CardToString(Card a)
        {
            string cardName = "";

            if (a.Rank > 10)
            {
                switch (a.Rank)
                {
                    case 11:
                        cardName = "Jack";
                        break;
                    case 12:
                        cardName = "Queen";
                        break;
                    case 13:
                        cardName = "King";
                        break;
                    case 14:
                        cardName = "Ace";
                        break;
                }
            }
            else
            {
                cardName = a.Rank.ToString();
            }
            switch (a.Suit)
            {
                case 0:
                    cardName += " of Hearts";
                    break;
                case 1:
                    cardName += " of Diamonds";
                    break;
                case 2:
                    cardName += " of Spades";
                    break;
                case 3:
                    cardName += " of Clubs";
                    break;
            }
            return cardName;
        }
        #endregion

        #region Shuffle
        /*****************************************
        * Queue<Card> Shuffle(Queue<card>, int N *
        * Swaps two random cards 1000 times      *
        * to "shuffle" the deck.                 *
        *****************************************/
        public static Queue<Card> Shuffle(Queue<Card> shuffled, int nCards)
        {
            Card[] cardArray = new Card[nCards];
            Random random = new Random();

            for (int i = 0; i < nCards; i++) //Sets all cards faceup so they can be used again.
            {
                Card tmpCard = shuffled.Dequeue();
                tmpCard.faceUp = true;
                cardArray[i] = tmpCard;
            }

            //swaps two random cards within the deck.
            for (int i = 0; i < 1000; i++)
            {
                int j = random.Next(0, nCards);
                int k = random.Next(0, nCards);

                Card tmpCard = cardArray[j];
                cardArray[j] = cardArray[k];
                cardArray[k] = tmpCard;
            }

            //Puts the cards into the queue
            for (int i = 0; i < nCards; i++)
            {
                shuffled.Enqueue(cardArray[i]);
            }
            return shuffled;

        }
        #endregion

        #region GetDeck
        /*****************************************
        * Queue<Card> GetRandomDeckOfCards()     *
        * Fills a queue with 52 cards in order   *
        * of rank and suit.                      *
        *****************************************/
        public static Queue<Card> GetRandomDeckOfCards()
        {
            Queue<Card> deckOfCards = new Queue<Card>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 2; j < 15; j++)
                {
                    deckOfCards.Enqueue(new Card(j, i));

                }
            }

            Queue<Card> MyPlayingCards = Shuffle(deckOfCards, 51);

            return MyPlayingCards;

        }
        #endregion

        #region DealCard
        /**********************************
        * Card DealCard(Queue<Card>, Int) *
        * Player deals a card and sets it *
        * face up.                        *
        ***********************************/
        public static Card DealCard(Queue<Card> PlayerDeck, int player)
        {
            CheckShuffle(PlayerDeck, player);
            Card playertmpCard = PlayerDeck.Dequeue();
            playertmpCard.faceUp = false;
            Console.WriteLine("Player {0}'s Card: {1}", player, CardToString(playertmpCard));

            return playertmpCard;

        }
        #endregion

        #region CheckGameOver
        /**************************************************
        * bool CheckGameOver(bool,Queue<Card>,Queue<Card> *
        * Checks to see if either player is out of cards  *
        * if they are, then the opposite player wins.     *
        ***************************************************/
        public static bool CheckGameOver(bool isGameOn, Queue<Card> Player1Deck, Queue<Card> Player2Deck)
        {

            if (Player1Deck.Count == 0)
            {
                isGameOn = false;

            }

            if (Player2Deck.Count == 0)
            {
                isGameOn = false;

            }

            return isGameOn;
        }
        #endregion

        #region CheckShuffle
        /************************************
        * CheckShuffle(Queue<Card>, Int)    *
        * Checks if the player has any      *
        * face up cards, if not it shuffles *
        * their deck, and returns it.       *
        *************************************/
        public static void CheckShuffle(Queue<Card> PlayerDeck, int player)
        {
            if (PlayerDeck.Peek().faceUp == false)
            {
                Console.WriteLine("Player {0} Is shuffling.\n", player);
                Shuffle(PlayerDeck, PlayerDeck.Count);
            }
        }
        #endregion

        #region PlayGame
        /***********************************************************
        * PlayGame()                                               *
        * Starts by spliting the deck of cards equally.            *
        * The game then plays automatically, if both               *
        * player's ranks are the same then WAR is played.          *
        * War is played until a player's 4th card is a higher rank *
        * or the opposing player is out of cards and loses.        *
        * Otherwise the higher rank hand wins the round and takes  *
        * both cards. This continues until a player is out of cards*
        ************************************************************/
        public static void PlayGame()
        {
            #region Initalize Player's Decks
            Queue<Card> MyPlayingCards = GetRandomDeckOfCards();
            Queue<Card> Player1Deck = new Queue<Card>();
            Queue<Card> Player2Deck = new Queue<Card>();
            bool isGameOn = true;

            //Splits the deck of cards evenly.
            for (int i = 0; i < 26; i++)
            {
                Player1Deck.Enqueue(MyPlayingCards.Dequeue());
            }

            for (int i = 26; i < 52; i++)
            {
                Player2Deck.Enqueue(MyPlayingCards.Dequeue());
            }
            #endregion

            while (CheckGameOver(isGameOn, Player1Deck, Player2Deck))
            {
                Card player1tmpCard = DealCard(Player1Deck, 1);
                Card player2tmpCard = DealCard(Player2Deck, 2);

                if (player1tmpCard.Rank > player2tmpCard.Rank)
                {
                    Console.WriteLine("\nPlayer 1 Wins the round!\n");
                    Player1Deck.Enqueue(player1tmpCard);
                    Player1Deck.Enqueue(player2tmpCard);
                }

                else if (player1tmpCard.Rank == player2tmpCard.Rank)
                {
                    Console.WriteLine("\nWAR!!!!\n");
                    Queue<Card> cardBank = new Queue<Card>();
                    bool war = true;

                    while (war)
                    {
                        cardBank.Enqueue(player1tmpCard);
                        cardBank.Enqueue(player2tmpCard);

                        //Deals three cards from each player's deck
                        for (int i = 0; i < 3; i++)
                        {
                            if (!CheckGameOver(isGameOn, Player1Deck, Player2Deck))
                            {
                                break;

                            }

                            player1tmpCard = DealCard(Player1Deck, 1);
                            player2tmpCard = DealCard(Player2Deck, 2);

                            cardBank.Enqueue(player1tmpCard);
                            cardBank.Enqueue(player2tmpCard);

                        }

                        if (!CheckGameOver(isGameOn, Player1Deck, Player2Deck))
                        {
                            break;
                        }
                        //Deals one last card from each deck
                        player1tmpCard = DealCard(Player1Deck, 1);
                        player2tmpCard = DealCard(Player2Deck, 2);


                        if (player1tmpCard.Rank > player2tmpCard.Rank)
                        {
                            Console.WriteLine("Player 1 Wins the War!");

                            Player1Deck.Enqueue(player1tmpCard);
                            Player1Deck.Enqueue(player2tmpCard);
                            //Puts the pile of cards into the player's deck
                            while (cardBank.Count != 0)
                            {
                                Player1Deck.Enqueue(cardBank.Dequeue());
                            }

                            break;
                        }
                        else if (player1tmpCard.Rank == player2tmpCard.Rank)
                        {
                            Console.WriteLine("The war Continues!\n");
                        }
                        else
                        {
                            Console.WriteLine("Player 2 Wins the War!");

                            Player2Deck.Enqueue(player1tmpCard);
                            Player2Deck.Enqueue(player2tmpCard);

                            while (cardBank.Count != 0)
                            {
                                Player2Deck.Enqueue(cardBank.Dequeue());
                            }

                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\nPlayer 2 Wins the round!\n");
                    Player2Deck.Enqueue(player1tmpCard);
                    Player2Deck.Enqueue(player2tmpCard);
                }

                if (Player1Deck.Count == 0)
                {
                    Console.WriteLine("Player Two Wins the Game!\n");
                }
                if (Player2Deck.Count == 0)
                {
                    Console.WriteLine("Player One Wins the Game!\n");
                }
            }
        }
        #endregion
        public static void Main()
        {
            int PlayerAnswer = 0;
            bool validInput = false;

            while (PlayerAnswer != 3 || validInput == false)
            {
                Console.WriteLine("Welcome to War, please select an option below.\n 1. Play Game.\n 2. Display Rules.\n 3. Exit Game.\n");

                String input = Console.ReadLine();

                if (int.TryParse(input, out PlayerAnswer))
                {
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("Please Enter Option 1-3, you entered {0}\n", input);
                }

                switch (PlayerAnswer)
                {
                    case 1:
                        PlayGame();
                        break;
                    case 2:
                        #region Rules Text Block
                        Console.WriteLine("The Game Of War: \n Each Player will be dealt 26 cards, both players then flip a card, the\n higher rank wins the" +
    " round and the winner will take both cards and return\n" +
    " them to a face down pile. When a player runs out of face up cards,\n" +
    " they shuffle all of their cards and turn them face up to be used again.\n" +
    " Incase of a tie, the player's shall enter \"War\". During War both players\n" +
    " will place three cards face down and the last card face up. The winner will\n" +
    " take the pile of eight cards, and a tie will result in another\n" +
    " war. The first player to lose all of their cards will be declared the loser.\n" +
    " In the event that a player runs out of cards during war, they will instantly\n " +
    " lose the game since they " + "can not adequately particpate in war.\n ");
                        #endregion
                        break;
                    case 3:
                        break;
                    default:
                        Console.WriteLine("Please Select a Valid Menu Option, you entered {0}\n", PlayerAnswer);
                        break;

                }
            }
        }

    }
}
