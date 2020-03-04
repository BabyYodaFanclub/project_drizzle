
# Readme

## Chunks

Jede als Chunk verlinkte Szene muss unter `Window -> Asset Management -> Adressables -> Groups` unter ihrem im Chunk angegebenen Namen verlinkt werden.

Diese Szenen, müssen nicht in den Buildsettings eingetragen werden.

## Editor Scritps

### BuildAdressablesProcessor.cs

Dieses Script ist dazu da, dass bei jedem Standalone-Buildvorgang auch die Adressables gebaut werden.
Bei Github Actions Builds wird dieses Script zuerst ausgeführt, danach der normale Build.

### CameraScrollFix.cs

Setzt das Scrollverhalten im Scene View auf eine ältere Version, in der Scrollen bei halten des RMB auch ein Zoom in/out war.
Das dadurch überschriebene Verhalten ist das Setzen der Bewegugnsgeschwindigkeit im Scene View.
Diese kann auch über das Kamera Symbol am oberen Rand des Scene Views gesetzt werden.

### PlayFromFirstScene.cs

Dieses Script sorgt dafür, dass wenn im Playmode Play gedrückt wird, die Scene 0 gestartet wird.
Der Sinn dahinter ist, dass alle Chunks im Kontext der Hauptszene Sinn machen.
Der Spieler wird dann in die Mitte der Szene teleportiert.

Per **`Alt+P`** kann das Ladeverhalten und per **`Alt+T`** der automatische Teleport getoggelt werden.
