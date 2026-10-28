using AwesomeAssertions;
using Humans.Web.ViewComponents;

namespace Humans.Web.Tests.ViewComponents;

public class AdminMenuTests
{
    [HumansFact]
    public void Folds_Groups_Into_Menus_With_Unknown_In_Other_And_Settings_At_The_Foot()
    {
        var model = new AdminSidebarViewModel(true,
            [Group("Debug"), Group("Settings"), Group("Tickets", active: true), Group("Brand new"), Group("Gate"), Group("Users")]);

        model.Menus.Select(m => m.Label).Should().Equal("Members", "On site", "System", "Other");
        model.Menus.Single(m => string.Equals(m.Label, "On site", StringComparison.Ordinal)).Groups.Select(g => g.Label).Should().Equal("Tickets", "Gate");
        model.Menus.Single(m => string.Equals(m.Label, "Other", StringComparison.Ordinal)).Groups.Select(g => g.Label).Should().Equal("Brand new");
        model.Menus.Where(m => m.IsActive).Select(m => m.Label).Should().Equal("On site");
        model.Menus.SelectMany(m => m.Groups).Should().NotContain(g => string.Equals(g.Label, "Settings", StringComparison.Ordinal));
        model.Foot!.Label.Should().Be("Settings");
    }

    private static AdminSidebarGroupViewModel Group(string label, bool active = false) =>
        new(label, [new AdminSidebarItemViewModel(label, "C", "A", null, null, "icon", false, null)], active);
}
