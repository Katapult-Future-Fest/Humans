namespace Humans.Web.ViewComponents;

/// <summary>
/// KFF fork: the admin sidebar's main menu, after KFF Center's. Each entry folds the section groups
/// it names (by <see cref="Humans.Base.Interfaces.AdminNavGroup"/> label) into one top item whose
/// hover panel lists them. A group no entry names lands in "Other", so a section added upstream never
/// disappears; <see cref="FootGroup"/> renders as a plain row at the bottom instead.
/// </summary>
internal static class AdminMenu
{
    public const string FootGroup = "Settings";

    private sealed record Entry(string Label, string Description, string IconCssClass, string[] Groups);

    private static readonly Entry[] Entries =
    [
        new("Members", "People, onboarding, consent and governance", "fa-solid fa-users",
            ["Users", "Onboarding", "Consent", "Governance", "Surveys", "Workgroups", "Pre-approvals", "Low income"]),
        new("Programme", "Events, barrios and the city map", "fa-solid fa-calendar-days",
            ["Events", "Barrios", "City Planning"]),
        new("On site", "Tickets, gate, shifts and on-site services", "fa-solid fa-location-dot",
            ["Tickets", "Gate", "Scanner", "Early Entry", "Shifts", "Rideshare", "Cantina"]),
        new("Money", "Finance, budget, expenses, store and grants", "fa-solid fa-coins",
            ["Finance", "Holded", "Budget", "Expenses", "Store", "Grants"]),
        new("Comms", "Email, campaigns, Google and issues", "fa-solid fa-envelope",
            ["Email", "Campaigns", "MailerLite", "Google", "Issues"]),
        new("System", "Audit, agents, debug and developer tools", "fa-solid fa-gears",
            ["Audit", "Agent", "Backdoor", "Debug", "Development"]),
    ];

    private static readonly Entry Other = new("Other", "Sections not in a menu yet", "fa-solid fa-ellipsis", []);

    /// <summary>The visible groups folded into menus, in menu order; menus with no visible group are dropped.</summary>
    public static IReadOnlyList<AdminSidebarMenuViewModel> Arrange(IReadOnlyList<AdminSidebarGroupViewModel> groups)
    {
        var byLabel = groups.ToDictionary(g => g.Label, StringComparer.Ordinal);
        var menus = Entries
            .Select(e => Menu(e, e.Groups.Where(byLabel.ContainsKey).Select(l => byLabel[l]).ToList()))
            .ToList();

        var named = Entries.SelectMany(e => e.Groups).Append(FootGroup).ToHashSet(StringComparer.Ordinal);
        menus.Add(Menu(Other, groups.Where(g => !named.Contains(g.Label)).ToList()));

        return menus.Where(m => m.Groups.Count > 0).ToList();
    }

    private static AdminSidebarMenuViewModel Menu(Entry e, IReadOnlyList<AdminSidebarGroupViewModel> groups) =>
        new(e.Label, e.Description, e.IconCssClass, groups);
}
