﻿using System.Collections.Generic;
using Fargowiltas.Items.Summons.Deviantt;
using Fargowiltas.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Fargowiltas.NPCs
{
    [AutoloadHead]
    public class Deviantt : ModNPC
    {
        private bool saidDefeatQuote;

        //public override bool Autoload(ref string name)
        //{
        //    name = "Deviantt";
        //    return mod.Properties.Autoload;
        //}

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deviantt");

            Main.npcFrameCount[NPC.type] = 23;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = -1f,
                Direction = -1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness.SetBiomeAffection<JungleBiome>(AffectionLevel.Like);
            //NPC.Happiness.LoveBiome(PrimaryBiomeID.Sky); //enable this when it exists
            NPC.Happiness.SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike);
            NPC.Happiness.SetBiomeAffection<DesertBiome>(AffectionLevel.Hate);

            NPC.Happiness.SetNPCAffection<Mutant>(AffectionLevel.Love);
            NPC.Happiness.SetNPCAffection<Abominationn>(AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Dislike);
            NPC.Happiness.SetNPCAffection(NPCID.Angler, AffectionLevel.Hate);

            NPCID.Sets.DebuffImmunitySets.Add(NPC.type, new Terraria.DataStructures.NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[]
                {
                    BuffID.Suffocation
                }
            });
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("Mods.Fargowiltas.Bestiary.Deviantt")
            });
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = NPC.downedMoonlord ? 50 : 15;
            NPC.lifeMax = NPC.downedMoonlord ? 2500 : 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.Angler;

            //if (GetInstance<FargoConfig>().CatchNPCs)
            //{
            //    Main.NPCCatchable[NPC.type] = true;
            //    NPC.catchItem = (short)mod.ItemType("Deviantt");
            //}
                
            NPC.buffImmune[BuffID.Suffocation] = true;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("DevianttAlive"))
                return false;

            return GetInstance<FargoConfig>().Devi && !FargoGlobalNPC.AnyBossAlive() 
                && ((FargoWorld.DownedBools.TryGetValue("rareEnemy", out bool value) && value)
                || (Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("EternityMode")));
        }

        public override bool CanGoToStatue(bool toKingStatue) => !toKingStatue;

        public override void AI()
        {
            NPC.breath = 200;
        }

        public override string TownNPCName()
        {
            string[] names = { "Akira", "Remi", "Saku", "Seira", "Koi", "Elly", "Lori", "Calia", "Teri", "Artt", "Flan", "Shion", "Tewi" };
            return Main.rand.Next(names);
        }

        public override string GetChat()
        {
            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("EternityMode")
                && !(bool)ModLoader.GetMod("FargowiltasSouls").Call("GiftsReceived"))
            {
                ModLoader.GetMod("FargowiltasSouls").Call("GiveDevianttGifts");
                return Main.npcChatText = "This world looks tougher than usual, so you can have these on the house just this once! Talk to me if you need any tips, yeah?";
            }

            if (Main.notTheBeesWorld)
            {
                string text = "HA";
                int max = Main.rand.Next(10, 50);
                for (int i = 0; i < max; i++)
                    text += " " + Main.rand.Next(new string[] { "HA", "HA", "HA", "HEE", "HOO", "HEH" });
                text += "!";
                return text;
            }

            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("EridanusArmor") && Main.rand.NextBool())
            {
                return "UWAH! Please don't hurt... wait, it's just you. Don't scare me like that! And why is that THING following you?!";
            }

            if (NPC.homeless && !saidDefeatQuote && Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedDevi"))
            {
                saidDefeatQuote = true;
                return "Good work getting one over on me! Hope I didn't make you sweat too much. Keep at the grind - I wanna see how far you can go!";
            }

            if (Main.bloodMoon && Main.rand.NextBool(2))
            {
                return "The blood moon's effects? I'm not human anymore, so nope!";
            }

            List<string> dialogue = new List<string>
            {
                "Did you know? The only real music genres are death metal and artcore.",
                "I'll have you know I'm over a hundred Fargo years old! Don't ask me how long a Fargo year is.",
                "I might be able to afford a taller body if you keep buying!",
                "Where's that screm cat?",
                $"{Main.LocalPlayer.name}! I saw something rodent-y just now! You don't have a hamster infestation, right? Right!?",
                "You're the Terrarian? Honestly, I was expecting someone a little... taller.",
                "Don't look at me like that! The only thing I've deviated from is my humanity.",
                "Rip and tear and buy from me for more things to rip and tear!",
                "What's a chee-bee doe-goe?",
                "Wait a second. Are you sure this house isn't what they call 'prison?'",
                "Deviantt has awoken! Quick, give her all your money to defeat her!",
                "One day, I'll sell a summon for myself! ...Just kidding.",
                "Hmm, I can tell! You've killed a lot, but you haven't killed enough!",
                "Why the extra letter, you ask? Only the strongest sibling is allowed to remove their own!",
                "The more rare things you kill, the more stuff I sell! Simple, right?",
            };

            int mutant = NPC.FindFirstNPC(NPCType<Mutant>());
            if (mutant != -1)
            {
                dialogue.Add($"Can you tell {Main.npc[mutant].GivenName} to put some clothes on?");
                dialogue.Add($"One day, I'll sell a summon for myself! ...Just kidding. That's {Main.npc[mutant].GivenName}'s job.");
            }

            int lumberjack = NPC.FindFirstNPC(NPCType<LumberJack>());
            if (lumberjack != -1)
            {
                dialogue.Add($"What's that? You want to fight {Main.npc[lumberjack].GivenName}? ...even I know better than to try.");
            }

            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("EternityMode"))
            {
                dialogue.Add("Embrace suffering... and while you're at it, embrace another purchase!");
            }

            return Main.rand.Next(dialogue);
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("EternityMode"))
            {
                button2 = "Help"; //(bool)ModLoader.GetMod("FargowiltasSouls").Call("GiftsReceived") ? "Help" : "Receive Gift";
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
            }
            else if (Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("EternityMode"))
            {
                Main.npcChatText = Fargowiltas.dialogueTracker.GetDialogue(NPC.GivenName);
            }
        }

        public static void AddItem(bool check, int item, int price, ref Chest shop, ref int nextSlot)
        {
            if (check)
            {
                shop.item[nextSlot].SetDefaults(item);
                shop.item[nextSlot].shopCustomPrice = price;
                nextSlot++;
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            if (Fargowiltas.ModLoaded["FargowiltasSoulsDLC"] && TryFind("FargowiltasSoulsDLC", "PandorasBox", out ModItem pandorasBox))
            {
                shop.item[nextSlot].SetDefaults(pandorasBox.Type);
                nextSlot++;
            }

            if (Fargowiltas.ModLoaded["FargowiltasSouls"])
            {
                if (TryFind("FargowiltasSouls", "EurusSock", out ModItem eurusSock))
                {
                    shop.item[nextSlot].SetDefaults(eurusSock.Type);
                    nextSlot++;
                }

                /*if ((bool)ModLoader.GetMod("FargowiltasSouls").Call("EternityMode"))
                {
                    shop.item[nextSlot].SetDefaults(ItemType<Items.Misc.EternityAdvisor>());
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 1);
                    nextSlot++;
                }*/
            }

            AddItem(FargoWorld.DownedBools["worm"], ItemType<WormSnack>(), Item.buyPrice(0, 2), ref shop, ref nextSlot);
            AddItem(FargoWorld.DownedBools["pinky"], ItemType<PinkSlimeCrown>(), Item.buyPrice(0, 5), ref shop, ref nextSlot);
            AddItem(FargoWorld.DownedBools["goblinScout"], ItemType<GoblinScrap>(), Item.buyPrice(0, 1), ref shop, ref nextSlot);
            AddItem(FargoWorld.DownedBools["doctorBones"], ItemType<Eggplant>(), Item.buyPrice(0, 2), ref shop, ref nextSlot);
            AddItem(FargoWorld.DownedBools["undeadMiner"], ItemType<AttractiveOre>(), Item.buyPrice(0, 3), ref shop, ref nextSlot);
            AddItem(FargoWorld.DownedBools["tim"], ItemType<HolyGrail>(), Item.buyPrice(0, 5), ref shop, ref nextSlot);
            AddItem(FargoWorld.DownedBools["gnome"], ItemType<GnomeHat>(), Item.buyPrice(0, 5), ref shop, ref nextSlot);
            AddItem(FargoWorld.DownedBools["goldenSlime"], ItemType<GoldenSlimeCrown>(), Item.buyPrice(0, 60), ref shop, ref nextSlot);
            AddItem(NPC.downedBoss3 && FargoWorld.DownedBools["dungeonSlime"], ItemType<SlimyLockBox>(), Item.buyPrice(0, 10), ref shop, ref nextSlot);
            AddItem(Main.hardMode && FargoWorld.DownedBools["medusa"], ItemType<AthenianIdol>(), Item.buyPrice(0, 5), ref shop, ref nextSlot);
            AddItem(Main.hardMode && FargoWorld.DownedBools["clown"], ItemType<ClownLicense>(), Item.buyPrice(0, 5), ref shop, ref nextSlot);
            AddItem(FargoWorld.DownedBools["nymph"], ItemType<HeartChocolate>(), Item.buyPrice(0, 10), ref shop, ref nextSlot);
            AddItem(Main.hardMode && FargoWorld.DownedBools["moth"], ItemType<MothLamp>(), Item.buyPrice(0, 10), ref shop, ref nextSlot);
            AddItem(Main.hardMode && FargoWorld.DownedBools["rainbowSlime"], ItemType<DilutedRainbowMatter>(), Item.buyPrice(0, 10), ref shop, ref nextSlot);
            AddItem(Main.hardMode && FargoWorld.DownedBools["wyvern"], ItemType<CloudSnack>(), Item.buyPrice(0, 10), ref shop, ref nextSlot);
            AddItem(Main.hardMode && FargoWorld.DownedBools["runeWizard"], ItemType<RuneOrb>(), Item.buyPrice(0, 15), ref shop, ref nextSlot);
            AddItem(Main.hardMode && FargoWorld.DownedBools["mimic"], ItemType<SuspiciousLookingChest>(), Item.buyPrice(0, 30), ref shop, ref nextSlot);
            AddItem(Main.hardMode && FargoWorld.DownedBools["mimicHallow"], ItemType<HallowChest>(), Item.buyPrice(0, 30), ref shop, ref nextSlot);
            AddItem(Main.hardMode && (FargoWorld.DownedBools["mimicCorrupt"] || FargoWorld.DownedBools["mimicCrimson"]), ItemType<CorruptChest>(), Item.buyPrice(0, 30), ref shop, ref nextSlot);
            AddItem(Main.hardMode && (FargoWorld.DownedBools["mimicCrimson"] || FargoWorld.DownedBools["mimicCorrupt"]), ItemType<CrimsonChest>(), Item.buyPrice(0, 30), ref shop, ref nextSlot);
            AddItem(Main.hardMode && FargoWorld.DownedBools["mimicJungle"], ItemType<JungleChest>(), Item.buyPrice(0, 30), ref shop, ref nextSlot);
            AddItem(Main.hardMode && FargoWorld.DownedBools["iceGolem"], ItemType<CoreoftheFrostCore>(), Item.buyPrice(0, 10), ref shop, ref nextSlot);
            AddItem(Main.hardMode && FargoWorld.DownedBools["sandElemental"], ItemType<ForbiddenForbiddenFragment>(), Item.buyPrice(0, 10), ref shop, ref nextSlot);
            AddItem(NPC.downedMechBossAny && FargoWorld.DownedBools["redDevil"], ItemType<DemonicPlushie>(), Item.buyPrice(0, 10), ref shop, ref nextSlot);
            AddItem(FargoWorld.DownedBools["eyeFish"] || FargoWorld.DownedBools["zombieMerman"], ItemType<SuspiciousLookingLure>(), Item.buyPrice(0, 10), ref shop, ref nextSlot);
            AddItem(Main.hardMode && FargoWorld.DownedBools["bloodEel"], ItemType<BloodUrchin>(), Item.buyPrice(0, 10), ref shop, ref nextSlot);
            AddItem(Main.hardMode && FargoWorld.DownedBools["goblinShark"], ItemType<HemoclawCrab>(), Item.buyPrice(0, 10), ref shop, ref nextSlot);
            AddItem(FargoWorld.DownedBools["dreadnautilus"], ItemType<BloodSushiPlatter>(), Item.buyPrice(0, 20), ref shop, ref nextSlot);
            AddItem(Main.hardMode && NPC.downedGoblins && FargoWorld.DownedBools["goblinSummoner"], ItemType<ShadowflameIcon>(), Item.buyPrice(0, 10), ref shop, ref nextSlot);
            AddItem(Main.hardMode && NPC.downedPirates && FargoWorld.DownedBools["pirateCaptain"], ItemType<PirateFlag>(), Item.buyPrice(0, 15), ref shop, ref nextSlot);
            AddItem(NPC.downedPlantBoss && FargoWorld.DownedBools["nailhead"], ItemType<Pincushion>(), Item.buyPrice(0, 15), ref shop, ref nextSlot);
            AddItem(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && FargoWorld.DownedBools["mothron"], ItemType<MothronEgg>(), Item.buyPrice(0, 15), ref shop, ref nextSlot);
            AddItem(NPC.downedPlantBoss && FargoWorld.DownedBools["boneLee"], ItemType<LeesHeadband>(), Item.buyPrice(0, 15), ref shop, ref nextSlot);
            AddItem(NPC.downedPlantBoss && FargoWorld.DownedBools["paladin"], ItemType<GrandCross>(), Item.buyPrice(0, 15), ref shop, ref nextSlot);
            AddItem(NPC.downedPlantBoss && FargoWorld.DownedBools["skeletonGun"], ItemType<AmalgamatedSkull>(), Item.buyPrice(0, 30), ref shop, ref nextSlot);
            AddItem(NPC.downedPlantBoss && FargoWorld.DownedBools["skeletonMage"], ItemType<AmalgamatedSpirit>(), Item.buyPrice(0, 30), ref shop, ref nextSlot);
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            if (NPC.downedMoonlord)
            {
                damage = 80;
                knockback = 4f;
            }
            else if (Main.hardMode)
            {
                damage = 40;
                knockback = 4f;
            }
            else
            {
                damage = 20;
                knockback = 2f;
            }
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = NPC.downedMoonlord ? 1 : 30;
            if (!NPC.downedMoonlord)
            {
                randExtraCooldown = 30;
            }
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = NPC.downedMoonlord ? ProjectileType<FakeHeartMarkDeviantt>() : ProjectileType<FakeHeartDeviantt>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 10f;
            randomOffset = 0f;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 8; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2.5f * (float)hitDirection, -2.5f, 0, default, 0.8f);
                }

                if (!Main.dedServ)
                {
                    Vector2 pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(pos, NPC.velocity, ModContent.Find<ModGore>("Fargowiltas/DevianttGore3").Type);

                    pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(pos, NPC.velocity, ModContent.Find<ModGore>("Fargowiltas/DevianttGore2").Type);

                    pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(pos, NPC.velocity, ModContent.Find<ModGore>("Fargowiltas/DevianttGore1").Type);
                }
            }
            else
            {
                for (int k = 0; k < damage / NPC.lifeMax * 50.0; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, hitDirection, -1f, 0, default, 0.6f);
                }
            }
        }

        public override void OnKill()
        {
            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && TryFind("FargowiltasSouls", "CosmosChampion", out ModNPC cosmosChamp) && NPC.AnyNPCs(cosmosChamp.Type))
                Item.NewItem(NPC.GetItemSource_Loot(), NPC.Hitbox, ItemType<Items.Tiles.WalkingRick>());
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (Fargowiltas.ModLoaded["FargowiltasSouls"] && !(bool)ModLoader.GetMod("FargowiltasSouls").Call("GiftsReceived"))
            {
                Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
                Rectangle rectangle = NPC.frame;//new Rectangle(0, y3, texture2D13.Width, num156);
                Vector2 origin2 = rectangle.Size() / 2f;
                SpriteEffects effects = NPC.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                Color color26 = Main.DiscoColor;
                color26.A = 0;

                //for (int i = 0; i < NPCID.Sets.TrailCacheLength[NPC.type]; i++)
                //{
                //    Color color27 = color26 * 0.5f;
                //    color27 *= (float)(NPCID.Sets.TrailCacheLength[NPC.type] - i) / NPCID.Sets.TrailCacheLength[NPC.type];
                //    Vector2 value4 = NPC.oldPos[i];
                //    float num165 = NPC.rotation; //NPC.oldRot[i];
                //    Main.EntitySpriteDraw(texture2D13, value4 + NPC.Size / 2f - Main.screenPosition + new Vector2(0, NPC.gfxOffY - 4), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, NPC.scale, effects, 0);
                //}

                float scale = (Main.mouseTextColor / 200f - 0.35f) * 0.5f + 1f;
                scale *= NPC.scale;
                Main.EntitySpriteDraw(texture2D13, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY - 4), new Microsoft.Xna.Framework.Rectangle?(rectangle), color26, NPC.rotation, origin2, scale, effects, 0);
            }
            //Main.EntitySpriteDraw(texture2D13, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), NPC.GetAlpha(drawColor), NPC.rotation, origin2, NPC.scale, effects, 0);
            return true;
        }
    }
}
