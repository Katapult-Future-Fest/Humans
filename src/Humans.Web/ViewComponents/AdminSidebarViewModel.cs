namespace Humans.Web.ViewComponents;

/// <summary>The dashboard entry and visible sidebar navigation groups.</summary>
/// <param name="Groups">The navigation groups visible to the current user.</param>
/// <param name="ShowDashboard">The pinned Dashboard row: only for admin-shaped roles, the dashboard's own gate.</param>
public sealed record AdminSidebarViewModel(bool ShowDashboard, IReadOnlyList<AdminSidebarGroupViewModel> Groups)
{
    /// <summary>The groups folded into the main menu (<see cref="AdminMenu"/>).</summary>
    public IReadOnlyList<AdminSidebarMenuViewModel> Menus { get; } = AdminMenu.Arrange(Groups);

    /// <summary>The group pinned to the foot of the sidebar, when visible.</summary>
    public AdminSidebarGroupViewModel? Foot => Groups.FirstOrDefault(g => string.Equals(g.Label, AdminMenu.FootGroup, StringComparison.Ordinal));
}

/// <summary>One main-menu item: its groups show in a panel on hover or focus.</summary>
public sealed record AdminSidebarMenuViewModel(
    string Label, string Description, string IconCssClass, IReadOnlyList<AdminSidebarGroupViewModel> Groups)
{
    public bool IsActive => Groups.Any(g => g.IsActive);

    /// <summary>Where the top item links: its first group's first item.</summary>
    public AdminSidebarItemViewModel First => Groups[0].First;

    public int? PillSum
    {
        get
        {
            var sum = Groups.Sum(g => g.PillSum ?? 0);
            return sum > 0 ? sum : null;
        }
    }
}

/// <summary>One sidebar row: the group, linking to its first visible item.</summary>
public sealed record AdminSidebarGroupViewModel(string Label, IReadOnlyList<AdminSidebarItemViewModel> Items, bool IsActive)
{
    /// <summary>Where the row links, and whose icon it shows.</summary>
    public AdminSidebarItemViewModel First => Items[0];

    /// <summary>Sum of the group's item pill counts; the per-item pills show on its tab strip.</summary>
    public int? PillSum
    {
        get
        {
            var sum = Items.Sum(i => i.PillCount ?? 0);
            return sum > 0 ? sum : null;
        }
    }
}

/// <summary>One item of a group, rendered as a tab on the group's pages.</summary>
public sealed record AdminSidebarItemViewModel(
    string Label,
    string? Controller,
    string? Action,
    object? RouteValues,
    string? RawHref,
    string IconCssClass,
    bool IsActive,
    int? PillCount);
