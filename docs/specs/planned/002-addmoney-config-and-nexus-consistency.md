# 002 - AddMoney: consistência entre configuração e Nexus

**Prioridade:** P2  
**Área:** AddMoney

## Contexto

O valor padrão observado no código para `GoldToAdd` é `100000`, enquanto a descrição publicada no Nexus foi relatada como “100.00 Gold”. A unidade ou a formatação pode estar inconsistente.

## Requisitos

- Confirmar a unidade e o valor padrão pretendidos com base na implementação e no histórico de versões.
- Alinhar o valor padrão da configuração, a descrição exibida no jogo, o README, o changelog e a descrição no Nexus.
- Explicar se o valor representa ouro inteiro sem formatação ou um valor monetário formatado.
- Não alterar silenciosamente configurações já existentes dos jogadores.

## Critérios de aceitação

- O valor padrão é descrito sem ambiguidades em todos os canais.
- Arquivos de configuração existentes preservam seus valores, salvo se houver uma migração explícita.
- Exemplos da documentação correspondem ao comportamento real do comando e da configuração.
