////////////////////
//STALKER DM: SHOP//
////////////////////

//////////////
// CONTENTS //
//////////////
//#1. SHOP
//	#1.1

function RegisterItem(%item, %uinames, %slot, %basePrice, %description)
{
	if(!isObject(%item))
	{
		error("Could not find item: " @ %item);
		return;
	}
	%item = %item.getName();
	%item.slot = %slot;
	for(%i = 0; %i < getFieldCount(%uiNames); %i++)
	{
		%name = getField(%uiNames, %i);
		$StalkerDM::UInameTable[%name] = %item;
		$StalkerDM::HelpMessage[%name] = %description;
	}
	$BasePrice[%item] = %basePrice;
	$ItemList::Object[$ItemList::Count++ - 1] = %item;
	$ItemList::Object[%slot, $ItemList::Count[%slot]++ - 1] = %item;
	%item.num = $ItemList::Count[%slot];
}

RegisterItem(StalkerKnifeItem, "Stalker Knife\tKnife\tDefault Knife", "Knife", 0, "\c2The \c3Stalker Knife\c2 deals 15 damage, and can\n\c2deal 70 damage from behind if the Stalker uncloaked within 1.5 seconds.");
RegisterItem(StalkerSerratedKnifeItem, "Serrated Knife\tSerrated", "Knife", 30, "\c3The \c3Serrated Knife\c2 deals 20 damage and an additional 20 over 6 seconds.\n\c2The bleeding effect will stack from repeated hits.");
RegisterItem(StalkerShadowKnifeItem, "Shadow Knife\tShadow", "Knife", 30, "\c2The \c3Shadow Knife\c2 deals 20 damage and blinds the target for 1.5 seconds.\n\c2Using the shadow knife while cloaked will drain energy but not uncloak the Stalker.");
RegisterItem(StalkerMomentumKnifeItem, "Momentum Knife\tMomentum", "Knife", 30, "\c2The \c3Momentum Knife\c2 deals damage relative to the speed you're moving.");
RegisterItem(StalkerBloodlustKnifeItem, "Bloodlust Knife\tBloodlust", "Knife", 30, "\c2The \c3Bloodlust Knife\c2 deals 20 damage, but grows stronger the more damage it deals.\n\c25% of the damage dealt with the Bloodlust Knife is added to its power, making it hit even harder.");

RegisterItem(DarkMitem, "Darkness\tBlind\tMagic Darkness", "Magic", 0, "\c3Darkness\c2 will blind its victim for a short time");
RegisterItem(ParalysisMitem, "Paralysis\tStun", "Magic", 20, "\c3Paralysis\c2 will completely paralyze its victim for a short time.\n\c2The Stalker must concentrate to use this magic, and is also stunned briefly.");
RegisterItem(ShadowStepItem, "Shadowstep\tTeleport\tTele", "Magic", 20, "\c3Shadowstep\c2 allows the Stalker to step through the shadows, and appear behind the target.");
RegisterItem(GSItem, "Gripping Shadows\tRoot\tGrip", "Magic", 20, "\c3Gripping Shadows\c2 stop the target in their tracks and immobilize them for a short time.");
RegisterItem(SchizophreniaItem, "Schizophrenia\tSchizo\tScizo", "Magic", 20, "\c3Scizophrenia\c2 will cause your target to hallucinate as if being attacked.");

RegisterItem(ToxinItem, "Toxin\tPoison\tToxin Curse", "Curse", 0, "\c2The \c3Toxin Curse\c2 will stun any Humans that remain within its area of effect for a short time.");
RegisterItem(WrathItem, "Wrath\tFire\tWrath Curse", "Curse", 20, "\c2The \c3Wrath Curse\c2 burns its victims, causing them to take rapidly increasing fire damage.");
RegisterItem(RageItem, "Rage\tRage Curse", "Curse", 20, "\c2The \c3Rage Curse\c2 weakens Humans in its area of effect, causing them to take increased damage.");
RegisterItem(AbyssItem, "Abyss\tAbiss", "Curse", 20, "\c2The \c3Abyss\c2 will slow its victims down and blind them after a short amount of time.");
RegisterItem(PsychosisItem, "Psychosis\tPsych", "Curse", 20, "\c3Psychosis\c2 will cause hallucinations to appear and attack its victims.");

