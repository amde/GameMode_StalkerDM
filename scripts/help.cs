////////////////////
//STALKER DM: HELP//
////////////////////

//////////////
// CONTENTS //
//////////////
//#1. FUNCTION CREATION
//	#1.1 serverCmdHelp
//	#1.2 gameconnection::MultiMessage
//#2. HELP REGISTRY
//	#2.1 (blank)
//	#2.2 Controls
//	#2.3 Gamemodes
//	#2.4 Achievements
//	#2.5 Unlockables
//	#2.6 Credits
//	#2.7 FAQ
//	#2.8 Donation

//#1.
//	#1.1
function serverCmdHelp(%client, %arg1, %arg2, %arg3, %arg4, %arg5, %arg6, %arg7, %arg8, %arg9, %arg10)
{
	%arg = trim(%arg1 SPC %arg2 SPC %arg3 SPC %arg4 SPC %arg5 SPC %arg6 SPC %arg7 SPC %arg8 SPC %arg9 SPC %arg10);
	%msg = $StalkerDM::HelpMessage[%arg];
	if(strLen(%msg))
	{
		%client.multiMessage('', 1000, %msg);
	}
	else
	{
		messageClient(%client, '', "<color:ff0000>Could not find help topic \"<spush><color:ffff00>" @ %arg @ "<spop>\".");
	}
}

function serverCmdForceHelp(%client, %target, %arg1, %arg2, %arg3, %arg4, %arg5)
{
	if(!%client.isAdmin)
	{
		return;
	}
	%arg = trim(%arg1 SPC %arg2 SPC %arg3 SPC %arg4 SPC %arg5);
	%tclient = findClientByName(%target);
	if(!isObject(%tclient))
	{
		messageClient(%client, 'MsgError', "<color:ff0000>Could not find \"<spush><color:ffff00>" @ %target @ "<spop>\".");
		return;
	}
	%msg = $StalkerDM::HelpMessage[%arg];
	if(strLen(%msg))
	{
		for(%i = 0; %i < clientGroup.getCount(); %i++)
		{
			%cl = clientGroup.getObject(%i);
			if(%cl.isAdmin)
			{
				messageClient(%cl, '', "\c3" @ %client.name @ "\c2 forcefully helped \c3" @ %tclient.name @ "\c2 with " @ %arg @ ".");
			}
		}
		%tclient.multiMessage('', 1000, %msg);
	}
	else
	{
		messageClient(%client, 'MsgError', "<color:ff0000>Could not find help topic \"<spush><color:ffff00>" @ %arg @ "<spop>\".");
	}
}

//	#1.2
function gameConnection::MultiMessage(%client, %msgType, %delay, %msgText)
{
	for(%i = 0; %i < getFieldCount(%msgText); %i++)
	{
		schedule(%i * %delay, 0, messageClient, %client, %msgType, getField(%msgText, %i));
	}
}

//#2.
//	#2.1
$StalkerDM::HelpMessage[""] = "\c3Stalker Deathmatch: Help topics list\n\c2Controls, Accounts, Gamemodes, Achievements, Shop (item shop), Loadouts (equipping weapons), Credits, FAQ";

//	#2.2
$StalkerDM::HelpMessage["Controls"] = "\c3Stalker Deathmatch: Controls\n\c2Stalker Controls: Cloaking - \c3Jet\c2 to cloak. The cloak is drained based on how fast your are moving. \c2Flashlights will make the cloaking less effective.\n\c2Stalker Controls: Sniffing - \c3Light Key\c2 to sniff for humans.\n\c2Human Controls: Light - Use the \c3light key\c2 to activate your pistol's flashlight, which reduces cloaking power.";

//	#2.3
$StalkerDM::HelpMessage["Gamemodes"] = "\c3Stalker Deathmatch: Gamemodes\n\c3Last Man Standing\c2 - Both teams fight to the death. A team can win in two ways: Eliminating the other team or having the most points left when the time limit expires.\n\c3Deathmatch\c2 - A war-like mode. A team can win in one of two ways: Achieving the point limit, or having the most points at the end of the round.\n\c3Capture the Artifact\c2 - A one-sided CTF type gamemode where the Stalkers attempt to defend their artifacts from the humans.";

