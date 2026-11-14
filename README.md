# Portal Freight

A client-side BepInEx mod for Valheim: carry anything through portals (ores, metal bars, eggs), whatever the world settings. Shorter loading screen for far portals, and portals light up only when you come a bit closer.

Mod para Valheim (BepInEx, client-side): leve qualquer item pelos portais (minérios, barras de metal, ovos), sejam quais forem as configurações do mundo. Tela de carregamento mais curta em portais distantes, e os portais só acendem quando você chega um pouco mais perto.

For players, see [package/README.md](package/README.md). That file is also the Thunderstore page.
Para jogadores, veja [package/README.md](package/README.md).

Download: [Thunderstore](https://thunderstore.io/c/valheim/p/DJAventuras/PortalFreight/) · [GitHub Releases](https://github.com/Felihens/PortalFreight/releases)

## Building

`sh build.sh` compiles `src/*.cs` against the game's assemblies and installs the dll into `BepInEx/plugins/PortalFreight/` (`sh build.sh nocopy` only compiles). Then `powershell -File pack.ps1` builds `dist/PortalFreight-<version>.zip` from `package/`.

## License

MIT