RegisterItem(PistolItem, "Pistol\tGun\tFlashlight", "Gun", 0, "\c2The \c3Pistol\c2 is a simple weapon with an anti-cloak Flashlight attached.");
RegisterItem(PumpShotgunItem, "Shotgun\tPump Shotgun\tShotty", "Gun", 30, "\c2The \c3Shotgun\c2 is a weapon most effective at close range. It fires a burst of inaccurate projectiles.");
RegisterItem(TAssaultRifleItem, "Assault Rifle\tRifle", "Gun", 30, "\c2The \c3Assault Rifle\c2 is an all-purpose weapon. It is viable both in close and longrange combat, and fires very quickly.");
RegisterItem(ACRAitem, "ACRA\tA.C.R.A.\tMedigun", "Gun", 0, "\c2The \c3A.C.R.A.\c2 will heal nearby teammates when used.\n\c2However, its user takes double damage while it is being used.");
RegisterItem(FlamethrowerItem, "Flamethrower", "Gun", 30, "\c2The \c3Flamethrower\c2 will mow down any foolish to stand in its users way with a wall of fire.");

RegisterItem(StimpakItem, "Stimpak\tStimpack\tMedkit", "Secondary", 0, "\c2The \c3Stimpak\c2 will heal you or a teammate over a short amount of time.");
RegisterItem(FlareItem, "Flare\tFlares\tFlare Gun", "Secondary", 20, "\c2The \c3Flare\c2 will create a stationary zone that uncloaks nearby Stalkers.");
RegisterItem(ProxShockItem, "Prox\tProx Shox\tProximity Shocker", "Secondary", 20, "\c2The \c3Proximity Shocker\c2 will uncloak and stun Stalkers that bump into you.");
RegisterItem(ArmorItem, "Armor\tBody Armor\tVest", "Secondary", 20, "\c2The \c3Armor\c2 reduces damage taken. It is most effective on frontal attacks.");
RegisterItem(RocketBootsItem, "Rocket Boots\tBoots\tRockets", "Secondary", 20, "\c2The \c3Rocket Boots\c2 allow you to make a blazing fast escape when used.");

RegisterItem(L4BKatanaItem, "Katana\tSword", "Melee", 0, "\c2The \c3Katana\c2 is a basic melee weapon that can block frontal knife attacks.");
RegisterItem(TaserItem, "Taser\tTazer\tStungun\tStun gun", "Melee", 20, "\c2The \c3Taser\c2 will incapacitate its victim for a short time and deal a small amount of damage.");
RegisterItem(RepulsorItem, "Repulsor\tWind cannon", "Melee", 20, "\c2The \c3Repulsor\c2 will knock nearby Stalkers away from the user.");
RegisterItem(SledgehammerItem, "Sledge\tHammer\tSledgehammer", "Melee", 20, "\c2The \c3Sledgehammer\c2 will cripple its victims, slowing them down for a short time, but deals low damage.");
RegisterItem(ShieldItem, "Shield\tRiot Shield\tSheild", "Melee", 20, "\c2The \c3Shield\c2 will block frontal melee attacks, and can be used to bash.");

function getPrice(%item)
{
	return mClamp(mCeil($BasePrice[%item] + $PriceMod[%item]), $BasePrice[%item] / 2, $BasePrice[%item] * 2);
}

