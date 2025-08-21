using GTA;
using GTA.Math;
using GTA.UI;
using GTA.Native;
using System;
using System.Drawing;

namespace GTAVWebhook.Types
{
   internal class SpawnnerEnemies
   {
      private Ped npc = null;
      private string name = null;
      private Color nameColor;
      private EnemyConfiguration enemyConf = new EnemyConfiguration();
      public EnemyConfiguration EnemyConfiguration => enemyConf;

      public  SpawnnerEnemies(string name, PedHash modelName, WeaponHash weaponName, string spawnName, bool withWeapons = false, bool firing = false) { 
         this.name = name;
         Vector3 position = ((Entity)Game.Player.Character).Position;
         Model model = new Model(modelName);
         npc = World.CreatePed(model, position, 0f);

         if (withWeapons || new Random().Next(1, 25) == 10)
         {
            npc.Weapons.Give(weaponName, 9999, true, true);
            Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_PED, npc, (uint)3337513804);
            npc.Task.Combat(Game.Player.Character);
            if (firing) {
               Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, npc, 16, true);
               Function.Call(Hash.SET_PED_INFINITE_AMMO_CLIP, npc, true);
               npc.Task.ShootAt(Game.Player.Character, -1, FiringPattern.FullAuto);
            }
         }

         if (spawnName != "Spiderman") {
            int gangGroup = Function.Call<int>(Hash.GET_HASH_KEY, "GANG_1");
            Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, npc, gangGroup);

            int playerGroup = Function.Call<int>(Hash.GET_HASH_KEY, "PLAYER");
            Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 5, gangGroup, playerGroup);
            Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 5, playerGroup, gangGroup);
         }

         Function.Call(Hash.TASK_COMBAT_PED, npc.Handle, Game.Player.Character.Handle, 0, 16);
         npc.Task.Combat(Game.Player.Character);

         enemyConf.Configuration(npc, spawnName);
         this.nameColor = GetRandomBrightColor();
      }

      public void StartShooting(int durationSeconds)
        {
            if (npc != null)
            {

                npc.Weapons.Give((WeaponHash)1834241177, 100, true, true);
                npc.Task.ShootAt(Game.Player.Character, durationSeconds * 1000, (FiringPattern)0);
            }
        }

        public void Remove()
        {
            npc?.Delete();
            npc = null;
        }

        public void DrawName()
        {
            if (npc != null && npc.IsOnScreen &&
                World.GetDistance(npc.Position, Game.Player.Character.Position) <= 30f)
            {
                PointF screenPos = Screen.WorldToScreen(npc.Position);
                TextElement nameTag = new TextElement(name, screenPos, 1.0f, nameColor, GTA.UI.Font.Pricedown, Alignment.Center)
                {
                    Outline = true
                };
                nameTag.Draw();
            }
        }

        private Color GetRandomBrightColor()
        {
            Random random = new Random();
            int red;
            int green;
            int blue;
            switch (random.Next(1, 6))
            {
                case 1:
                    red = random.Next(30, 80);
                    green = random.Next(200, 256);
                    blue = random.Next(30, 80);
                    break;
                case 2:
                    red = random.Next(200, 256);
                    green = random.Next(20, 50);
                    blue = random.Next(20, 50);
                    break;
                case 3:
                    red = random.Next(0, 50);
                    green = random.Next(200, 256);
                    blue = random.Next(200, 256);
                    break;
                case 4:
                    red = random.Next(200, 256);
                    green = random.Next(200, 256);
                    blue = random.Next(0, 50);
                    break;
                case 5:
                    red = random.Next(150, 200);
                    green = random.Next(50, 100);
                    blue = random.Next(150, 200);
                    break;
                case 6:
                    red = random.Next(200, 255);
                    green = random.Next(100, 180);
                    blue = random.Next(150, 255);
                    break;
                default:
                    red = (green = (blue = 255));
                    break;
            }
            return Color.FromArgb(red, green, blue);
        }
    }
}
