namespace SWE.Models
{
    public class SpellCard : Cards
    {
        public SpellCard()
        {
            GetSetCardType = (int)CardType.monster;
            GetSetDescSpell = 2;
            GetSetSpellDamage = 2;
            GetSetCardName = "Empty Name";
        }

        public void GenereateRandomSpellCard(Random rnd)    // wird momentan nicht benötigt wird benutzt bei random karten kreation
        {
            GetSetCardType = (int)CardType.spell;
            int r = rnd.Next(1, 6);
            if (r % 2 != 0)
            {
                r += 1;
            }
            GetSetDescSpell = r;
            GetSetSpellDamage = r;
            GetSetCardName = "Empty Name";
        }

    }
}
