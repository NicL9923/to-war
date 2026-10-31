#!/usr/bin/env python3
"""Build, import and smoke test with an isolated Godot editor."""
import os
import re
import subprocess
import sys
from pathlib import Path

root = Path(__file__).resolve().parents[2]
os.chdir(root)
godot = Path(os.environ['GODOT']).resolve(strict=True)
blender = Path(os.environ['BLENDER']).resolve(strict=True)

def run(args, timeout=600, errors=False, env=None):
    print('+', ' '.join(map(str, args)), flush=True)
    try:
        result = subprocess.run(args, text=True, stdout=subprocess.PIPE, stderr=subprocess.STDOUT, timeout=timeout, env=env)
    except subprocess.TimeoutExpired as e:
        print(e.stdout.decode(errors='replace') if isinstance(e.stdout, bytes) else e.stdout, flush=True)
        raise SystemExit('Command timed out')
    print(result.stdout, flush=True)
    if result.returncode or (errors and re.search(r'(?im)^\s*(?:ERROR|SCRIPT ERROR|USER ERROR):', result.stdout)):
        raise SystemExit(f'Validation failed (exit {result.returncode})')
    return result.stdout

version = run([str(blender), '--version'], 30)
if not re.match(r'Blender 5\.2\.3(?:\s|$)', version):
    raise SystemExit('Blender 5.2.3 required')
# Self-contained mode must live beside the binary (beside the app bundle on macOS).
parent = godot.parents[2].parent if godot.parent.name == 'MacOS' else godot.parent
marker = parent / '_sc_'
if not marker.exists():
    raise SystemExit(f'Create isolated editor marker: {marker}')
settings = parent / 'editor_data' / 'editor_settings-4.7.tres'
settings.parent.mkdir(parents=True, exist_ok=True)
path = str(blender).replace('\\', '\\\\').replace('"', '\\"')
settings.write_text('[gd_resource type="EditorSettings" format=3]\n\n[resource]\nfilesystem/import/blender/blender_path = "' + path + '"\nfilesystem/import/blender/rpc_port = 0\n')
run(['dotnet', 'build', 'To War.csproj', '--configuration', 'Debug'])
run([str(godot), '--headless', '--editor', '--path', '.', '--import'], errors=True)
for name in subprocess.check_output(['git', 'ls-files', '-z', '*.import']).decode().split('\0'):
    if not name:
        continue
    contents = Path(name).read_text()
    match = re.search(r'^dest_files=\[(.*?)\]', contents, re.M)
    if not match:
        raise SystemExit(f'Missing dest_files: {name}')
    destinations = re.findall(r'"res://([^"\n]+)"', match.group(1))
    if not destinations:
        raise SystemExit(f'No import destinations: {name}')
    for dest in destinations:
        output = root / dest
        if not output.is_file() or output.stat().st_size == 0:
            raise SystemExit(f'Missing or empty import output: {dest} ({name})')
run([str(godot), '--headless', '--path', '.', '--script', 'res://.agents/tools/menu_smoke.gd'], 180, True, {**os.environ, 'MENU_QUIT': 'button'})
run([str(godot), '--headless', '--path', '.', '--script', 'res://.agents/tools/menu_smoke.gd'], 180, True, {**os.environ, 'MENU_QUIT': 'window'})
run([str(godot), '--headless', '--path', '.', 'res://main.tscn', '--quit-after', '120'], 180, True)
if os.environ.get('ALLOW_IMPORT_CHANGES') != '1':
    run(['git', 'diff', '--exit-code', '--', '*.import'])
print('Validation passed')
