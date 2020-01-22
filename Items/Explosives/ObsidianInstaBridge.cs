using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace test.Items.Explosives
{
    public class ObsidianInstaBridge : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Obsidian Instabridge");
            Tooltip.SetDefault("Creates a bridge of obsidian platforms instantly" +
                               "\nAlso clears the area right above the platforms" +
                               "\nDo not use if any important building is nearby");
        }

        public override void SetDefaults()
        {
            item.damage = 50;
            item.width = 10;
            item.height = 32;
            item.maxStack = 99;
            item.consumable = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 20;
            item.useTime = 20;
            item.value = Item.buyPrice(0, 0, 3);
            item.noUseGraphic = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("ObsInstabridgeProj");
            item.shootSpeed = 5f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FossilOre, 20);
            recipe.AddIngredient(ItemID.Dynamite, 10);
            recipe.AddIngredient(ItemID.ObsidianPlatform, 2000);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}