namespace Archer.Controlls.IHitableAction {
    public interface IHitable
    {
        void HitAction();
        HitableAccessFlag Type {get;}
    }
}
