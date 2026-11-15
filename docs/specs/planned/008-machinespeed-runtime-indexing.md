# 008 - Configure Machine Speed: indexação durante a execução

**Prioridade:** P2  
**Área:** Configure Machine Speed

## Contexto
O método `ConfigureAllMachines` aparentemente reconstrói índices de máquinas por ID e nome legado em chamadas repetidas. O impacto real no desempenho deve ser medido antes de qualquer otimização.

## Requisitos
- Medir o desempenho do fluxo atual com quantidades representativas de máquinas e frequências de chamada.
- Armazenar índices derivados em cache somente quando a medição demonstrar benefício.
- Invalidar os caches com segurança quando houver mudanças na configuração, no salvamento ou no conteúdo relevante.
- Preferir IDs qualificados e estáveis, mantendo a compatibilidade com nomes legados quando necessário.

## Critérios de aceitação
- Os resultados dos testes de desempenho documentam o cenário inicial e o resultado da mudança.
- Testes de invalidação impedem o uso de configurações obsoletas.
- O comportamento permanece idêntico para todas as definições de máquinas suportadas.
