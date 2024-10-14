namespace SWE.Models
{
    public class Cards
    {
        public Cards()
        {
            
            ID = string.Empty;
            CardName = "Empty Name";
            cardType = 2;
            ElementType = 2;
            monsterFamily = 2;
            damage = 2;
            
        }

        private string ID;
        private string CardName;
        private int cardType;
        private int ElementType;
        private int monsterFamily;
        private float damage;

        private int descSpell;
        private int SpellDamage;

        public enum CardType
        {
            monster = 2,
            spell = 4
        }
        public enum elementType
        {
            fire = 2,
            water = 4,
            normal = 6
        }
        public enum monsterFam
        {
            Goblins = 2,
            Dragons = 4,
            Wizzard = 6,
            Orks = 8,
            Knights = 10,
            Kraken = 12,
            FireElves = 14
        }

        public Dictionary<int, string> desc = new Dictionary<int, string>()      // wird dann später für random karte benutz
        {
            {2, "SchmeiSSe einen Stein auf den Gegner"},
            {4, "SchmeiSSe einen groSSen Feuerball auf den Gegner"},
            {6, "METEOR!!!!!"}
        };

        public Dictionary<int, int> spellDamage = new Dictionary<int, int>()       // wird dann später für random karte benutz
        {
            {2, 1},
            {4, 4},
            {6, 7},
        };

        public Dictionary<int, string> families = new Dictionary<int, string>() {    // wird dann später für random karte benutz
            {2, "Goblins"},
            {4, "Dragons"},
            {6, "Wizzard"},
            {8, "Orks"},
            {10, "Knights"},
            {12, "Kraken"},
            {14, "FireElves"}
        };

        public string GetSetID
        {
            get => ID; 
            set => ID = value;
        }

        public string GetSetCardName
        {
            get => CardName;
            set => CardName = value;
        }

        public int GetSetCardType
        {
            get => cardType;
            set => cardType = value;
        }

        public int GetSetElementType
        {
            get => ElementType;
            set => ElementType = value;
        }

        public int GetSetMonsterFamily
        {
            get => monsterFamily;
            set => monsterFamily = value;
        }

        public float GetSetDamage
        {
            get => damage; 
            set => damage = value;
        }

        public int GetSetDescSpell
        {
            get => descSpell;
            set => descSpell = value;
        }

        public int GetSetSpellDamage
        {
            get => SpellDamage;
            set => SpellDamage = value;
        }

        public void printCardInfo(int i)    //printed die karte mit id  (gedacht während dem battle)
        {
            if (GetSetCardType == 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("- Card ID: " + i + " ------------------------");
                Console.WriteLine(" Card Type: Monster");
                Console.WriteLine("  ID: " + GetSetID);
                Console.WriteLine("  Card Name: " + GetSetCardName);
                Console.WriteLine("   Monster Family: " + families[GetSetMonsterFamily]);
                switch (GetSetElementType)
                {
                    case 2:
                        Console.WriteLine("    Element Type: Fire");
                        break;
                    case 4:
                        Console.WriteLine("    Element Type: Water");
                        break;
                    case 6:
                        Console.WriteLine("    Element Type: Normal");
                        break;
                    default:
                        Console.WriteLine("    Element Type: No Element Given");
                        break;
                }
                Console.WriteLine("-------------------------------------");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("- Card ID: " + i + " ------------------------");
                Console.WriteLine(" Card Type: Spell");
                Console.WriteLine("  Description: " + desc[GetSetDescSpell]);
                Console.WriteLine("   Spell Damage: " + spellDamage[GetSetSpellDamage]);
                Console.WriteLine("-------------------------------------");
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void printCardInfo()      //printed die karte ohne id
        {
            if (GetSetCardType == 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("-------------------------------------");
                Console.WriteLine(" Card Type: Monster");
                Console.WriteLine("  Card Name: " + GetSetCardName);
                Console.WriteLine("  ID: " + GetSetID);
                Console.WriteLine("   Monster Family: " + families[GetSetMonsterFamily]);
                switch (GetSetElementType)
                {
                    case 2:
                        Console.WriteLine("    Element Type: Fire");
                        break;
                    case 4:
                        Console.WriteLine("    Element Type: Water");
                        break;
                    case 6:
                        Console.WriteLine("    Element Type: Normal");
                        break;
                    default:
                        Console.WriteLine("    Element Type: No Element Given");
                        break;
                }
                Console.WriteLine("-------------------------------------");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("-------------------------------------");
                Console.WriteLine(" Card Type: Spell");
                Console.WriteLine("  Description: " + desc[GetSetDescSpell]);
                Console.WriteLine("   Spell Damage: " + spellDamage[GetSetSpellDamage]);
                Console.WriteLine("-------------------------------------");
            }

            Console.WriteLine();
            Console.ForegroundColor= ConsoleColor.White;
        }
    }
}
