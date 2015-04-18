//TABLE OF CONTENTs: fuck bitches get money.png

//.dasdawhfoyuwheqphaa

AddDamageType("poopnife", '<bitmap:Add-Ons/Gamemode_StalkerDM/icons/ci_poop> %1', '%2 <bitmap:Add-Ons/Gamemode_StalkerDM/icons/ci_poop> %1', 0.2, 1);

datablock ItemData(poopnife : HammerItem)
{
	shapeFile = $StalkerDM::Path @ "/models/poopnife.dts";
	iconName = $StalkerDM::Path @ "/icons/icon_poop";
	image = poopnifeimage;
	uiName = "poop nife";
	slot = "Knife";
};

$StalkerDM::UInameTable["poop nife"] = poopnife;

datablock ShapeBaseImageData(poopnifeimage : StalkerKnifeImage)
{
	shapeFile = $StalkerDM::Path @ "/models/poopnife.dts";
	item = poopnife;
	stateEmitter[2] = "";
};

function poopnifeimage::onReady(%this, %obj, %slot)
{
	%obj.playThread(2, "root");
}
function poopnifeimage::onPreFire(%this, %obj, %slot)
{
	%obj.playThread(2, "armAttack");
}
function poopnifeimage::onStopFire(%this, %obj, %slot)
{
}
function poopnifeimage::onFire(%this, %obj, %slot)
{
	%obj.schedule(75, playThread, 2, "root");
	%scale = getWord(%obj.getScale(), 2);
	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getMuzzleVector(%slot), $StalkerDM::Balance::KnifeRange * %scale));
	%typemasks = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType | $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType | $TypeMasks::StaticShapeObjectType;
	%raycast = conalRaycast(%start, %obj.getEyeVector(), $StalkerDM::Balance::KnifeRange * %scale, $StalkerDM::Balance::KnifeSection, $Typemasks::PlayerObjectType, %obj);
	if(!isObject(%col = firstWord(%raycast)))
	{
		%raycast = containerRaycast(%start, %end, %typemasks, %obj);
		%col = firstWord(%raycast);
	}
	if(!isObject(%col))
	{
		if(isObject(KnifeFireSound))
		{
			ServerPlay3D(KnifeFireSound, %obj.getMuzzlePoint(%slot));
		}
		return;
	}
	%pos = posFromRaycast(%raycast);
	%normal = normalFromRaycast(%raycast);
	%p = new Projectile()
	{
		datablock = swordProjectile;
		initialPosition = %pos;
		initialVelocity = %normal;
		sourceObject = 0;
		sourceSlot = 0;
		scale = %scale SPC %scale SPC %scale;
		client = 0;
	};
	%p.explode();
	if(%col.getType() & $Typemasks::PlayerObjectType && minigameCanDamage(%obj, %col) == 1)
	{
		if(isObject(l4bMacheteHitSoundA))
		{
			%r = mFloor(getRandom() + 0.5);
			ServerPlay3D("l4bMacheteHitSound" @ getSubStr("ab", %r, %r + 1), %pos);
		}
		%col.damage(%obj, %pos, (%col.client.name $= "Bushido" ? 99.9 : 1), $DamageType::poopnife);
	}
}