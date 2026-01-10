using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadrocopterFightController : QuadFlightControler
{

    public override void LeanAhead()
    {
        engineLevels[2] = generalLevel * RotationPowerMultiplyer;
        engineLevels[3] = generalLevel * RotationPowerMultiplyer;
    }

    public override void LeanBack()
    {
        engineLevels[0] = generalLevel * RotationPowerMultiplyer;
        engineLevels[1] = generalLevel * RotationPowerMultiplyer;
    }

    public override void LeanLeft()
    {
        engineLevels[1] = generalLevel * RotationPowerMultiplyer;
        engineLevels[2] = generalLevel * RotationPowerMultiplyer;
    }

    public override void LeanRight()
    {
        engineLevels[0] = generalLevel * RotationPowerMultiplyer;
        engineLevels[3] = generalLevel * RotationPowerMultiplyer; 
    }

    public override void TurnLeft()
    {
        engineLevels[0] = generalLevel * RotationPowerMultiplyer;
        engineLevels[2] = generalLevel * RotationPowerMultiplyer;
    }

    public override void TurnRight()
    {
        engineLevels[1] = generalLevel * RotationPowerMultiplyer;
        engineLevels[3] = generalLevel * RotationPowerMultiplyer;
    }
}
