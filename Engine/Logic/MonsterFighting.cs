using Engine.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Logic
{
    public static class MonsterFighting
    {
        private static Monster _monster;
        private static Player _player;
        private static Weapon _weapon;

        private static void InitiateVariables(Player player, Monster monster, Weapon weapon)
        {
            _monster = monster;
            _player = player;
            _weapon = weapon;
        }

        private static bool IsThereAMonster()
        {
            if (_monster == null)
                return false;
            return true;
        }

        private static bool IsMonsterAlive()
        {
            if (_monster.CurrentHitPoints == 0)
                return false;

            return true;
        }


        private static bool DoesPlayerHaveTheWeapon()
        {
            if (_player.InventoryItems.ContainsKey(_weapon.ID))
            {
                if (_player.InventoryItems[_weapon.ID] > 0)
                    return true;
            }
            return false;
        }

        private static void PlayerKilledTheMonster()
        {
            _monster.Alive = false;

            // Get Rewards
            _player.Gold += _monster.RewardGold;
            _player.AddExperiencePoints(_monster.RewardExperiencePoints);

          
            foreach (var item in _monster.LootItems)
            {
                if (ComplexRandomGenerator.NumberBetween(0, 100) <= item.Value)
                {
                    _player.AddItemToInventory(item.Key);
                }
            }
        }

        private static void MonsterKilledThePlayer()
        {
            _player.Alive = false;
        }

        public static int FightMonster(Player player, Monster monster, Weapon weapon)
        {
            InitiateVariables(player, monster, weapon);

            if (!IsThereAMonster()) return 1;
            if (!IsMonsterAlive()) return 2;
            if (!DoesPlayerHaveTheWeapon()) return 3;

            // The Fight Begins
            int weaponDamage = ComplexRandomGenerator.NumberBetween(_weapon.MinimumDamage, _weapon.MaximumDamage);

            _player.UseItemFromInventory(_weapon.ID);
            _monster.CurrentHitPoints -= weaponDamage;
            _monster.LastDamageTaken = weaponDamage;

            // Monster Died
            if (_monster.CurrentHitPoints <= 0)
            {
                _monster.CurrentHitPoints = 0;
                PlayerKilledTheMonster();
                return 4;
            }
            // Monster Still Alive and Attacks the player back
            else
            {
                int monsterDamage = ComplexRandomGenerator.NumberBetween(0, _monster.MaxDamage);
                _player.CurrentHitPoints -= monsterDamage;
                _player.LastDamageTaken = monsterDamage;

                if(player.CurrentHitPoints <= 0)
                {
                    MonsterKilledThePlayer();
                    return 5;
                }
                else
                {
                    // Both Monster and Player are Alive.
                    return 6;
                }
                
            }

        }
    }
}
