///////////////////////
//STALKER DM: GADGETS//
///////////////////////

//////////////
// CONTENTS //
//////////////
//#1. GADGETS
//	#1.1 Stimpak
//		note: see misc.cs #2.1 for alt-fire.
//	#1.2 Flare
//	#1.3 Proximity Shocker
//	#1.4 Body Armor
//	#1.5 Rocket boots

//#1.
//	#1.1
datablock ItemData(StimpakItem)
{
	category = "Weapon";
	className = "Weapon";
	slot = "Secondary";

	shapeFile = $StalkerDM::Path @ "/models/stimpak.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Stimpak";
	iconName = $StalkerDM::Path @ "/icons/icon_Stimpak";
	doColorShift = false;
	colorShiftColor = "0 0 0 1";

	image = StimpakImage;
	canDrop = true;
};

datablock ShapeBaseImageData(StimpakImage)
{
	shapeFile = $StalkerDM::Path @ "/models/stimpak.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = StimpakItem;
	ammo = " ";
	//projectile = " ";
	//projectileType = Projectile;

	melee = false;
	armReady = true;

	stateName[0]                   = "Activate";
	stateTimeoutValue[0]           = 0.1;
	stateTransitionOnTimeout[0]    = "Ready";

	stateName[1]                   = "Ready";
	stateTransitionOnTriggerDown[1]= "Fire";
	stateAllowImageChange[1]       = true;

	stateName[2]                   = "Fire";
	stateScript[2]                 = "onFire";
	stateTransitionOnTimeout[2]    = "Ready";
	stateTimeoutValue[2]           = 0.5;
	stateAllowImageChange[2]       = false;
	stateWaitForTimeout[2]         = true;
};

function StimpakImage::onFire(%this, %obj, %slot)
{
	if(%obj.CDstimpak > getSimTime())
	{
		if(isObject(%obj.client) && %obj.client.getClassName() $= "GameConnection")
		{
			%obj.client.centerPrint("Not ready yet!", 3);
		}
		return;
	}
	if(%obj.getDamageLevel() > 0 || GlobalStorage.StimpakOverheal)
	{
		%obj.stimpakHeal();
		%obj.CDstimpak = getSimTime() + GlobalStorage.StimpakCD;
		%obj.schedule(GlobalStorage.StimpakCD, StimpakReadyNote);
	}
	else
	{
		if(isObject(%obj.client) && %obj.client.getClassName() $= "GameConnection")
		{
			%obj.client.centerPrint("Already at full health!\nUse the jet button with the stimpak equipped to heal teammates.", 1);
		}
	}
}

function player::StimpakHeal(%this, %sObj)
{
	if(%this.getDamageLevel() > 0)
	{
		%this.emote(HealImage, 1);
		if(%sObj.client.healing+= mClamp(GlobalStorage.StimpakIH, 0, %this.getDamageLevel()) >= 200)
		{
			%sObj.client.unlockAchievement("Medical Specialist");
		}
		%this.addHealth(GlobalStorage.StimpakIH);
	}
	%this.StimpakHoT(GlobalStorage.StimpakHOT / GlobalStorage.StimpakTicks, GlobalStorage.StimpakTicks, %sObj);
}

function player::StimpakHoT(%this, %tickamt, %remaining, %sObj)
{
	if(%sObj.client.healing+= mClamp(%tickamt, 0, %this.getDamageLevel()) >= 200)
	{
		%sObj.client.unlockAchievement("Medical Specialist");
	}
	if(%this.getDamageLevel() > 0 || GlobalStorage.StimpakOverheal)
	{
		%this.tempHealth = mClampF(%this.tempHealth + mClampF(%tickamt - %this.getDamageLevel(), 0, %tickamt), 0, 100);
		%this.TempHealthDecSchec = %this.schedule(10000, TempHealthDec);
		%this.addHealth(%tickamt);
		%this.emote(HealImage, 1);
	}
	if(%remaining-- > 0)
	{
		%this.schedule(1000, StimpakHoT, %tickamt, %remaining);
	}
}

function Player::TempHealthDec(%obj)
{
	if(%obj.tempHealth > 0)
	{
		%obj.tempHealth = mClamp(%obj.tempHealth - (GlobalStorage.StimpakHOT / GlobalStorage.StimpakTicks), 0, 100);
		%obj.TempHealthDecSched = %obj.schedule(2000, TempHealthDec);
	}
}

//	#1.2
datablock fxLightData(FlareLight)
{
	uiName = "Flare Light";

	LightOn = true;
	radius = 4;
	brightness = 2;
	color = "1 0.8 0";

	flareOn = true;
	flarebitmap = "base/lighting/corona";
	NearSize = 2;
	FarSize = 1;

	AnimColor = true;
	ColorTime = 0.2;
	MinColor = "1 0.4 0";
	MaxColor = "1 1 0";
	AnimBrightness = true;
	BrightnessTime = 0.15;
	MinBrightness = 25;
	MaxBrightness = 50;
	AnimRadius = true;
	RadiusTime = 0.2;
	MinRadius = 2;
	MaxRadius = 5;
};

