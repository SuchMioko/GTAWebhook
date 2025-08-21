using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTAVWebhook;
using GTAVWebhook.Types;

public class GTAVWebhookScript : Script
{
    private HttpServer httpServer = new HttpServer();
    private bool isFirstTick = true;
    private List<Vehicle> spawnedVehicles = new List<Vehicle>();
    private List<NPC> npcRandom = new List<NPC>();
    private List<SpawnnerEnemies> siButa = new List<SpawnnerEnemies>();
    private List<SpawnnerEnemies> siIronman = new List<SpawnnerEnemies>();
    private List<SpawnnerEnemies> siKuya = new List<SpawnnerEnemies>();
    private List<SpawnnerEnemies> siNaruto = new List<SpawnnerEnemies>();
    private List<SpawnnerEnemies> siSpiderman = new List<SpawnnerEnemies>();
    private List<SpawnnerEnemies> siTransformer = new List<SpawnnerEnemies>();
    private List<SpawnnerEnemies> siThanos = new List<SpawnnerEnemies>();
    private List<SpawnnerEnemies> siZeus = new List<SpawnnerEnemies>();

    private SpawnnerAir spawnAir;
    private SpawnnerVehicles spawnVehicles;
    private List<Vector3> spawnedVehiclePositions = new List<Vector3>();

    private Random rng = new Random();

