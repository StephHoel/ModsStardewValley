# 010 - Configure Machine Speed: regras de processamento

**Prioridade:** P1  
**Área:** Configure Machine Speed

## Contexto

O comportamento de processamento envolve `MachineConfigurator`, `MachineList`, exclusões, `DayUpdate`, chocadeiras, Cristalários e barris de envelhecimento (Casks). `ProcessCask` pode chamar `DayUpdate` repetidamente até que a qualidade mude, com um limite relatado de 100 iterações. Esses fluxos precisam de regras explícitas e verificação durante a execução do jogo.

## Requisitos

- Documentar a fonte de verdade da velocidade das máquinas e a precedência das regras de configuração.
- Definir exclusões e tratamentos especiais para chocadeiras, Cristalários e barris de envelhecimento.
- Limitar os loops e garantir sua terminação sem interromper progressões válidas.
- Utilizar IDs qualificados e estáveis para os itens; oferecer migração ou fallback seguro para nomes legados ou localizados.
- Garantir que o processamento diário não seja executado duas vezes nem viole as regras de progressão do jogo.

## Critérios de aceitação

- Cada categoria de máquina especial possui comportamento esperado documentado e testado.
- Casos extremos e término dos loops são cobertos por testes.
- Testes durante a execução confirmam que não há produtos duplicados, etapas ignoradas ou corrupção dos arquivos de salvamento.
