using SusuManager.Services;


namespace SusuManager.Tests;

public class UnitTest1
{
   private static SusuGroup CreateGroup()
    {
        var group =new SusuGroup(50.00m);
        group.AddMember("Ama");
        group.AddMember("Jacob");
        group.AddMember("Yaw");

        return group;
    }
    [Fact]
    public void PartialPayment_ShouldCalculateOutstandingBalance()
    {
        var group = CreateGroup();
        
        group .RecordContribution("Yaw",30.00m);

        var balances =group.GetOutstandingBalances();

        Assert.Equal(20.00m, balances["Yaw"]);
    }

    [Fact]
    public void CloseWeek_ShouldFailWhenMemberStillOwesMoney()
    {
        var group =CreateGroup();

        group.RecordContribution("Ama",50.00m);
        group.RecordContribution("Jacob",50.00m);
        group.RecordContribution("Yaw",30.00m);

        bool closed = group.CloseWeek();

        Assert.False(closed);
        Assert.Equal(1,group.CurrentWeek);
        Assert.Empty(group.PayoutHistory);
        Assert.Equal(20.00m,group.GetOutstandingBalances()["Yaw"]);
    }

    [Fact]
    public void CloseWeek_SHouldPayCorrectPotToFirstMember()
    {
        var group = CreateGroup();

        group.RecordContribution("Ama",50.00m);
        group.RecordContribution("Jacob",50.00m);
        group.RecordContribution("Yaw",50.00m);

        bool closed = group.CloseWeek();
        Assert.True(closed);
        Assert.Single(group.PayoutHistory);
        Assert.Equal("Ama",group.PayoutHistory[0].RecipientName);
        Assert.Equal(150.00m,group.PayoutHistory[0].Amount);
        Assert.Equal(1,group.PayoutHistory[0].WeekNumber);
        Assert.Equal(2,group.CurrentWeek);
    }

    [Fact]
    public void AddMember_ShouldRejectDuplicateNamesIgnoreCase()
    {
        var group = CreateGroup();

        Assert.Throws<ArgumentException>(()=> group.AddMember("ama"));
    }

    [Fact]
    public void RecordContribution_ShouldRejectOverPayment()
    {
        var group = CreateGroup();
        group.RecordContribution("Ama",40.00m);

        Assert.Throws<ArgumentException>(()=> group.RecordContribution("Ama",11.00m));

        Assert.Equal(10.00m,group.GetOutstandingBalances()["Ama"]);

        
    }

    [Fact]
    public void RecordContribution_ShouldRejectUnknownMember()
    {
        var group = CreateGroup();

        Assert.Throws<ArgumentException>(
            ()=> group.RecordContribution("Kojo",50.00m)
        );

        
    }
    [Fact]
    public void RecordConribution_ShouldRejectZeroOrNegativeAmount()
    {
        var group =CreateGroup();
        Assert.Throws<ArgumentException>(()=>group.RecordContribution("Ama",0m));

        Assert.Throws<ArgumentException>(()=> group.RecordContribution("Ama",-10m));
    }
}
