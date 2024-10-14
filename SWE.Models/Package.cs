namespace SWE.Models
{
    public class Package
    {

        public Package() 
        {
            
        }

        Random rnd = new Random();
        public Cards[] card = new Cards[6];
        

        
        public Cards[] openPackage()
        {

            for (int i = 0; i < 5; i++)
            {
                int random = rnd.Next(0, 2);
                if (random == 0)
                {
                    MonsterCard cardTemp = new MonsterCard();
                    cardTemp.GenereateRandomMonsterCard(rnd);
                    card[i] = cardTemp;
                }
                else
                {
                   SpellCard cardTemp = new SpellCard();
                   cardTemp.GenereateRandomSpellCard(rnd);
                   card[i] = cardTemp;
                }
                
            }

            return card;
        }


    }
}
