using Helium.Data.Datapack;
using Helium.Data.Resource.Api;
using Helium.Utility.Types;
using Helium.World.Environment.Block.State;
using Helium.World.Environment.Item;
using Helium.World.Environment.Shapes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.World.Environment.Block
{
    public interface IBlock
    {
        public static ILootTable LootTable { get; set; }
        public static BlockStateRegistry AllowedBlockStates { get; set; }
        public static IShape CollisionBoxShape { get; set; }
        public static IShape HitboxShape { get; set; }

        public static Single BreakingSpeed { get; set; }
        public static Single BlastResistance { get; set; }
        public static BlockItem Item { get; set; }

        public static Boolean IsTransparent { get; set; }
        public static Byte? LightOcclusion { get; set; }
        public static ITranslationString TranslationString { get; set; }
        public static Single SpeedFactor { get; set; }
        public static Single JumpFactor { get; set; }
        public static Material Material { get; set; }
        public static MaterialColor MaterialColor { get; set; }
        public static Boolean IsOpaque { get; set; }

        protected static IBlockState DefaultBlockState { get; set; }

        

        public BlockPosition Position { get; set; }
        public UInt32 Identifier { get; set; }
        public IBlockState[] BlockStates { get; set; }
        public Boolean[] Solid { get; set; }

        //if your mod wants light level changing, use block states to trigger this since it is not serialized
        public Byte LightEmission { get; set; }

        public void Update();
    }
}