function serverCmdViewStore(%client, %section)
{
	messageClient(%client, 'ChatMessage', "\c3Stalker DM: Store Listings \c2(Buy items with the \c3/buy \c0<item>\c2 command)");
	switch$(%section)
	{
		case "Guns":
			messageClient(%client, 'ChatMessage', "\c2For a description of what an item does, enter \c3/help \c0<item name>\c2.");
			for(%i = 0; %i < $ItemList::Count["Gun"]; %i++)
			{
				%obj = $ItemList::Object["Gun", %i];
				if(!%client.file.read("UnlockedI_" @ %obj))
				{
					messageClient(%client, 'ChatMessage', "\c2The \c3" @ %obj.uiName @ " \c2costs \c3" @ getPrice(%obj) @ "\c2 points.");
				}
				else
				{
					messageClient(%client, 'ChatMessage', "\c7The " @ %obj.uiName @ " costs " @ getPrice(%obj) @ " points. (Owned)");
				}
			}
		case "Gadgets":
			messageClient(%client, 'ChatMessage', "\c2For a description of what an item does, enter \c3/help \c0<item name>\c2.");
			for(%i = 0; %i < $ItemList::Count["Secondary"]; %i++)
			{
				%obj = $ItemList::Object["Secondary", %i];
				if(!%client.file.read("UnlockedI_" @ %obj))
				{
					messageClient(%client, 'ChatMessage', "\c2The \c3" @ %obj.uiName @ " \c2costs \c3" @ getPrice(%obj) @ "\c2 points.");
				}
				else
				{
					messageClient(%client, 'ChatMessage', "\c7The " @ %obj.uiName @ " costs " @ getPrice(%obj) @ " points. (Owned)");
				}
			}
		case "Melee":
			messageClient(%client, 'ChatMessage', "\c2For a description of what an item does, enter \c3/help \c0<item name>\c2.");
			for(%i = 0; %i < $ItemList::Count["Melee"]; %i++)
			{
				%obj = $ItemList::Object["Melee", %i];
				if(!%client.file.read("UnlockedI_" @ %obj))
				{
					messageClient(%client, 'ChatMessage', "\c2The \c3" @ %obj.uiName @ " \c2costs \c3" @ getPrice(%obj) @ "\c2 points.");
				}
				else
				{
					messageClient(%client, 'ChatMessage', "\c7The " @ %obj.uiName @ " costs " @ getPrice(%obj) @ " points. (Owned)");
				}
			}
		case "Knives":
			messageClient(%client, 'ChatMessage', "\c2For a description of what an item does, enter \c3/help \c0<item name>\c2.");
			for(%i = 0; %i < $ItemList::Count["Knife"]; %i++)
			{
				%obj = $ItemList::Object["Knife", %i];
				if(!%client.file.read("UnlockedI_" @ %obj))
				{
					messageClient(%client, 'ChatMessage', "\c2The \c3" @ %obj.uiName @ " \c2costs \c3" @ getPrice(%obj) @ "\c2 points.");
				}
				else
				{
					messageClient(%client, 'ChatMessage', "\c7The " @ %obj.uiName @ " costs " @ getPrice(%obj) @ " points. (Owned)");
				}
			}
		case "Spells":
			messageClient(%client, 'ChatMessage', "\c2For a description of what an item does, enter \c3/help \c0<item name>\c2.");
			for(%i = 0; %i < $ItemList::Count["Magic"]; %i++)
			{
				%obj = $ItemList::Object["Magic", %i];
				if(!%client.file.read("UnlockedI_" @ %obj))
				{
					messageClient(%client, 'ChatMessage', "\c2The \c3" @ %obj.uiName @ " \c2costs \c3" @ getPrice(%obj) @ "\c2 points.");
				}
				else
				{
					messageClient(%client, 'ChatMessage', "\c7The " @ %obj.uiName @ " costs " @ getPrice(%obj) @ " points. (Owned)");
				}
			}
		case "Curses":
			messageClient(%client, 'ChatMessage', "\c2For a description of what an item does, enter \c3/help \c0<item name>\c2.");
			for(%i = 0; %i < $ItemList::Count["Curse"]; %i++)
			{
				%obj = $ItemList::Object["Curse", %i];
				if(!%client.file.read("UnlockedI_" @ %obj))
				{
					messageClient(%client, 'ChatMessage', "\c2The \c3" @ %obj.uiName @ " \c2costs \c3" @ getPrice(%obj) @ "\c2 points.");
				}
				else
				{
					messageClient(%client, 'ChatMessage', "\c7The " @ %obj.uiName @ " costs " @ getPrice(%obj) @ " points. (Owned)");
				}
			}
		default:
			messageClient(%client, 'ChatMessage', "\c2Human Section: [\c6/viewstore \c0<\c3Guns / Gadgets / Melee\c0>\c2]");
			messageClient(%client, 'ChatMessage', "\c2Stalker Section: [\c6/viewstore \c0<\c3Knives / Spells / Curses\c0>\c2]");
	}
}
function serverCmdViewShop(%client, %section)
{
	serverCmdViewStore(%client, %section);
}
function serverCmdShop(%client, %section)
{
	serverCmdViewStore(%client, %section);
}