function FlareLight::work(%this, %obj)
{
	if(!isObject(%obj))
	{
		return;
	}
	InitContainerRadiusSearch(%obj.getPosition(), GlobalStorage.FlareRadius, $Typemasks::PlayerObjectType);
	while(%hit = containerSearchNext())
	{
		if(%hit.isCloaked())
		{
			%hit.combatEnd = getSimTime() + GlobalStorage.CombatLength;
			%hit.cloak();
		}
	}
	%this.schedule(501, work, %obj);
}

datablock itemData(FlareItem : DarkMItem)
{
	shapeFile = $StalkerDM::Path @ "/models/flare.dts";
	iconName = $StalkerDM::Path @ "/icons/icon_Flare";
	image = FlareImage;
	uiName = "Flare";
	slot = "Secondary";
};

datablock ShapeBaseImageData(FlareImage)
{
	shapeFile = $StalkerDM::Path @ "/models/flare.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = FlareItem;
	ammo = " ";
	//projectile = " ";
	//projectileType = Projectile;

	melee = false;
	armReady = true;

	stateName[0]                   = "Activate";
	stateTimeoutValue[0]           = 0.15;
	stateTransitionOnTimeout[0]    = "Ready";

	stateName[1]                   = "Ready";
	stateTransitionOnTriggerDown[1]= "Fire";
	stateAllowImageChange[1]       = true;
	stateSequence[1]               = "Ready";

	stateName[2]                   = "Fire";
	stateTransitionOnTimeout[2]    = "Reload";
	stateTimeoutValue[2]           = 1;
	stateFire[2]                   = true;
	stateAllowImageChange[2]       = false;
	stateScript[2]                 = "onFire";
	stateWaitForTimeout[2]         = true;

	stateName[3]                   = "Reload";
	stateTransitionOnTriggerUp[3]  = "Ready";
};

function FlareImage::onFire(%this, %obj, %slot)
{
	if(!%obj.isOnGround())
	{
		if(%obj.client)
		{
			%obj.client.centerPrint("Can only do that while on the ground.", 3);
		}
		return;
	}
	if(%obj.CDflare >= getSimTime())
	{
		if(%obj.client)
		{
			%obj.client.centerPrint("Not ready yet!", 1);
		}
		return;
	}
	%obj.CDflare = getSimTime() + GlobalStorage.FlareCD;
	%flare = new fxLight()
	{
		datablock = FlareLight;
		position = %obj.getPosition();
		scale = "1 1 1";
	};
	%flare.setTransform(%obj.getTransform());
	%flare.reset();
	FlareLight.work(%flare);
	%flare.schedule(8000, delete);
}

//	#1.3
datablock itemData(ProxShockItem : DarkMItem)
{
	shapeFile = $StalkerDM::Path @ "/models/flashgadget.dts";
	iconName = $StalkerDM::Path @ "/icons/icon_proxshox";
	image = "";
	uiName = "Proximity Shocker";
	slot = "Secondary";
};
datablock ShapeBaseImageData(ProxShockImage)
{
	shapeFile = $StalkerDM::Path @ "/models/flashgadget.dts";
	emap = true;

	mountPoint = 7;
	offset = "0.2 0.3 0.4";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = ProxShockItem;
	ammo = " ";
	//projectile = " ";
	//projectileType = Projectile;

	melee = false;
	armReady = true;

	stateName[0]                   = "Activate";
	stateTimeoutValue[0]           = 0.1;
	stateTransitionOnTimeout[0]    = "Ready";

	stateName[1]                   = "Ready";
	stateTransitionOnTriggerDown[1]= "Fire";
	stateAllowImageChange[1]       = false;
	stateScript[1]                 = "onReady";
	stateEmitter[1]                = ArrowTrailEmitter;
	stateEmitterTime[1]            = 600;

	stateName[2]                   = "Fire";
	stateTransitionOnTimeout[2]    = "Ready";
	stateTimeoutValue[2]           = GlobalStorage.ProxShockRecharge / 1000;
	stateAllowImageChange[2]       = false;
	stateWaitForTimeout[2]         = true;
};
function ProxShockImage::onReady(%this, %obj, %slot)
{
	if(%obj.client)
	{
		%obj.client.centerPrint("\c3Proximity Shocker recharged!", 3);
	}
}
function ProxShockItem::onUse(%this, %obj, %slot)
{
	if(%obj.client)
	{
		%obj.client.centerPrint("\c3This item does not need to be used manually.", 3);
	}
}
//See misc.cs #2.12 for functionality of Proximity Shocker

