# Atualizações manuais das páginas do Nexus Mods

As páginas públicas do Nexus Mods não são atualizadas por commits ou pull requests deste repositório. Antes de publicar uma nova versão, aplicar manualmente os ajustes abaixo e conferir os requisitos de cada página.

| Mod | Página | Versão documentada no repositório | Ajuste externo necessário |
| --- | --- | --- | --- |
| AddMoney | https://www.nexusmods.com/stardewvalley/mods/21016 | 1.3.0 | Substituir “100.00 Gold” por “100,000 Gold”; informar Stardew Valley 1.6.x e SMAPI 4.2.0+ |
| AutoWater | https://www.nexusmods.com/stardewvalley/mods/21022 | 1.4.2 | Alterar requisito mínimo de SMAPI de 3.0.0 para 4.2.0+; manter GMCM como opcional |
| Configure Machine Speed | https://www.nexusmods.com/stardewvalley/mods/21005 | 2.1.3 | Informar SMAPI 4.2.0+; corrigir a explicação de IDs/nomes legados e documentar exclusões e comportamento especial de Casks |

## Fontes de conteúdo

- AddMoney: copiar a descrição de AddMoney/DESCRIPTION.md.
- AutoWater: copiar a descrição de AutoWater/DESCRIPTION.md.
- Configure Machine Speed: copiar a descrição de ConfigureMachineSpeed/DESCRIPTION.md.

## Conferência antes da publicação

1. Confirmar que a versão no Nexus corresponde à propriedade Version do projeto .csproj.
2. Conferir que o requisito mínimo de SMAPI corresponde a MinimumApiVersion no manifesto.
3. Conferir que as dependências opcionais estão marcadas como opcionais no Nexus.
4. Atualizar o changelog público com as alterações da versão publicada.
5. Conferir o ZIP final gerado em _releases antes de fazer upload.
6. Não anunciar como publicado nenhum ajuste que ainda esteja apenas nesta documentação.
