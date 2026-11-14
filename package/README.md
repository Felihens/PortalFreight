# Portal Freight

**[English](#english) · [Português](#português)**

---

## English

Carry anything through portals: ores, metal bars, dragon eggs and every other item the game holds back. Arrive faster, and portals you only walk past stay quiet.

### Features

- **Anything goes through portals,** whatever the world settings say, on your own world and on any server.
- **No more "can't go through portals" marks** on the items in your bag, nor the line saying so in their tooltip (can be turned off).
- **Shorter loading screen:** for a far portal the game always keeps the screen black for at least 8 seconds. Here you arrive as soon as the place is loaded (3 seconds at the least). The game still finds the floor before letting you go.
- **Portals light up and hum only when you come a bit closer** (70% of the game's distance), so the ones you walk past stay quiet.

### Requirements

- [BepInExPack Valheim](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/)
- Client-side. Other players and the server don't need it.

### Installation

**r2modman / Thunderstore Mod Manager:** click *Install*. To install a downloaded zip: Settings → *Import local mod*.

**Manual:**
1. Extract the zip.
2. Copy the `plugins/PortalFreight` folder into `Valheim/BepInEx/plugins/`.
   Result: `Valheim/BepInEx/plugins/PortalFreight/PortalFreight.dll`.
3. Launch the game.

To update, delete the old `PortalFreight` folder before copying the new one.

### Configuration

Press **F1** in game (requires Configuration Manager) or edit `BepInEx/config/felih.PortalFreight.cfg`.

| Option | Default | What it does |
|---|---|---|
| Enabled | on | Lets everything through portals. |
| HideNoPortalMarks | on | Hides the no-portal mark on items and the line in their tooltip. |
| FastArrival | on | Ends the loading screen once the place is loaded. |
| MinLoadSeconds | 3 | Shortest loading screen with FastArrival (the game: 8). |
| GlowRange | 0.7 | How close you must be for a portal to light up, as a share of the game's distance. |
| DetailedLog | off | Writes details to the BepInEx log (for bug reports). |

### Notes

- It doesn't touch the console or the chat, and it needs no other library.

---

## Português

Leve qualquer item pelos portais: minérios, barras de metal, ovos de dragão e todos os outros itens que o jogo bloqueia. Chegue mais rápido, e os portais por onde você só passa ao lado ficam quietos.

### Recursos

- **Tudo passa pelos portais,** sejam quais forem as configurações do mundo, no seu mundo e em qualquer servidor.
- **Sem a marca de "não passa no portal"** nos itens da mochila, nem a linha que diz isso no tooltip (dá para desligar).
- **Tela de carregamento mais curta:** num portal distante o jogo sempre deixa a tela preta por pelo menos 8 segundos. Aqui você chega assim que o lugar carrega (no mínimo 3 segundos). O jogo continua achando o chão antes de soltar você.
- **Os portais só acendem e fazem barulho quando você chega um pouco mais perto** (70% da distância do jogo), então os que você só passa ao lado ficam quietos.

### Requisitos

- [BepInExPack Valheim](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/)
- Só no cliente. Os outros jogadores e o servidor não precisam do mod.

### Instalação

**r2modman / Thunderstore Mod Manager:** clique em *Install*. Para instalar um zip baixado: Settings → *Import local mod*.

**Manual:**
1. Extraia o zip.
2. Copie a pasta `plugins/PortalFreight` para `Valheim/BepInEx/plugins/`.
   Resultado: `Valheim/BepInEx/plugins/PortalFreight/PortalFreight.dll`.
3. Abra o jogo.

Para atualizar, apague a pasta `PortalFreight` antiga antes de copiar a nova.

### Configuração

Aperte **F1** no jogo (precisa do Configuration Manager) ou edite `BepInEx/config/felih.PortalFreight.cfg`.

| Opção | Padrão | O que faz |
|---|---|---|
| Enabled | ligado | Deixa tudo passar pelos portais. |
| HideNoPortalMarks | ligado | Esconde a marca de "não passa no portal" nos itens e a linha no tooltip. |
| FastArrival | ligado | Termina a tela de carregamento assim que o lugar carrega. |
| MinLoadSeconds | 3 | Tela de carregamento mais curta possível com FastArrival (jogo: 8). |
| GlowRange | 0.7 | Quão perto você precisa estar para o portal acender, como fração da distância do jogo. |
| DetailedLog | desligado | Escreve detalhes no log do BepInEx (para relatar bugs). |

### Observações

- Não mexe no console nem no chat, e não precisa de nenhuma outra biblioteca.
