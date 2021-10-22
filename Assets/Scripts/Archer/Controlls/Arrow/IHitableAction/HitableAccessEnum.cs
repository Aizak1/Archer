using System;

namespace Archer.Controlls.IHitableAction
{
    [Flags]
    public enum HitableAccessFlag {
        None = 0,
        Everything = 127,
        destroyObject = 1,
        brokeJoint = 2,
        avtivateBomb = 4,
        enableObject = 8,
        changeRigid = 16,
        levelTarget = 32,
        other = 64

    }
}
