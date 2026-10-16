namespace SusuManager.Models;
public class Member
{
    public string Name {get; }
    public int JoiningOrder {get; }

    public Member(string name, int joiningOrder)
    {
        Name = name;
        JoiningOrder = joiningOrder;
    }

}