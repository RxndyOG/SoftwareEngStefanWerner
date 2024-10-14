using System.Runtime.Intrinsics.X86;

namespace SWE.Models
{
    public class User
    {
        public User()
        {
            UserID = 1;
            Username = "Empty Name";
            HashPass = "xdxdxd";
            Wins = 0;
            Lose = 0;
            Elo = 100;
            coins = 16;
            token = string.Empty;
            BIO = string.Empty;
            IMG = string.Empty;

        }

        private int UserID;
        public string Username;
        private string HashPass;
        private int Wins = 0;
        private int Lose = 0;
        public int Elo = 100;
        private int coins = 20;
        private string token;

        private string BIO;
        private string IMG;

        private List<Cards> stack = new List<Cards>();
        private List<Cards> deck = new List<Cards>();

        public int setGetUserID
        {
            get => UserID;
            set => UserID = value;
        }

        public string setGetPassword
        {
            get => HashPass;
            set => HashPass = value;
        }

        public string setGetToken
        {
            get => token;
            set => token = value;
        }

        public string setGetBio
        {
            get => BIO;
            set => BIO = value;
        }

        public string setGetImg
        {
            get => IMG;
            set => IMG = value;
        }

        public int setGetWins
        {
            get => Wins;
            set => Wins = value;
        }

        public int setGetLose
        {
            get => Lose;
            set => Lose = value;
        }

        public int setGetCoins
        {
            get => coins;
            set => coins = value;
        }

        public int calcElo()
        {
            return Elo + (setGetWins * 3 - setGetLose * 5);
        }

        public List<Cards> getSetStack
        {
            get => stack;
            set => stack = value;
        }

        public List<Cards> setGetDeck
        {
            get => deck;
            set => deck = value;
        }

        public void addToStack(Cards[] newCards)       // gibt neue karten dem stack hinzu
        {

            foreach (var card in newCards)
            {
                getSetStack.Add(card);
            }

            int i = 0;

            while (i < getSetStack.Count)
            {
                if (getSetStack[i] == null)
                {
                    getSetStack.RemoveAt(i);
                    i--;
                }
                i++;
            }
        }

        public void printUserData()       // printed das profil
        {
            Console.WriteLine("\nUser ID: " + setGetUserID);
            Console.WriteLine("Username: " + Username);
            Console.WriteLine("Password: " + setGetPassword);
            Console.WriteLine("Bio: " + BIO);
            Console.WriteLine("IMG: " + IMG);
            Console.WriteLine("Wins: " + setGetWins);
            Console.WriteLine("Loses: " + setGetLose);
            Console.WriteLine("Elo: " + calcElo());
            Console.WriteLine("Coins: " + setGetCoins);
            Console.WriteLine("Token: " + setGetToken);
            Console.WriteLine("Amount of owned Cards: " + getSetStack.Count());
            Console.WriteLine("Amount of Cards in Deck: " + setGetDeck.Count() + "\n");
        }

        public void changeAccount(Dictionary<string, string> data)    //ändert das profil momentan nur 4 Werte weil nicht mehr im curl skript gefragt wird. Bei der endabgabe dann mehr
        {

            foreach (var _data in data)
            {
                switch (_data.Key.GetHashCode())
                {
                    case int n when n == "Name".GetHashCode():
                            Username  = _data.Value.ToString();
                        break;
                    case int n when n == "Password".GetHashCode():
                        setGetPassword = _data.Value.ToString();
                        break;
                    case int n when n == "Bio".GetHashCode():
                        setGetBio = _data.Value.ToString();
                        break;
                    case int n when n == "Image".GetHashCode():
                        setGetImg = _data.Value.ToString();
                        break;
                }
            }
        }

        public void tryPackage()    // wird benutzt wenn packages random sein sollen momentan wird es nicht benutzt
        {
            Cards[] openedCards = new Cards[6];

            Package packages = new Package();

            if (setGetCoins >= 4)
            {
                setGetCoins = setGetCoins - 4;
                openedCards = packages.openPackage();
                addToStack(openedCards);
            }
            else
            {
                Console.WriteLine("Not enough Coins");
            }
        }

        public bool printStack()    //printed den gesammten stack
        {
            if (getSetStack == null || getSetStack.Count == 0)
            {
                Console.WriteLine("No Cards in Stack");
                return false;
            }

            int i = 0;

            while (i < getSetStack.Count)
            {
         
                if (getSetStack[i] != null)
                {
                    getSetStack[i].printCardInfo(i);
                }
                else
                {
                    getSetStack.RemoveAt(i);
                    i--;
                }
                i++;
            }

            Console.WriteLine("Amount of Cards in Stack: " + i);
            return true;
        }


        public bool printDeck()     //printed das deck
        {
            if (setGetDeck == null || setGetDeck.Count == 0)
            {
                Console.WriteLine("No Cards in Deck");
                return false;
            }

            for (int i = 0; i < setGetDeck.Count(); i++)
            {
                setGetDeck[i].printCardInfo(i);
            }

            return true;
        }
    }
}