function serverCmdBuy(%client, %w1, %w2, %w3, %w4, %w5)
{
	if(%client.file.readOnly)
	{
		messageClient(%client, 'MsgError', "You must register an account (\c3/register \c5Username\c0) or login to do that.");
		return;
	}
	%str = trim(%w1 SPC %w2 SPC %w3 SPC %w4 SPC %w5);
	%item = $StalkerDM::UInameTable[%str];
	if(!isObject(%item))
	{
		messageClient(%client, 'MsgError', "<color:ff0000>Could not find item \"\c3" @ %str @ "<color:ff0000>\".");
		return;
	}
	else
	{
		if(%client.file.read("UnlockedI_" @ %item))
		{
			messageClient(%client, 'MsgError', "<color:ff0000>You already have the \c3" @ %item.uiName @ "<color:ff0000>.");
			return;
		}
		%price = getPrice(%item);
		%points = %client.file.read("PointsA");
		if(%points < %price)
		{
			messageClient(%client, 'MsgError', "<color:ff0000>Not enough points! You need \c3" @ %price @ "<color:ff0000> but only have \c3" @ mFloatLength(%client.file.read("PointsA"), 0) @ "<color:ff0000>.");
			return;
		}
		%client.file.modify("PointsA", %points - %price);
		%slot = %item.slot;
		for(%i = 0; %i < $ItemList::Count[%slot]; %i++)
		{
			if(getPrice($ItemList::Object[%slot, %i]) > 0)
			{
				%Ritems++;
			}
		}
		$PriceMod[%item]+= 1;
		for(%i = 0; %i < $ItemList::Count[%slot]; %i++)
		{
			$PriceMod[$ItemList::Object[%slot, %i]]-= 1 / %Ritems;
		}
		%client.file.modify("UnlockedI_" @ %item, 1);
		switch$(%slot)
		{
			case "Magic": %slot = "Spell";
			case "Secondary": %slot = "Gadget";
		}
		commandToClient(%client, 'ItemUnlocked', %slot, %item.num, 1);
		messageAll('ChatMessage', "\c3" @ %client.name @ "<color:800000> has purchased the \c3" @ %item.uiName @ "<color:800000>!");
		messageClient(%client, '', "\c2You can use this item by entering \c3/setloadout \c0<item name>\c2.");
	}
}
function serverCmdPurchase(%client, %w1, %w2, %w3, %w4, %w5)
{
	serverCmdBuy(%client, %w1, %w2, %w3, %w4, %w5);
}

function serverCmdCheckPoints(%client)
{
	if(%client.file.readOnly)
	{
		messageClient(%client, 'MsgError', "You must register an account (\c3/register \c5Username\c0) or login to do that.");
		return;
	}
	messageClient(%client, '', "\c2Points available: \c3" @ mFloatLength(%client.file.read("PointsA"), 0) @ "\c2     Total Lifetime Points: \c3" @ mFloatLength(%client.file.read("PointsT"), 0));
}