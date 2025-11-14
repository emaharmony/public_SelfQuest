Project: SelfQuest (Unity To-Do RPG)

This document captures project-specific guidance for building, configuring, testing, and extending the SelfQuest Unity project. It is intended for experienced Unity/C# developers and focuses on the particulars of this repository and its roadmap.

1. Build and Configuration

- Unity version: Unity 6000.2.6f2 (per ProjectSettings/ProjectVersion.txt and Rider). Open the project in this exact version or a compatible 6000.2 patch stream. If you intentionally change the Unity version, document it here and commit the updated ProjectVersion.txt.
- Target platforms: Primarily mobile (Android/iOS). Recommended backends:
  - Editor and Windows standalone: Mono/.NET 4.x
  - Android/iOS: IL2CPP, .NET 4.x API Compatibility
- Scripting define symbols: Keep feature flags well-scoped, e.g., SELFQUEST_ANALYTICS, SELFQUEST_CLOUD_SAVE, SELFQUEST_NOTIFICATIONS. Add SELFQUEST_AI when experimenting with AI suggestions.
- Packages/assets present: TextMesh Pro, various environment packs, Pixelate shader, Adaptive Performance. Avoid editing imported package files; prefer wrappers or partials under Assets/Scripts.
- Scenes and art: A large number of demo/import scenes exist under Assets/Import. The project scene content (work-in-progress) lives in Assets/Scenes. Keep production scenes under Assets/Scenes and mark demo/example scenes as DoNotShip.
- Serialization and data stability:
  - Skill model: Assets/Scripts/Quest/Skill.cs uses UnityEngine types (Color, Mathf) and Serializable fields. For persistence, wrap Unity types in DTOs (e.g., Color32 or hex strings) before serializing to JSON.
  - Maintain backward-compatibility by versioning save payloads. Add a version field and migration functions for breaking changes.

Repository state notes (as of 2025-11-14):
- No asmdef files are currently present under Assets (search indicates none). Creating asmdefs is recommended before growing test coverage (see Testing section).
- Pixelate shader and handler exist under Assets/Scripts/PixelShader.
- Skill progression formula currently uses XOR for cubic growth (see Known Pitfalls).

2. Testing: Setup and Running

2.1 Unity Test Framework (recommended for game code)

- Create assembly definitions (asmdef) to enable fast, isolated compile units and testing:
  1) Production asmdef: Assets/Scripts/SelfQuest.asmdef
     - Name: SelfQuest
     - Optional references: UnityEngine, UnityEngine.CoreModule, TextMeshPro, etc., as needed.
  2) EditMode tests: Assets/Tests/EditMode/EditModeTests.asmdef
     - References: SelfQuest, UnityEditor.TestRunner, nunit.framework
     - Test platforms: Editor
  3) PlayMode tests: Assets/Tests/PlayMode/PlayModeTests.asmdef
     - References: SelfQuest, UnityEngine.TestRunner, nunit.framework
     - Test platforms: Any platform

- Example EditMode test (place under Assets/Tests/EditMode):
  using NUnit.Framework;
  using SelfQuest;
  using UnityEngine;

  public class SkillTests
  {
      [Test]
      public void AddExp_LevelsUp_WhenThresholdReached()
      {
          var s = new Skill("Fishing", Color.cyan);
          // EXP starts at 0, threshold starts at 5 in current implementation
          s.AddEXP(5);
          Assert.GreaterOrEqual(s.LVL, 2);
      }
  }

- Running Unity tests:
  - Editor UI: Window > General > Test Runner. Choose EditMode/PlayMode and Run All.
  - Rider: Use the Unit Tests tool window; it discovers Unity tests from asmdefs. You can run per-assembly, per-class, or per-test.
  - CLI (useful for CI):
    Unity.exe -batchmode -projectPath "<repo_root>" -runTests -testResults "<repo_root>/TestResults.xml" -testPlatform EditMode -quit

2.2 Quick smoke tests in Rider without Unity dependencies

For algorithmic utilities or demonstration, you can run plain NUnit tests in Rider without Unity assemblies. The following trivial example works as a self-contained check using Rider or dotnet test:

  using NUnit.Framework;

  public class MathSmokeTests
  {
      [Test]
      public void AddsNumbers()
      {
          Assert.AreEqual(4, 2 + 2);
      }
  }

How to run quickly (validated on this repo):
- Option A (Unity-less, via dotnet):
  - A lightweight NUnit test project exists at Tests.Temp/MathSmokeTests.csproj targeting net8.0 with NUnit 3.13.3 and Microsoft.NET.Test.Sdk 17.9.0. To execute a one-off smoke test:
    1) Create a file Tests.Temp/MathSmokeTests.cs with the example above.
    2) Run: dotnet test Tests.Temp
       - Expected: 1 Passed, 0 Failed (validated locally).
    3) Delete the temporary test file afterward to keep the repo clean (leave the csproj as-is unless you intend to remove the folder entirely).
- Option B (Rider without csproj):
  - Create a temporary .cs file with the test above and run it via Rider’s Unit Tests window. Rider can run single-file NUnit tests even without Unity assemblies. Delete the temp file afterward.
