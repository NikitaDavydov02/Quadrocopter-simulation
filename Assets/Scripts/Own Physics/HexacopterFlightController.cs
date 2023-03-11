using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexacopterFlightController : QuadFlightControler
{
    // Start is called before the first frame update
    public override void LeanAhead()
    {
        engineLevels[2] = generalLevel * RotationPowerMultiplyer;
        engineLevels[4] = generalLevel * RotationPowerMultiplyer;
    }

    public override void LeanBack()
    {
        engineLevels[5] = generalLevel * RotationPowerMultiplyer;
        engineLevels[3] = generalLevel * RotationPowerMultiplyer;
    }

    public override void LeanLeft()
    {
        engineLevels[5] = generalLevel * RotationPowerMultiplyer;
        engineLevels[2] = generalLevel * RotationPowerMultiplyer;
    }

    public override void LeanRight()
    {
        engineLevels[3] = generalLevel * RotationPowerMultiplyer;
        engineLevels[4] = generalLevel * RotationPowerMultiplyer;
    }

    public override void TurnLeft()
    {
        engineLevels[1] = generalLevel * RotationPowerMultiplyer;
        engineLevels[3] = generalLevel * RotationPowerMultiplyer;
        engineLevels[4] = generalLevel * RotationPowerMultiplyer;
    }

    public override void TurnRight()
    {
        engineLevels[0] = generalLevel * RotationPowerMultiplyer;
        engineLevels[2] = generalLevel * RotationPowerMultiplyer;
        engineLevels[5] = generalLevel * RotationPowerMultiplyer;
    }
}