//	#1.4
//See misc.cs #2.3 for functionality of the Armor
datablock itemData(ArmorItem : DarkMItem)
{
	shapeFile = $StalkerDM::Path @ "/models/lantern.dts";
	iconName = $StalkerDM::Path @ "/icons/icon_armor";
	image = "";
	uiName = "Body Armor";
	slot = "Secondary";
};
function ArmorItem::onUse(%this, %obj, %slot)
{
	if(%obj.client)
	{
		%obj.client.centerPrint("\c3This item does not need to be used manually.", 3);
	}
}

//	#1.5
datablock ParticleData(RBparticle)
{
	dragCoefficient = 2;
	windCoefficient = 0.3;
	gravityCoefficient = -0.01;
	inheritedVelFactor = 0.5;
	constantAcceleration = 0;
	lifetimeMS = 850;
	lifetimeVarianceMS = 200;
	spinSpeed = 16;
	spinRandomMin = -500;
	spinRandomMax = 500;
	useInvAlpha = 0;
	textureName = "base/data/particles/cloud";
	colors[0] = "0.5 0.5 1 0.4";
	colors[1] = "0.9 0.6 0.3 0.2";
	colors[2] = "0.9 0.3 0.1 0.1";
	colors[3] = "0.9 0.3 0.1 0";
	sizes[0] = 0.5;
	sizes[1] = 0.6;
	sizes[2] = 1.5;
	sizes[3] = 3;
	times[0] = 0;
	times[1] = 0.3;
	times[2] = 0.6;
	times[3] = 1;
};
datablock ParticleEmitterData(RBemitter)
{
	className = "ParticleEmitterData";
	ejectionPeriodMS = 6;
	periodVarianceMS = 1;
	ejectionVelocity = -0.1;
	velocityVariance = 0;
	ejectionOffset = 0.1;
	thetaMin = 0;
	thetaMax = 5;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = RBparticle;
	lifetimeMS = 0;
	lifetimeVarianceMS = 0;
	uiName = "Rocket Boots Flame";
};

datablock itemData(RocketBootsItem : DarkMItem)
{
	shapeFile = $StalkerDM::Path @ "/models/lantern.dts";
	iconName = $StalkerDM::Path @ "/icons/icon_RocketBoots";
	image = RocketBootsImage;
	uiName = "Rocket Boots";
	slot = "Secondary";
};
datablock ShapeBaseImageData(RocketBootsImage)
{
	shapeFile = $StalkerDM::Path @ "/models/lantern.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = RocketBootsItem;
	ammo = " ";
	//projectile = " ";
	//projectileType = Projectile;

	melee = false;
	armReady = true;

	stateName[0]                   = "Activate";
	stateTimeoutValue[0]           = 0.15;
	stateTransitionOnTimeout[0]    = "Ready";

	stateName[1]                   = "Ready";
	stateTransitionOnTriggerDown[1]= "Fire";
	stateAllowImageChange[1]       = true;
	stateSequence[1]               = "Ready";

	stateName[2]                   = "Fire";
	stateTransitionOnTimeout[2]    = "Reload";
	stateTimeoutValue[2]           = 1;
	stateFire[2]                   = true;
	stateAllowImageChange[2]       = false;
	stateScript[2]                 = "onFire";
	stateWaitForTimeout[2]         = true;

	stateName[3]                   = "Reload";
	stateTransitionOnTriggerUp[3]  = "Ready";
};
datablock ShapeBaseImageData(RocketBootsLFoot)
{
	shapeFile = "base/data/shapes/empty.dts";

	mountPoint = 4;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = "0 0 0";

	stateName[0]        = "Activate";
	stateEmitter[0]     = RBemitter;
	stateEmitterTime[0] = 600;
};
datablock ShapeBaseImageData(RocketBootsRFoot : RocketBootsLFoot)
{
	mountPoint = 3;
};

function RocketBootsImage::onFire(%this, %obj, %slot)
{
	if((%obj.CDrocketboots) >= getSimTime())
	{
		if(%obj.client)
		{
			%obj.client.centerPrint("Not ready yet!", 1);
		}
		return;
	}
	%obj.CDrocketboots = getSimTime() + GlobalStorage.RocketBootsCD;
	%obj.pushDatablock(PlayerHumanFastArmor);
	%obj.unmountImage(%slot);
	for(%i = 0; %i < 10; %i++)
	{
		%obj.schedule(%i * 20, mountImage, RocketBootsLFoot, 0); //for some reason it won't mount if it's not scheduled
	}
	%obj.mountImage(RocketBootsRFoot, 1);
	%obj.schedule(GlobalStorage.RocketBootsDuration, RemoveRocketBoots);
}

function Player::RemoveRocketBoots(%obj)
{
	if(%obj.getMountedImage(0) == nameToID(RocketBootsLFoot))
	{
		%obj.unmountImage(0);
	}
	if(%obj.getMountedImage(1) == nameToID(RocketBootsRFoot))
	{
		%obj.unmountImage(1);
	}
	%obj.popDatablock(PlayerHumanFastArmor);
}