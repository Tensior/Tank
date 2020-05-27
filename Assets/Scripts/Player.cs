public class Player : CombatController
{
    protected override void AllHealthLost()
    {
        base.AllHealthLost();
        gameObject.SetActive( false );
    }
}
