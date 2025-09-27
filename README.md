# Attendo — Attendance Tracking Service

Attendo is a simple, reliable web application that helps teachers, instructors, and educators track student attendance, generate reports, and calculate attendance percentages - all from any device.

---

## 📁 Project Architecture

This project uses a monorepo structure with clearly separated frontend and backend applications.

```txt
attendo-monorepo/
├── front/                   # React frontend
├── back/                    # .NET 9 Web API backend
├── docker/                  # Docker & Docker Compose configs
├── .github/
│   └── workflows/           # CI/CD pipelines
├── .gitignore
└── README.md
```

## 🧩 Core Functionalities

Attendo is built around four essential features designed to simplify attendance tracking for educators:

1. **Create and Manage Student Groups**
   - Teachers can create one or more student groups
   - Each group supports adding, editing, and removing students by name.
2. **Track Daily Attendance**
   - For any selected group, the teacher can mark attendance for each student on a given date.
   - Attendance status: Present / Absent
   - Calendar-based navigation allows quick access to past or future dates.
3. **Calculate Attendance Percentage**
   - Automatically computes individual and group-wide attendance rates.
   - Displayed as a clear percentage (e.g., “Student X: 85% attendance this month”).
   - Helps identify students with frequent absences at a glance.
4. **Export Attendance Reports (.xlsx)**
   - One-click export of attendance data for a selected group and date range.
   - Output includes:
     - Student names
     - Dates attended/missed
     - Total attendance percentage
   - File format: Microsoft Excel (.xlsx) for easy sharing and printing.

## 🧱 Coding Standards

### General

- Follow **SOLID**, **KISS** and **DRY** principles
- Avoid magic strings/numbers—use constants or enums
- Keep functions small
- Prefer composition over inheritance

### Front-end

- Use React hooks for logic; avoid class components
- Follow the principles of the FSD (Feature-sliced-design)

### Back-end

- Follow Clean Architecture principles:
  - `Controllers/` – HTTP layer only
  - `Services/` – Business logic
  - `Models/` – EF Core entities
  - `DTOs/` – Request/response models
  - `Data/` – DbContext and migrations
- Use dependency injection for all services
- All public methods must have XML documentation

## 🔀 Version Control & Workflow

### Branching strategy

- `main` – always deployable, protected branch
- `feature/*` – short-lived branches for new features or fixes
  Naming: `feature/group-creation`, `fix/attendance-export`
- No direct commits to main

### Pull Requests (PRs)

- Every change must go through a Pull Request
- PR title format:
  `feat|fix|chore|docs(scope): brief description`
  Example: `feat(web): add group creation form`
- PR description must include:
  - Link to related issue (if any)
  - Summary of changes

### Code Review & Merge Rules

- Minimum 1 approved review from a team member
- Squash and merge preferred to keep history clean.
- Delete branch after merge.

### 📝 Commit Message Convention

```txt
<type>(<scope>): <short summary>
<BLANK LINE>
<detailed description (optional)>
```

#### Types

- `feat` – new feature
- `fix` – bug fix
- `docs` – documentation only
- `style` – formatting, missing semicolons, etc.
- `refactor` – code change that neither fixes a bug nor adds a feature
- `perf` – performance improvement
- `test` – adding or correcting tests
- `chore` – maintenance (deps, tooling, etc.)

#### Scopes (examples)

- `front` – frontend changes
- `back` – backend changes
- `ci` – GitHub Actions, build scripts
- `db` – database migrations/schema

**✅ Good:**

- `feat(api): add endpoint to create student group`
- `fix(web): attendance toggle not updating UI`

**❌ Bad:**

- `fixed stuff`
- `update code`
