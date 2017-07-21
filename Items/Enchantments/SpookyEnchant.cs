using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fargowiltas.Items.Enchantments
{
	public class SpookyEnchant : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spooky Enchantment");
			Tooltip.SetDefault("'Melting souls since 1902' \n12% increased minion damage \nIncreases your max number of minions by 1 \nWhen you get hit, you release a legion of scythes\n" + 
									"Summons a cursed sapling, turn off vanity to despawn it");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.accessory = true;
			ItemID.Sets.ItemNoGravity[item.type] = true;
			item.rare = 8; 
			item.value = 250000; 
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
        {
			FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>(mod);
			if(soulcheck.spoopy == true)
			{
			modPlayer.spookyEnchant = true;
			}
			player.maxMinions += 1;
            player.minionDamage += 0.12f;
			
			if (player.whoAmI == Main.myPlayer)
            {
				if(!hideVisual)
				{
					modPlayer.saplingPet = true;
					
					if(player.FindBuffIndex(85) == -1)
					{
						if (player.ownedProjectileCounts[ProjectileID.CursedSapling] < 1)
						{
							Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -1f, ProjectileID.CursedSapling, 0, 2f, Main.myPlayer, 0f, 0f);
						}
					}
				}
				else
				{
					modPlayer.saplingPet = false;
				}
			}
			
        }
		
		public override void AddRecipes()
		{
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpookyHelmet);
			recipe.AddIngredient(ItemID.SpookyBreastplate);
			recipe.AddIngredient(ItemID.SpookyLeggings);
			recipe.AddIngredient(ItemID.DemonScythe);
			recipe.AddIngredient(ItemID.DeathSickle);
			recipe.AddIngredient(ItemID.CursedSapling);
			recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
		}
	}	
}
		