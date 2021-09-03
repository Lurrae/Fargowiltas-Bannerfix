using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Tiles
{
    public class OmnistationPlus : ModItem
    {
        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null || ModLoader.GetMod("CalamityMod") != null;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Omnistation+");
            Tooltip.SetDefault(@"Can be reused indefinitely
Effects of all buff stations, modded and vanilla
Grants Honey when touched
Right click while holding a weapon for its respective buff
Currently only supports Thorium and Calamity");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.rare = ItemRarityID.Blue;
            item.createTile = mod.TileType("OmnistationPlusSheet");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("Fargowiltas:AnyOmnistation", 1);
            if (Fargowiltas.ModLoaded["ThoriumMod"])
            {
                Mod thorium = ModLoader.GetMod("ThoriumMod");
                recipe.AddIngredient(thorium.ItemType("Mistletoe"), 30);
                recipe.AddIngredient(thorium.ItemType("ConductorsStand"), 5);
                recipe.AddIngredient(thorium.ItemType("Altar"), 5);
                recipe.AddIngredient(thorium.ItemType("NinjaRack"), 5);
            }
            if (Fargowiltas.ModLoaded["CalamityMod"])
            {
                Mod calamity = ModLoader.GetMod("CalamityMod");
                recipe.AddIngredient(calamity.ItemType("PurpleCandle"), 5);
                recipe.AddIngredient(calamity.ItemType("YellowCandle"), 5);
                recipe.AddIngredient(calamity.ItemType("PinkCandle"), 5);
                recipe.AddIngredient(calamity.ItemType("BlueCandle"), 5);

            }

            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}