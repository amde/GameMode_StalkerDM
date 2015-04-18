//blind
function Player::Blind(%this, %dur, %dr)
{
	if(%this.getClassName() $= "AIplayer")
	{
	}
	else if(%this.BlindDR < 3)
	{
		if(%dr)
		{
			%dur/= mClamp(%this.BlindDR + 1, 1, 4);
			%this.BlindDR++;
			%this.schedule(15000 + %dur, BlindDRdec);
		}
		if(isObject(%data = %this.getDatablock().firstPersonOnlyAnalogue))
		{
			%this.setDatablock(%data);
		}
		%this.blinded = true;
		%this.mountImage(DarkBlindPlayerImage, 1);
		if(isEventPending(%this.unblindSched))
		{
			cancel(%this.unblindSched);
		}
		%this.unblindSched = %this.schedule(%dur, unblind);
	}
}

function Player::Unblind(%this)
{
	if(isEventPending(%this.unblindSched))
	{
		cancel(%this.unblindSched);
	}
	if(isObject(%data = %this.getDatablock().nonFirstPersonOnlyAnalogue))
	{
		%this.setDatablock(%data);
	}
	%this.blinded = false;
	if(%this.getMountedImage(1) == nameToID(DarkBlindPlayerImage))
	{
		%this.unmountImage(1);
		return true;
	}
	return false;
}

function Player::BlindDRDec(%this)
{
	%this.BlindDR--;
}

//stun
function Player::Stun(%this, %dur, %dr)
{
	if(%this.getClassName() $= "AIplayer")
	{
	}
	else if(%this.StunDR <= 3)
	{
		if(%dr)
		{
			%dur/= mClamp(%this.StunDR + 1, 1, 4);
			%this.StunDR++;
			%this.schedule(15000 + %dur, StunDRDec);
		}
		%this.stunned = true;
		if(isEventPending(%this.unstunSched))
		{
			cancel(%this.unstunSched);
		}
		%this.unstunSched = %this.schedule(%dur, Unstun);
		if(isEventPending(%client.stunSched))
		{
			cancel(%client.stunSched);
		}
		StunSched(%this.client, %this.unstunSched);
	}
}

function StunSched(%client, %sched)
{
	%dur = getTimeRemaining(%sched) / 1000;
	if(!isObject(%client))
	{
		return;
	}
	if(!isObject(%obj = %client.player) || !%obj.stunned)
	{
		cancel(%sched);
		return;
	}
	if(%client.getControlObject() != (%cam = %client.camera))
	{
		%client.setControlObject(%cam);
		%cam.setMode("Corpse", %obj);
	}
	commandToClient(%client, 'centerPrint', "<color:FF0000>Stunned! " @ mFloatLength(%dur, 1), 0.2);
	if(%dur > 0.1)
	{
		%client.stunSched = schedule(100, 0, StunSched, %client, %sched);
	}
}

function Player::StunDRDec(%this)
{
	%this.StunDR--;
}

function Player::Unstun(%this)
{
	cancel(%this.unstunSched);
	if(isObject(%client = %this.client) && %client.getControlObject() == %client.camera)
	{
		%client.setControlObject(%this);
	}
	%this.stunned = false;
}

//slows / haste
function Player::RecalculateSpeed(%this)
{
	%armor = %this.getdatablock();
	%fspeed = %armor.maxForwardSpeed;
	%sspeed = %armor.maxSideSpeed;
	%bspeed = %armor.maxBackwardSpeed;
	%rec = %this.speedFactors;
	for(%i = 0; %i < getWordCount(%rec); %i++)
	{
		%fspeed*= getWord(%rec, %i);
		%sspeed*= getWord(%rec, %i);
		%bspeed*= getWord(%rec, %i);
	}
	%this.setMaxForwardSpeed(%fspeed);
	%this.setMaxSideSpeed(%sspeed);
	%this.setMaxBackwardSpeed(%bspeed);
}

function Player::AddSpeedFactor(%this, %amt, %dur)
{
	%this.speedFactors = trim(%this.speedFactors SPC %amt);
	%this.schedule(%dur, EndSpeedFactor, %amt);
	%this.RecalculateSpeed();
}

function Player::EndSpeedFactor(%this, %amt)
{
	%speedFactors = %this.speedFactors;
	for(%i = 0; %i < getWordCount(%speedFactors); %i++)
	{
		if(getWord(%speedFactors, %i) $= %amt)
		{
			%this.speedFactors = removeWord(%speedFactors, %i);
			break;
		}
	}
	%this.recalculateSpeed();
}

function Player::Slow(%this, %amt, %dur)
{
	%this.addSpeedFactor(1 - %amt, %dur);
}