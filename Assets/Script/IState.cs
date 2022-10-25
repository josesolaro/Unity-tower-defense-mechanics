using System.Collections;

public interface IState
{
    IState Tick(MinionController controller);
    void OnEnter();
    void OnExit();
}