//	#2.4
$StalkerDM::HelpMessage["Achievements"] = "\c3Stalker Deathmatch: Achievements\n\c2Achievements can be unlocked by completing a specified task. For a list of achievements, enter \c3/help Achievements List\c2.";
for(%i = 0; %i < $StalkerDM::AchievementCount; %i++)
{
	%msg = %msg NL "\c3" @ $StalkerDM::Achievement[%i, "Name"] @ "\c2: " @ $StalkerDM::Achievement[%i, "Description"];
}
$StalkerDM::HelpMessage["Achievements List"] = "\c3Stalker Deathmatch: Achievements List\n\c2For the description of / item unlocked by an achievement, enter \c3/help \c0Achievement Name\c2." @ %msg;
for(%i = 0; %i <= $StalkerDM::AchievementCount; %i++)
{
	%item = $StalkerDM::Achievement[%i, "Unlock"];
	$StalkerDM::HelpMessage[$StalkerDM::Achievement[%i, "Name"]] = "\c3" @ $StalkerDM::Achievement[%i, "Name"] @ " \c2(" @ $StalkerDM::Achievement[%i, "Value"] @ " points)" @ "\n\c2" @ $StalkerDM::Achievement[%i, "Description"] @ (%item !$= "" ? "\n\c2 - Unlocks item: " @ %item : "");
}

//	#2.5
$StalkerDM::HelpMessage["Shop"] = "\c3Stalker Deathmatch: Shop\n\c2The item shop can be accessed by entering \c3/viewStore\c2.\n\c2You can purchase an item in the store with the \c3/buy \c0<item> \c2command using Achievement Points\n\c2Achievement points are gained from earning Achievements.";
$StalkerDM::HelpMessage["Store"] = $StalkerDM::HelpMessage["Shop"];

$StalkerDM::HelpMessage["Loadouts"] = "\c2You can use an unlocked item by entering \c3/setloadout \c0<item name>\c2.\n\c2You will be equipped with the selected item when you next spawn.";
$StalkerDM::HelpMessage["Accounts"] = "\c2To create an account: \c3/register \c0username password\n\c2To log into an existing account: \c3/login \c0username password\n\c2To log out: \c3/logout   \c2To delete the logged-in account: \c3/deletefile";

//	#2.6
%credits = "\c3Stalker Deathmatch: Version " @ $StalkerDM::Version;
%credits = %credits NL "\c2Authors: \c3The Craftsmen Clan";
%credits = %credits NL "\c2Contributors:";
%credits = %credits NL "   \c3Amadé \c2(Programming, original knife models, shadow knife model, proximity shocker model, icons)";
%credits = %credits NL "   \c3Spock \c2(Hosting, support, ideas)";
%credits = %credits NL "   \c3Wizzeh \c2(Hosting, support, ideas)";
%credits = %credits NL "   \c3Blastdown \c2(ACRA, proximity shocker, and flare models)";
%credits = %credits NL "   \c3Bushido \c2(Stalker, momentum, bloodlust, and serrated knife models; stimpak, taser, flamethrower, and shield models; some particle effects and sounds)";
%credits = %credits NL "   \c3Tingalz \c2(Repulsor and poop nife models)";
%credits = %credits NL "   \c3Crysist \c2(Original serrated knife model)";
%credits = %credits NL "\c2Additional Credits / Thanks to:";
%credits = %credits NL "   \c3Chrono \c2(Inventory slot adjustment support script)";
%credits = %credits NL "   \c3Devastator \c2(Modified form of LightTinyEmitter used for flashlight)";
%credits = %credits NL "   \c3Space Guy \c2(Support_AltDatablock support script)";
%credits = %credits NL "   \c3Kalphiter \c2(For his dedicated hosting service)";
%credits = %credits NL "   \c2All of the players who participated in beta testing.";
$StalkerDM::HelpMessage["Credits"] = %credits;

//	#2.7
$StalkerDM::HelpMessage["FAQ"] = "\c2\"How do I check my achievement points?\" - \c3/checkPoints\n\c2\"How can I set a handicap?\" - \c3/setHandicap \c0<0.01 to 0.99>";

//	#2.8
$StalkerDM::HelpMessage["Donation"] = "\c2You can donate to support the Craftsmen. 100% of your donation will go towards the costs of\n\c2running this server. You can donate at <a:wbjah.com/cmen/donate.htm>http://wbjah.com/cmen/donate.htm</a>\c2.\n\c2Donors get special priveleges, including an achievement, and a 95% or higher chance\n\c2of being on their preferred team.";