# 009 - Configure Machine Speed: isolamento de estado dos patches

**Prioridade:** P1  
**Área:** Configure Machine Speed

## Contexto
O patch `MachineCollectPatch` utiliza estados estáticos, como `wasReady` e `previousHeld`. Um estado compartilhado pode vazar entre máquinas, jogadores ou chamadas intercaladas do patch.

## Requisitos
- Identificar o ciclo de vida exato do patch Harmony e os requisitos de escopo do estado.
- Substituir o estado mutável compartilhado por estado associado à chamada ou ao contexto correto.
- Garantir compatibilidade com outros patches e não depender de uma ordem de execução presumida.
- Manter a aplicação dos patches idempotente e informar claramente falhas de aplicação.

## Critérios de aceitação
- Testes cobrem operações intercaladas em várias máquinas e transições de botões pressionados.
- O estado de uma máquina não interfere no processamento de outra.
- Testes de integração verificam a compatibilidade com as versões suportadas de Harmony e SMAPI.
