using System;
using WpfApp1.MVVM.Model;

namespace WpfApp1.MVVM.ViewModel
{
    internal class FightVM
    {
        public Monster PlayerMonster { get; private set; }
        public Monster OpponentMonster { get; private set; }
        public int MaxHealthPlayerMonster { get; private set; }
        public int MaxHealthOpponentMonster { get; private set; }

        // Constructeur de la classe FightVM
        public FightVM(Monster playerMonster, Monster opponentMonster, int maxHealthPlayerMonster, int maxHealthOpponentMonster)
        {
            PlayerMonster = playerMonster;
            OpponentMonster = opponentMonster;
            MaxHealthPlayerMonster = maxHealthPlayerMonster;
            MaxHealthOpponentMonster = maxHealthOpponentMonster;

            // Initialiser la santé des monstres
            PlayerMonster.Health = MaxHealthPlayerMonster;
            OpponentMonster.Health = MaxHealthOpponentMonster;
        }

        // Méthode pour infliger des dégâts à l'adversaire
        public void UseSpell(Spell spell)
        {
            OpponentMonster.Health -= spell.Damage;
            if (OpponentMonster.Health < 0) OpponentMonster.Health = 0;
        }

        // Méthode pour infliger des dégâts au joueur
        public void OpponentAttack(Spell opponentSpell)
        {
            PlayerMonster.Health -= opponentSpell.Damage;
            if (PlayerMonster.Health < 0) PlayerMonster.Health = 0;
        }

        // Vérifier si un monstre est mort
        public bool IsOpponentMonsterDead()
        {
            return OpponentMonster.Health <= 0;
        }

        public bool IsPlayerMonsterDead()
        {
            return PlayerMonster.Health <= 0;
        }
    }
}
