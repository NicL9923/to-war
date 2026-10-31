# To War

Open the project with the [Godot 4.7.2 .NET editor](https://godotengine.org/download/archive/4.7.2-stable/) and install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0). `global.json` selects SDK 10.0.403 or a later patch in its feature band. Use matching 4.7.2 .NET export templates when configuring exports.

The project targets .NET 10 on all platforms. Godot's C# support on Android and iOS is experimental; platform exports and device behavior still need validation. No export presets are checked in yet. Local validation does not replace platform export or gameplay testing.

For a clean import, install Blender 5.2.3 and configure its executable in Godot's editor settings. To validate locally, use isolated Godot .NET 4.7.2 and Blender 5.2.3 installations. Create `_sc_` next to the Godot binary (or next to `Godot_mono.app` on macOS), then run `GODOT=/absolute/path/to/godot BLENDER=/absolute/path/to/blender python3 .agents/tools/validate.py`. The script writes settings in that isolated installation. On the Mac mini, run the command through `~/.agents/tools/mac-heavy-lock.sh`. It checks the Debug build, imported files, and both menu and gameplay scenes. Review and commit intentional import metadata changes before rerunning it.
