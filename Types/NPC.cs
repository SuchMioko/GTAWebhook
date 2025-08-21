using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.UI;

namespace GTAVWebhook.Types
{
    internal class NPC
    {
        private Ped npc = null;
        private string name = null;
        private Dictionary<Entity, Color> npcColors = new Dictionary<Entity, Color>();

        private static readonly List<WeaponHash> meleeWeapons = new List<WeaponHash>
        {
           WeaponHash.Crowbar,
           WeaponHash.Machete,
           WeaponHash.Hammer,
           WeaponHash.Pistol
        };

        private PedHash[] pedHashes;

        public NPC(string name, bool withMeleeWeapon = false)
        {
           pedHashes = new PedHash[]
           {
              PedHash.ChemWork01GMM,
              PedHash.Zombie01,
              PedHash.Babyd,
              PedHash.Clown01SMY,
              PedHash.Autopsy01SMY,
              PedHash.BallaOrig01GMY,
              PedHash.ChiCold01GMM,
              PedHash.FilmNoir
           };

           this.name = name;
           Random random = new Random();
           PedHash val = pedHashes[random.Next(pedHashes.Length)];
           Model model = new Model(val);
           if (Game.Player.Character.IsInVehicle())
           {
              Vector3 val2 = ((Entity)Game.Player.Character).Position + ((Entity)Game.Player.Character).ForwardVector * -1f;
              npc = World.CreatePed(model, val2, 0f);
           }
           else
           {
              Vector3 position = ((Entity)Game.Player.Character).Position;
              npc = World.CreatePed(model, position, 0f);
           }
           if (withMeleeWeapon || new Random().Next(1, 25) == 10)
           {
              WeaponHash val3 = meleeWeapons[new Random().Next(meleeWeapons.Count)];
              npc.Weapons.Give(val3, 100, true, true);
              npc.Task.FightAgainst(Game.Player.Character);
           }
           else
           {
              npc.Task.FightAgainst(Game.Player.Character);
           }
           npc.Task.FightAgainst(Game.Player.Character);
           ((Entity)npc).MaxHealth = 250;
           ((Entity)npc).Health = 250;
           npc.CanRagdoll = false;
           ((Entity)npc).IsExplosionProof = false;
           ((Entity)npc).IsFireProof = true;
           ((Entity)npc).IsMeleeProof = false;
           ((Entity)npc).IsBulletProof = false;
        }

        public void StartShooting(int duration)
        {
           if ((Entity)(object)npc != (Entity)null)
           {
              WeaponHash val = meleeWeapons[new Random().Next(meleeWeapons.Count)];
              npc.Weapons.Give(val, 100, true, true);
              npc.Task.FightAgainst(Game.Player.Character);
           }
        }

        public void DrawName()
        {
           if ((Entity)(object)npc != (Entity)null && name != null && World.GetDistance(((Entity)npc).Position, ((Entity)Game.Player.Character).Position) <= 30f && ((Entity)npc).IsOnScreen)
           {
              PointF pointF = Screen.WorldToScreen(((Entity)npc).Position, false);
              if (!npcColors.ContainsKey((Entity)(object)npc))
              {
                 Color randomBrightSpecificColor = GetRandomBrightSpecificColor();
                 npcColors.Add((Entity)(object)npc, randomBrightSpecificColor);
              }
              Color color = npcColors[(Entity)(object)npc];
              TextElement val = new TextElement(name, pointF, 0.8f, color, GTA.UI.Font.Pricedown, Alignment.Center);
              val.Outline = true;
              val.Draw();
           }
        }

        private Color GetRandomBrightSpecificColor()
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
