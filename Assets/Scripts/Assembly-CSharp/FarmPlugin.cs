using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UnityEngine;

public class FarmPlugin : GEPlugin
{
	[CompilerGenerated]
	private static Dictionary<string, int> _003C_003Ef__switch_0024map15;

	public FarmPlugin()
	{
		ResourceManager.AddResourceGroup("FarmEditor");
		ResourceManager.AddResourceToGroup("FarmEditor", new UnityResource("FarmEditorIcons", "Farm/SpriteSheets/Editor/FarmEditorIconsDif", ResourceType.Texture));
		GEState.groundMats.Add(new GEMat("Farm", "Mud", "MudFill", 5f, "MudRoad", 1f));
		GEState.groundMats.Add(new GEMat("Farm", "Rock", "RockFill", 5f, "RockRoad", 1f));
		GEState.groundMats.Add(new GEMat("Farm", "Snow", "SnowFill", 5f, "SnowRoad", 1f));
		GEState.blockMats.Add(new GEMat("Farm", "Fabric", "FabricGray", 0.025f, "MetalDynamic", 1f, 0f, 0f, 0));
		GEState.blockMats.Add(new GEMat("Farm", "Fabric Red", "FabricRed", 0.025f, "MetalDynamic", 1f, 0f, 0f, 0));
		GEState.blockMats.Add(new GEMat("Farm", "Metal", "MetalFill", 0.025f, "MetalDynamic", 1f, 0f, 0f, 0));
		GEState.blockMats.Add(new GEMat("Farm", "Mud", "MudFill", 0.025f, "MudDynamic", 1f, 0f, 0f));
		GEState.blockMats.Add(new GEMat("Farm", "Rock", "RockFill", 0.025f, "RockDynamic", 1f, 0f, 0f, 0));
		GEState.blockMats.Add(new GEMat("Farm", "Snow", "SnowFill", 0.025f, "SnowDynamic", 1f, 0f, 0f));
		GEState.blockMats.Add(new GEMat("Farm", "Wood", "WoodFill", 0.025f, "WoodDynamic", 1f, 0f, 0f));
		GEState.blockMats.Add(new GEMat("Farm", "Plank", "WoodFill", 0.025f, "WoodPlankDynamic", 1f, 0f, 0f));
		GEState.blockMats.Add(new GEMat("Farm", "Wood Beam", "WoodBeam", 0.025f, "WoodBeamDynamic", 1f, 0f, 0f));
		GEState.blockMats.Add(new GEMat("Farm", "Wood Wall", "WoodWall", 0.025f, "WoodBeamDynamic", 1f, 0f, 0f));
		GEState.backgroundMats.Add(new GEMat("Farm", "Circus", "CircusPanel", 10f));
		GEState.backgroundMats.Add(new GEMat("Farm", "Fabric", "FabricGray", 10f));
		GEState.backgroundMats.Add(new GEMat("Farm", "Grass", "GrassGray", 10f));
		GEState.backgroundMats.Add(new GEMat("Farm", "WoodPanel", "WoodPanel", 10f));
		GEState.backgroundMats.Add(new GEMat("Farm", "WoodWall", "WoodWall", 10f));
		GEState.backgroundMats.Add(new GEMat("Farm", "Foliage", "Foliage2", 2f));
		GEState.landscapeMats.Add(new GEMat("Farm", "2", "Landscape2"));
		GEState.landscapeMats.Add(new GEMat("Farm", "3", "Landscape3"));
		GEState.landscapeMats.Add(new GEMat("Farm", "4", "Landscape4"));
		GEState.landscapeMats.Add(new GEMat("Farm", "5", "Landscape5"));
		GEState.landscapeMats.Add(new GEMat("Farm", "6", "Landscape6"));
		GEState.landscapeMats.Add(new GEMat("Farm", "7", "Landscape7"));
		GEState.landscapeMats.Add(new GEMat("Farm", "8", "Landscape8"));
		GEState.landscapeMats.Add(new GEMat("Farm", "Nighty", "NightySky"));
		ResourceManager.AddResourceGroup("FarmCommon");
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("CharacterAtlas", "Farm/SpritePrefabs/CharacterAtlas", ResourceType.Texture));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropsAtlas", "Farm/SpritePrefabs/PropsAtlas", ResourceType.Texture));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("MenuSheet", "Farm/SpriteSheets/MenuSheet", ResourceType.Texture));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("EffectSheet", "Farm/SpriteSheets/EffectSheet", ResourceType.Texture));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("CharacterShader", "Farm/Shaders/CharacterShader", ResourceType.Shader));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropsShader", "Farm/Shaders/PropsShader", ResourceType.Shader));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropsForegroundShader", "Farm/Shaders/PropsForegroundShader", ResourceType.Shader));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("MenuShader", "Farm/Shaders/MenuShader", ResourceType.Shader));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("EffectsShader", "Farm/Shaders/EffectsShader", ResourceType.Shader));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("Elvis", "Farm/SpritePrefabs/Characters/Elvis", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("ElvisAnims", "Farm/SpritePrefabs/Characters/ElvisAnims", ResourceType.SpritePrefabAnimation));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("Kevin", "Farm/SpritePrefabs/Characters/Kevin", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("KevinAnims", "Farm/SpritePrefabs/Characters/KevinAnims", ResourceType.SpritePrefabAnimation));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("Helmet", "Farm/SpritePrefabs/Hats/Helmet", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("Indian", "Farm/SpritePrefabs/Hats/Indian", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("BigKart", "Farm/SpritePrefabs/Vehicles/BigKart", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("SmallKart", "Farm/SpritePrefabs/Vehicles/SmallKart", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("Runner", "Farm/SpritePrefabs/Vehicles/Runner", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("Radish", "Farm/Models/Collectibles/Radish", ResourceType.GameObject));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("RadishStem", "Farm/Models/Collectibles/RadishStem", ResourceType.GameObject));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("Carrot", "Farm/Models/Collectibles/Carrot", ResourceType.GameObject));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("CarrotStem", "Farm/Models/Collectibles/CarrotStem", ResourceType.GameObject));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("Plant1", "Farm/Models/Plants/Plant1", ResourceType.GameObject));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("Plant2", "Farm/Models/Plants/Plant2", ResourceType.GameObject));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("Plant3", "Farm/Models/Plants/Plant3", ResourceType.GameObject));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("Tree1", "Farm/Models/Plants/Tree1", ResourceType.GameObject));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("Milestone", "Farm/SpritePrefabs/Props/Milestone", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropFarmBarnBase", "Farm/SpritePrefabs/Props/PropFarmBarnBase", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropFarmBerrybush", "Farm/SpritePrefabs/Props/PropFarmBerrybush", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropFarmField", "Farm/SpritePrefabs/Props/PropFarmField", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropFarmLightpost1", "Farm/SpritePrefabs/Props/PropFarmLightpost1", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropFarmLightpost2", "Farm/SpritePrefabs/Props/PropFarmLightpost2", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropFarmOuthouse", "Farm/SpritePrefabs/Props/PropFarmOuthouse", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropFarmPicketfence", "Farm/SpritePrefabs/Props/PropFarmPicketfence", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropFarmWatertower", "Farm/SpritePrefabs/Props/PropFarmWatertower", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropBush01", "Farm/SpritePrefabs/Props/PropBush01", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropBush02", "Farm/SpritePrefabs/Props/PropBush02", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropBush03", "Farm/SpritePrefabs/Props/PropBush03", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropBush04", "Farm/SpritePrefabs/Props/PropBush04", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropBush05", "Farm/SpritePrefabs/Props/PropBush05", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropBush06", "Farm/SpritePrefabs/Props/PropBush06", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropBush07", "Farm/SpritePrefabs/Props/PropBush07", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropBush08", "Farm/SpritePrefabs/Props/PropBush08", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropBush09", "Farm/SpritePrefabs/Props/PropBush09", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropLeaf01", "Farm/SpritePrefabs/Props/PropLeaf01", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropLeaf02", "Farm/SpritePrefabs/Props/PropLeaf02", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropMushrooms01", "Farm/SpritePrefabs/Props/PropMushrooms01", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropMushrooms02", "Farm/SpritePrefabs/Props/PropMushrooms02", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropMushrooms03", "Farm/SpritePrefabs/Props/PropMushrooms03", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropMushrooms04", "Farm/SpritePrefabs/Props/PropMushrooms04", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropRock01", "Farm/SpritePrefabs/Props/PropRock01", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropRock02", "Farm/SpritePrefabs/Props/PropRock02", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropRock03", "Farm/SpritePrefabs/Props/PropRock03", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropRock04", "Farm/SpritePrefabs/Props/PropRock04", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropRock05", "Farm/SpritePrefabs/Props/PropRock05", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropRock06", "Farm/SpritePrefabs/Props/PropRock06", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropRock07", "Farm/SpritePrefabs/Props/PropRock07", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("PropRock08", "Farm/SpritePrefabs/Props/PropRock08", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("SignDown01", "Farm/SpritePrefabs/Props/SignDown01", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("SignDown02", "Farm/SpritePrefabs/Props/SignDown02", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("SignLeft01", "Farm/SpritePrefabs/Props/SignLeft01", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("SignLeft02", "Farm/SpritePrefabs/Props/SignLeft02", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("SignRight01", "Farm/SpritePrefabs/Props/SignRight01", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("SignRight02", "Farm/SpritePrefabs/Props/SignRight02", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("SignUp01", "Farm/SpritePrefabs/Props/SignUp01", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("SignUp02", "Farm/SpritePrefabs/Props/SignUp02", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("TreeBirch01", "Farm/SpritePrefabs/Props/TreeBirch01", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("TreeBirch02", "Farm/SpritePrefabs/Props/TreeBirch02", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("TreeBirch03", "Farm/SpritePrefabs/Props/TreeBirch03", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("TreeBirch04", "Farm/SpritePrefabs/Props/TreeBirch04", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("TreeBirch05", "Farm/SpritePrefabs/Props/TreeBirch05", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("TreeBirch06", "Farm/SpritePrefabs/Props/TreeBirch06", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("TreeBirch07", "Farm/SpritePrefabs/Props/TreeBirch07", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("TreeBirch08", "Farm/SpritePrefabs/Props/TreeBirch08", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("TreeBrown01", "Farm/SpritePrefabs/Props/TreeBrown01", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("TreeBrown02", "Farm/SpritePrefabs/Props/TreeBrown02", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("TreeBrown03", "Farm/SpritePrefabs/Props/TreeBrown03", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("TreeBrown04", "Farm/SpritePrefabs/Props/TreeBrown04", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("TreeBrown05", "Farm/SpritePrefabs/Props/TreeBrown05", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("TreeBrown06", "Farm/SpritePrefabs/Props/TreeBrown06", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("TreeBrown07", "Farm/SpritePrefabs/Props/TreeBrown07", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("TreeBrown08", "Farm/SpritePrefabs/Props/TreeBrown08", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("WarningCliff", "Farm/SpritePrefabs/Props/WarningCliff", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("WarningClimb", "Farm/SpritePrefabs/Props/WarningClimb", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("WarningDownhill", "Farm/SpritePrefabs/Props/WarningDownhill", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("WarningNarrow", "Farm/SpritePrefabs/Props/WarningNarrow", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("WarningPit", "Farm/SpritePrefabs/Props/WarningPit", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("WarningSpikes", "Farm/SpritePrefabs/Props/WarningSpikes", ResourceType.SpritePrefab));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("Checkpoint", "Farm/Models/Checkpoints/Checkpoint", ResourceType.GameObject));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("MusicGame", "Farm/Music/game_music", ResourceType.Sound));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("SoundCollect", "Farm/Sounds/collect", ResourceType.Sound));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("SoundSlingStretch", "Farm/Sounds/sling_stretch", ResourceType.Sound));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("SoundSlingRelease", "Farm/Sounds/sling_release", ResourceType.Sound));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("SoundCrash", "Farm/Sounds/crash", ResourceType.Sound));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("SoundCartRumble", "Farm/Sounds/cart_rumble", ResourceType.Sound));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("SoundCartBreak", "Farm/Sounds/cart_break", ResourceType.Sound));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("SoundCartLanding", "Farm/Sounds/cart_landing", ResourceType.Sound));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("SoundCheckpoint", "Farm/Sounds/checkpoint", ResourceType.Sound));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("TireSparks", "Farm/Effects/Particles/Tires/Sparks", ResourceType.GameObject));
		ResourceManager.AddResourceToGroup("FarmCommon", new UnityResource("TireDust", "Farm/Effects/Particles/Tires/Dust", ResourceType.GameObject));
		ResourceManager.LoadResourceGroup("FarmCommon");
	}

	public override void Enter(IStatedObject _parent)
	{
		ResourceManager.LoadResourceGroup("FarmEditor");
		if (FarmState.iconSheet == null)
		{
			FarmState.iconSheet = SpriteS.AddSpriteSheet(200, Main.uiCamera, ResourceManager.GetTexture("FarmEditorIcons"), ResourceManager.GetShader("EditorUIShader"), 1f);
		}
	}

	public override void Execute()
	{
	}

	public override void Exit()
	{
		if (FarmState.iconSheet != null)
		{
			SpriteS.RemoveSpriteSheet(FarmState.iconSheet);
			FarmState.iconSheet = null;
		}
		ResourceManager.UnloadResourceGroup("FarmEditor");
	}

	public override void Initialize()
	{
		FarmS.Initialize();
		Font font = TextS.AddFont("LovePotion45", "Farm/Fonts/LovePotion/", 200, 512, 256, 1f, Main.uiCamera);
		TextS.AddStyle("LP45", font);
		if (FarmState.tireSparks == null)
		{
			FarmState.tireSparks = ResourceManager.GetGameObject("TireSparks");
		}
		if (FarmState.tireDust == null)
		{
			FarmState.tireDust = ResourceManager.GetGameObject("TireDust");
		}
		if (FarmState.effectSheet == null)
		{
			FarmState.effectSheet = SpriteS.AddSpriteSheet(300, Main.camera, ResourceManager.GetTexture("EffectSheet"), ResourceManager.GetShader("PropsForegroundShader"), 1f);
		}
		if (FarmState.menuSheet == null)
		{
			FarmState.menuSheet = SpriteS.AddSpriteSheet(100, Main.uiCamera, ResourceManager.GetTexture("MenuSheet"), ResourceManager.GetShader("MenuShader"), 1f);
		}
		if (FarmState.propBackgroundSheet == null)
		{
			FarmState.propBackgroundSheet = SpriteS.AddSpriteSheet(500, Main.camera, ResourceManager.GetTexture("PropsAtlas"), ResourceManager.GetShader("PropsShader"), 1f);
		}
		if (FarmState.characterSheet == null)
		{
			FarmState.characterSheet = SpriteS.AddSpriteSheet(500, Main.camera, ResourceManager.GetTexture("CharacterAtlas"), ResourceManager.GetShader("CharacterShader"), 1f);
		}
		if (FarmState.propForegroundSheet == null)
		{
			FarmState.propForegroundSheet = SpriteS.AddSpriteSheet(500, Main.camera, ResourceManager.GetTexture("PropsAtlas"), ResourceManager.GetShader("PropsForegroundShader"), 1f);
		}
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("Elvis"), "Elvis", FarmState.characterSheet);
		SpritePrefabA.ParseAnimation(ResourceManager.GetResourceClass("ElvisAnims"), "Elvis");
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("Kevin"), "Kevin", FarmState.characterSheet);
		SpritePrefabA.ParseAnimation(ResourceManager.GetResourceClass("KevinAnims"), "Kevin");
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("Helmet"), "Helmet", FarmState.characterSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("Indian"), "Indian", FarmState.characterSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("BigKart"), "BigKart", FarmState.characterSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("SmallKart"), "SmallKart", FarmState.characterSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("Runner"), "Runner", FarmState.characterSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("Milestone"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropFarmBarnBase"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropFarmBerrybush"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropFarmField"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropFarmLightpost1"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropFarmLightpost2"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropFarmOuthouse"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropFarmPicketfence"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropFarmWatertower"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropBush01"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropBush02"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropBush03"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropBush04"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropBush05"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropBush06"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropBush07"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropBush08"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropBush09"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropLeaf01"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropLeaf02"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropMushrooms01"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropMushrooms02"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropMushrooms03"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropMushrooms04"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropRock01"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropRock02"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropRock03"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropRock04"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropRock05"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropRock06"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropRock07"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("PropRock08"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("SignDown01"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("SignDown02"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("SignLeft01"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("SignLeft02"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("SignRight01"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("SignRight02"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("SignUp01"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("SignUp02"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("TreeBirch01"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("TreeBirch02"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("TreeBirch03"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("TreeBirch04"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("TreeBirch05"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("TreeBirch06"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("TreeBirch07"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("TreeBirch08"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("TreeBrown01"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("TreeBrown02"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("TreeBrown03"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("TreeBrown04"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("TreeBrown05"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("TreeBrown06"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("TreeBrown07"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("TreeBrown08"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("WarningCliff"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("WarningClimb"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("WarningDownhill"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("WarningNarrow"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("WarningPit"), FarmState.propBackgroundSheet);
		SpritePrefabA.Parse(ResourceManager.GetResourceClass("WarningSpikes"), FarmState.propBackgroundSheet);
	}

	public override void Update()
	{
		FarmS.Update();
	}

	public override bool RemoveComponent(IComponent _c)
	{
		ComponentType componentType = _c.componentType;
		if (componentType == (ComponentType)40)
		{
			FarmS.RemoveSlingComponent(_c as FSlingC);
			return true;
		}
		return false;
	}

	public override bool FillItemBar(UIC m_itemBar)
	{
		UIC uIC = GELibraryCategoryA.Assemble(m_itemBar, false, "Farm");
		UIC uIC2 = GELibraryCategoryA.Assemble(uIC, false, "Collectibles");
		GELibraryItemA.Assemble(uIC2, "Carrot", 9, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC2, "Radish", 8, FarmState.iconSheet);
		UIS.PlaceCanvasContent(uIC2);
		UIC uIC3 = GELibraryCategoryA.Assemble(uIC, false, "UI");
		GELibraryItemA.Assemble(uIC3, "Jump Button", 2, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC3, "Next Level Button", 2, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC3, "Reset Level Button", 2, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC3, "Level Menu Button", 2, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC3, "Small Reset Button", 2, FarmState.iconSheet);
		UIS.PlaceCanvasContent(uIC3);
		UIC parent = GELibraryCategoryA.Assemble(uIC, false, "LevelItems");
		UIC uIC4 = GELibraryCategoryA.Assemble(parent, false, "Farm");
		GELibraryItemA.Assemble(uIC4, "PropFarmBarnBase", 2, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC4, "PropFarmBerrybush", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC4, "PropFarmField", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC4, "PropFarmLightpost1", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC4, "PropFarmLightpost2", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC4, "PropFarmOuthouse", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC4, "PropFarmPicketfence", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC4, "PropFarmWatertower", 8, FarmState.iconSheet);
		UIS.PlaceCanvasContent(uIC4);
		UIC uIC5 = GELibraryCategoryA.Assemble(parent, false, "Plants");
		GELibraryItemA.Assemble(uIC5, "PropBush01", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC5, "PropBush02", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC5, "PropBush03", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC5, "PropBush04", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC5, "PropBush05", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC5, "PropBush06", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC5, "PropBush07", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC5, "PropBush08", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC5, "PropBush09", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC5, "PropLeaf01", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC5, "PropLeaf02", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC5, "PropMushrooms01", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC5, "PropMushrooms02", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC5, "PropMushrooms03", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC5, "PropMushrooms04", 8, FarmState.iconSheet);
		UIS.PlaceCanvasContent(uIC5);
		UIC uIC6 = GELibraryCategoryA.Assemble(parent, false, "Rocks");
		GELibraryItemA.Assemble(uIC6, "PropRock01", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC6, "PropRock02", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC6, "PropRock03", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC6, "PropRock04", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC6, "PropRock05", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC6, "PropRock06", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC6, "PropRock07", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC6, "PropRock08", 8, FarmState.iconSheet);
		UIS.PlaceCanvasContent(uIC6);
		UIC uIC7 = GELibraryCategoryA.Assemble(parent, false, "Signs");
		GELibraryItemA.Assemble(uIC7, "SignDown01", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC7, "SignDown02", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC7, "SignLeft01", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC7, "SignLeft02", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC7, "SignRight01", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC7, "SignRight02", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC7, "SignUp01", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC7, "SignUp02", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC7, "WarningCliff", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC7, "WarningClimb", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC7, "WarningDownhill", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC7, "WarningNarrow", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC7, "WarningPit", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC7, "WarningSpikes", 8, FarmState.iconSheet);
		UIS.PlaceCanvasContent(uIC7);
		UIC uIC8 = GELibraryCategoryA.Assemble(parent, false, "Trees");
		GELibraryItemA.Assemble(uIC8, "TreeBirch01", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC8, "TreeBirch02", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC8, "TreeBirch03", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC8, "TreeBirch04", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC8, "TreeBirch05", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC8, "TreeBirch06", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC8, "TreeBirch07", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC8, "TreeBirch08", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC8, "TreeBrown01", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC8, "TreeBrown02", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC8, "TreeBrown03", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC8, "TreeBrown04", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC8, "TreeBrown05", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC8, "TreeBrown06", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC8, "TreeBrown07", 8, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC8, "TreeBrown08", 8, FarmState.iconSheet);
		UIS.PlaceCanvasContent(uIC8);
		GELibraryItemA.Assemble(uIC, "Kevin & Kart", 1, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC, "Elvis & Kart", 1, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC, "Kevin", 0, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC, "Elvis", 0, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC, "Big Kart", 1, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC, "Small Kart", 1, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC, "Sling", 4, FarmState.iconSheet);
		GELibraryItemA.Assemble(uIC, "Goal", 5, FarmState.iconSheet);
		UIS.PlaceCanvasContent(uIC);
		return true;
	}

	public override bool CreateNewEditorItem(GELevel _level, List<EIC> _newItems, EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		bool result = true;
		switch (_identifier)
		{
		case "PropFarmBarnBase":
		case "PropFarmBerrybush":
		case "PropFarmField":
		case "PropFarmLightpost1":
		case "PropFarmLightpost2":
		case "PropFarmOuthouse":
		case "PropFarmPicketfence":
		case "PropFarmWatertower":
		case "PropBush01":
		case "PropBush02":
		case "PropBush03":
		case "PropBush04":
		case "PropBush05":
		case "PropBush06":
		case "PropBush07":
		case "PropBush08":
		case "PropBush09":
		case "PropLeaf01":
		case "PropLeaf02":
		case "PropMushrooms01":
		case "PropMushrooms02":
		case "PropMushrooms03":
		case "PropMushrooms04":
		case "PropRock01":
		case "PropRock02":
		case "PropRock03":
		case "PropRock04":
		case "PropRock05":
		case "PropRock06":
		case "PropRock07":
		case "PropRock08":
		case "SignDown01":
		case "SignDown02":
		case "SignLeft01":
		case "SignLeft02":
		case "SignRight01":
		case "SignRight02":
		case "SignUp01":
		case "SignUp02":
		case "TreeBirch01":
		case "TreeBirch02":
		case "TreeBirch03":
		case "TreeBirch04":
		case "TreeBirch05":
		case "TreeBirch06":
		case "TreeBirch07":
		case "TreeBirch08":
		case "TreeBrown01":
		case "TreeBrown02":
		case "TreeBrown03":
		case "TreeBrown04":
		case "TreeBrown05":
		case "TreeBrown06":
		case "TreeBrown07":
		case "TreeBrown08":
		case "WarningCliff":
		case "WarningClimb":
		case "WarningDownhill":
		case "WarningNarrow":
		case "WarningPit":
		case "WarningSpikes":
		case "Plant1":
		case "Plant2":
		case "Plant3":
		case "Tree1":
			_newItems.Add(FLevelItemA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
			break;
		case "Carrot":
		case "Radish":
			_newItems.Add(FCollectibleA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
			break;
		case "Sling":
		case "Goal":
			_newItems.Add(FSlingA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
			break;
		case "Kevin & Kart":
			_newItems.Add(FKevinAndKartA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
			break;
		case "Elvis & Kart":
			_newItems.Add(FElvisAndKartA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
			break;
		case "Kevin":
			_newItems.Add(FKevinA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
			break;
		case "Elvis":
			_newItems.Add(FElvisA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
			break;
		case "Big Kart":
			_newItems.Add(FBigKartA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
			break;
		case "Small Kart":
			_newItems.Add(FSmallKartA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
			break;
		case "Next Level Button":
		case "Reset Level Button":
		case "Level Menu Button":
		case "Jump Button":
			_newItems.Add(FRoundButtonA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
			break;
		case "Small Reset Button":
			_newItems.Add(FResetButtonA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
			break;
		default:
			result = false;
			break;
		}
		return result;
	}

	public override EIC CreateLoadedEditorItem(GELevel _level, EIC _container, EIC _loadedItem)
	{
		EIC result = null;
		switch (_loadedItem.identifier)
		{
		case "PropFarmBarnBase":
		case "PropFarmBerrybush":
		case "PropFarmField":
		case "PropFarmLightpost1":
		case "PropFarmLightpost2":
		case "PropFarmOuthouse":
		case "PropFarmPicketfence":
		case "PropFarmWatertower":
		case "PropBush01":
		case "PropBush02":
		case "PropBush03":
		case "PropBush04":
		case "PropBush05":
		case "PropBush06":
		case "PropBush07":
		case "PropBush08":
		case "PropBush09":
		case "PropLeaf01":
		case "PropLeaf02":
		case "PropMushrooms01":
		case "PropMushrooms02":
		case "PropMushrooms03":
		case "PropMushrooms04":
		case "PropRock01":
		case "PropRock02":
		case "PropRock03":
		case "PropRock04":
		case "PropRock05":
		case "PropRock06":
		case "PropRock07":
		case "PropRock08":
		case "SignDown01":
		case "SignDown02":
		case "SignLeft01":
		case "SignLeft02":
		case "SignRight01":
		case "SignRight02":
		case "SignUp01":
		case "SignUp02":
		case "TreeBirch01":
		case "TreeBirch02":
		case "TreeBirch03":
		case "TreeBirch04":
		case "TreeBirch05":
		case "TreeBirch06":
		case "TreeBirch07":
		case "TreeBirch08":
		case "TreeBrown01":
		case "TreeBrown02":
		case "TreeBrown03":
		case "TreeBrown04":
		case "TreeBrown05":
		case "TreeBrown06":
		case "TreeBrown07":
		case "TreeBrown08":
		case "WarningCliff":
		case "WarningClimb":
		case "WarningDownhill":
		case "WarningNarrow":
		case "WarningPit":
		case "WarningSpikes":
		case "Plant1":
		case "Plant2":
		case "Plant3":
		case "Tree1":
			result = FLevelItemA.CreateLoadedEditorItem(_container, _loadedItem);
			break;
		case "Carrot":
		case "Radish":
			result = FCollectibleA.CreateLoadedEditorItem(_container, _loadedItem);
			break;
		case "Sling":
		case "Goal":
			result = FSlingA.CreateLoadedEditorItem(_container, _loadedItem);
			break;
		case "Kevin & Kart":
			result = FKevinAndKartA.CreateLoadedEditorItem(_container, _loadedItem);
			break;
		case "Elvis & Kart":
			result = FElvisAndKartA.CreateLoadedEditorItem(_container, _loadedItem);
			break;
		case "Kevin":
			result = FKevinA.CreateLoadedEditorItem(_container, _loadedItem);
			break;
		case "Elvis":
			result = FElvisA.CreateLoadedEditorItem(_container, _loadedItem);
			break;
		case "Big Kart":
			result = FBigKartA.CreateLoadedEditorItem(_container, _loadedItem);
			break;
		case "Small Kart":
			result = FSmallKartA.CreateLoadedEditorItem(_container, _loadedItem);
			break;
		case "Next Level Button":
		case "Reset Level Button":
		case "Level Menu Button":
		case "Jump Button":
			result = FRoundButtonA.CreateLoadedEditorItem(_container, _loadedItem);
			break;
		case "Small Reset Button":
			result = FResetButtonA.CreateLoadedEditorItem(_container, _loadedItem);
			break;
		}
		return result;
	}

	public override bool FillEditorItem(EIC _eic)
	{
		bool result = true;
		switch (_eic.identifier)
		{
		case "PropFarmBarnBase":
		case "PropFarmBerrybush":
		case "PropFarmField":
		case "PropFarmLightpost1":
		case "PropFarmLightpost2":
		case "PropFarmOuthouse":
		case "PropFarmPicketfence":
		case "PropFarmWatertower":
		case "PropBush01":
		case "PropBush02":
		case "PropBush03":
		case "PropBush04":
		case "PropBush05":
		case "PropBush06":
		case "PropBush07":
		case "PropBush08":
		case "PropBush09":
		case "PropLeaf01":
		case "PropLeaf02":
		case "PropMushrooms01":
		case "PropMushrooms02":
		case "PropMushrooms03":
		case "PropMushrooms04":
		case "PropRock01":
		case "PropRock02":
		case "PropRock03":
		case "PropRock04":
		case "PropRock05":
		case "PropRock06":
		case "PropRock07":
		case "PropRock08":
		case "SignDown01":
		case "SignDown02":
		case "SignLeft01":
		case "SignLeft02":
		case "SignRight01":
		case "SignRight02":
		case "SignUp01":
		case "SignUp02":
		case "TreeBirch01":
		case "TreeBirch02":
		case "TreeBirch03":
		case "TreeBirch04":
		case "TreeBirch05":
		case "TreeBirch06":
		case "TreeBirch07":
		case "TreeBirch08":
		case "TreeBrown01":
		case "TreeBrown02":
		case "TreeBrown03":
		case "TreeBrown04":
		case "TreeBrown05":
		case "TreeBrown06":
		case "TreeBrown07":
		case "TreeBrown08":
		case "WarningCliff":
		case "WarningClimb":
		case "WarningDownhill":
		case "WarningNarrow":
		case "WarningPit":
		case "WarningSpikes":
		case "Plant1":
		case "Plant2":
		case "Plant3":
		case "Tree1":
			FLevelItemA.FillEditorItem(_eic);
			break;
		case "Carrot":
		case "Radish":
			FCollectibleA.FillEditorItem(_eic);
			break;
		case "Sling":
		case "Goal":
			FSlingA.FillEditorItem(_eic);
			break;
		case "Kevin & Kart":
			FKevinAndKartA.FillEditorItem(_eic);
			break;
		case "Elvis & Kart":
			FElvisAndKartA.FillEditorItem(_eic);
			break;
		case "Kevin":
			FKevinA.FillEditorItem(_eic);
			break;
		case "Elvis":
			FElvisA.FillEditorItem(_eic);
			break;
		case "Big Kart":
			FBigKartA.FillEditorItem(_eic);
			break;
		case "Small Kart":
			FSmallKartA.FillEditorItem(_eic);
			break;
		case "Next Level Button":
		case "Reset Level Button":
		case "Level Menu Button":
		case "Jump Button":
			FRoundButtonA.FillEditorItem(_eic);
			break;
		case "Small Reset Button":
			FResetButtonA.FillEditorItem(_eic);
			break;
		default:
			result = false;
			break;
		}
		return result;
	}

	public override bool UpdatePropertyBar(EIC _eic, UIC _propertyBar)
	{
		bool result = true;
		switch (_eic.identifier)
		{
		case "PropFarmBarnBase":
		case "PropFarmBerrybush":
		case "PropFarmField":
		case "PropFarmLightpost1":
		case "PropFarmLightpost2":
		case "PropFarmOuthouse":
		case "PropFarmPicketfence":
		case "PropFarmWatertower":
		case "PropBush01":
		case "PropBush02":
		case "PropBush03":
		case "PropBush04":
		case "PropBush05":
		case "PropBush06":
		case "PropBush07":
		case "PropBush08":
		case "PropBush09":
		case "PropLeaf01":
		case "PropLeaf02":
		case "PropMushrooms01":
		case "PropMushrooms02":
		case "PropMushrooms03":
		case "PropMushrooms04":
		case "PropRock01":
		case "PropRock02":
		case "PropRock03":
		case "PropRock04":
		case "PropRock05":
		case "PropRock06":
		case "PropRock07":
		case "PropRock08":
		case "SignDown01":
		case "SignDown02":
		case "SignLeft01":
		case "SignLeft02":
		case "SignRight01":
		case "SignRight02":
		case "SignUp01":
		case "SignUp02":
		case "TreeBirch01":
		case "TreeBirch02":
		case "TreeBirch03":
		case "TreeBirch04":
		case "TreeBirch05":
		case "TreeBirch06":
		case "TreeBirch07":
		case "TreeBirch08":
		case "TreeBrown01":
		case "TreeBrown02":
		case "TreeBrown03":
		case "TreeBrown04":
		case "TreeBrown05":
		case "TreeBrown06":
		case "TreeBrown07":
		case "TreeBrown08":
		case "WarningCliff":
		case "WarningClimb":
		case "WarningDownhill":
		case "WarningNarrow":
		case "WarningPit":
		case "WarningSpikes":
		case "Plant1":
		case "Plant2":
		case "Plant3":
		case "Tree1":
			FLevelItemA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
			break;
		default:
			result = false;
			break;
		}
		return result;
	}

	public override IControlledComponent GetControlledComponentWithUniqueId(uint _id)
	{
		return null;
	}

	public override bool GetObjectData(SerializationInfo info, ILevelData data)
	{
		bool flag = true;
		return false;
	}

	public override SpriteSheet GetIconSheet()
	{
		return FarmState.iconSheet;
	}

	public override int GetIconIndex(string _identifier)
	{
		if (_identifier != null)
		{
			if (_003C_003Ef__switch_0024map15 == null)
			{
				_003C_003Ef__switch_0024map15 = new Dictionary<string, int>(0);
			}
			int value;
			if (!_003C_003Ef__switch_0024map15.TryGetValue(_identifier, out value))
			{
			}
		}
		return 2;
	}
}