Note: These smoke tests do not reference Unity; they are only for validating the test runner wiring.

3. Development Guidelines and Project-Specific Notes

- Code organization
  - Keep all first-party runtime code in Assets/Scripts under clear domains: Managers, Quest, UI, Rendering.
  - Avoid editing imported third-party scripts. If changes are required, wrap them or extend via composition.
  - Use namespaces consistently (e.g., SelfQuest.*) to prevent symbol collisions with imported assets.
  - Scenes: Keep production scenes under Assets/Scenes. Mark demo/example scenes (from Assets/Import and similar) as DoNotShip.

- Skill progression model
  - Current Skill.cs levels up when exp >= expTillNextLvl; initial threshold is 5. The level-up formula uses Mathf.FloorToInt(100 * (lvl ^ 3)). Note: in C#, ^ is bitwise XOR, not power. If cubic growth is intended, replace (lvl ^ 3) with Mathf.Pow(lvl, 3). Consider adding unit tests to lock desired behavior.
  - Expose and document progression curves for balancing and telemetry.
  - Add EditMode tests around Skill.AddEXP/LevelSkill once asmdefs are created to prevent regressions.

- Repeating quests (core roadmap feature)
  - Model quest templates with a recurrence rule (cron-like or simple daily/weekly/monthly enums) and a skill impact vector (select X skills and weights).
  - Generate instances per recurrence window; track completion state per instance. Store next occurrence to support notifications.

- AI suggestions for quests
  - Data inputs: historical completions, skill leveling rates, time of day, streaks.
  - Start with on-device heuristics; later integrate cloud models. Keep the suggestion API async and cancellable. Gate with SELFQUEST_AI.

- Visual shaders (retro aesthetics)
  - A basic Pixelate shader exists under Assets/Scripts/PixelShader. Use a post-process pass controlled via ShaderHandler.cs. Consider providing presets (GB, NES, SNES palettes) and exposing them via ScriptableObjects.
  - When adding more retro shaders, gate experimental ones with a define (e.g., SELFQUEST_SHADERS_EXPERIMENTAL) and ship only whitelisted variants to minimize build bloat.

- Achievements and inventory
  - Use ScriptableObjects for definitions; runtime state should be serializable and independent of definitions. Emit events on unlock to drive UI/FX.

- Save states and cross-device sync
  - Implement a SaveService with pluggable backends: Local JSON (Application.persistentDataPath) and Cloud (e.g., Google Play Games, Apple iCloud, or a custom backend). Use conflict resolution (last-write-wins + merge for accumulative stats). Gate cloud with SELFQUEST_CLOUD_SAVE.
  - Persist Unity structs via DTOs (e.g., Color as hex, Vector3 as float[3]) to ensure JSON stability across Unity versions.

- Device integrations (alarms, widgets)
  - Notifications/alarms: Use platform-appropriate plugins (Android Notification Package, iOS Local Notifications). Centralize scheduling behind an abstraction. Gate with SELFQUEST_NOTIFICATIONS.
  - Widgets: On Android use App Widgets via a plugin; on iOS use WidgetKit via native bridge. Provide fallback in-app reminders if not available.

- Performance and assets
  - Many imported demo assets can bloat build size. Use Addressables or a curated include list. Strip unused shaders and variants. Consider Adaptive Performance settings on mobile.

- Coding style
  - C# 8.0+ where possible (nullable awareness helpful). Properties over fields for public API. Use readonly where applicable. Prefer composition over inheritance for gameplay behaviors.

4. Adding New Tests (Unity)

- Create an EditMode test when logic does not require scene play; create a PlayMode test when timing, coroutines, or scene objects are involved.
- Place new tests under Assets/Tests/EditMode or Assets/Tests/PlayMode and ensure the test asmdef references the production asmdef(s).
- Mocking: For simple isolation, prefer small pure C# methods. For Unity-specific types, write thin adapters and mock those in tests.

5. CI/CD Hooks (suggested)

- Add a CI job that runs EditMode tests headless using Unity’s -runTests flag and collects XML results. Cache Library/ to speed up consecutive runs.
- Add a job to export Android and iOS builds with version bumps driven by git tags.

6. Known Pitfalls in This Repo

- The Skill progression formula uses XOR instead of exponentiation; verify intended design before locking it in tests.
- Large third-party folders under Assets/Import can slow import times; consider moving demo content to a separate project or converting to Asset Store packages via UPM if available.

7. Build Targets and Backends (Unity 6000)
- Editor and Windows standalone: Mono scripting backend is fine for iteration; IL2CPP recommended for production builds.
- Mobile (Android/iOS): Use IL2CPP. API Compatibility Level should match package requirements (traditionally “.NET 4.x” equivalent; in newer Unity streams, select the compatibility level that satisfies dependencies—verify in Player Settings after upgrading Unity).
- Always test both Development Build and Release configurations on target devices due to Adaptive Performance and shader variant stripping.

Appendix: Quick Checklist

- Use correct Unity editor version.
- Keep production code and tests in separate asmdefs.
- Version save data; test migrations.
- Gate platform/integrations behind defines and abstractions.
- Keep imported assets immutable; extend instead of editing.
