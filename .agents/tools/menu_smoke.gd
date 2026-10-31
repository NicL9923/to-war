extends SceneTree

func _initialize() -> void:
	call_deferred("exercise_menu")

func exercise_menu() -> void:
	var menu: Node = load("res://MainMenu.tscn").instantiate()
	root.add_child(menu)
	for i in range(120):
		await process_frame
	var music: AudioStreamPlayer = menu.get_node("AudioStreamPlayer")
	if not music.playing:
		push_error("Menu music did not start")
		quit(1)
		return
	if OS.get_environment("MENU_QUIT") == "window":
		menu.notification(Node.NOTIFICATION_WM_CLOSE_REQUEST)
	else:
		menu.get_node("VBoxContainer/ExitButton").pressed.emit()
