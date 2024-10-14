
namespace SWE.Models
{
    public class MonsterCard : Cards
    {
        public MonsterCard() 
        {
            GetSetCardType = (int) CardType.monster;
            GetSetMonsterFamily = (int) monsterFam.Goblins;
            GetSetElementType = (int) elementType.fire;
            GetSetDamage = 2;
            GetSetCardName = "Empty Name";
        }

        public void GenereateRandomMonsterCard(Random rnd)
        {
            GetSetCardType= (int) CardType.monster;
            int r = rnd.Next(1,15);
            if (r % 2 != 0)
            {
                r += 1;
            }
            GetSetMonsterFamily = r;
            r = rnd.Next(1, 7);
            if (r % 2 != 0)
            {
                r += 1;
            }
            GetSetElementType = r;
            GetSetDamage = 2;
            GetSetCardName = "Empty Name";
        }

    }
}
