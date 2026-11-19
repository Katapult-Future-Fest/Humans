# KFF Humans: update and setup checklist

Run this after every upstream sync, every deploy and every fresh setup of `humans.thrivbe.com` (Thrivbe-4, `/opt/humans`).
Tick each line. A failed line means the update is not done.

## 1. Fork is up to date
- [ ] `gh api repos/nobodies-collective/Humans/compare/main...Katapult-Future-Fest:Humans:main --jq '.ahead_by'` is 0 (the fork has nothing the original lacks, except the KFF UI layer).
- [ ] `/opt/humans` is on the fork's `main`: `git -C /opt/humans fetch && git -C /opt/humans status -sb` shows no "behind".

## 2. Started the right way
- [ ] Deploy only with `/opt/humans/deploy.sh` (or `oprun --env-file /opt/humans/.env -- docker compose up -d`). Never plain `docker compose up`.
- [ ] No env value is an `op://` text: `docker inspect humans-app-1 --format '{{range .Config.Env}}{{println .}}{{end}}' | grep -c 'op://'` prints 0.
- [ ] Container is `Up ... (healthy)` and `/health/live` answers 200.

## 3. Resend (all email, including login links)
- [ ] The key is real: it starts with `re_`. (It is send-only, so `restricted_api_key` on the domains call is normal. `API key is invalid` is a failure.)
- [ ] Logs show no `535: Authentication credentials invalid` and no `Failed to send email` since start.
- [ ] A test email goes out and the log shows `Email sent to ...`.
- [ ] Login link works for a Gmail address AND a Hotmail/Outlook address (use test mailboxes; the logs hide addresses). Check the spam folder once.

## 4. UI layer (KFF look on top of upstream)
- [ ] Every UI file still exists and is the KFF version (list in the "UI layer files" section below).
- [ ] `AdminMenuTests` pass.
- [ ] The KFF admin menu and member side menu show, with KFF colours.
- [ ] The 7 languages (en, ca, de, es, fr, it, nl) have no missing menu keys (no raw key names on screen).

## 5. UI is connected to the new backend
- [ ] Click every link in the admin menu and the member side menu. No 404, no error page.
- [ ] Menu code compiles against upstream's new view models, routes and action names.
- [ ] Ticket stub and Profile edit pages open and save.

## 6. App is healthy
- [ ] Database migrations applied. No `ERR` lines in the first 5 minutes of logs.
- [ ] Expected warnings only: Stripe keys not set, GitHub KB folder not found.
- [ ] You can sign in as an admin and open the admin area.

## UI layer files (must survive every sync)
- `src/Humans.Base/Resources/SharedResource{,.ca,.de,.es,.fr,.it,.nl}.resx`
- `src/Humans.Web/ViewComponents/AdminMenu.cs`, `AdminSidebarViewModel.cs`
- `src/Humans.Web/Views/Shared/Components/AdminSidebar/*`, `Views/Shared/_Layout.cshtml`, `wwwroot/js/site.js`
- `src/Humans.Web/wwwroot/css/{admin-shell,site,tokens}.css`
- `src/Sections/Humans.Tickets/Views/Shared/Components/TicketStub/Default.cshtml`
- `src/Sections/Humans.Users/Views/Profile/Edit.cshtml`
- `tests/Humans.Web.Tests/ViewComponents/AdminMenuTests.cs`

Rules: the KFF UI is a separate layer on top. On a clash, keep the UI edit and port the upstream change in by hand.
