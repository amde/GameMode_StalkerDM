function serverCmdSetPreferredTeam(%client, %team, %gui)
{
	switch$(%team)
	{
		case "Stalkers": %team = 0;
		case "Stalker": %team = 0;
		case "0": %team = 0;
		case "Humans": %team = 1;
		case "Human": %team = 1;
		case "1": %team = 1;
		case "None": %team = -1;
		case "Neither": %team = -1;
		case "-1": %team = -1;
		default:
			messageClient(%client, 'MsgError', "<color:ff0000>Could not find the team \"\c3" @ %team @ "<color:ff0000>\".");
			return;
	}
	if(isObject(%client.file) && %client.file.modify("Opt_PrefTeam", %team))
	{
		if(!%gui)
		{
			messageClient(%client, 'ChatMessage', "\c2Your preferred team has been set successfully.");
		}
	}
	else
	{
		messageClient(%client, 'MsgError', "You must register an account (\c3/register \c5Username Password\c0) or login to do that.");
	}
}

function serverCmdSortPlayers(%client)
{
	if(%client.isAdmin)
	{
		$DefaultMinigame.sortPlayers();
	}
}

function MinigameSO::SortPlayers(%mini)
{
	%stalkers = new simSet();
	%stalkers_np = new simSet(); //No Preference
	%stalkers_nd = new simSet(); //Non Donor
	%humans = new simSet();
	%humans_np = new simSet();
	%humans_nd = new simSet();
	for(%i = 0; %i < %mini.numMembers; %i++)
	{
		%obj = %mini.member[%i];
		%lastTeam[%obj] = %obj.tdmTeam;
		switch(%obj.file.read("Opt_PrefTeam"))
		{
			case 0: %stalkers.add(%obj);
				if(!%obj.file.read("Donor"))
				{
					%stalkers_nd.add(%obj);
				}
			case 1: %humans.add(%obj);
				if(!%obj.file.read("Donor"))
				{
					%humans_nd.add(%obj);
				}
			default:
				if(getRandom() >= 0.5)
				{
					%humans.add(%obj);
					%humans_np.add(%obj);
					%humans_nd.add(%obj);
				}
				else
				{
					%stalkers.add(%obj);
					%stalkers_nd.add(%obj);
				}
		}
	}
	while(mClamp(%humans.getCount(), %stalkers.getCount() - 1, %stalkers.getCount() + 1) != %humans.getCount())
	{
		if(%humans.getCount() > %stalkers.getCount())
		{
			if(%humans_np.getCount() > 0)
			{
				%obj = %humans_np.getObject(0);
				%humans_np.remove(%obj);
				%stalkers_np.add(%obj);
			}
			else if(%humans_nd.getCount() > 0)
			{
				%obj = %humans_nd.getObject(0);
				%humans_nd.remove(%obj);
				%stalkers_np.add(%obj);
			}
			else
			{
				%obj = %humans.getObject(getRandom(0, %humans.getCount() - 1));
			}
			%humans.remove(%obj);
			%stalkers.add(%obj);
		}
		else if(%stalkers.getCount() > %humans.getCount())
		{
			if(%stalkers_np.getCount() > 0)
			{
				%obj = %stalkers_np.getObject(0);
				%stalkers_np.remove(%obj);
				%humans_np.add(%obj);
			}
			else if(%stalkers_nd.getCount() > 0)
			{
				%obj = %stalkers_nd.getObject(0);
				%stalkers_nd.remove(%obj);
				%humans_np.add(%obj);
			}
			else
			{
				%obj = %stalkers.getObject(getRandom(0, %stalkers.getCount() - 1));
			}
			%stalkers.remove(%obj);
			%humans.add(%obj);
		}
	}
	for(%i = 0; %i < %mini.numMembers; %i++)
	{
		%obj = %mini.member[%i];
		if(%stalkers.isMember(%obj))
		{
			%obj.tdmTeam = 0;
		}
		else if(%humans.isMember(%obj))
		{
			%obj.tdmTeam = 1;
		}
		if(%obj.tdmTeam != %lastteam[%obj])
		{
			%obj.instantRespawn();
			%mini.messageAll('',"\c3" @ %obj.getPlayerName() @ "\c5 was put into " @ (%obj.tdmTeam ? "Humans" : "Stalkers") @ "\c5. (Team scramble)");
		}
	}
	%humans.delete();
	%humans_np.delete();
	%humans_nd.delete();
	%stalkers.delete();
	%stalkers_np.delete();
	%stalkers_nd.delete();
}