    public GTAVWebhookScript()
    {
        spawnAir = new SpawnnerAir();
        spawnVehicles = new SpawnnerVehicles();
        Tick += OnTick;
        KeyUp += OnKeyUp;
        KeyDown += OnKeyDown;
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (isFirstTick)
        {
            isFirstTick = false;

            Logger.Clear();

            try
            {
                httpServer.Start();
                Logger.Log("HttpServer listening on port " + httpServer.Port);
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to start HttpServer - " + ex.Message);
            }

        }

        spawnVehicles.DrawName();
        spawnAir.DrawName();



        while (siButa.Count > 100)
        {
            try
            {
                siButa[0].Remove();
                siButa.RemoveAt(0);
                Logger.Log("Attacker3 over limit removed");
            }
            catch (Exception ex1)
            {
                Logger.Log("Failed to remove old attacker3 - " + ex1.Message);
            }
        }

        while (siThanos.Count > 100)
        {
            try
            {
                siThanos[0].Remove();
                siThanos.RemoveAt(0);
                Logger.Log("Attacker3 over limit removed");
            }
            catch (Exception ex2)
            {
                Logger.Log("Failed to remove old attacker3 - " + ex2.Message);
            }
        }

        while (siNaruto.Count > 100)
        {
            try
            {
                siNaruto[0].Remove();
                siNaruto.RemoveAt(0);
                Logger.Log("Attacker3 over limit removed");
            }
            catch (Exception ex3)
            {
                Logger.Log("Failed to remove old attacker3 - " + ex3.Message);
            }
        }

        while (siKuya.Count > 100)
        {
            try
            {
                siKuya[0].Remove();
                siKuya.RemoveAt(0);
                Logger.Log("Attacker3 over limit removed");
            }
            catch (Exception ex4)
            {
                Logger.Log("Failed to remove old attacker3 - " + ex4.Message);
            }
        }

        while (siIronman.Count > 100)
        {
            try
            {
                siIronman[0].Remove();
                siIronman.RemoveAt(0);
                Logger.Log("Attacker3 over limit removed");
            }
            catch (Exception ex5)
            {
                Logger.Log("Failed to remove old attacker3 - " + ex5.Message);
            }
        }

        while (siSpiderman.Count > 100)
        {
            try
            {
                siSpiderman[0].Remove();
                siSpiderman.RemoveAt(0);
                Logger.Log("Attacker3 over limit removed");
            }
            catch (Exception ex6)
            {
                Logger.Log("Failed to remove old attacker3 - " + ex6.Message);
            }
        }

        while (siTransformer.Count > 100)
        {
            try
            {
                siTransformer[0].Remove();
                siTransformer.RemoveAt(0);
                Logger.Log("Attacker3 over limit removed");
            }
            catch (Exception ex7)
            {
                Logger.Log("Failed to remove old attacker3 - " + ex7.Message);
            }
        }

        while (siZeus.Count > 100)
        {
            try
            {
                siZeus[0].Remove();
                siZeus.RemoveAt(0);
                Logger.Log("Attacker3 over limit removed");
            }
            catch (Exception ex8)
            {
                Logger.Log("Failed to remove old attacker3 - " + ex8.Message);
            }
        }

        try
        {
            foreach (SpawnnerEnemies ngaranIronman in siIronman)
            {
               ngaranIronman.DrawName();
            }
            foreach (SpawnnerEnemies ngaranZeus in siZeus)
            {
               ngaranZeus.DrawName();
            }
            foreach (SpawnnerEnemies ngaranKuya in siKuya)
            {
                ngaranKuya.DrawName();
            }
            foreach (SpawnnerEnemies ngaranButa in siButa)
            {
                ngaranButa.DrawName();
            }
            foreach (NPC npc in npcRandom)
            {
               npc.DrawName();
            }
        }
        catch (Exception ex)
        {
            Logger.Log("Failed to draw attacker names - " + ex.Message);
        }

        CommandInfo command = httpServer.DequeueCommand();

        if (command != null)
        {
            try
            {
                ProcessCommand(command);
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to execute command " + command.cmd + ". Error: " + ex.Message);
            }
        }

        foreach (Vehicle spawnedVehicle in spawnedVehicles)
        {
            if ((Entity)(object)spawnedVehicle != (Entity)null && spawnedVehicle.Exists())
            {
                spawnVehicles.FlipVehicleIfUpsideDown(spawnedVehicle);
            }
        }
        foreach (Vehicle spawnedVehicle2 in spawnedVehicles)
        {
            if ((Entity)(object)spawnedVehicle2 != (Entity)null && spawnedVehicle2.Exists())
            {
                spawnAir.FlipVehicleIfUpsideDown(spawnedVehicle2);
            }
        }

    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {

    }

    private void ProcessCommand(CommandInfo command)
    {
        switch (command.cmd)
        {
            case "spawn_npc":
            {
               if (!int.TryParse(command.custom, out var result7)) {
                  result7 = 1;
               }
               if (result7 > 30)
               {
                  result7 = 30;
               }
               for (int num7 = 0; num7 < result7; num7++)
               {
                  Logger.Log("Spawn Attacker");
                  NPC item16 = new NPC(command.username, withMeleeWeapon: true);
                  npcRandom.Add(item16);
               }
               break;
            }
            case "spawn_spiderman":
            {
               int result9;
               if (!int.TryParse(command.custom, out result9)) {
                  result9 = 1;
               }
               if (result9 > 100) {
                  result9 = 100;
               }
               for (int num9 = 0; num9 < result9; num9++) {
                  Logger.Log("Spawn Attacker4 with gun");
                  SpawnnerEnemies enemy = new SpawnnerEnemies(
                     command.username, PedHash.Barry, WeaponHash.Railgun, "Spiderman", true, false
                  );
                  siSpiderman.Add(enemy);
               }
               break;
            }
            case "spawn_thanos":
            {
               int jumlahSpawn;
               if (!int.TryParse(command.custom, out jumlahSpawn)) {
                  jumlahSpawn = 1;
               }
               if (jumlahSpawn > 100) {
                  jumlahSpawn = 100;
               }
               for (int i = 0; i < jumlahSpawn; i++) {
                  Logger.Log("Spawn Attacker4 with gun");
                  SpawnnerEnemies enemy = new SpawnnerEnemies(
                     command.username, PedHash.Josef, WeaponHash.Railgun, "Thanos", true, false
                  );
                  siThanos.Add(enemy);

                }
                break;
            }
            case "spawn_ninja":
                {
                    int jumlahSpawn;
                    if (!int.TryParse(command.custom, out jumlahSpawn))
                    {
                        jumlahSpawn = 1;
                    }
                    if (jumlahSpawn > 100)
                    {
                        jumlahSpawn = 100;
                    }
                    for (int i = 0; i < jumlahSpawn; i++)
                    {
                        Logger.Log("Spawn Attacker4 with gun");
                        SpawnnerEnemies enemy = new SpawnnerEnemies(
                           command.username, PedHash.Car3Guy1, WeaponHash.UnholyHellbringer, "Turtle", true, false
                        );
                        siKuya.Add(enemy);
                    }
                    break;
                }
            case "spawn_transformer":
            {
               int result9;
               if (!int.TryParse(command.custom, out result9)) {
                  result9 = 1;
               }
               if (result9 > 100) {
                  result9 = 100;
               }
               for (int num9 = 0; num9 < result9; num9++) {
                  Logger.Log("Spawn Attacker4 with gun");
                  SpawnnerEnemies enemy = new SpawnnerEnemies(
                     command.username, PedHash.ONeil, WeaponHash.Railgun, "Transformer", true, false
                  );
                  siTransformer.Add(enemy);
               }
               break;
            }
            case "spawn_naruto":
            {
               int result9;
               if (!int.TryParse(command.custom, out result9)) {
                  result9 = 1;
               }
               if (result9 > 100) {
                  result9 = 100;
               }
               for (int num9 = 0; num9 < result9; num9++) {
                  Logger.Log("Spawn Attacker4 with gun");
                  SpawnnerEnemies enemy = new SpawnnerEnemies(
                     command.username, PedHash.Hunter, WeaponHash.Railgun, "Naruto", true, false
                  );
                  siNaruto.Add(enemy);
               }
               break;
            }
            case "spawn_godzila":
            {
               int result9;
               if (!int.TryParse(command.custom, out result9)) {
                  result9 = 1;
               }
               if (result9 > 100) {
                  result9 = 100;
               }
               for (int num9 = 0; num9 < result9; num9++) {
                  Logger.Log("Spawn Attacker4 with gun");
                  SpawnnerEnemies enemy = new SpawnnerEnemies(
                     command.username, PedHash.Ortega, WeaponHash.Railgun, "Godzilla", true, false
                  );
                  siButa.Add(enemy);
               }
               break;
            }

            case "spawn_ironman":
            {
               int result9;
               if (!int.TryParse(command.custom, out result9)) {
                  result9 = 1;
               }
               if (result9 > 100) {
                  result9 = 100;
               }
               for (int num9 = 0; num9 < result9; num9++) {
                  Logger.Log("Spawn Attacker4 with gun");
                  SpawnnerEnemies enemy = new SpawnnerEnemies(
                     command.username, PedHash.Car3Guy2, WeaponHash.Railgun, "Ironman", true, false
                  );
                  siIronman.Add(enemy);
               }
               break;
            }
            case "spawn_zeus":
            {
               int result9;
               if (!int.TryParse(command.custom, out result9)) {
                  result9 = 1;
               }
               if (result9 > 100) {
                  result9 = 100;
               }
               for (int num9 = 0; num9 < result9; num9++) {
                  Logger.Log("Spawn Attacker4 with gun");
                  SpawnnerEnemies enemy = new SpawnnerEnemies(
                     command.username, PedHash.Chef, WeaponHash.Railgun, "Zeus", true, false
                  );
                  siZeus.Add(enemy);
               }
               break;
            }
            case "spawn_batman":
            {
               Vector3 playerPos = Game.Player.Character.Position;
               Vector3 offset = Game.Player.Character.ForwardVector * 2f;
               float minDistance = 2f;
               int spawnCount = 1;
               float spawnRadius = 7f;

               for (int i = 0; i < spawnCount; i++) {
                  Vector3 spawnPos = default(Vector3);
                  bool foundPos = false;

                  for (int attempt = 0; attempt < 100; attempt++) {
                     float angle = (float)(rng.NextDouble() * 2.0 * Math.PI);
                     float distance = (float)(rng.NextDouble() * spawnRadius);
                     Vector3 candidatePos = offset * distance;
                     spawnPos = playerPos + candidatePos;

                     bool tooClose = false;
                     foreach (Vector3 existingPos in spawnedVehiclePositions) {
                        if (Vector3.Distance(spawnPos, existingPos) < minDistance) {
                           tooClose = true;
                           break;
                        }
                     }
                     if (!tooClose) {
                       foundPos = true;
                       break;
                     }
                  }

                  if (foundPos) {
                     spawnVehicles.SpawnVehicle(command.custom, spawnPos, command.username, PedHash.Zombie01, WeaponHash.Pistol);
                     spawnedVehiclePositions.Add(spawnPos);
                  }
               }
               break;
            }
            case "spawn_gojo":
            {
               Vector3 playerPos = Game.Player.Character.Position;
               Vector3 offset = Game.Player.Character.ForwardVector * 2f;
               float minDistance = 2f;
               int spawnCount = 1;
               float spawnRadius = 7f;

               for (int i = 0; i < spawnCount; i++) {
                  Vector3 spawnPos = default(Vector3);
                  bool foundPos = false;

                  for (int attempt = 0; attempt < 100; attempt++) {
                     float angle = (float)(rng.NextDouble() * 2.0 * Math.PI);
                     float distance = (float)(rng.NextDouble() * spawnRadius);
                     Vector3 candidatePos = offset * distance;
                     spawnPos = playerPos + candidatePos;

                     bool tooClose = false;
                     foreach (Vector3 existingPos in spawnedVehiclePositions) {
                        if (Vector3.Distance(spawnPos, existingPos) < minDistance) {
                           tooClose = true;
                           break;
                        }
                     }
                     if (!tooClose) {
                       foundPos = true;
                       break;
                     }
                  }

                  if (foundPos) {
                     spawnVehicles.SpawnVehicle(command.custom, spawnPos, command.username, PedHash.Groom, WeaponHash.MicroSMG);
                     spawnedVehiclePositions.Add(spawnPos);
                  }
               }
               break;
            }
            case "spawn_army":
            {
               Vector3 playerPos = Game.Player.Character.Position;
               Vector3 offset = Game.Player.Character.ForwardVector * 2f;
               float minDistance = 2f;
               int spawnCount = 1;
               float spawnRadius = 7f;

               for (int i = 0; i < spawnCount; i++) {
                  Vector3 spawnPos = default(Vector3);
                  bool foundPos = false;

                  for (int attempt = 0; attempt < 100; attempt++) {
                     float angle = (float)(rng.NextDouble() * 2.0 * Math.PI);
                     float distance = (float)(rng.NextDouble() * spawnRadius);
                     Vector3 candidatePos = offset * distance;
                     spawnPos = playerPos + candidatePos;

                     bool tooClose = false;
                     foreach (Vector3 existingPos in spawnedVehiclePositions) {
                        if (Vector3.Distance(spawnPos, existingPos) < minDistance) {
                           tooClose = true;
                           break;
                        }
                     }
                     if (!tooClose) {
                       foundPos = true;
                       break;
                     }
                  }

                  if (foundPos) {
                     spawnVehicles.SpawnVehicle(command.custom, spawnPos, command.username, PedHash.Zombie01, WeaponHash.Pistol);
                     spawnedVehiclePositions.Add(spawnPos);
                  }
               }
               break;
            }
            case "spawn_heli":
            {
               Vector3 playerPos = Game.Player.Character.Position;
               Vector3 offset = Game.Player.Character.ForwardVector * 2f;
               float minDistance = 2f;
               int spawnCount = 1;
               float spawnRadius = 7f;

               for (int i = 0; i < spawnCount; i++)
               {
                  Vector3 spawnPos = default(Vector3);
                  bool foundPos = false;

                  for (int attempt = 0; attempt < 100; attempt++)
                  {
                     float angle = (float)(rng.NextDouble() * 2.0 * Math.PI);
                     float distance = (float)(rng.NextDouble() * spawnRadius);
                     Vector3 candidatePos = offset * distance;
                     spawnPos = playerPos + candidatePos;

                     bool tooClose = false;
                     foreach (Vector3 existingPos in spawnedVehiclePositions)
                     {
                        if (Vector3.Distance(spawnPos, existingPos) < minDistance)
                        {
                           tooClose = true;
                           break;
                        }
                     }

                     if (!tooClose)
                     {
                       foundPos = true;
                       break;
                     }
                  }

                  if (foundPos)
                  {
                     spawnAir.SpawnAir(command.custom, spawnPos, command.username);
                     spawnedVehiclePositions.Add(spawnPos);
                  }
               }
               break;
            }
          

            default:
                {
                    Logger.Log("Unknown Command " + command.cmd);
                    break;
                }
        }
    }
}

