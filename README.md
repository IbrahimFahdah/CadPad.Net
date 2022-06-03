# Motivation
A fork of a great [Ibrahim Fahdah's repository](https://github.com/IbrahimFahdah/CadPad.Net). 
The goal is to actualize targets to newer .NET and add some missing functionalities (Layers, Splines, Colors etc.).

# CadPad.Net
2D CAD library using .Net6. A WPF .NET6 program is included to demonstrate how the library can be used using one of Microsoft technologies for Windows applications. It should be very easy to convert the program to another technology such as Winforms because the library has not been written to target a specific Windows technology. 

# Current capabilities:
- [x] zoom and pan.
- [x] add basic geometries such as lines, circles, arcs and rectangles.
- [x] copy and move objects.
- [x] undo and redo actions.
- [x] import and export to dxf using netdxf.
- [x] layers (currently only read & visibility control) 

# Planned future tasks:
- [ ] adding layer editing
- [ ] adding more drawing options
  - [ ] PolyLine2D (combination of lines and arcs)
  - [ ] Spline
  - [ ] ...some others

# Disclaimer:
Some core drawing functionalities were ported from other projects, notably https://github.com/wangyao1052/LitCAD
