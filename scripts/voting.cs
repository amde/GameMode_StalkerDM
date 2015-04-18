function GetHighestNumber(%str)
{
	%max = -9999999;
	for(%i = 0; %i < getWordCount(%str); %i++)
	{
		%w = getWord(%str, %i);
		if(%w > %max)
		{
			%max = %w;
			%highest = %i;
		}
		else if(%w == %max)
		{
			%highest = %highest SPC %i;
		}
	}
	return %highest;
}
function PickRandomWord(%str)
{
	return getWord(%str, getRandom(0, getWordCount(%str)-1));
}

function MinigameSO::StartStalkerGamemodeVote(%mini, %opts)
{
	cancel(%mini.voteSched);
	if(!strLen(%opts))
	{
		%opts = "Last Man Standing" TAB "Deathmatch" TAB "Capture the Artifact";
	}
	%mini.messageAll('', "\c5Voting on the next gamemode has started! Enter \c3/vote \c0<number of your choice> \c5to vote!");
	%c = getFieldCount(%opts);
	for(%i = 0; %i < %c; %i++)
	{
		%mini.opt[%i] = getField(%opts, %i);
		%mini.vote[%i] = 0;
		%mini.messageAll('', "\c3" @ %i @ ". \c5" @ %mini.opt[%i]);
	}
	%mini.voteSched = %mini.schedule(30000, EndStalkerGamemodeVote);
}
function MinigameSO::EndStalkerGamemodeVote(%mini)
{
	for(%i = 0; strLen(%mini.opt[%i]); %i++)
	{
	}
	%nOpts = %i;
	if(!%nOpts)
	{
		return;
	}
	for(%i = 0; %i < %mini.numMembers; %i++)
	{
		%mini.member[%i].hasVoted = false;
	}
	for(%i = 0; %i < %nOpts; %i++)
	{
		%s = %s SPC %mini.vote[%i];
	}
	%w = getHighestNumber(trim(%s));
	if(getWordCount(%w) > 1)
	{
		%result = %mini.opt[PickRandomWord(%w)];
		%mini.messageAll('', "\c5The vote was a \c3tie\c5. \c3" @ %result @ "\c5 will be the next gamemode.");
	}
	else
	{
		%result = %mini.opt[%w];
		%mini.messageAll('', "\c5The vote resulted in \c3" @ %result @ "\c5 being picked as the next gamemode.");
	}
	%mini.nextGamemode = %result;
	for(%i = 0; strLen(%mini.opt[%i]); %i++)
	{
		%mini.opt[%i] = "";
		%mini.vote[%i] = "";
	}
}

function serverCmdVote(%client, %opt)
{
	%mini = %client.minigame;
	if(!%mini)
	{
		messageClient(%client, 'MsgError', "\c0You are not in a minigame!");
		return;
	}
	else if(!strLen(%mini.opt[0]))
	{
		messageClient(%client, 'MsgError', "\c0Your minigame is not running a vote!");
		return;
	}
	else if(%client.hasVoted)
	{
		%mini.vote[%client.opt]--;
		%mini.vote[%opt]++;
		messageClient(%client, '', "\c2Your vote was changed to \c3" @ %mini.opt[%opt] @ " \c2.");
		return;
	}
	else if(!strLen(%mini.opt[%opt]))
	{
		messageClient(%client, 'MsgError', "\c0That is not a valid choice.");
		return;
	}
	%client.hasVoted = true;
	%client.votedFor = %opt;
	messageClient(%client, '', "\c2Your vote for \c3" @ %mini.opt[%opt] @ " \c2has been counted.");
	%mini.vote[%opt]++;
	for(%i = 0; %i < %mini.numMembers; %i++)
	{
		%c+= %mini.member[%i].hasVoted;
	}
	if(%c == %i)
	{
		%mini.EndStalkerGamemodeVote();
	}
}