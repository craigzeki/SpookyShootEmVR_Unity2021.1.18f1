//interface to ensure DoAnimations is always available regardless of how animation takes place
//i.e. could be using animation controller or animated via code, etc.

public interface iAnimatableObject
{
    public void DoAnimations();
    public void DoAnimations(int health);
}
