# Medição de desempenho

## Instrumentação disponível

O Configure Machine Speed registra eventos de nível `Trace` para cada execução de configuração:

- `ConfigureAllMachines`: quantidade de localidades visitadas, objetos inspecionados, máquinas configuradas, tempo de criação dos índices e tempo total.
- `ConfigureOneMachine`: quantidade de entradas indexadas, se houve correspondência e tempo total.

Ative o log de diagnóstico do SMAPI para coletar essas medições.

## Procedimento de comparação

1. Use o mesmo salvamento, a mesma versão do jogo/SMAPI e a mesma configuração do mod.
2. Registre medições em fazenda pequena, média e grande, com aproximadamente 25, 100 e 300 máquinas configuráveis.
3. Colete pelo menos 30 execuções de cada cenário, incluindo chamadas de início do dia, mudança de horário e interação com máquinas.
4. Compare mediana e percentil 95 do tempo total e do tempo de criação dos índices.
5. Repita os mesmos cenários após qualquer otimização.

| Cenário | Máquinas | Mediana total | P95 total | Mediana dos índices | Observações |
| --- | ---: | ---: | ---: | ---: | --- |
| Pequeno | 25 | A medir no jogo | A medir no jogo | A medir no jogo | Baseline pendente |
| Médio | 100 | A medir no jogo | A medir no jogo | A medir no jogo | Baseline pendente |
| Grande | 300 | A medir no jogo | A medir no jogo | A medir no jogo | Baseline pendente |

## Decisão sobre cache

Nenhum cache adicional dos dicionários de configuração foi introduzido nesta alteração. O repositório não contém medições de runtime que demonstrem que a construção dos índices seja um gargalo relevante. O cache só deve ser considerado depois da coleta do baseline acima e precisa ter invalidação explícita quando a configuração mudar, for redefinida ou recarregada.

As listas de dados estáticos do catálogo de máquinas e de resolução de nomes legados já possuem cache próprio em `Machines`; isso é separado dos índices derivados da configuração reconstruídos por chamada.
