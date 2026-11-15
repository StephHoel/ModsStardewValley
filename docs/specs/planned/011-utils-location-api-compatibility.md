# 011 - Utils: compatibilidade com a API pública de localidades

**Prioridade:** P1  
**Área:** Utils

## Contexto
O arquivo `Utils/Locations.cs` aparentemente referencia um namespace interno do SMAPI: `StardewModdingAPI.Framework.ModLoading.Rewriters.StardewValley_1_6`. APIs internas não são contratos estáveis.

## Requisitos
- Substituir referências à implementação interna do SMAPI por APIs públicas e suportadas de Stardew Valley e SMAPI.
- Preservar a enumeração pretendida de localidades da fazenda, interiores e localidades personalizadas.
- Documentar o comportamento para localidades não carregadas e referências duplicadas.
- Manter compatibilidade de código-fonte com os consumidores sempre que possível ou documentar qualquer migração necessária.

## Critérios de aceitação
- Não restam dependências do namespace interno identificado.
- Testes cobrem interiores do jogo base e localidades personalizadas.
- A compilação funciona com as versões declaradas como compatíveis de Stardew Valley e SMAPI.
